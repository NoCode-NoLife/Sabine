using System;
using Sabine.Shared.World;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities;
using Yggdrasil.Scripting;

namespace Sabine.Zone.Scripting
{
	/// <summary>
	/// A map script, used to set up NPCs, warps, etc.
	/// </summary>
	public class MapScript : IScript
	{
		/// <summary>
		/// Initializes script.
		/// </summary>
		/// <returns></returns>
		public bool Init()
		{
			this.Load();
			return true;
		}

		/// <summary>
		/// Called when the script is being initialized.
		/// </summary>
		public virtual void Load()
		{
		}

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
		protected Npc AddNpc(string name, int classId, string mapStringId, int x, int y, DialogFunc dialogFunc = null)
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
		protected Npc AddWarp(Location from, Location to)
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
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected Location From(string mapStringId, int x, int y)
			=> this.ToLocation(mapStringId, x, y);

		/// <summary>
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected Location To(string mapStringId, int x, int y)
			=> this.ToLocation(mapStringId, x, y);

		/// <summary>
		/// Converts parameters into a Location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		protected Location ToLocation(string mapStringId, int x, int y)
		{
			if (!ZoneServer.Instance.World.Maps.TryGetByStringId(mapStringId, out var map))
				throw new ArgumentException($"Map '{mapStringId}' not found.");

			return new Location(map.Id, new Position(x, y));
		}
	}
}
