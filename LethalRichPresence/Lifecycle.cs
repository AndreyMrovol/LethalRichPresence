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

    public float period = 0.0f;
    public float desiredPeriod = 1.0f;

    public static int partyMaxSize = 0;
    public static string partyID = "";

    private void Start()
    {
        // Plugin startup logic
        Plugin.logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");

        DiscordAbstraction.Initialize();

        DiscordActivityManager = DiscordAbstraction.GetActivityManager();
        DiscordActivity = DiscordAbstraction.GetActivity();

        DiscordActivity.Timestamps.Start = ((DateTimeOffset)DateTime.Now).ToUnixTimeSeconds();

        SceneManager.sceneLoaded += OnSceneLoaded;

        ActivityUpdate();

        SteamMatchmaking.OnLobbyEntered += (lobby) =>
        {
            partyMaxSize = lobby.MaxMembers;
            partyID = lobby.Id.ToString();
        };
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var currentScene = scene.name;
        if (currentScene is "MainMenu")
        {
            if (inGame)
                DiscordActivity.Timestamps.Start = (
                    (DateTimeOffset)DateTime.Now
                ).ToUnixTimeSeconds();
            inGame = false;
        }
        else if (currentScene == "SampleSceneRelay")
        {
            if (!inGame)
            {
                DiscordActivity.Timestamps.Start = (
                    (DateTimeOffset)DateTime.Now
                ).ToUnixTimeSeconds();
            }
            inGame = true;
        }
    }

    private void Update()
    {
        if (DiscordActivityManager == null)
        {
            return;
        }

        if (period > desiredPeriod)
        {
            try
            {
                try
                {
                    DiscordAbstraction.discord.RunCallbacks();
                }
                catch (Exception ein)
                {
                    Plugin.logger.LogError(
                        $"Discord exception in Update: {ein.Message}, retrying in 10 seconds"
                    );
                    if (ConfigManager.Debug.Value)
                        Plugin.logger.LogError(ein);

                    desiredPeriod = 10.0f;

                    DiscordAbstraction.RestartDiscord();

                    DiscordActivityManager = DiscordAbstraction.GetActivityManager();
                    DiscordActivity = DiscordAbstraction.GetActivity();

                    throw ein;
                }

                ActivityUpdate();
                period = 0;
            }
            catch (Exception e)
            {
                Plugin.logger.LogError($"Update() error: {e.Message}");
                period = 0;
                desiredPeriod = 10.0f;
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

            if (ConfigManager.Debug.Value) { }

            Dictionary<string, string> placeholderDictionary =
                PlaceholderResolver.PlaceholderDictionary();

            if (Variables.IsShipInOrbit())
            {
                DiscordActivity.Assets.LargeText = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.OrbitLargeText.Value,
                    placeholderDictionary
                );
                DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.OrbitLargeImage.Value,
                    placeholderDictionary
                );

                DiscordActivity.Assets.SmallText = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.OrbitSmallText.Value,
                    placeholderDictionary
                );
                DiscordActivity.Assets.SmallImage = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.OrbitSmallImage.Value,
                    placeholderDictionary
                );
            }
            else
            {
                DiscordActivity.Assets.LargeText = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.PlanetLargeText.Value,
                    placeholderDictionary
                );
                DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.PlanetLargeImage.Value,
                    placeholderDictionary
                );

                DiscordActivity.Assets.SmallText = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.PlanetSmallText.Value,
                    placeholderDictionary
                );
                DiscordActivity.Assets.SmallImage = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.PlanetSmallImage.Value,
                    placeholderDictionary
                );
            }

            DiscordActivity.Details = PlaceholderResolver.ResolvePlaceholders(
                ConfigManager.ActivityDetails.Value,
                placeholderDictionary
            );
            DiscordActivity.State = PlaceholderResolver.ResolvePlaceholders(
                ConfigManager.ActivityState.Value,
                placeholderDictionary
            );

            if (Variables.IsFiringSequenceActive())
            {
                DiscordActivity.Assets.LargeImage = PlaceholderResolver.ResolvePlaceholders(
                    ConfigManager.FiredSequenceLargeImage.Value,
                    placeholderDictionary
                );
                DiscordActivity.Assets.LargeText = "Getting fired";

                DiscordActivity.Details = "Getting fired";
                DiscordActivity.State =
                    $"Did not meet {Variables.QuotaNoCountifier()} quota ({Variables.Quota()})";

                DiscordActivity.Assets.SmallImage = null;

                DiscordActivity.Party.Size.CurrentSize = 0;
                DiscordActivity.Party.Size.MaxSize = 0;
            }

            if (
                ConfigManager.ActivityPlayers.Value
                && Variables.IsShipInOrbit()
                && !Variables.IsFiringSequenceActive()
            )
            {
                DiscordActivity.Party.Size.CurrentSize = Variables.PartySize();
                DiscordActivity.Party.Size.MaxSize = Variables.PartyMaxSize();

                if (Variables.PartyMaxSize() == 0)
                {
                    DiscordActivity.Party.Size.CurrentSize = 0;
                    DiscordActivity.Party.Size.MaxSize = 0;
                }
            }

            if (ConfigManager.DisplayLivingPlayers.Value)
            {
                DiscordActivity.Party.Size.CurrentSize = Variables.PartySize();
            }

            if (Variables.PartyLeaderID() != "0")
                DiscordActivity.Party.Id = Variables.PartyLeaderID();

            string joinSecret;

            // allow for joining the lobby only when orbiting
            if (ConfigManager.AllowJoin.Value && Variables.IsShipInOrbit())
            {
                joinSecret = Variables.PartyID();

                if (Variables.PartyLeaderID() == "0" || Variables.PartyLeaderID() == "")
                {
                    joinSecret = null;
                }

                if (Variables.PartyID() == "0" || Variables.PartyID() == "")
                {
                    joinSecret = null;
                }

                if (
                    Variables.IsPartyInviteOnly()
                    || (!Variables.IsPartyPublic() && ConfigManager.JoinOnlyInPublicLobby.Value)
                )
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
            if (ConfigManager.Debug.Value)
                Plugin.logger.LogDebug("DiscordActivityUpdate");

            if (desiredPeriod != 1.0f)
                desiredPeriod = 1.0f;
        }
        catch (Exception e)
        {
            Plugin.logger.LogError("ActivityUpdate:: " + e.Message);
            throw;
        }
    }
}
