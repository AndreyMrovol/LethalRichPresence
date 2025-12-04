using BepInEx.Logging;
using MrovLib;

namespace LethalRichPresence
{
  public class Logger : MrovLib.Logger
  {
    public override ManualLogSource LogSource { get; set; }

    public Logger(string SourceName, LoggingType defaultLoggingType = LoggingType.Debug)
      : base(SourceName, defaultLoggingType)
    {
      ModName = SourceName;
      LogSource = BepInEx.Logging.Logger.CreateLogSource("WeatherRegistry");
      _name = SourceName;
    }

    public override bool ShouldLog(LoggingType type)
    {
      return ConfigManager.Debug.Value >= type;
    }
  }
}
