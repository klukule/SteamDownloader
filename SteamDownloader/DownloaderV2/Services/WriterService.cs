using ShellProgressBar;
using SteamKit2;
using System.Collections.Concurrent;

namespace SteamDownloader.DownloaderV2.Services
{
    /// <summary>
    /// Handles writing of the chunks to the file system
    /// </summary>
    internal class WriterService : BaseService<WriteRequest>
    {
        private ConcurrentDictionary<FileInfo, Stream> _streams = new();

        public WriterService(int threadCount, ProgressBar masterBar) : base(threadCount, masterBar, "Write Progress", nameof(WriterService) + "_Worker")
        {

        }

        protected override void DoWork()
        {
            while (true)
            {
                _progress.Message = $"Write Progress (Queued: {_queue.Count})";


                if (!_queue.IsEmpty && _queue.TryDequeue(out var request))
                {
                    Interlocked.Increment(ref _inflight);

                    using var entryPB = _progress.Spawn(1, request.Key, new ProgressBarOptions { CollapseWhenFinished = true });
                    try
                    {
                        request.Attempt++;

                        WriteFile(request);

                        Interlocked.Increment(ref _totalDone);
                        UpdateProgress();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[DOWNLOAD] Exception - " + ex.Message);
                        if (request.Attempt <= 3)
                        {
                            _queue.Enqueue(request); // Put back to queue
                        }
                        else
                        {
                            Console.WriteLine("[DOWNLOAD] File failed to download after 3 attempts");
                        }
                    }
                    finally
                    {
                        entryPB.Tick();
                        Interlocked.Decrement(ref _inflight);
                    }
                }
                else
                {
                    Thread.Sleep(100);
                }
            }
        }

        private void WriteFile(WriteRequest request)
        {
            byte[] processedData = CryptoHelper.SymmetricDecrypt(request.ChunkData, request.RemoteFile.DecryptionKey);

            if (processedData.Length > 1 && processedData[0] == 'V' && processedData[1] == 'Z')
            {
                processedData = VZipUtil.Decompress(processedData);
            }
            else
            {
                processedData = ZipUtil.Decompress(processedData);
            }

            if (!CryptoHelper.AdlerHash(processedData).SequenceEqual(request.Chunk.Checksum))
            {
                // TODO: Globally limit this, this could result if we have wrong decryption key
                // If checksum failed, request file download again
                Manager.EnqueueDownloadRequest(request, request.Chunk);
                return;
            }

            if (!_streams.TryGetValue(request.LocalFile, out var stream))
                _streams.TryAdd(request.LocalFile, stream = Stream.Synchronized(request.LocalFile.OpenWrite()));

            stream.Position = (long)request.Chunk.Offset;
            stream.Write(processedData, 0, processedData.Length);
            stream.Flush();
        }

        public void FlushAndCloseAll()
        {
            // NOTE This is kinda hacky way to do it, it would be better to periodically flush to avoid memory buildup, but hey it works
            foreach (var stream in _streams)
            {
                stream.Value.Flush();
                stream.Value.Close();
                stream.Value.Dispose();
            }
            _streams.Clear();
        }
    }

    public class WriteRequest : DownloadRequest // We expand on data from download request
    {
        public byte[] ChunkData { get; set; }
    }
}
