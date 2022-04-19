using System;
using Sabine.Shared.Configuration.Files;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Commands;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Shops;

namespace Sabine.Zone.Scripting
{
	public static class Shortcuts
	{
		/// <summary>
		/// Returns an option element, to be used with the Menu function.
		/// </summary>
		/// <param name="text">Text to display in the menu.</param>
		/// <param name="key">Key to return if the option was selected.</param>
		/// <returns></returns>
		public static DialogOption Option(string text, string key)
			=> new DialogOption(text, key);

		/// <summary>
		/// Returns a localized version of the given string.
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string L(string key)
			=> Localization.Get(key);

		/// <summary>
		/// Returns a localized version of the given string, formatted
		/// with the optional arguments.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string LF(string key, params object[] args)
			=> string.Format(Localization.Get(key), args);

		/// <summary>
		/// Returns a localized plural version of the given string.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyPlural"></param>
		/// <param name="n"></param>
		/// <returns></returns>
		public static string LN(string key, string keyPlural, int n)
			=> Localization.GetPlural(key, keyPlural, n);

		/// <summary>
		/// Returns a localized plural version of the given string,
		/// formatted with the optional arguments.
		/// </summary>
		/// <param name="key"></param>
		/// <param name="keyPlural"></param>
		/// <param name="n"></param>
		/// <param name="args"></param>
		/// <returns></returns>
		public static string LNF(string key, string keyPlural, int n, params object[] args)
			=> string.Format(Localization.GetPlural(key, keyPlural, n), args);


		/// <summary>
		/// Spawns an NPC at the given location.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="classId"></param>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <param name="dialogFunc"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddNpc(string name, int classId, string mapStringId, int x, int y, DialogFunc dialogFunc = null)
		{
			if (!ZoneServer.Instance.World.Maps.TryGetByStringId(mapStringId, out var map))
				throw new ArgumentException($"Map '{mapStringId}' not found.");

			var npc = new Npc(classId);
			npc.Name = name;
			npc.DialogFunc = dialogFunc;

			npc.Warp(map.Id, new Position(x, y));

			return npc;
		}

		/// <summary>
		/// Spawns a warp at the given location.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static Npc AddWarp(Location from, Location to)
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(from.MapId, out var mapFrom))
				throw new ArgumentException($"Map '{from.MapId}' not found.");

			if (!ZoneServer.Instance.World.Maps.TryGet(to.MapId, out var mapTo))
				throw new ArgumentException($"Map '{from.MapId}' not found.");

			var npc = new Npc(32);
			npc.WarpDestination = to;
			//npc.Trigger = WarpOnTouch

			npc.Warp(from);

			return npc;
		}

		/// <summary>
		/// Creates shop and returns it. It can be filled via the callback
		/// function or with the returned object.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="creationFunc"></param>
		/// <returns></returns>
		public static NpcShop AddShop(string name, Action<NpcShop> creationFunc = null)
		{
			var shop = new NpcShop(name);
			creationFunc?.Invoke(shop);

			if (ZoneServer.Instance.World.NpcShops.ContainsKey(name))
				throw new ArgumentException($"A shop with the name '{name}' already exists.");

			ZoneServer.Instance.World.NpcShops.Add(name, shop);

			return shop;
		}

		/// <summary>
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Location From(string mapStringId, int x, int y)
			=> ToLocation(mapStringId, x, y);

		/// <summary>
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public static Location To(string mapStringId, int x, int y)
			=> ToLocation(mapStringId, x, y);

		/// <summary>
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		private static Location ToLocation(string mapStringId, int x, int y)
		{
			if (!ZoneServer.Instance.World.Maps.TryGetByStringId(mapStringId, out var map))
				throw new ArgumentException($"Map '{mapStringId}' not found.");

			return new Location(map.Id, new Position(x, y));
		}

		/// <summary>
		/// Adds new chat command.
		/// </summary>
		/// <param name="name">Name of the command.</param>
		/// <param name="usage">Description of the command's arguments.</param>
		/// <param name="description">A description of what the command does.</param>
		/// <param name="func">The handler function to execute when the command is used.</param>
		/// <param name="selfAuthLevel">Authority level required to execute the command for yourself.</param>
		/// <param name="targetAuthLevel">Authority level required to execute the command for someone else.</param>
		public static void AddChatCommand(string name, string usage, string description, CommandFunc func, int selfAuthLevel, int targetAuthLevel)
		{
			ZoneServer.Instance.ChatCommands.Add(name, usage, description, func);
			ZoneServer.Instance.Conf.Commands.Levels[name] = new AuthLevels() { Self = selfAuthLevel, Target = targetAuthLevel };
		}
	}
}
