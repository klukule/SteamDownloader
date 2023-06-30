# Steam Downloader

Quick and dirty downloader capable of downloading data from Steam CDN using decrypted manifests in combination with file decryption keys. The difference compared to other programs like steamctl etc.. is that this program also removes files that are not in the depot thus keeping the folder fully synchonized with steam CDN.

**!! In rare cases some chunks fail to download, just re-run the program to fix that !!**

File parsing, decryption and decompression is handled by modified version of [SteamKit](https://github.com/SteamRE/SteamKit) library (the only changes are to expose interal classes so it can be used externally).

To build the program simply compile SteamDownloader project.

To use:
- Compile the SteamDownloader project
- next to the compiled exe create file `depot_keys.json` containing the depot keys
- create folder `manifests` and put all manifest files you want to download in it
- start the exe and wait for the game to download, the game will be downloaded to `cs2` folder

To change location where the game is downloaded to, or where the depot keys and manifests are, update the Program.cs in SteamDownloader project and rebuild

### TODO
- [ ] Calculate chunk hashes when file hash is different and download only changed chunks instead of whole files
- [ ] Optimalize persistence
- [ ] Implement CLI interface
- [ ] Split allocation and download stages so they can be ran in parallel
- [ ] Auto-retry on chunk download failure
- [ ] Handle special case while calculating hashes for files with 0 length

### Third-party libraries
- [SteamKit](https://github.com/SteamRE/SteamKit) - Licensed under [LGPL-2.1](https://github.com/SteamRE/SteamKit/blob/master/LICENSE)