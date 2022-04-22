using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Xsl;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Network;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;
using Yggdrasil.Util;
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
			this.Add("language", "<language>", Localization.Get("Changes the localization language."), this.Language);

			// GM commands
			this.Add("broadcast", "<message>", Localization.Get("Broadcasts message to everyone on the server."), this.Broadcast);
			this.Add("warp", "<map> <x> <y>", Localization.Get("Warps player to destination."), this.Warp);
			this.Add("jump", "[[+-]x] [[+-]y]", Localization.Get("Warps player to another position on the same map."), this.Jump);
			this.Add("spawn", "<monster id|name>", Localization.Get("Spawns monsters."), this.Spawn);
			this.Add("stat", "<str|agi|vit|int|dex|luck|stp|skp> <modifier>", Localization.Get("Modifies the character's stats."), this.Stat);
			this.Add("item", "<item> [amount]", Localization.Get("Spawns item for character."), this.Item);
			this.Add("job", "<job>", Localization.Get("Changes character's job."), this.Job);
			this.Add("heal", "", Localization.Get("Restores character's health."), this.Heal);

			// Dev commands
			this.Add("test", "", Localization.Get("Behaviour undefined."), this.Test);
			this.Add("sprite", "<Class|Hair> <value>", Localization.Get("Changes the target's sprite."), this.Sprite);
			this.Add("reloadscripts", "", Localization.Get("Reloads all scripts."), this.ReloadScripts);
			this.Add("reloadconf", "", Localization.Get("Reloads server configuration."), this.ReloadConf);
			this.Add("reloaddata", "", Localization.Get("Reloads data files."), this.ReloadData);
			this.Add("debugpath", "", Localization.Get("Toggles path debugging on and off."), this.DebugPath);

			// Aliases
			this.AddAlias("reloadscripts", "rs");
			//this.AddAlias("item", "drop");
			this.AddAlias("broadcast", "bc");
			this.AddAlias("spawn", "monster");
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
			if (args.Count < 1)
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

			Position warpPos;

			if (args.Count >= 3)
			{
				if (!int.TryParse(args.Get(1), out var x))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(args.Get(2), out var y))
					return CommandResult.InvalidArgument;

				warpPos = new Position(x, y);
			}
			else
			{
				warpPos = target.Map.GetRandomWalkablePosition();
			}

			if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var map))
			{
				sender.ServerMessage(Localization.Get("Map not found."));
				return CommandResult.Okay;
			}

			target.Warp(map.Id, warpPos);

			sender.ServerMessage(Localization.Get("Warped to {0}, {1}, {2}."), map.StringId, warpPos.X, warpPos.Y);
			if (sender != target)
				target.ServerMessage(Localization.Get("You were warped by {0}."), sender.Name);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Warps target to another position on the same map.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Jump(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			Position warpPos;

			if (args.Count >= 2)
			{
				var xStr = args.Get(0);
				var yStr = args.Get(1);

				if (!int.TryParse(xStr, out var x))
					return CommandResult.InvalidArgument;

				if (!int.TryParse(yStr, out var y))
					return CommandResult.InvalidArgument;

				// If one of the coordinates were prefixed with a plus or
				// a minus, we apply both relative to the target's current
				// position, because if you type "+4 0", you presumably
				// mean to move 4 right, and not 4 right and down to
				// the bottom of the map.
				if (xStr.StartsWith("+") || xStr.StartsWith("-") || yStr.StartsWith("+") || yStr.StartsWith("-"))
				{
					var pos = target.Position;
					x += pos.X;
					y += pos.Y;
				}

				warpPos = new Position(x, y);
			}
			else
			{
				warpPos = target.Map.GetRandomWalkablePosition();
			}

			target.Warp(target.MapId, warpPos);

			sender.ServerMessage(Localization.Get("Warped to {0}, {1}."), warpPos.X, warpPos.Y);
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
			sender.ServerMessage(Localization.Get("Reloading scripts..."));
			ZoneServer.Instance.World.RemoveScriptedEntities();
			ZoneServer.Instance.ReloadScripts();
			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads server's configuration files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult ReloadConf(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			sender.ServerMessage(Localization.Get("Reloading configuration..."));
			ZoneServer.Instance.LoadConf();
			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Reloads server's data files.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult ReloadData(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			sender.ServerMessage(Localization.Get("Reloading data..."));
			ZoneServer.Instance.LoadData();
			sender.ServerMessage(Localization.Get("Done."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Toggles path debugging on and off.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult DebugPath(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			// This makes the character move between two specific positions
			// in Prontera South, where the client's path and the one
			// generated by our path finder currently diverge. Leaving
			// this here for testing purposes until it's fixed.
			if (args.Count != 0 && args.Get(0) == "test")
			{
				if (sender.Map.StringId != "prt_vilg01")
				{
					sender.ServerMessage("Go to prt_vilg01 first.");
					return CommandResult.Okay;
				}

				var fromPos = new Position(108, 92);
				var toPos = new Position(111, 86);

				sender.Warp(sender.MapId, fromPos);

				Task.Delay(2000).ContinueWith(_ =>
				{
					Send.ZC_NOTIFY_PLAYERMOVE(sender, fromPos, toPos);

					var path = sender.Map.PathFinder.FindPath(fromPos, toPos);
					foreach (var pathPos in path)
					{
						var npc = new Npc(66);
						npc.Warp(sender.Map.Id, pathPos);

						Task.Delay(3000).ContinueWith(__ => sender.Map.RemoveNpc(npc));
					}
				});

				return CommandResult.Okay;
			}

			var enabled = target.Vars.Temp.ToggleBool("Sabine.DebugPathEnabled");

			if (enabled)
				sender.ServerMessage(Localization.Get("Path debugging is now enabled."));
			else
				sender.ServerMessage(Localization.Get("Path debugging is now disabled."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Spawns monsters at the target's location.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Spawn(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(0), out var monsterId))
			{
				var monsterName = args.Get(0);

				if (!SabineData.Monsters.TryFind(monsterName, out var monsterData1))
				{
					sender.ServerMessage(Localization.Get("Monster '{0}' not found."), monsterName);
					return CommandResult.Okay;
				}

				monsterId = monsterData1.Id;
			}

			var amount = 1;
			if (args.Count > 1)
			{
				if (!int.TryParse(args.Get(1), out amount))
					return CommandResult.InvalidArgument;

				amount = Math2.Clamp(1, 1000, amount);
			}

			if (!SabineData.Monsters.Contains(monsterId))
			{
				sender.ServerMessage(Localization.Get("Monster with id '{0}' not found."), monsterId);
				return CommandResult.Okay;
			}

			for (var i = 0; i < amount; ++i)
			{
				var monster = new Monster(monsterId);
				var pos = target.Position.GetRandomInSquareRange(4);

				monster.Warp(sender.MapId, pos);
			}

			sender.ServerMessage(Localization.Get("Monsters spawned."));

			return CommandResult.Okay;
		}

		/// <summary>
		/// Modifies a character's stats.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Stat(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 2)
				return CommandResult.InvalidArgument;

			if (!int.TryParse(args.Get(1), out var modifier))
				return CommandResult.InvalidArgument;

			var type = args.Get(0);

			switch (type)
			{
				case "str": target.Parameters.Modify(ParameterType.Str, modifier); break;
				case "agi": target.Parameters.Modify(ParameterType.Agi, modifier); break;
				case "vit": target.Parameters.Modify(ParameterType.Vit, modifier); break;
				case "int": target.Parameters.Modify(ParameterType.Int, modifier); break;
				case "dex": target.Parameters.Modify(ParameterType.Dex, modifier); break;
				case "luk": target.Parameters.Modify(ParameterType.Luk, modifier); break;
				case "stp": target.Parameters.Modify(ParameterType.StatPoints, modifier); break;
				case "skp": target.Parameters.Modify(ParameterType.SkillPoints, modifier); break;

				default:
					sender.ServerMessage(Localization.Get("Unknown stat type '{0}'."), type);
					return CommandResult.Okay;
			}

			sender.ServerMessage(Localization.Get("Stat {0} has been modified by {1}."), type, modifier);
			if (sender != target)
				target.ServerMessage(Localization.Get("{0} has modified your stats."), sender.Name);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Adds item to target's inventory.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Item(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
				return CommandResult.InvalidArgument;

			var itemIdent = args.Get(0);

			if (!int.TryParse(itemIdent, out var classId))
			{
				if (!SabineData.Items.TryFind(itemIdent, out var itemData))
				{
					sender.ServerMessage(Localization.Get("Item '{0}' not found."), itemIdent);
					return CommandResult.Okay;
				}

				classId = itemData.ClassId;
			}

			if (!SabineData.Items.TryFind(classId, out _))
			{
				sender.ServerMessage(Localization.Get("Item with id '{0}' not found."), classId);
				return CommandResult.Okay;
			}

			var amount = 1;
			if (args.Count > 1)
			{
				if (!int.TryParse(args.Get(1), out amount))
					return CommandResult.InvalidArgument;
			}

			var item = new Item(classId);
			item.Amount = Math.Max(1, amount);

			target.Inventory.AddItem(item);

			sender.ServerMessage(Localization.Get("Item '{0}' was added to inventory."), item.Data.Name);
			if (target != sender)
				target.ServerMessage(Localization.Get("{0} added item '{1}' to your inventory."), sender.Name, item.Data.Name);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Changes target's job.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Job(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count == 0)
				return CommandResult.InvalidArgument;

			var jobIdent = args.Get(0);

			if (!Enum.TryParse<JobId>(jobIdent, out var jobId))
			{
				sender.ServerMessage(Localization.Get("Unknown job '{0}'."), jobIdent);
				return CommandResult.Okay;
			}

			target.ChangeJob(jobId);

			sender.ServerMessage(Localization.Get("Job changed to {0}."), jobId);
			if (target != sender)
				target.ServerMessage(Localization.Get("{0} changed your job to {1}."), sender.Name, jobId);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Changes the target's selected localization language.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Language(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			if (args.Count < 1)
			{
				var languages = ZoneServer.Instance.Localization.GetLanguages();
				var available = string.Join(", ", languages);

				sender.ServerMessage(Localization.Get("Available languages: {0}"), available);
				return CommandResult.InvalidArgument;
			}

			var languageName = args.Get(0);
			if (!ZoneServer.Instance.Localization.Contains(languageName))
			{
				sender.ServerMessage(Localization.Get("Language '{0}' is not available."), languageName);
				return CommandResult.InvalidArgument;
			}

			target.SelectedLanguage = languageName;

			sender.ServerMessage(Localization.Get("Language changed to {0}."), languageName);

			return CommandResult.Okay;
		}

		/// <summary>
		/// Restores target's health.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="target"></param>
		/// <param name="message"></param>
		/// <param name="commandName"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		private CommandResult Heal(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
		{
			target.Heal();

			sender.ServerMessage(Localization.Get("Healed."));
			if (target != sender)
				target.ServerMessage(Localization.Get("You were healed by {0}."), sender.Name);

			return CommandResult.Okay;
		}
	}
}
