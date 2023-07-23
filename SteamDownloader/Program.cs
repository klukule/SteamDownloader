using SteamDownloader.DownloaderV2;
using System.CommandLine;
using System.Reflection;

namespace SteamDownloader;

class Program
{
    static async Task<int> Main(string[] args)
    {
        var targetDirOpt = new Option<DirectoryInfo>("--target", () => new DirectoryInfo("./cs2"), description: "Target Directory");
        var manifestDirOpt = new Option<DirectoryInfo>("--manifests", () => new DirectoryInfo("./manifests"), description: "Directory containing manifest files");
        var depotKeyFileOpt = new Option<FileInfo>("--depot_keys", () => new FileInfo("./depot_keys.json"), description: "File containig depot keys");
        var removeFilesOpt = new Option<bool>("--remove_files", () => true, description: "Whether to remove local files that are not in the depot or not");

        var downloadThreadsOpt = new Option<int>("--download_threads", () => 8, description: "Number of threads used for chunk downloading operation");
        var verifyThreadsOpt = new Option<int>("--verify_threads", () => 8, description: "Number of threads used for verifying local files");
        var writeThreadsOpt = new Option<int>("--write_threads", () => 8, description: "Number of threads used for writing (and decompressing) downloaded chunks");

        var downloadCmd = new RootCommand("Download files from steam CDN");
        downloadCmd.AddOption(targetDirOpt);
        downloadCmd.AddOption(manifestDirOpt);
        downloadCmd.AddOption(depotKeyFileOpt);
        downloadCmd.AddOption(removeFilesOpt);
        downloadCmd.AddOption(downloadThreadsOpt);
        downloadCmd.AddOption(verifyThreadsOpt);
        downloadCmd.AddOption(writeThreadsOpt);

        downloadCmd.SetHandler(Manager.Entry, targetDirOpt, manifestDirOpt, depotKeyFileOpt, removeFilesOpt, downloadThreadsOpt, verifyThreadsOpt, writeThreadsOpt);
        
        return await downloadCmd.InvokeAsync(args);
    }
}