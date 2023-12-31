# 0.1.1

- fixed a bug where the mod would stop working after starting ship from a planet (thanks, [DHStyle541](https://github.com/DHStyle541))

# 0.2.0

- party id is set by default
- added config option
- added placeholder for detecting online/lan mode
- added method for detecting if the player is the host
- fixed party size/max size being set to null and disabling the mod
- small optimization in resolving placeholders
- fixed the option to join lobbies through Discord

# 0.2.1

- added main menu image (thanks, [SheepCommander](https://github.com/SheepCommander))

# 0.2.2

- fixed debug messages being logged always
- optimization: all placeholder values are pulled once per update
- party size is updated only in orbit
- added new placeholders: %onlineorlan%, %hosting%, %partyprivacy%

# 0.3.0

### Please re-generate your config file!

- added option to allow for join option to work only in public lobbies (thanks, [1A3Dev](https://github.com/1A3Dev))

# 0.3.1

- changed lobby size detection to support MoreCompany/BiggerLobby (thanks, [FLozyXD](https://github.com/FLozyXD))

# 0.4.0

### Please re-generate your config file!

- **hopefully** fixed the issue where max party size would be set to 0 (thanks, [FLozyXD](https://github.com/FLozyXD), [Clark919](https://github.com/Clark919))
- added placeholder for modded moons using `InitSceneLaunchOptions`
- added support for **[LobbyInviteOnly](https://thunderstore.io/c/lethal-company/p/Dev1A3/LobbyInviteOnly/)** mod (thanks for [your PR](https://github.com/AndreyMrovol/LethalRichPresence/pull/3), [1A3Dev](https://github.com/1A3Dev))
- added support for MoreCompany/AdvancedCompany bigger lobbies
- fixed an issue with `IsPartyPublic` erroring on LAN (thanks for [your PR](https://github.com/AndreyMrovol/LethalRichPresence/pull/7), [1A3Dev](https://github.com/1A3Dev))
- fixed an issue when the _Join_ button would be always active
