using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Microsoft.CodeDom.Providers.DotNetCompilerPlatform;
using Sabine.Shared.Configuration;
using Sabine.Shared.Data;
using Sabine.Shared.Database;
using Sabine.Shared.L10N;
using Sabine.Shared.Util;
using Shared.Scripting;
using Yggdrasil.Data;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Scripting;
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
		/// Returns a reference to the server's script loader.
		/// </summary>
		protected ScriptLoader ScriptLoader { get; private set; }

		/// <summary>
		/// Returns a reference to the server's string localizer manager.
		/// </summary>
		public MultiLocalizer Localization { get; } = new MultiLocalizer();

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

			var serverLanguage = conf.Localization.Language;
			var relativeFolderPath = "localization";
			var systemFolderPath = Path.Combine("system", relativeFolderPath);
			var userFolderPath = Path.Combine("system", relativeFolderPath);

			Log.Info("Loading localization...");

			// Load everything from user first, then check system, without
			// overriding the ones loaded from user
			foreach (var filePath in Directory.EnumerateFiles(userFolderPath, "*.po", SearchOption.AllDirectories))
			{
				var languageName = Path.GetFileNameWithoutExtension(filePath);
				this.Localization.Load(languageName, filePath);

				Log.Info("  loaded {0}.", languageName);
			}

			foreach (var filePath in Directory.EnumerateFiles(systemFolderPath, "*.po", SearchOption.AllDirectories))
			{
				var languageName = Path.GetFileNameWithoutExtension(filePath);
				if (this.Localization.Contains(languageName))
					continue;

				this.Localization.Load(languageName, filePath);

				Log.Info("  loaded {0}.", languageName);
			}


			Log.Info("  setting default language to {0}.", serverLanguage);

			// Try to set the default localizer, and warn the user about
			// missing localizers if the selected server language isn't
			// US english.
			if (!this.Localization.Contains(serverLanguage))
			{
				if (serverLanguage != "en-US")
					Log.Warning("Localization file '{0}.po' not found.", serverLanguage);
			}
			else
			{
				this.Localization.SetDefault(serverLanguage);
			}

			Sabine.Shared.Util.Localization.SetLocalizer(this.Localization.GetDefault());
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

		/// <summary>
		/// Loads data from JSON files.
		/// </summary>
		public void LoadData()
		{
			Log.Info("Loading data...");

			this.LoadDataFile(SabineData.Items, "items.txt");
			this.LoadDataFile(SabineData.Maps, "maps.txt");
			this.LoadDataFile(SabineData.MapCache, "map_cache.dat");
			this.LoadDataFile(SabineData.Monsters, "monsters.txt");
		}

		/// <summary>
		/// Loads files for the given database from the given file.
		/// </summary>
		/// <param name="db"></param>
		/// <param name="fileName"></param>
		private void LoadDataFile(IDatabase db, string fileName)
		{
			try
			{
				var systemPath = Path.Combine("system", "data", fileName);
				var userPath = Path.Combine("user", "data", fileName);

				if (!File.Exists(systemPath))
				{
					Log.Error("LoadDataFile: File '{0}' not found.", systemPath);
					ConsoleUtil.Exit(1);
					return;
				}

				db.Clear();
				db.LoadFile(systemPath);
				foreach (var ex in db.GetWarnings())
					Log.Warning(ex);

				if (File.Exists(userPath))
				{
					db.LoadFile(systemPath);
					foreach (var ex in db.GetWarnings())
						Log.Warning(ex);
				}

				if (db.Count == 1)
					Log.Info("  done loading {0} entry from {1}.", db.Count, fileName);
				else
					Log.Info("  done loading {0} entries from {1}.", db.Count, fileName);
			}
			catch (DatabaseErrorException ex)
			{
				Log.Error(ex);
				ConsoleUtil.Exit(1);
			}
		}

		/// <summary>
		/// Loads all scripts from given list.
		/// </summary>
		public void LoadScripts(string listFilePath, ConfFiles conf)
		{
			if (this.ScriptLoader != null)
			{
				Log.Error("The script loader has been created already.");
				return;
			}

			Log.Info("Loading scripts...");

			if (!File.Exists(listFilePath))
			{
				Log.Error("Script list not found: " + listFilePath);
				return;
			}

			var timer = Stopwatch.StartNew();

			try
			{
				var provider = new Microsoft.CodeDom.Providers.DotNetCompilerPlatform.CSharpCodeProvider();
				provider.SetCompilerServerTimeToLive(TimeSpan.FromMinutes(20));
				provider.SetCompilerFullPath(Path.GetFullPath("libs/roslyn/csc.exe"));

				var cachePath = (string)null;
				//if (conf.Scripting.EnableCaching)
				//{
				//	var fileName = Path.GetFileNameWithoutExtension(listFilePath);
				//	cachePath = string.Format("cache/scripts/{0}.compiled", fileName);
				//}

				this.ScriptLoader = new ScriptLoader(provider, cachePath);
				//this.ScriptLoader.AddPrecompiler(new AiScriptPrecompiler());
				this.ScriptLoader.LoadFromListFile(listFilePath, "user/scripts/");

				foreach (var ex in this.ScriptLoader.LoadingExceptions)
					Log.Error(ex);
			}
			catch (CompilerErrorException ex)
			{
				this.DisplayScriptErrors(ex);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			Log.Info("  loaded {0} scripts from {3} files in {2:n2}s ({1} init fails).", this.ScriptLoader.LoadedCount, this.ScriptLoader.FailCount, timer.Elapsed.TotalSeconds, this.ScriptLoader.FileCount);
		}

		/// <summary>
		/// Reloads previously loaded scripts.
		/// </summary>
		public void ReloadScripts()
		{
			Log.Info("Reloading scripts...");

			var timer = Stopwatch.StartNew();

			try
			{
				this.ScriptLoader.Reload();
			}
			catch (CompilerErrorException ex)
			{
				this.DisplayScriptErrors(ex);
			}
			catch (Exception ex)
			{
				Log.Error(ex);
			}

			Log.Info("  reloaded {0} scripts from {3} files in {2:n2}s ({1} init fails).", this.ScriptLoader.LoadedCount, this.ScriptLoader.FailCount, timer.Elapsed.TotalSeconds, this.ScriptLoader.FileCount);
		}

		/// <summary>
		/// Displays the script errors in the console.
		/// </summary>
		/// <param name="ex"></param>
		private void DisplayScriptErrors(CompilerErrorException ex)
		{
			foreach (System.CodeDom.Compiler.CompilerError err in ex.Errors)
			{
				if (string.IsNullOrWhiteSpace(err.FileName))
				{
					Log.Error("While loading scripts: " + err.ErrorText);
				}
				else
				{
					var relativefileName = err.FileName;
					var cwd = Directory.GetCurrentDirectory();
					if (relativefileName.ToLower().StartsWith(cwd.ToLower()))
						relativefileName = relativefileName.Substring(cwd.Length + 1);

					var lines = File.ReadAllLines(err.FileName);
					var sb = new StringBuilder();

					// Error msg
					sb.AppendLine("In {0} on line {1}, column {2}", relativefileName, err.Line, err.Column);
					sb.AppendLine("          {0}", err.ErrorText);

					// Display lines around the error
					var startLine = Math.Max(1, err.Line - 1);
					var endLine = Math.Min(lines.Length, startLine + 2);
					for (var i = startLine; i <= endLine; ++i)
					{
						// Make sure we don't get out of range.
						// (ReadAllLines "trims" the input)
						var line = (i <= lines.Length) ? lines[i - 1] : "";

						sb.AppendLine("  {2} {0:0000}: {1}", i, line, (err.Line == i ? '*' : ' '));
					}

					if (err.IsWarning)
						Log.Warning(sb.ToString());
					else
						Log.Error(sb.ToString());
				}
			}
		}
	}
}
