using ShellProgressBar;
using SteamKit2;
using System.Net;

namespace SteamDownloader.DownloaderV2.Services
{
    /// <summary>
    /// Handles downloading of the chunks
    /// </summary>
    internal class DownloaderService : BaseService<DownloadRequest>
    {
        public DownloaderService(int threadCount, ProgressBar masterBar) : base(threadCount, masterBar, "Download Progress", nameof(DownloaderService) + "_Worker")
        {

        }

        protected override async void DoWork()
        {
            HttpClient client = new HttpClient
            {
                DefaultRequestVersion = HttpVersion.Version20,
                DefaultVersionPolicy = HttpVersionPolicy.RequestVersionOrLower
            };

            while (true)
            {
                _progress.Message = $"Download Progress (Queued: {_queue.Count})";

                // Throttle verify due to slow writes (avoids memory buildup)
                if (Manager.Writer.QueueLength > Manager.WriteQueueLength)
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

                        await DownloadChunk(request, client);

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

        private async Task DownloadChunk(DownloadRequest request, HttpClient client)
        {
            var chunkData = await client.GetByteArrayAsync($"https://google.cdn.steampipe.steamcontent.com/depot/{request.RemoteFile.DepotID}/chunk/{Utils.EncodeHexString(request.Chunk.ChunkID)}");
            Manager.EnqueueWriteRequest(request, chunkData);
        }
    }

    public class DownloadRequest : VerifyRequest // We expand the data from verify request
    {
        public DepotManifest.ChunkData Chunk { get; set; }
    }
}