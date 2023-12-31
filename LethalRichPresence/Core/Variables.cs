using System.Linq;
using UnityEngine;

namespace LethalRichPresence;

public class Variables
{

  private static string NullToString(object Value)
  {
    // Value.ToString() allows for Value being DBNull, but will also convert int, double, etc.
    return Value == null ? "" : Value.ToString();
  }

  /// <summary>
  /// Calculate the value of all scrap in the ship.
  /// 
  /// <seealso href="https://github.com/tinyhoot/ShipLoot/blob/023cb32d1dbc36dfd407d276996a4a8ea4f4fc7b/ShipLoot/Patches/HudManagerPatcher.cs#L56">Original code from tinyhoot's ShipLoot mod</seealso>
  /// </summary>
  /// <returns>The total scrap value.</returns>
  public static float LootValue()
  {
    GameObject ship = GameObject.Find("/Environment/HangarShip");
    // Get all objects that can be picked up from inside the ship. Also remove items which technically have
    // scrap value but don't actually add to your quota.
    var loot = ship.GetComponentsInChildren<GrabbableObject>().ToList()
    .Where(obj => obj.name != "ClipboardManual" && obj.name != "StickyNoteItem" && obj.name != "Key(Clone)" && obj.name != "Key").Where(obj => obj.scrapValue > 0).ToList();
    return loot.Sum(scrap => scrap.scrapValue);
  }

  public static bool AmIHost()
  {
    return GameNetworkManager.Instance.isHostingGame;
  }

  public static bool IsShipInOrbit()
  {
    return CurrentPlanet() == "In orbit";
  }

  public static string IsOnlineOrLAN()
  {
    return GameNetworkManager.Instance.disableSteam ? "on LAN" : "online";
  }

  public static bool IsPartyPublic()
  {
    return GameNetworkManager.Instance.lobbyHostSettings?.isLobbyPublic != null ? GameNetworkManager.Instance.lobbyHostSettings.isLobbyPublic : false;
  }

  public static bool IsPartyInviteOnly()
  {
    return GameNetworkManager.Instance.currentLobby?.GetData("inviteOnly") == "true";
  }

  public static string IsHostingOrMember()
  {
    return AmIHost() ? "hosting" : "member";
  }

  public static string PartyID()
  {
    return GameNetworkManager.Instance.currentLobby?.Id.ToString();
  }

  public static int PartySize()
  {
    return StartOfRound.Instance.livingPlayers;
  }

  public static int PartyMaxSize()
  {
    return GameNetworkManager.Instance.currentLobby?.MaxMembers ?? GameNetworkManager.Instance.maxAllowedPlayers;
  }

  public static string PartyLeader()
  {
    return GameNetworkManager.Instance.currentLobby?.Owner.Name;
  }

  public static string PartyLeaderID()
  {
    return GameNetworkManager.Instance.currentLobby?.Owner.Id.ToString();
  }

  public static string PartyPrivacy()
  {
    return IsPartyPublic() ? "public" : "private";
  }

  public static int Quota()
  {
    return TimeOfDay.Instance.profitQuota;
  }

  public static int QuotaFulfilled()
  {
    return TimeOfDay.Instance.quotaFulfilled;
  }

  public static int QuotaNo()
  {
    return TimeOfDay.Instance.timesFulfilledQuota + 1;
  }

  public static string QuotaNoCountifier()
  {
    return ((Variables.QuotaNo() % 20) + Countifiers.countifiers[Variables.QuotaNo() % 20].ToString()).ToString();
  }

  public static int DaysRemaining()
  {
    return TimeOfDay.Instance.daysUntilDeadline;
  }

  public static string TimeRemaining()
  {
    var isMultiple = TimeOfDay.Instance.daysUntilDeadline != 1 ? "s" : "";
    return TimeOfDay.Instance.daysUntilDeadline + " day" + isMultiple;
  }

  public static string CurrentPlanet()
  {
    return Planets.planets[NullToString(Lifecycle.currentPlanet)];
  }

  public static string CurrentWeather()
  {
    return Planets.weathers[TimeOfDay.Instance.currentLevelWeather.ToString()];
  }

}
