using Sabine.Shared.Configuration.Files;

namespace Sabine.Shared.Configuration
{
	/// <summary>
	/// Holds references to all conf files.
	/// </summary>
	public class ConfFiles
	{
		/// <summary>
		/// auth.conf
		/// </summary>
		public AuthConf Auth { get; } = new();

		/// <summary>
		/// char.conf
		/// </summary>
		public CharConf Char { get; } = new();

		/// <summary>
		/// zone.conf
		/// </summary>
		public ZoneConf Zone { get; } = new();

		/// <summary>
		/// commands.conf
		/// </summary>
		public CommandsConf Commands { get; } = new();

		/// <summary>
		/// database.conf
		/// </summary>
		public DatabaseConf Database { get; } = new();

		/// <summary>
		/// localization.conf
		/// </summary>
		public LocalizationConf Localization { get; } = new();

		/// <summary>
		/// version.conf
		/// </summary>
		public VersionConf Version { get; } = new();

		/// <summary>
		/// world.conf
		/// </summary>
		public WorldConf World { get; } = new();

		/// <summary>
		/// Loads all conf files.
		/// </summary>
		public void Load()
		{
			this.Auth.Load("system/conf/auth.conf");
			this.Char.Load("system/conf/char.conf");
			this.Zone.Load("system/conf/zone.conf");

			this.Commands.Load("system/conf/commands.conf");
			this.Database.Load("system/conf/database.conf");
			this.Localization.Load("system/conf/localization.conf");
			this.Version.Load("system/conf/version.conf");
			this.World.Load("system/conf/world.conf");
		}
	}
}
