using System.Collections.Generic;
using System.Text.RegularExpressions;
using LethalRichPresence.Definitions;

namespace LethalRichPresence;

public static class PlaceholderResolver
{
	public static Dictionary<string, Placeholder> Placeholders = [];

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
				output = output.Replace(match.Value, Placeholders[placeholder].Value);

				Regex unwantedCharactersRegex = new Regex(@"\ |\(|\)");

				if (toLower)
					output = unwantedCharactersRegex.Replace(output.ToLower(), "");
			}
		}

		return output;
	}
}
