using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace LethalRichPresence;

public static class PlaceholderResolver
{

  public static Dictionary<string, string> PlaceholderDictionary()
  {

    Dictionary<string, string> placeholders = new(){
      {"currentplanet", Variables.CurrentPlanet()},
      {"currentweather", Variables.CurrentWeather()},
      {"quota", Variables.Quota().ToString()},
      {"quotafullfilled", Variables.QuotaFulfilled().ToString()},
      {"quotacountifier", Variables.QuotaNoCountifier()},
      {"collected", Variables.LootValue().ToString()},
      {"timeleft", Variables.TimeRemaining().ToString()},
      {"onlineorlan", Variables.IsOnlineOrLAN().ToString()},
      {"hosting", Variables.IsHostingOrMember().ToString()},
      {"partyprivacy", Variables.PartyPrivacy().ToString()},

    };
    return placeholders;
  }

  public static string ResolvePlaceholders(string input, Dictionary<string, string> placeholders)
  {
    string output = input;

    // detect %string% placeholders and create switch to replace them
    Regex regex = new Regex(@"\%.+?\%");
    MatchCollection matches = regex.Matches(output);

    foreach (Match match in matches)
    {

      bool toLower = false;
      string placeholder = match.Value.Replace("%", "");

      // if there's & in front of the placeholder, make it lowercase and remove spaces
      if (placeholder.StartsWith("&"))
      {
        toLower = true;
        placeholder = placeholder.Replace("&", "");
      }

      // actual resolving placeholders
      if (placeholders.ContainsKey(placeholder))
      {
        output = output.Replace(match.Value, placeholders[placeholder]);

        if (toLower) output = output.ToLower().Replace(" ", "");
      }
    }

    if (ConfigManager.Debug.Value) Plugin.logger.LogDebug($"Resolved |{input}| to |{output}|");

    return output;
  }

}