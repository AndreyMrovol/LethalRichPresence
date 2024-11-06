# 0.5.9

- added LobbyCompatibility compatibility (thanks, zetaarcade)
- changed config entry for debug logs & made it disabled by default
- changed some code thingies for the future releases

### Important information for all people who are using their own Discord App Ids

In future version (mid-May) all planet names will be changed to the numberless versions without decorations, so for example:

- `102-wateridge` will be `wateridge`
- `86-ether---downtown` will be `etherdowntown`
- `8-titan` will be `titan`

All named files in my repo will be updated accordingly to reflect the change.

# 0.5.8

- removed unused code
- small image will be cleared on firing animation
- dustclouds and modded weathers should work now

# 0.5.7

- changed the lowercase placeholder resolver to match discord's asset regex

# 0.5.6

- fixed issues with Rich Presence when playing on LAN

# 0.5.5

- fixed issue with discord party ID not accepting 0 as an argument (thanks, [DHStyle541](https://github.com/DHStyle541))

# 0.5.4

- fixed issue with game setting `null` party ID after starting in vanilla (thanks, [DHStyle541](https://github.com/DHStyle541))

# 0.5.3

- Linux Rich Presence is finally (and hopefully) fixed (thank you: [Readek](https://github.com/Readek) and [Naku](https://nnaku.com/))

# 0.5.2

- Discord Activity Joining is working as intended (thanks, Qik'in!)

# 0.5.1

- fixed issue with Discord not reconnecting after Discord restart/shutdown (thanks, [northerncarrots](https://github.com/northerncarrots))
- fixed issue with logs being spammed with Discord errors (thanks, [northerncarrots](https://github.com/northerncarrots))

# 0.5.0

### Please re-generate your config file!

- added IsFiringSequenceActive variable
- fixed handling empty config strings
- changed target build to net46
- added config option to display living players count
- changed debug config names
- added separate config options for being in orbit, on planet, in main menu and in firing sequence
- presence updates every second instead of every 2 seconds
- fixed null in PartyLeaderID
- all Discord assets are now part of repository under CC-BY 4.0
- **added support for modded moons**

# 0.4.1

- update PartyMaxSize only when joining new lobby (thanks, [Clark919](https://github.com/Clark919))

# 0.4.0

### Please re-generate your config file!

- **hopefully** fixed the issue where max party size would be set to 0 (thanks, [FLozyXD](https://github.com/FLozyXD), [Clark919](https://github.com/Clark919))
- added placeholder for modded moons using `InitSceneLaunchOptions`
- added support for **[LobbyInviteOnly](https://thunderstore.io/c/lethal-company/p/Dev1A3/LobbyInviteOnly/)** mod (thanks for [your PR](https://github.com/AndreyMrovol/LethalRichPresence/pull/3), [1A3Dev](https://github.com/1A3Dev))
- added support for MoreCompany/AdvancedCompany bigger lobbies
- fixed an issue with `IsPartyPublic` erroring on LAN (thanks for [your PR](https://github.com/AndreyMrovol/LethalRichPresence/pull/7), [1A3Dev](https://github.com/1A3Dev))
- fixed an issue when the _Join_ button would be always active

# 0.3.1

- changed lobby size detection to support MoreCompany/BiggerLobby (thanks, [FLozyXD](https://github.com/FLozyXD))

# 0.3.0

### Please re-generate your config file!

- added option to allow for join option to work only in public lobbies (thanks, [1A3Dev](https://github.com/1A3Dev))

# 0.2.2

- fixed debug messages being logged always
- optimization: all placeholder values are pulled once per update
- party size is updated only in orbit
- added new placeholders: %onlineorlan%, %hosting%, %partyprivacy%

# 0.2.1

- added main menu image (thanks, [SheepCommander](https://github.com/SheepCommander))

# 0.2.0

- party id is set by default
- added config option
- added placeholder for detecting online/lan mode
- added method for detecting if the player is the host
- fixed party size/max size being set to null and disabling the mod
- small optimization in resolving placeholders
- fixed the option to join lobbies through Discord

# 0.1.1

- fixed a bug where the mod would stop working after starting ship from a planet (thanks, [DHStyle541](https://github.com/DHStyle541))
