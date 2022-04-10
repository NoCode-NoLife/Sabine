using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Sabine.Shared.Configuration;
using Sabine.Shared.Database;
using Sabine.Shared.Util;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Shared
{
	/// <summary>
	/// Base class for servers.
	/// </summary>
	public abstract class Server
	{
		/// <summary>
		/// Returns a reference to all conf files.
		/// </summary>
		public ConfFiles Conf { get; private set; } = new ConfFiles();

		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="args"></param>
		public abstract void Run(string[] args);

		/// <summary>
		/// Changes current directory to the project's root folder.
		/// </summary>
		protected void NavigateToRoot()
		{
			var folderNames = new[] { "bin", "libs" };
			var tries = 3;

			var cwd = Directory.GetCurrentDirectory();
			for (var i = 0; i < tries; ++i)
			{
				if (folderNames.All(a => Directory.Exists(a)))
					return;

				Directory.SetCurrentDirectory("../");
			}

			throw new DirectoryNotFoundException("Couldn't navigate to root. (Not found: " + string.Join(", ", folderNames) + ")");
		}

		/// <summary>
		/// Loads all configuration files.
		/// </summary>
		/// <returns></returns>
		protected ConfFiles LoadConf()
		{
			Log.Info("Loading configuration...");

			this.Conf.Load();
			return this.Conf;
		}

		/// <summary>
		/// Loads localization files and updates cultural settings.
		/// </summary>
		/// <returns></returns>
		protected void LoadLocalization(ConfFiles conf)
		{
			CultureInfo.DefaultThreadCurrentCulture = CultureInfo.GetCultureInfo(conf.Localization.Culture);
			CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.GetCultureInfo(conf.Localization.CultureUi);
			Thread.CurrentThread.CurrentCulture = CultureInfo.DefaultThreadCurrentCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.DefaultThreadCurrentUICulture;

			var language = conf.Localization.Language;
			var path = Path.Combine("localization", language + ".po");

			Log.Info("Loading localization ({0})...", language);

			// Try user first
			try
			{
				var userPath = Path.Combine("user", path);
				Localization.Load(userPath);
			}
			catch (FileNotFoundException)
			{
				// Try system second, if the file wasn't in user
				try
				{
					var systemPath = Path.Combine("system", path);
					Localization.Load(systemPath);
				}
				catch (FileNotFoundException)
				{
					// Warn if language wasn't the default
					if (language != "en-US")
						Log.Warning("Localization file '{0}.po' not found.", language);
				}
			}
		}

		/// <summary>
		/// Initializes database connection.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="conf"></param>
		protected void InitDatabase(Db db, ConfFiles conf)
		{
			Log.Info("Initializing database...");

			try
			{
				db.Init(conf.Database.Host, conf.Database.Port, conf.Database.Username, conf.Database.Password, conf.Database.Name);
			}
			catch (Exception ex)
			{
				Log.Error("Unable to open database connection. ({0})", ex.Message);
				ConsoleUtil.Exit(1);
			}
		}
	}
}
