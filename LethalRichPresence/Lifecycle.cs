using System;
using System.Collections.Generic;
using Discord;
using Steamworks;
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

  public static int partyMaxSize = 0;

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

    SteamMatchmaking.OnLobbyEntered += (lobby) =>
    {
      partyMaxSize = lobby.MaxMembers;
    };
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

      if (ConfigManager.Debug.Value)
      {
      }

      Dictionary<string, string> placeholderDictionary = PlaceholderResolver.PlaceholderDictionary();

      if (Variables.IsShipInOrbit())
      {
        DiscordActivity.Assets.LargeText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.OrbitLargeText.Value, placeholderDictionary);
        DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.OrbitLargeImage.Value, placeholderDictionary);

        DiscordActivity.Assets.SmallText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.OrbitSmallText.Value, placeholderDictionary);
        DiscordActivity.Assets.SmallImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.OrbitSmallImage.Value, placeholderDictionary);
      }
      else
      {
        DiscordActivity.Assets.LargeText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.PlanetLargeText.Value, placeholderDictionary);
        DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.PlanetLargeImage.Value, placeholderDictionary);

        DiscordActivity.Assets.SmallText = PlaceholderResolver.ResolvePlaceholders(ConfigManager.PlanetSmallText.Value, placeholderDictionary);
        DiscordActivity.Assets.SmallImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.PlanetSmallImage.Value, placeholderDictionary);
      }

      DiscordActivity.Details = PlaceholderResolver.ResolvePlaceholders(ConfigManager.ActivityDetails.Value, placeholderDictionary);
      DiscordActivity.State = PlaceholderResolver.ResolvePlaceholders(ConfigManager.ActivityState.Value, placeholderDictionary);

      if (Variables.IsFiringSequenceActive())
      {
        DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(ConfigManager.FiredSequenceLargeImage.Value, placeholderDictionary);
        DiscordActivity.Assets.LargeText = "Getting fired";

        DiscordActivity.Details = "Getting fired";
        DiscordActivity.State = $"Did not meet {Variables.QuotaNoCountifier()} quota ({Variables.Quota()})";

        DiscordActivity.Party.Size.CurrentSize = 0;
        DiscordActivity.Party.Size.MaxSize = 0;
      }

      if (ConfigManager.ActivityPlayers.Value && Variables.IsShipInOrbit() && !Variables.IsFiringSequenceActive())
      {
        DiscordActivity.Party.Size.CurrentSize = Variables.PartySize();
        DiscordActivity.Party.Size.MaxSize = Variables.PartyMaxSize();
      }

      if (ConfigManager.DisplayLivingPlayers.Value)
      {
        DiscordActivity.Party.Size.CurrentSize = Variables.PartySize();
      }

      DiscordActivity.Party.Id = Variables.PartyID();

      string joinSecret;

      // allow for joining the lobby only when orbiting
      if (ConfigManager.AllowJoin.Value && Variables.IsShipInOrbit())
      {
        joinSecret = $"steam://joinlobby/1966720/${Variables.PartyID()}/${Variables.PartyLeaderID()}";

        if (Variables.IsPartyInviteOnly() || (!Variables.IsPartyPublic() && ConfigManager.JoinOnlyInPublicLobby.Value))
        {
          joinSecret = null;
        }

        // Plugin.logger.LogDebug(DiscordActivity.Secrets.Join);
      }
      else
      {
        joinSecret = null;
      }

      DiscordActivity.Secrets.Join = joinSecret;
    }
    else
    {
      // main menu - reset activity
      DiscordActivity.State = "In Main Menu";
      DiscordActivity.Details = null;
      DiscordActivity.Assets.SmallText = null;
      DiscordActivity.Assets.SmallImage = null;
      DiscordActivity.Assets.LargeText = null;
      DiscordActivity.Assets.LargeImage = ConfigManager.MainMenuLargeImage.Value;
      DiscordActivity.Party.Id = null;
      DiscordActivity.Party.Size.CurrentSize = 0;
      DiscordActivity.Party.Size.MaxSize = 0;
      DiscordActivity.Secrets.Join = null;
    }


    try
    {
      DiscordActivityManager.UpdateActivity(DiscordActivity, result => { });
      if (ConfigManager.Debug.Value) Plugin.logger.LogDebug("DiscordActivityUpdate");
    }
    catch (Exception e)
    {
      Plugin.logger.LogError("ActivityUpdate:: " + e.Message);
      throw;
    }


  }


}
