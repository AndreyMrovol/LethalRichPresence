using BepInEx;
using BepInEx.Bootstrap;
using BepInEx.Logging;
using UnityEngine;

namespace LethalRichPresence
{
	[BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
	public class Plugin : BaseUnityPlugin
	{
		internal static ManualLogSource logger;

		private void Awake()
		{
			logger = Logger;

			ConfigManager.Init(Config);

			if (Chainloader.PluginInfos.ContainsKey("BMX.LobbyCompatibility"))
			{
				LobbyCompatibilityCompatibility.Init();
			}

			GameObject discordGameObject = new GameObject();
			discordGameObject.AddComponent<Lifecycle>();
			DontDestroyOnLoad(discordGameObject);
			discordGameObject.hideFlags = HideFlags.HideAndDontSave;

			// Plugin startup logic
			logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
		}
	}
}
