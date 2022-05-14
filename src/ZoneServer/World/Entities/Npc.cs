using System;
using System.Threading;
using Sabine.Shared;
using Sabine.Shared.World;
using Sabine.Zone.Compatibility;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities.Components.Characters;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a non-player character.
	/// </summary>
	public class Npc : Character
	{
		private static int HandlePool = 400_000_000;

		/// <summary>
		/// Returns the NPC's unique handle.
		/// </summary>
		public override int Handle { get; protected set; }

		/// <summary>
		/// Returns the NPC's class id, defining its look.
		/// </summary>
		public override int ClassId { get; protected set; }

		// Temporary, for testing
		public Location WarpDestination { get; set; }

		/// <summary>
		/// Gets or sets the function called when a dialog with this
		/// NPC is initiated.
		/// </summary>
		public DialogFunc DialogFunc { get; set; }

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		/// <param name="classId"></param>
		public Npc(int classId)
		{
			this.Handle = GetNewHandle();
			this.ClassId = classId;
			this.DisplayClassId = CharacterClasses.GetDisplayClassId(Game.Version, classId);

			this.Parameters = new NpcParameters(this);

			this.Direction = Direction.South;
			this.Parameters.Speed = 400;
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);

		/// <summary>
		/// Warps NPC to the given position.
		/// </summary>
		/// <param name="mapId"></param>
		/// <param name="pos"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Warp(int mapId, Position pos)
			=> this.Warp(new Location(mapId, pos));

		/// <summary>
		/// Warps NPC to the given position.
		/// </summary>
		/// <param name="location"></param>
		/// <exception cref="ArgumentException"></exception>
		public override void Warp(Location location)
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(location.MapId, out var newMap))
				throw new ArgumentException($"Map '{location.MapId}' not found.");

			var curMap = this.Map;

			this.MapId = location.MapId;
			this.Position = location.Position;

			curMap.RemoveNpc(this);
			newMap.AddNpc(this);
		}
	}
}
