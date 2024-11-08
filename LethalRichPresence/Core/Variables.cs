using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace LethalRichPresence;

public class Variables
{
	public static float LootValue()
	{
		return MrovLib.SharedMethods.GetShipObjects().Sum(shipObject => shipObject.scrapValue);
	}

	public static bool AmIHost()
	{
		return GameNetworkManager.Instance.isHostingGame;
	}

	public static bool IsShipInOrbit()
	{
		return StartOfRound.Instance.inShipPhase;
	}

	public static bool IsPartyPublic()
	{
		return GameNetworkManager.Instance.lobbyHostSettings?.isLobbyPublic != null
			? GameNetworkManager.Instance.lobbyHostSettings.isLobbyPublic
			: false;
	}

	public static bool IsPartyInviteOnly()
	{
		return GameNetworkManager.Instance.currentLobby?.GetData("inviteOnly") == "true";
	}

	public static bool IsFiringSequenceActive()
	{
		return StartOfRound.Instance.firingPlayersCutsceneRunning;
	}

	public static string PartyID()
	{
		return GameNetworkManager.Instance.currentLobby?.Id.ToString() ?? Lifecycle.partyID;
	}

	public static int PartySize()
	{
		return StartOfRound.Instance.livingPlayers;
	}

	public static int PartyMaxSize()
	{
		return Lifecycle.partyMaxSize;
	}

	public static string PartyLeader()
	{
		return GameNetworkManager.Instance.currentLobby?.Owner.Name;
	}

	public static string PartyLeaderID()
	{
		return GameNetworkManager.Instance.currentLobby?.Owner.Id.ToString() ?? "0";
	}

	public static int Quota()
	{
		return TimeOfDay.Instance.profitQuota;
	}

	public static int QuotaFulfilled()
	{
		return TimeOfDay.Instance.quotaFulfilled;
	}

	public static int QuotaNo()
	{
		return TimeOfDay.Instance.timesFulfilledQuota + 1;
	}

	public static string QuotaNoCountifier()
	{
		return ((QuotaNo() % 20) + Countifiers.countifiers[QuotaNo() % 20].ToString()).ToString();
	}

	public static int DaysRemaining()
	{
		return TimeOfDay.Instance.daysUntilDeadline;
	}

	public static string TimeRemaining()
	{
		var isMultiple = TimeOfDay.Instance.daysUntilDeadline != 1 ? "s" : "";
		return TimeOfDay.Instance.daysUntilDeadline + " day" + isMultiple;
	}

	public static string CurrentPlanet()
	{
		var currentPlanet = StartOfRound.Instance.currentLevel.PlanetName;

		// If planet name starts with digit (e.g. "71 Gordion"), replace space with dash (e.g. "71-Gordion") (backwards compatibility with stupid-ass dictionary system)
		var startsWithDigit = new Regex(@"^[0-9]{1}");
		if (startsWithDigit.IsMatch(currentPlanet))
		{
			currentPlanet = currentPlanet.Replace(" ", "-");
		}

		// If planet name starts with letter (e.g. "E Gypt", "springfield"), make first letter uppercase (e.g. "E Gypt", "Springfield")
		var startsWithLetter = new Regex(@"^[a-zA-Z]{1}");
		if (startsWithLetter.IsMatch(currentPlanet))
		{
			currentPlanet = currentPlanet.First().ToString().ToUpper() + currentPlanet.Substring(1);
		}

		return currentPlanet;
	}

	public static string CurrentWeather()
	{
		var currentWeather = StartOfRound.Instance.currentLevel.currentWeather.ToString();

		if (currentWeather == LevelWeatherType.None.ToString())
		{
			return "Clear weather";
		}

		return currentWeather;
	}
}
