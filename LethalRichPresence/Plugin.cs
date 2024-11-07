using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace LethalRichPresence
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	[BepInDependency("MrovLib", BepInDependency.DependencyFlags.HardDependency)]
	[BepInDependency("BMX.LobbyCompatibility", BepInDependency.DependencyFlags.SoftDependency)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;
		internal static MrovLib.Logger debugLogger = new("LethalRichPresence", ConfigManager.Debug);

		private void Awake()
		{
			logger = Logger;

			ConfigManager.Init(Config);

			if (Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
			{
				LobbyCompatibilityCompatibility.Init();
			}

			Harmony harmony = new(PluginInfo.PLUGIN_GUID);
			harmony.PatchAll();

			GameObject discordGameObject = new();
			discordGameObject.AddComponent<Lifecycle>();
			DontDestroyOnLoad(discordGameObject);
			discordGameObject.hideFlags = HideFlags.HideAndDontSave;

			// Plugin startup logic
			logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
