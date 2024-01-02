using System;
using BepInEx.Configuration;

namespace LethalRichPresence;

public class ConfigManager
{
  public static ConfigManager Instance { get; private set; }

  public static void Init(ConfigFile config)
  {
    Instance = new ConfigManager(config);
  }

  public static ConfigEntry<long> AppID { get; private set; }

  public static ConfigEntry<bool> AllowJoin { get; private set; }
  public static ConfigEntry<bool> JoinOnlyInPublicLobby { get; private set; }
  public static ConfigEntry<bool> DisplayLivingPlayers { get; private set; }

  public static ConfigEntry<string> OrbitLargeImage { get; private set; }
  public static ConfigEntry<string> OrbitLargeText { get; private set; }
  public static ConfigEntry<string> OrbitSmallImage { get; private set; }
  public static ConfigEntry<string> OrbitSmallText { get; private set; }


  public static ConfigEntry<string> PlanetLargeImage { get; private set; }
  public static ConfigEntry<string> PlanetLargeText { get; private set; }
  public static ConfigEntry<string> PlanetSmallImage { get; private set; }
  public static ConfigEntry<string> PlanetSmallText { get; private set; }

  public static ConfigEntry<string> MainMenuLargeImage { get; private set; }

  public static ConfigEntry<string> FiredSequenceLargeImage { get; private set; }


  public static ConfigEntry<string> ActivityState { get; private set; }
  public static ConfigEntry<string> ActivityDetails { get; private set; }
  public static ConfigEntry<bool> ActivityPlayers { get; private set; }

  public static ConfigEntry<bool> Debug { get; private set; }

  private ConfigManager(ConfigFile config)
  {
    AppID = config.Bind("General", "AppID", 1184272051075305483, "The Discord App ID for this game.");

    AllowJoin = config.Bind("Party", "AllowJoin", true, "Allow players to join your game from Discord.");
    JoinOnlyInPublicLobby = config.Bind("Party", "JoinOnlyInPublicLobby", false, "Only allow players to join your game from Discord if your lobby is public.");

    ActivityPlayers = config.Bind("Presence", "ActivityPlayers", true, "Display number of players");
    DisplayLivingPlayers = config.Bind("Presence", "DisplayLivingPlayers", false, "Display amount of currently alive players in the rich presence.");

    ActivityDetails = config.Bind("Presence", "ActivityDetails", "%quotacountifier% quota: %quota% (%collected% on ship)", "The details of the rich presence.");
    ActivityState = config.Bind("Presence", "ActivityState", "%timeleft% left, playing %onlineorlan%", "The state of the rich presence.");

    OrbitLargeImage = config.Bind("Presence.InOrbit", "LargeImage", "inorbit", "The large image key for the rich presence.");
    OrbitLargeText = config.Bind("Presence.InOrbit", "LargeText", "In Orbit", "The large image tooltip for the rich presence.");
    OrbitSmallImage = config.Bind("Presence.InOrbit", "SmallImage", "%&currentplanet%", "The small image key for the rich presence.");
    OrbitSmallText = config.Bind("Presence.InOrbit", "SmallText", "Orbiting %currentplanet%", "The small image tooltip for the rich presence.");

    PlanetLargeImage = config.Bind("Presence.OnPlanet", "LargeImage", "%&currentplanet%", "The large image key for the rich presence.");
    PlanetLargeText = config.Bind("Presence.OnPlanet", "LargeText", "%currentplanet%", "The large image tooltip for the rich presence.");
    PlanetSmallImage = config.Bind("Presence.OnPlanet", "SmallImage", "%&currentweather%", "The small image key for the rich presence.");
    PlanetSmallText = config.Bind("Presence.OnPlanet", "SmallText", "%currentweather%", "The small image tooltip for the rich presence.");

    MainMenuLargeImage = config.Bind("Presence.MainMenu", "LargeImage", "mainmenu", "The large image key for the rich presence.");

    FiredSequenceLargeImage = config.Bind("Presence.FiredSequence", "LargeImage", "fired", "The large image key for the rich presence.");


    Debug = config.Bind("Debugging", "Debug", true, "Enable debug logging.");
  }
}