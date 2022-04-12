using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps.PathFinding;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// Represents a map in the world.
	/// </summary>
	public class Map
	{
		private readonly Dictionary<int, PlayerCharacter> _characters = new Dictionary<int, PlayerCharacter>();
		private readonly Dictionary<int, Npc> _npcs = new Dictionary<int, Npc>();

		/// <summary>
		/// Returns a reference to the Limbo map. See Limbo class for
		/// more information.
		/// </summary>
		public static readonly Limbo Limbo = new Limbo();

		/// <summary>
		/// Returns the map's id.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Returns the map's string-based id, which is used to identify
		/// the map on the client and is equal to the map's file name.
		/// </summary>
		public string StringId { get; }

		/// <summary>
		/// Returns a reference to the data for this map.
		/// </summary>
		public MapsData Data { get; }

		/// <summary>
		/// Returns a reference to the map's cache data.
		/// </summary>
		public MapCacheData CacheData { get; protected set; }

		/// <summary>
		/// Returns the map's path finder.
		/// </summary>
		public IPathFinder PathFinder { get; protected set; }

		/// <summary>
		/// Returns the number of players on this number.
		/// </summary>
		public int PlayerCount
		{
			get
			{
				lock (_characters)
					return _characters.Count;
			}
		}

		/// <summary>
		/// Creates new map from data.
		/// </summary>
		/// <param name="mapData"></param>
		public Map(MapsData mapData)
		{
			this.Id = mapData.Id;
			this.StringId = mapData.StringId;
			this.Data = mapData;

			this.LoadData();
		}

		/// <summary>
		/// Loads the map's data.
		/// </summary>
		protected virtual void LoadData()
		{
			this.CacheData = SabineData.MapCache.Find(this.StringId);
			if (this.CacheData == null)
			{
				Log.Warning("Map: No cache data found for '{0}'.", this.StringId);
				return;
			}

			this.PathFinder = new HercPathFinder(this.CacheData);
		}

		/// <summary>
		/// Adds character to this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void AddCharacter(PlayerCharacter character)
		{
			lock (_characters)
			{
				if (_characters.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' already exists on the map.");

				_characters[character.Id] = character;
				character.Map = this;
				Log.Debug("+ Characters on {0}: {1}", this.StringId, _characters.Count);
			}
		}

		/// <summary>
		/// Removes character from this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void RemoveCharacter(PlayerCharacter character)
		{
			lock (_characters)
			{
				if (!_characters.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' doesn't exists on the map.");

				_characters.Remove(character.Id);
				character.Map = null;

				Log.Debug("- Characters on {0}: {1}", this.StringId, _characters.Count);
			}
		}

		/// <summary>
		/// Return the character with the given handle, or null if the
		/// character wasn't found.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public ICharacter GetCharacter(int handle)
		{
			lock (_characters)
			{
				var character = _characters.Values.FirstOrDefault(a => a.Handle == handle);
				if (character != null)
					return character;
			}

			lock (_npcs)
			{
				var npc = _npcs.Values.FirstOrDefault(a => a.Handle == handle);
				if (npc != null)
					return npc;
			}

			return null;
		}

		/// <summary>
		/// Adds NPC to this map.
		/// </summary>
		/// <param name="npc"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void AddNpc(Npc npc)
		{
			lock (_npcs)
			{
				if (_npcs.ContainsKey(npc.Handle))
					throw new ArgumentException($"An NPC with the id '{npc.Handle}' already exists on the map.");

				_npcs[npc.Handle] = npc;
				npc.Map = this;

				Send.ZC_NOTIFY_STANDENTRY_NPC(npc);
			}
		}

		/// <summary>
		/// Removes NPC from this map.
		/// </summary>
		/// <param name="npc"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void RemoveNpc(Npc npc)
		{
			lock (_npcs)
			{
				if (!_npcs.ContainsKey(npc.Handle))
					throw new ArgumentException($"An NPC with the id '{npc.Handle}' doesn't exists on the map.");

				Send.ZC_NOTIFY_VANISH(npc, DisappearType.Vanish);

				_npcs.Remove(npc.Handle);
				npc.Map = null;
			}
		}

		/// <summary>
		/// Returns a list of all NPCs on this map.
		/// </summary>
		/// <returns></returns>
		public Npc[] GetAllNpcs()
		{
			lock (_npcs)
				return _npcs.Values.ToArray();
		}

		/// <summary>
		/// Returns a list of all NPCs on this map that match the given
		/// predicate.
		/// </summary>
		/// <returns></returns>
		public Npc[] GetAllNpcs(Func<Npc, bool> predicate)
		{
			lock (_npcs)
				return _npcs.Values.Where(predicate).ToArray();
		}

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		/// <param name="source">Source of the packet if it's only sent in a range around the source. Use null for map-wide broadcast.</param>
		/// <param name="includeSource">If true, the packet is sent to the source as well.</param>
		public void Broadcast(Packet packet, IEntity source = null, bool includeSource = false)
		{
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (source != null && !includeSource && source == character)
						continue;

					character.Connection.Send(packet);
				}
			}
		}
	}
}
