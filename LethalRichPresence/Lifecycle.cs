using System;
using Discord;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace LethalRichPresence;

public class Lifecycle : MonoBehaviour
{
  private ActivityManager DiscordActivityManager;
  private Activity DiscordActivity;

  private bool inGame = false;
  public static string currentPlanet;

  public float period = 0.0f;

  private void Start()
  {
    // Plugin startup logic
    Plugin.logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

    DiscordAbstraction.Initialize();

    DiscordActivityManager = DiscordAbstraction.GetActivityManager();
    DiscordActivity = DiscordAbstraction.GetActivity();

    DiscordActivity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

    SceneManager.sceneLoaded += OnSceneLoaded;
    SceneManager.sceneUnloaded += OnSceneUnloaded;

    ActivityUpdate();
  }


  private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
  {
    var currentScene = scene.name;
    if (currentScene is "MainMenu")
    {
      if (inGame) DiscordActivity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
      inGame = false;
    }
    else if (currentScene == "SampleSceneRelay")
    {
      if (!inGame)
      {
        DiscordActivity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();
      }
      inGame = true;
    }
    if (Planets.planets.ContainsKey(currentScene))
      currentPlanet = currentScene;
  }


  private void OnSceneUnloaded(Scene scene)
  {
    if (currentPlanet == scene.name)
      currentPlanet = null;
  }


  private void Update()
  {
    if (DiscordActivityManager == null) { return; }

    if (period > 2.0f)
    {
      try
      {
        DiscordAbstraction.discord.RunCallbacks();
        ActivityUpdate();
        period = 0;
      }
      catch (Exception e)
      {
        Plugin.logger.LogError($"Discord exception in Update: {e}");
      }
    }
    else
    {
      period += Time.deltaTime;
      return;
    }

  }


  public void ActivityUpdate()
  {
    // Plugin.logger.LogDebug("ActivityUpdate() called");
    // Plugin.logger.LogDebug($"inGame: {inGame}");

    if (inGame)
    {
      // in game - not in main menu

      DiscordActivity.State = PlaceholderResolver.ResolvePlaceholders(ConfigManager.ActivityState.Value);
      DiscordActivity.Details = PlaceholderResolver.ResolvePlaceholders(ConfigManager.ActivityDetails.Value);

      DiscordActivity.Assets.LargeText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.LargeText.Value);
      DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.LargeImage.Value);

      DiscordActivity.Assets.SmallText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.SmallText.Value);
      DiscordActivity.Assets.SmallImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.SmallImage.Value);

      if (ConfigManager.ActivityPlayers.Value)
      {
        DiscordActivity.Party.Size.CurrentSize = Variables.PartySize();
        DiscordActivity.Party.Size.MaxSize = Variables.PartyMaxSize();
      }

      DiscordActivity.Party.Id = Variables.PartyID();

      // allow for joining the lobby only when orbiting
      if (ConfigManager.AllowJoin.Value && Variables.IsShipInOrbit())
      {
        Plugin.logger.LogDebug($"steam://joinlobby/1966720/{Variables.PartyID()}/{Variables.PartyLeaderID()}");
        DiscordActivity.Secrets.Join = $"steam://joinlobby/1966720/${Variables.PartyID()}/${Variables.PartyLeaderID()}";
      }
      else
      {
        DiscordActivity.Secrets.Join = null;
      }
    }
    else
    {
      // main menu - reset activity
      DiscordActivity.State = "In Main Menu";
      DiscordActivity.Details = null;
      DiscordActivity.Assets.SmallText = null;
      DiscordActivity.Assets.SmallImage = null;
      DiscordActivity.Assets.LargeText = null;
      DiscordActivity.Assets.LargeImage = "mainmenu";
      DiscordActivity.Party.Id = null;
      DiscordActivity.Party.Size.CurrentSize = 0;
      DiscordActivity.Party.Size.MaxSize = 0;
      DiscordActivity.Secrets.Join = null;
    }


    try
    {
      DiscordActivityManager.UpdateActivity(DiscordActivity, result => { });
      Plugin.logger.LogDebug("DiscordActivityUpdate");
    }
    catch (Exception e)
    {
      Plugin.logger.LogError("ActivityUpdate:: " + e.Message);
      throw;
    }


  }


}