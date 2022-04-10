using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using Sabine.Zone.Network;
using Sabine.Shared.Util;
using Yggdrasil.Extensions;
using Yggdrasil.Logging;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;
using Sabine.Zone.World.Entities;
using System.Threading.Tasks;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;

namespace Sabine.Zone.Commands
{
	/// <summary>
	/// The chat command manager. It holds the commands and executes.
	/// </summary>
	public partial class ChatCommands : CommandManager<ChatCommand, CommandFunc>
	{
		/// <summary>
		/// Sets up commands.
		/// </summary>
		public void Load()
		{
			_commands.Clear();

			// Normal commands
			this.Add("help", "[commandName]", Localization.Get("Displays a list of usable commands or details about one command."), this.Help);
			this.Add("where", "", Localization.Get("Displays current location."), this.Where);

			// GM commands
			// ...

			// Dev commands
			this.Add("test", "", Localization.Get("Behaviour undefined."), this.Test);
			//this.Add("reloadscripts", "", Localization.Get("Reloads all scripts."), this.ReloadScripts);
			//this.Add("reloadconf", "", Localization.Get("Reloads server configuration."), this.ReloadConf);
			//this.Add("reloaddata", "", Localization.Get("Reloads data files."), this.ReloadData);

			// Aliases
			//this.AddAlias("reloadscripts", "rs");
			//this.AddAlias("item", "drop");
		}

		/// <summary>
		/// Test command that may be used to test code. Don't ever commit
		/// changes to this command!
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Test(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			Log.Debug("Hello, test!");

			return CommandResult.Okay;
		}

		/// <summary>
		/// Displays a list of usable commands or details about one command.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Help(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			var targetAuthLevel = target.Connection.Account.Authority;

			// Display info about one command
			if (args.Count != 0)
			{
				var helpCommandName = args.Get(0);
				var command = this.GetCommand(helpCommandName);

				var levels = ZoneServer.Instance.Conf.Commands.GetLevels(command.Name) ?? ZoneServer.Instance.Conf.Commands.GetLevels("default");

				if (levels.Self > targetAuthLevel)
				{
					sender.ServerMessage(Localization.Get("Command not found or not available."));
					return CommandResult.Okay;
				}

				var aliases = _commands.Where(a => a.Value == command && a.Key != helpCommandName).Select(a => a.Key);

				sender.ServerMessage(Localization.Get("Name: {0}"), command.Name);
				if (aliases.Any())
					sender.ServerMessage(Localization.Get("Aliases: {0}"), string.Join(", ", aliases));
				sender.ServerMessage(Localization.Get("Description: {0}"), command.Description);
				sender.ServerMessage(Localization.Get("Arguments: {0}"), command.Usage);
			}
			// Display list of available commands
			else
			{
				var commandNames = new List<string>();

				foreach (var command in _commands.Values)
				{
					var levels = ZoneServer.Instance.Conf.Commands.GetLevels(command.Name) ?? ZoneServer.Instance.Conf.Commands.GetLevels("default");
					if (levels == null || levels.Self > targetAuthLevel)
						continue;

					commandNames.Add(command.Name);
				}

				if (commandNames.Count == 0)
				{
					sender.ServerMessage(Localization.Get("No commands found."));
					return CommandResult.Okay;
				}

				var sb = new StringBuilder();

				sender.ServerMessage(Localization.Get("Available commands:"));
				foreach (var name in commandNames)
				{
					// Group command names in strings up to 100 characters,
					// as that's the maximum amount some clients will display
					// as one message.
					if (sb.Length + 2 + name.Length >= 100)
					{
						sender.ServerMessage(sb.ToString());
						sb.Clear();
					}

					if (sb.Length != 0)
						sb.Append(", ");

					sb.Append(name);
				}

				if (sb.Length != 0)
					sender.ServerMessage(sb.ToString());
			}

			return CommandResult.Okay;
		}

		/// <summary>
		/// Display target's current location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Where(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			var mapName = target.MapName;
			var x = target.Position.X;
			var y = target.Position.Y;

			if (sender == target)
				sender.ServerMessage(Localization.Get("You're here: {1}, {2:n0}, {3:n0}"), target.Name, mapName, x, y);
			else
				sender.ServerMessage(Localization.Get("{0} is here: {1}, {2:n0}, {3:n0}"), target.Name, mapName, x, y);

			return CommandResult.Okay;
		}
	}
}
