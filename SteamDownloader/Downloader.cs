using SharpCompress;
using SteamKit2;
using System.Text.Json;

namespace Decryptor
{
    public static class Downloader
    {
        private static Dictionary<uint, byte[]> DEPOT_KEYS = new Dictionary<uint, byte[]>();
        private static string GAME_ROOT = null;
        private static Dictionary<string, FileSystemEntry> FILEMAP_DISK = new();
        private static Dictionary<string, ManifestFileEntry> FILEMAP_MANIFEST = new();
        private static HttpClient CLIENT = new HttpClient();
        public static void DownloadGame(string path, IEnumerable<DepotManifest> manifests, string depotKeys)
        {
            // Get target directory and load file decryption keys
            GAME_ROOT = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), path));
            DEPOT_KEYS = LoadDepotKeys(depotKeys);

            // Build file maps from both manifests and file system
            BuildDiskFileMap();

            FILEMAP_MANIFEST.Clear();
            foreach (var manifest in manifests)
            {
                BuildManifestFileMap(manifest);
            }

            Console.WriteLine("[BUILD] Built local filemap for {0} files", FILEMAP_DISK.Count);
            Console.WriteLine("[BUILD] Built remote filemap for {0} files", FILEMAP_MANIFEST.Count);


            // Now compare filemaps

            var filesToRemove = FILEMAP_DISK.Where(x => !FILEMAP_MANIFEST.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            var filesToCreate = FILEMAP_MANIFEST.Where(x => !FILEMAP_DISK.ContainsKey(x.Key)).ToDictionary(x => x.Key, x => x.Value);
            var filesToUpdate = FILEMAP_MANIFEST.Where(x => FILEMAP_DISK.ContainsKey(x.Key)).Where(x => !x.Value.Data.FileHash.SequenceEqual(FILEMAP_DISK[x.Key].FileHash)).ToDictionary(x => x.Key, x => x.Value);

            var filesToDownload = new Dictionary<string, ManifestFileEntry>();

            // Remove files that are no longer in the depot
            foreach (var file in filesToRemove)
            {
                Console.WriteLine("[DOWNLOAD] Removing file {0} - file not in depot", file.Key);
                File.Delete(file.Value.FullPath);
            }

            // Determine if the update file needs to be recreated or just has it's content updated
            foreach (var file in filesToUpdate)
            {
                var diskFile = FILEMAP_DISK[file.Key];
                if (file.Value.Data.TotalSize != diskFile.FileSize)
                {
                    File.Delete(diskFile.FullPath);
                    filesToCreate.Add(file.Key, file.Value);
                }
                else
                {
                    filesToDownload.Add(file.Key, file.Value);
                }
            }

            // Create new files and queue them for download
            Parallel.ForEach(filesToCreate, (file) =>
            {
                Console.WriteLine("[DOWNLOAD] Allocating file {0} - file size {1} bytes", file.Key, file.Value.Data.TotalSize);
                var fullPath = Path.GetFullPath(Path.Combine(GAME_ROOT, file.Key));
                var zeroes = new byte[file.Value.Data.TotalSize];
                var fs = File.Create(fullPath, zeroes.Length);
                fs.Write(zeroes, 0, zeroes.Length);
                fs.Close();

                lock (filesToDownload)
                {
                    filesToDownload.Add(file.Key, file.Value);
                }
            });

            Console.WriteLine("[DOWNLOAD] Downloading changed {0} files", filesToDownload.Count);

            // Now we download
            Parallel.ForEach(filesToDownload, file =>
            {
                var line = $"[DOWNLOAD] Downloading file {file.Key} - ";
                try
                {
                    DownloadFile(file.Value);
                    Console.WriteLine(line + "Success");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(line + "Failed - " + ex.Message);
                }
            });

            Console.WriteLine("------------------");
            Console.WriteLine("DOWNLOAD COMPLETED");
            Console.WriteLine("------------------");
        }

        private static void BuildManifestFileMap(DepotManifest manifest)
        {
            Console.WriteLine("[BUILD] Building remote filemap for depot {0} manifest {1}", manifest.DepotID, manifest.ManifestGID);
            foreach (var file in manifest.Files)
            {
                var relativePath = file.FileName.TrimNulls();
                FILEMAP_MANIFEST.Add(relativePath, new ManifestFileEntry { Data = file, DecryptionKey = DEPOT_KEYS[manifest.DepotID], DepotID = manifest.DepotID });
            }
        }

        private static void BuildDiskFileMap()
        {
            FILEMAP_DISK = new();

            if (!Path.Exists(GAME_ROOT))
            {
                return;
            }

            var files = Directory.GetFiles(GAME_ROOT, "*.*", SearchOption.AllDirectories).ToList();

            Console.WriteLine("Building local filemap");

            Parallel.ForEach(files, (file) =>
            {
                var relativePath = Path.GetRelativePath(GAME_ROOT, file);

                var entry = new FileSystemEntry
                {
                    Info = new FileInfo(file),
                    FileHash = CryptoHelper.SHAHash(File.ReadAllBytes(file))
                };

                // TODO: Calculate chunk hashes, so we can just replace changed chunks if the file size is the same instead of redownloading whole file

                lock (FILEMAP_DISK)
                {
                    FILEMAP_DISK.Add(relativePath, entry);
                }
            });

        }

        public static void DownloadFile(ManifestFileEntry file)
        {
            var fullPath = Path.GetFullPath(Path.Combine(GAME_ROOT, file.Data.FileName.TrimNulls()));
            using var fs = File.OpenWrite(fullPath);

            Parallel.ForEach(file.Data.Chunks, chunk =>
            {
                DownloadChunk(fs, file.DepotID, chunk, file.DecryptionKey);
            });
        }

        private static void DownloadChunk(FileStream fs, uint depotId, DepotManifest.ChunkData chunk, byte[] decryptionKey)
        {
            // TODO: Rewrite async/await
            var task = CLIENT.GetByteArrayAsync($"https://google.cdn.steampipe.steamcontent.com/depot/{depotId}/chunk/{Utils.EncodeHexString(chunk.ChunkID)}");
            task.Wait();
            var chunkData = task.Result;

            byte[] processedData = CryptoHelper.SymmetricDecrypt(chunkData, decryptionKey);

            if (processedData.Length > 1 && processedData[0] == 'V' && processedData[1] == 'Z')
            {
                processedData = VZipUtil.Decompress(processedData);
            }
            else
            {
                processedData = ZipUtil.Decompress(processedData);
            }

            if (!CryptoHelper.AdlerHash(processedData).SequenceEqual(chunk.Checksum))
            {
                throw new Exception("Invalid chunk checksum");
            }

            lock (fs)
            {
                fs.Position = (long)chunk.Offset;
                fs.Write(processedData, 0, processedData.Length);
            }
        }

        private static Dictionary<uint, byte[]> LoadDepotKeys(string v)
        {
            var rawKeys = JsonSerializer.Deserialize<Dictionary<string, string>>(File.ReadAllText("./depot_keys.json"));

            var keys = new Dictionary<uint, byte[]>();
            foreach (var key in rawKeys)
            {
                keys.Add(uint.Parse(key.Key), Utils.DecodeHexString(key.Value));
            }

            return keys;
        }
    }

    public class FileSystemEntry
    {
        public FileInfo Info { get; set; }
        public byte[] FileHash { get; set; }

        public string FullPath => Info.FullName;
        public uint FileSize => (uint)Info.Length;
    }

    public class ManifestFileEntry
    {
        public DepotManifest.FileData Data { get; set; }
        public byte[] DecryptionKey { get; set; }
        public uint DepotID { get; set; }
    }
}
