// create abstraction layer between discord (in Discord namespace) and the plugin

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
      discord = new Discord.Discord(ConfigManager.AppID.Value, (ulong)CreateFlags.NoRequireDiscord);
      activityManager = discord.GetActivityManager();

      if (activityManager == null) { return; }

      activityManager.RegisterSteam(1966720);

      activity = new() { Instance = true };
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