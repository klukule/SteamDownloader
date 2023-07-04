using Decryptor;
using SteamKit2;

List<DepotManifest> manifests = new();
foreach (var manifest in Directory.GetFiles("./manifests"))
    manifests.Add(DepotManifest.LoadFromFile(manifest));

Task.Run(() => Downloader.DownloadGame("./cs2", manifests, "./depot_keys.json")).GetAwaiter().GetResult();