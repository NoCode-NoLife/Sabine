using System;
using System.Linq;
using Sabine.Shared.Data;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;

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
		/// Loads world and its maps.
		/// </summary>
		public void Load()
		{
			this.LoadMaps();
		}

		/// <summary>
		/// Generates maps based on all map data.
		/// </summary>
		private void LoadMaps()
		{
			foreach (var data in SabineData.Maps.Entries.Values)
				this.Maps.Add(new Map(data));
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
