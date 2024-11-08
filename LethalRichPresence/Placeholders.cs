using System.Linq;
using LethalRichPresence.Definitions;

namespace LethalRichPresence
{
	public class CurrentPlanet : Placeholder
	{
		public CurrentPlanet()
			: base("currentplanet")
		{
		}

		public override string ValueReference()
		{
			return Variables.CurrentPlanet();
		}
	}

	public class CurrentWeather : Placeholder
	{
		public CurrentWeather()
			: base("currentweather") { }

		public override string ValueReference()
		{
			return Variables.CurrentWeather();
		}
	}

	public class Quota : Placeholder
	{
		public Quota()
			: base("quota") { }

		public override string ValueReference()
		{
			return TimeOfDay.Instance.profitQuota.ToString();
		}
	}

	public class QuotaFulfilled : Placeholder
	{
		public QuotaFulfilled()
			: base("quotafullfilled") { }

		public override string ValueReference()
		{
			return TimeOfDay.Instance.quotaFulfilled.ToString();
		}
	}

	public class QuotaCountifier : Placeholder
	{
		public QuotaCountifier()
			: base("quotacountifier") { }

		public override string ValueReference()
		{
			return (
				(TimeOfDay.Instance.timesFulfilledQuota + 1 % 20)
				+ Countifiers.countifiers[TimeOfDay.Instance.timesFulfilledQuota + 1 % 20].ToString()
			).ToString();
		}
	}

	public class Collected : Placeholder
	{
		public Collected()
			: base("collected") { }

		public override string ValueReference()
		{
			return MrovLib.SharedMethods.GetShipObjects().Sum(shipObject => shipObject.scrapValue).ToString();
		}
	}

	public class TimeLeft : Placeholder
	{
		public TimeLeft()
			: base("timeleft") { }

		public override string ValueReference()
		{
			return $"{TimeOfDay.Instance.daysUntilDeadline} day{(TimeOfDay.Instance.daysUntilDeadline != 1 ? "s" : "")}";
		}
	}

	public class OnlineOrLAN : Placeholder
	{
		public OnlineOrLAN()
			: base("onlineorlan") { }

		public override string ValueReference()
		{
			return GameNetworkManager.Instance.disableSteam ? "on LAN" : "online";
		}
	}

	public class Hosting : Placeholder
	{
		public Hosting()
			: base("hosting") { }

		public override string ValueReference()
		{
			return GameNetworkManager.Instance.isHostingGame ? "hosting" : "member";
			;
		}
	}

	public class PartyPrivacy : Placeholder
	{
		public PartyPrivacy()
			: base("partyprivacy") { }

		public override string ValueReference()
		{
			return Variables.IsPartyPublic() ? "public" : "private";
		}
	}

	public static class Placeholders
	{
		public static CurrentPlanet CurrentPlanet = new();
		public static CurrentWeather CurrentWeather = new();
		public static Quota Quota = new();
		public static QuotaFulfilled QuotaFulfilled = new();
		public static QuotaCountifier QuotaCountifier = new();
		public static Collected Collected = new();
		public static TimeLeft TimeLeft = new();
		public static OnlineOrLAN OnlineOrLAN = new();
		public static Hosting Hosting = new();
		public static PartyPrivacy PartyPrivacy = new();

		public static void Init() { }
	}
}
