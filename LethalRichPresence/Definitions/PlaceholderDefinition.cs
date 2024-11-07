namespace LethalRichPresence.Definitions
{
	public abstract class PlaceholderDefinition(string name)
	{
		internal MrovLib.Logger Logger { get; } = Plugin.debugLogger;

		public string Name { get; } = name;
		public virtual string Value
		{
			get { return default; }
		}
	}

	public class Placeholder : PlaceholderDefinition
	{
		public Placeholder(string name)
			: base(name)
		{
			PlaceholderResolver.Placeholders.Add(name, this);
		}

		public virtual string ValueReference()
		{
			return "";
		}

		public override string Value
		{
			get
			{
				string returned = ValueReference();

				Logger.LogDebug($"Resolved |%{Name}%| into |{returned}|");
				return returned;
			}
		}
	}
}
