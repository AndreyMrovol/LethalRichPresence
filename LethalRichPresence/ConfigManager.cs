using System;
using BepInEx.Configuration;
using Discord;

namespace LethalRichPresence;

public class ConfigManager
{
  public static ConfigManager Instance { get; private set; }

  public static void Init(ConfigFile config)
  {
    Instance = new ConfigManager(config);
  }

  public static ConfigEntry<long> AppID { get; private set; }

  public static ConfigEntry<bool> ShowParty { get; private set; }
  public static ConfigEntry<bool> AllowJoin { get; private set; }

  public static ConfigEntry<string> LargeImage { get; private set; }
  public static ConfigEntry<string> LargeText { get; private set; }

  public static ConfigEntry<string> SmallImage { get; private set; }
  public static ConfigEntry<string> SmallText { get; private set; }

  public static ConfigEntry<string> ActivityState { get; private set; }
  public static ConfigEntry<string> ActivityDetails { get; private set; }
  public static ConfigEntry<bool> ActivityPlayers { get; private set; }

  public static ConfigEntry<bool> Debug { get; private set; }

  private ConfigManager(ConfigFile config)
  {
    AppID = config.Bind("General", "AppID", 1184272051075305483, "The Discord App ID for this game.");

    ShowParty = config.Bind("Party", "ShowParty", true, "Group players into parties in the rich presence.");
    AllowJoin = config.Bind("Party", "AllowJoin", true, "Allow players to join your game from Discord.");


    LargeImage = config.Bind("Presence", "LargeImage", "%&currentplanet%", "The large image key for the rich presence.");
    LargeText = config.Bind("Presence", "LargeText", "%currentplanet%", "The large image tooltip for the rich presence.");

    SmallImage = config.Bind("Presence", "SmallImage", "%&currentweather%", "The small image key for the rich presence.");
    SmallText = config.Bind("Presence", "SmallText", "%currentweather%", "The small image tooltip for the rich presence.");

    ActivityDetails = config.Bind("Presence", "ActivityDetails", "%quotacountifier% quota: %quota% (%collected% on ship)", "The details of the rich presence.");
    ActivityState = config.Bind("Presence", "ActivityState", "%timeleft% left, playing with crew", "The state of the rich presence.");
    ActivityPlayers = config.Bind("Presence", "ActivityPlayers", true, "Display number of players");



    Debug = config.Bind("Debugging", "Debug", true, "Enable debug logging.");
  }
}