using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Network;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;
using Yggdrasil.Util.Commands;

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
			this.Clear();

			// Normal commands
			this.Add("help", "[commandName]", Localization.Get("Displays a list of usable commands or details about one command."), this.Help);
			this.Add("where", "", Localization.Get("Displays current location."), this.Where);

			// GM commands
			this.Add("broadcast", "<message>", Localization.Get("Broadcasts message to everyone on the server."), this.Broadcast);
			this.Add("warp", "<map> <x> <y>", Localization.Get("Warps player to destination."), this.Warp);

			// Dev commands
			this.Add("test", "", Localization.Get("Behaviour undefined."), this.Test);
			this.Add("sprite", "<Class|Hair> <value>", Localization.Get("Changes the target's sprite."), this.Sprite);
			this.Add("reloadscripts", "", Localization.Get("Reloads all scripts."), this.ReloadScripts);
			//this.Add("reloadconf", "", Localization.Get("Reloads server configuration."), this.ReloadConf);
			//this.Add("reloaddata", "", Localization.Get("Reloads data files."), this.ReloadData);

			// Aliases
			this.AddAlias("reloadscripts", "rs");
			//this.AddAlias("item", "drop");
			this.AddAlias("broadcast", "bc");
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
			var mapStringId = target.Map.StringId;
			var x = target.Position.X;
			var y = target.Position.Y;

			if (sender == target)
				sender.ServerMessage(Localization.Get("You're here: {1}, {2:n0}, {3:n0}"), target.Name, mapStringId, x, y);
			else
				sender.ServerMessage(Localization.Get("{0} is here: {1}, {2:n0}, {3:n0}"), target.Name, mapStringId, x, y);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Broadcasts message to everyone on the server.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Broadcast(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			// Use either the first argument, which can be a long, quoted
			// string, or join all arguments with spaces.
			// TODO: Maybe add a way to get only the arguments part as
			//   one string for such cases?
			var msg = args.Count == 1 ? args.Get(0) : string.Join(" ", args.GetAll());
			var text = string.Format("{0} : {1}", sender.Name, msg);

			Send.ZC_BROADCAST(text);
			sender.ServerMessage(Localization.Get("Message was broadcasted."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Changes player's sprite.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Sprite(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 2)
				return CommandResult.InvalidArgument;

			if (!Enum.TryParse<SpriteType>(args.Get(0), out var type))
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(1), out var value))
				return CommandResult.InvalidArgument;

			Send.ZC_SPRITE_CHANGE(target, type, value);

			sender.ServerMessage(Localization.Get("Changed {0} to {1}."), type, value);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps player to destination.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		/// <exception cref="NotImplementedException"></exception>
		private CommandResult Warp(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 3)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var mapId))
			{
				var mapStringId = args.Get(0);

				if (!SabineData.Maps.TryFind(mapStringId, out var mapData))
				{
					sender.ServerMessage(Localization.Get("Map '{0}' not found."), mapStringId);
					return CommandResult.Okay;
				}

				mapId = mapData.Id;
			}

			if (!int.TryParse(args.Get(1), out var x))
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(2), out var y))
				return CommandResult.InvalidArgument;

			if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var map))
			{
				sender.ServerMessage(Localization.Get("Map not found."));
				return CommandResult.Okay;
			}

			target.Warp(map.Id, new Position(x, y));

			sender.ServerMessage(Localization.Get("Warped to {0}, {1}, {2}."), map.StringId, x, y);
			if (sender != target)
				target.ServerMessage(Localization.Get("You were warped by {0}."), sender.Name);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads all scripts.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult ReloadScripts(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			ZoneServer.Instance.World.RemoveScriptedEntities();
			ZoneServer.Instance.ReloadScripts();

			return CommandResult.Okay;
		}
	}
}
