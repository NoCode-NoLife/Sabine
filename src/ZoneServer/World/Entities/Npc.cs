using System;
using System.Threading;
using Sabine.Shared.Const;
using Sabine.Shared.World;
using Sabine.Zone.World.Maps;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a non-player character.
	/// </summary>
	public class Npc : ICharacter
	{
		private static int HandlePool = 0x0300_0000;

		public int Handle { get; set; }
		public string Name { get; set; }
		public string Username { get; set; } = "";
		public int ClassId { get; set; }
		public int MapId { get; set; }
		public Position Position { get; set; }
		public Direction Direction { get; set; } = Direction.South;

		public int Speed { get; set; } = 200;
		public Sex Sex { get; set; }
		public int HairId { get; set; }
		public int WeaponId { get; set; }

		/// <summary>
		/// Gets or sets a reference to the map the NPC is currently on.
		/// </summary>
		public Map Map
		{
			get => _map;
			set => _map = value ?? Map.Limbo;
		}
		private Map _map = Map.Limbo;

		/// <summary>
		/// Creates new NPC.
		/// </summary>
		/// <param name="classId"></param>
		public Npc(int classId)
		{
			this.Handle = GetNewHandle();
			this.ClassId = classId;
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
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var newMap))
				throw new ArgumentException($"Map '{mapId}' not found.");

			var curMap = this.Map;

			this.MapId = mapId;
			this.Position = pos;

			curMap.RemoveNpc(this);
			newMap.AddNpc(this);
		}
	}
}
