using ShellProgressBar;
using System.Collections.Concurrent;

namespace SteamDownloader.DownloaderV2.Services
{
    internal abstract class BaseService<TRequest>
    {
        private List<Thread> _workerThreads = new();

        protected readonly ConcurrentQueue<TRequest> _queue = new ConcurrentQueue<TRequest>();

        protected readonly ChildProgressBar _progress;
        protected int _totalQueued = 0;
        protected int _totalDone = 0;

        public bool IsIdle => _queue.Count == 0 && _inflight == 0;

        public int QueueLength => _queue.Count;

        protected int _inflight = 0; // Number of requests in-flight

        public BaseService(int threadCount, ProgressBar masterBar, string message, string threadName)
        {
            _progress = masterBar.Spawn(1, message, new ProgressBarOptions { CollapseWhenFinished = false });

            for (int i = 0; i < threadCount; i++)
            {
                _workerThreads.Add(new Thread(DoWork));
                _workerThreads[i].Name = threadName + (i + 1);
                _workerThreads[i].IsBackground = true;
            }
        }

        public void Start()
        {
            foreach (var thread in _workerThreads)
            {
                thread.Start();
            }
        }

        protected abstract void DoWork();

        public void Enqueue(TRequest request)
        {
            _queue.Enqueue(request);
            _totalQueued++;
            UpdateProgress();
        }

        protected void UpdateProgress()
        {
            _progress.MaxTicks = _totalQueued;
            _progress.Tick(_totalDone);
        }
    }
}
