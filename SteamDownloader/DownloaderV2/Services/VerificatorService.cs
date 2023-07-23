using ShellProgressBar;
using SteamKit2;
using System.Collections.Concurrent;

namespace SteamDownloader.DownloaderV2.Services
{
    /// <summary>
    /// Class contaning all the logic responsible for verifying the hashes of files (and their chunks) on local system and allocating new files if they do not exist yet
    /// </summary>
    internal class VerificatorService : BaseService<VerifyRequest>
    {
        private bool _bRemoveLocalOnly;

        public VerificatorService(int threadCount, ProgressBar masterBar, bool bRemoveLocalOnly) : base(threadCount, masterBar, "Verify Progress", nameof(VerificatorService) + "_Worker")
        {
            _bRemoveLocalOnly = bRemoveLocalOnly;
        }

        protected override void DoWork()
        {
            while (true)
            {
                _progress.Message = $"Verify Progress (Queued: {_queue.Count})";


                // Throttle verify due to slow download (avoids memory buildup) + slows down writes due to I/O pressure
                if (Manager.Downloader.QueueLength > Manager.DownloadQueueLength)
                {
                    Thread.Sleep(100);
                    continue;
                }

                if (!_queue.IsEmpty && _queue.TryDequeue(out var request))
                {
                    Interlocked.Increment(ref _inflight);

                    using var entryPB = _progress.Spawn(1, request.Key, new ProgressBarOptions { CollapseWhenFinished = true });
                    try
                    {
                        request.Attempt++;

                        // Remote missing - remove file, if allowed
                        if (request.LocalFile.Exists && request.RemoteFile != null)
                        {
                            // A) Files have the same hash -> done, nothing to do
                            // B) Files have different hash -> check chunk hashes -> enqueue changed chunks

                            if (request.RemoteFile.FileInfo.TotalSize != (ulong)request.LocalFile.Length)
                            {
                                request.LocalFile.Delete();
                                CreateNewFile(request, entryPB);
                            }
                            else
                            {
                                CompareExistingFiles(request, entryPB);
                            }
                        }
                        // Remote missing
                        else if (request.LocalFile.Exists)
                        {
                            DeleteFile(request, entryPB);
                        }
                        // Local missing, allocate and queue all chunks to download
                        else if (request.RemoteFile != null)
                        {
                            CreateNewFile(request, entryPB);
                        }


                        Interlocked.Increment(ref _totalDone);
                        UpdateProgress();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("[VERIFY] Exception - " + ex.Message);
                        if (request.Attempt <= 3)
                        {
                            _queue.Enqueue(request); // Put back to queue
                        }
                        else
                        {
                            Console.WriteLine("[VERIFY] File failed to verify after 3 attempts");
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

        private void DeleteFile(VerifyRequest request, ChildProgressBar progress)
        {
            if (_bRemoveLocalOnly)
            {
                request.LocalFile.Delete();
            }
        }

        private void CreateNewFile(VerifyRequest request, ChildProgressBar progress)
        {
            Directory.CreateDirectory(request.LocalFile.DirectoryName);

            using (var fs = request.LocalFile.Create())
            {
                var zeros = new byte[request.RemoteFile.FileInfo.TotalSize];
                fs.Write(zeros, 0, zeros.Length);
                fs.Flush();
                fs.Close();
            }

            progress.MaxTicks = request.RemoteFile.FileInfo.Chunks.Count;
            foreach (var chunk in request.RemoteFile.FileInfo.Chunks)
            {
                Manager.EnqueueDownloadRequest(request, chunk);
                progress.Tick();
            }
        }

        private void CompareExistingFiles(VerifyRequest request, ChildProgressBar progress)
        {
            var localFileData = File.ReadAllBytes(request.LocalFile.FullName);
            var localFileHash = CryptoHelper.SHAHash(localFileData);

            if (!request.RemoteFile.FileInfo.FileHash.SequenceEqual(localFileHash))
            {
                progress.MaxTicks = request.RemoteFile.FileInfo.Chunks.Count;

                foreach (var chunk in request.RemoteFile.FileInfo.Chunks)
                {
                    var localChunk = localFileData.AsSpan((int)chunk.Offset, (int)chunk.UncompressedLength);

                    var chunkHash = CryptoHelper.SHAHash(localChunk.ToArray());

                    if (!chunk.ChunkID.SequenceEqual(chunkHash))
                    {
                        Manager.EnqueueDownloadRequest(request, chunk);
                    }

                    progress.Tick();
                }
            }
        }
    }

    public class VerifyRequest
    {
        public string Key { get; set; }
        public FileInfo LocalFile { get; set; }
        public RemoteFile RemoteFile { get; set; }
        public int Attempt { get; set; } = 0;
    }
}
