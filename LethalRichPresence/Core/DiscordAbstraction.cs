using System;
using Discord;

namespace LethalRichPresence
{
  public class DiscordAbstraction
  {
    public static Discord.Discord discord;
    public static ActivityManager activityManager;
    public static Activity activity;

    public static void Initialize()
    {
      CreateDiscord();
    }

    private static void CreateDiscord()
    {
      discord = new Discord.Discord(ConfigManager.AppID.Value, (ulong)CreateFlags.NoRequireDiscord);
      activityManager = discord.GetActivityManager();

      if (activityManager == null) { return; }

      // hacky workaround for linux users getting errors on initialization
      try
      {
        activityManager.RegisterSteam(1966720);
      }
      catch (Exception e)
      {
        Plugin.logger.LogError($"Error registering steam: {e}");
      }

      activity = new() { Instance = true };

      activityManager.OnActivityJoin += secret =>
      {
        Plugin.logger.LogMessage($"Joining lobby with {secret}");

        // secret is Steam Lobby ID
        Steamworks.SteamId lobbyID = ulong.Parse(secret.ToString());
        Steamworks.Data.Lobby SteamLobby = new Steamworks.Data.Lobby(lobbyID);

        try
        {
          GameNetworkManager.Instance.JoinLobby(SteamLobby, lobbyID);
        }
        catch (System.Exception e)
        {
          Plugin.logger.LogError(e);
        }


      };
    }

    public static void RestartDiscord()
    {
      // discord.Dispose();
      CreateDiscord();
    }

    // getters for activitymanager and activity
    public static ActivityManager GetActivityManager()
    {
      return activityManager;
    }

    public static Activity GetActivity()
    {
      return activity;
    }
  }
}