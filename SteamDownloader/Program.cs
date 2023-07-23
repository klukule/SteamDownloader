using SteamDownloader.DownloaderV2;
using System.CommandLine;

namespace SteamDownloader;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var targetDirOpt = new Option<DirectoryInfo>("--target", () => new DirectoryInfo("./cs2"), description: "Target Directory");
        var manifestDirOpt = new Option<DirectoryInfo>("--manifests", () => new DirectoryInfo("./manifests"), description: "Directory containing manifest files");
        var depotKeyFileOpt = new Option<FileInfo>("--depot_keys", () => new FileInfo("./depot_keys.json"), description: "File containig depot keys");
        var removeFilesOpt = new Option<bool>("--remove_files", () => true, description: "Whether to remove local files that are not in the depot or not");
        // TODO: Thread-limit related options

        var downloadCmd = new RootCommand("Download files from steam CMD");
        downloadCmd.AddOption(targetDirOpt);
        downloadCmd.AddOption(manifestDirOpt);
        downloadCmd.AddOption(depotKeyFileOpt);
        downloadCmd.AddOption(removeFilesOpt);

        downloadCmd.SetHandler(Manager.Entry, targetDirOpt, manifestDirOpt, depotKeyFileOpt, removeFilesOpt);

        return await downloadCmd.InvokeAsync(args);
    }
}