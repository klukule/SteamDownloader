# Steam Downloader 2.0

**The new and improved version 2.0 is out - completely rewritten, much faster, and generally better**

Alternative file downloader from Steam CDN using decrypted manifests in combination with file decryption keys. The difference compared to other programs like steamctl etc.. is that this program also removes files that are not in the depot thus keeping the folder fully synchonized with steam CDN.

Mafifest file parsing, decryption and decompression is handled by modified version of [SteamKit](https://github.com/SteamRE/SteamKit) library (the only changes are to expose interal classes so it can be used externally).

To build the program simply compile SteamDownloader project.

To use:
- Compile the SteamDownloader project
- next to the compiled exe create file `depot_keys.json` containing the depot keys
- create folder `manifests` and put all manifest files you want to download in it
- start the exe and wait for the game to download, the game will be downloaded to `cs2` folder

Since version 2.0 the program also contains CLI interface:

```
Description:
  Download files from steam CDN

Usage:
  SteamDownloader [options]

Options:
  --target <target>                      Target Directory [default: ./cs2]
  --manifests <manifests>                Directory containing manifest files [default: ./manifests]
  --depot_keys <depot_keys>              File containig depot keys [default: ./depot_keys.json]
  --remove_files                         Whether to remove local files that are not in the depot or not [default: True]
  --download_threads <download_threads>  Number of threads used for chunk downloading operation [default: 8]
  --verify_threads <verify_threads>      Number of threads used for verifying local files [default: 8]
  --write_threads <write_threads>        Number of threads used for writing (and decompressing) downloaded chunks [default: 8]
  --version                              Show version information
  -?, -h, --help                         Show help and usage information
```

The defaults are same as for version 1 so it is plug and play

### Third-party libraries
- [SteamKit](https://github.com/SteamRE/SteamKit) - Licensed under [LGPL-2.1](https://github.com/SteamRE/SteamKit/blob/master/LICENSE)