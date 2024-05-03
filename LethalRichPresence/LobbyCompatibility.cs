using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using LethalRichPresence;
using LobbyCompatibility;
using UnityEngine;

namespace LethalRichPresence
{
    internal class LobbyCompatibilityCompatibility
    {
        public static void Init()
        {
            Plugin.logger.LogWarning(
                "LobbyCompatibility detected, registering plugin with LobbyCompatibility."
            );

            Version pluginVersion = Version.Parse(LethalRichPresence.PluginInfo.PLUGIN_VERSION);

            LobbyCompatibility.Features.PluginHelper.RegisterPlugin(
                "LethalRichPresence",
                pluginVersion,
                LobbyCompatibility.Enums.CompatibilityLevel.ClientOptional,
                LobbyCompatibility.Enums.VersionStrictness.None
            );
        }
    }
}
