using SharpCompress;
using ShellProgressBar;
using SteamDownloader.DownloaderV2.Services;
using SteamKit2;
using System.Text.Json;

namespace SteamDownloader.DownloaderV2
{
    /// <summary>
    /// Main manager class
    /// </summary>
    internal static class Manager
    {
        internal static VerificatorService Verificator;
        internal static DownloaderService Downloader;
        internal static WriterService Writer;

        internal static DirectoryInfo DownloadDirectory;

        private static ProgressBar _masterProgressbar;

        public const int DownloadQueueLength = 1024; // If download queue has more than 1024 entries, verification will pause until stuff is downloaded
        public const int WriteQueueLength = 512; // If write queue has more than 512 entries download will pause and will wait for the queue to go to less than this number

        public static async Task Entry(DirectoryInfo targetDir, DirectoryInfo manifestDir, FileInfo depotKeyFile, bool bRemoveLocalOnly)
        {
            const int downloadThreads = 8;      // Number of threads used to download chunks over HTTP and decompress them
            const int verifyThreads = 8;        // Number of threads used to do file verification on local disk - if file does not exists, it sends write request
            const int writeThreads = 8;        // Number of threads used to write downloaded chunks to local disk


            // Create target directory if one does not exist
            if (!targetDir.Exists)
                targetDir.Create();

            DownloadDirectory = targetDir;

            // Verify presence of manifests
            if (!manifestDir.Exists || manifestDir.EnumerateFiles().Count() == 0)
            {
                Console.WriteLine("[ERROR] No manifest files found.");
                return;
            }

            if (!depotKeyFile.Exists)
            {
                Console.WriteLine("[ERROR] Depot key file not found.");
                return;
            }

            // Create progress-bar
            _masterProgressbar = new ProgressBar(1, "Parse Progress");

            // Create sub-managers
            Writer = new(writeThreads, _masterProgressbar);
            Downloader = new(downloadThreads, _masterProgressbar);
            Verificator = new(verifyThreads, _masterProgressbar, bRemoveLocalOnly);

            Writer.Start();
            Downloader.Start();
            Verificator.Start();

            ProcessFiles(manifestDir.EnumerateFiles(), depotKeyFile);

            await WaitDone();
            Writer.FlushAndCloseAll();
            _masterProgressbar.Dispose();
        }

        private static void ProcessFiles(IEnumerable<FileInfo> manifests, FileInfo depotKeyFile)
        {
            var remoteFiles = LoadManifestFiles(manifests, depotKeyFile);
            var localFiles = LoadLocalFiles(DownloadDirectory);

            var distinctKeys = remoteFiles.Keys.Concat(localFiles.Keys).Distinct().ToList();
            _masterProgressbar.MaxTicks = distinctKeys.Count;

            foreach (var key in distinctKeys)
            {
                FileInfo local = null;
                RemoteFile remote = null;

                bool bHasLocal = localFiles.TryGetValue(key, out local);
                bool bHasRemote = remoteFiles.TryGetValue(key, out remote);

                if (bHasLocal || bHasRemote)
                {
                    EnqueueVerifyRequest(key, local ?? new FileInfo(Path.Join(DownloadDirectory.FullName, key)), remote);
                }
                _masterProgressbar.Tick();
            }
        }

        private static Dictionary<string, FileInfo> LoadLocalFiles(DirectoryInfo targetDirectory)
        {
            var files = targetDirectory.EnumerateFiles("*.*", SearchOption.AllDirectories);
            var map = new Dictionary<string, FileInfo>();
            foreach (var file in files)
            {
                map.Add(Path.GetRelativePath(targetDirectory.FullName, file.FullName), file);
            }

            return map;
        }

        private static Dictionary<string, RemoteFile> LoadManifestFiles(IEnumerable<FileInfo> manifests, FileInfo depotKeyFile)
        {
            var keys = LoadDepotKeys(depotKeyFile);

            var map = new Dictionary<string, RemoteFile>();

            foreach (var manifestPath in manifests)
            {
                var manifest = DepotManifest.LoadFromFile(manifestPath.FullName);
                foreach (var file in manifest.Files)
                {
                    var relativePath = file.FileName.TrimNulls();
                    map.Add(relativePath, new RemoteFile { FileInfo = file, DecryptionKey = keys[manifest.DepotID], DepotID = manifest.DepotID });
                }
            }

            return map;
        }

        private static Dictionary<uint, byte[]> LoadDepotKeys(FileInfo depotKeyFile)
        {
            // Load depot keys
            var rawKeys = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText(depotKeyFile.FullName));

            var keys = new Dictionary<uint, byte[]>();
            foreach (var key in rawKeys)
                keys.Add(uint.Parse(key.Key), Utils.DecodeHexString(key.Value));

            return keys;
        }

        private static async Task WaitDone()
        {
            while (true)
            {
                var verificatorDone = Verificator.IsIdle;
                var downloadDone = Downloader.IsIdle;
                var writeDone = Writer.IsIdle;

                if (verificatorDone && downloadDone && writeDone)
                    return;

                await Task.Delay(1000);
            }
        }

        public static void EnqueueVerifyRequest(string key, FileInfo localFile, RemoteFile cdnFile)
        {
            Verificator.Enqueue(new VerifyRequest
            {
                Key = key,
                LocalFile = localFile,
                RemoteFile = cdnFile,
                Attempt = 0 // First attempt
            });
        }

        public static void EnqueueDownloadRequest(VerifyRequest verify, DepotManifest.ChunkData chunk)
        {
            Downloader.Enqueue(new DownloadRequest
            {
                Attempt = 0,
                Chunk = chunk,
                LocalFile = verify.LocalFile,
                RemoteFile = verify.RemoteFile,
                Key = $"{verify.Key} ({chunk.Offset} - {chunk.Offset + chunk.CompressedLength})"
            });
        }

        public static void EnqueueWriteRequest(DownloadRequest request, byte[] data)
        {
            Writer.Enqueue(new WriteRequest
            {
                Attempt = 0,
                Chunk = request.Chunk,
                LocalFile = request.LocalFile,
                RemoteFile = request.RemoteFile,
                Key = $"{request.Key.Split('(')[0]}({request.Chunk.Offset} - {request.Chunk.Offset + request.Chunk.UncompressedLength})",
                ChunkData = data
            });
        }
    }

    public class RemoteFile
    {
        public DepotManifest.FileData FileInfo { get; set; }
        public uint DepotID { get; set; }
        public byte[] DecryptionKey { get; set; }
    }
}
