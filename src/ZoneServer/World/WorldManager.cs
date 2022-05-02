using System;
using System.Linq;
using Sabine.Shared.Data;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;
using Sabine.Zone.World.Shops;
using Sabine.Zone.World.Spawning;
using Yggdrasil.Collections;
using Yggdrasil.Scheduling;

namespace Sabine.Zone.World
{
	/// <summary>
	/// Represents the world and everything in it.
	/// </summary>
	public class WorldManager
	{
		/// <summary>
		/// Returns a reference to a collection of maps in the world.
		/// </summary>
		public MapManager Maps { get; } = new MapManager();

		/// <summary>
		/// The world's heartbeast, which controls timed events.
		/// </summary>
		public Heartbeat Heartbeat { get; } = new Heartbeat();

		/// <summary>
		/// The world's scheduler. Use only if a high resolution timer is
		/// absolutely necessary.
		/// </summary>
		public Scheduler Scheduler { get; } = new Scheduler();

		/// <summary>
		/// Returns a reference to a collection of NPC shops in the world.
		/// </summary>
		public Collection<string, NpcShop> NpcShops { get; } = new Collection<string, NpcShop>();

		/// <summary>
		/// Returns the world's monster spawner collection.
		/// </summary>
		public Spawners Spawners { get; } = new Spawners();

		/// <summary>
		/// Loads world and its maps.
		/// </summary>
		public void Load()
		{
			this.LoadMaps();
			this.SetupUpdates();
		}

		/// <summary>
		/// Subscribes world to its heartbeat, to update its maps.
		/// </summary>
		private void SetupUpdates()
		{
			this.Heartbeat.HeartbeatTick += this.Maps.Update;
		}

		/// <summary>
		/// Generates maps based on all map data.
		/// </summary>
		private void LoadMaps()
		{
			foreach (var data in SabineData.Maps.Entries.Values)
			{
				var map = new Map(data);
				this.Maps.Add(map);
			}
		}

		/// <summary>
		/// Returns the first character in the world that matches the
		/// given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public PlayerCharacter GetPlayerCharacter(Func<PlayerCharacter, bool> predicate)
		{
			return null;
		}

		/// <summary>
		/// Returns the first character in the world that matches the given
		/// predicate via out. Returns false if no matching character was
		/// found.
		/// </summary>
		/// <param name="predicate"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool TryGetPlayerCharacter(Func<PlayerCharacter, bool> predicate, out PlayerCharacter character)
		{
			character = this.GetPlayerCharacter(predicate);
			return character != null;
		}

		/// <summary>
		/// Returns the number of players across all maps.
		/// </summary>
		/// <returns></returns>
		public int GetPlayerCount()
		{
			var maps = this.Maps.GetAll();
			return maps.Sum(a => a.PlayerCount);
		}

		/// <summary>
		/// Removes all entities that were added dynamically from the
		/// world, such as NPCs.
		/// </summary>
		public void RemoveScriptedEntities()
		{
			this.NpcShops.Clear();
			this.Spawners.Clear();

			// Yes, this is inefficient, but do we really care, with how
			// rare script reloads are?
			foreach (var map in this.Maps.GetAll())
			{
				var npcs = map.GetAllNpcs();
				foreach (var npc in npcs)
					map.RemoveNpc(npc);
			}
		}
	}
}
