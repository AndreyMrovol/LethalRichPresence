using System.Collections.Generic;
using System.Text.RegularExpressions;
using LethalRichPresence.Definitions;
using MrovLib;

namespace LethalRichPresence;

public static class PlaceholderResolver
{
	public static Dictionary<string, Placeholder> Placeholders = [];
	public static ResolverCache<string> Cache = new();

	public static string ResolvePlaceholders(string input)
	{
		string output = input;

		if (output == "")
			return null;

		// detect %string% placeholders and create switch to replace them
		Regex regex = new Regex(@"\%.+?\%");
		MatchCollection matches = regex.Matches(output);

		foreach (Match match in matches)
		{
			bool toLower = false;
			string placeholder = match.Value.Replace("%", "");

			// if there's & in front of the placeholder, make it lowercase and remove characters not accepted by Discord's Rich Presence Assets
			if (placeholder.StartsWith("&"))
			{
				toLower = true;
				placeholder = placeholder.Replace("&", "");
			}

			// actual resolving placeholders
			if (Placeholders.ContainsKey(placeholder))
			{
				string placeholderValue = Placeholders[placeholder].Value;

				output = output.Replace(match.Value, placeholderValue);

				Regex unwantedCharactersRegex = new Regex(@"\ |\(|\)");

				if (toLower)
				{
					output = unwantedCharactersRegex.Replace(output.ToLower(), "");
				}

				if (Cache.Get(placeholder) != placeholderValue)
				{
					Plugin.debugLogger.LogDebug($"Resolved |{match.Value}| into |{placeholderValue}|");
					Cache.Add(placeholder, placeholderValue);
				}
			}
		}

		return output;
	}
}
