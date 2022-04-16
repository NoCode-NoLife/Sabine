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
	public class Map : IUpdateable
	{
		private readonly Dictionary<int, PlayerCharacter> _characters = new Dictionary<int, PlayerCharacter>();
		private readonly Dictionary<int, Npc> _npcs = new Dictionary<int, Npc>();
		private readonly Dictionary<int, Item> _items = new Dictionary<int, Item>();
		private readonly List<IUpdateable> _updateEntities = new List<IUpdateable>();

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
		/// Gets or sets the visible range of players on this map.
		/// </summary>
		public int VisibleRange { get; set; } = 20;

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
		/// Updates the map and its entities.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			// Create a list of updatables instead of locking and then
			// updating monsters and characters separately, so that
			// actions taken by components that get updated don't
			// affect Map. For example, adding and removing monsters
			// would modify the collections, and broadcasts could
			// cause deadlocks under certain circumstances.
			lock (_updateEntities)
			{
				lock (_npcs)
					_updateEntities.AddRange(_npcs.Values);

				lock (_characters)
					_updateEntities.AddRange(_characters.Values);

				foreach (var entity in _updateEntities)
					entity.Update(elapsed);

				_updateEntities.Clear();
			}
		}

		/// <summary>
		/// Returns a list of entities that the given entity can see.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public List<IEntity> GetVisibleEntities(IEntity entity)
		{
			var result = new List<IEntity>();

			lock (_items)
				result.AddRange(_items.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			lock (_npcs)
				result.AddRange(_npcs.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			lock (_characters)
				result.AddRange(_characters.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			return result;
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

				Send.ZC_NOTIFY_NEWENTRY(character);
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

				Send.ZC_NOTIFY_NEWENTRY(npc);
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
		/// Adds NPC to this map.
		/// </summary>
		/// <param name="item"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void AddItem(Item item)
		{
			lock (_items)
			{
				if (_items.ContainsKey(item.Handle))
					throw new ArgumentException($"An NPC with the id '{item.Handle}' already exists on the map.");

				_items[item.Handle] = item;
				item.Map = this;

				Send.ZC_ITEM_FALL_ENTRY(item);
			}

			// Notify characters about the new item, so no additional
			// entry packets are sent, which would mess with the
			// drop animation. This could probably be handled better.
			lock (_characters)
			{
				foreach (var character in _characters.Values)
					character.MarkVisible(item.Handle);
			}
		}

		/// <summary>
		/// Removes NPC from this map.
		/// </summary>
		/// <param name="item"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void RemoveItem(Item item)
		{
			lock (_items)
			{
				if (!_items.ContainsKey(item.Handle))
					throw new ArgumentException($"An item with the id '{item.Handle}' doesn't exists on the map.");

				_items.Remove(item.Handle);
				item.Map = null;
			}
		}

		/// <summary>
		/// Returns the item with the given handle, or null if it doesn't
		/// exist.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public Item GetItem(int handle)
		{
			lock (_items)
			{
				_items.TryGetValue(handle, out var item);
				return item;
			}
		}

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		public void Broadcast(Packet packet)
			=> this.Broadcast(packet, null, this.VisibleRange, BroadcastTargets.All);

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		/// <param name="source">Source of the packet if it's only sent in a range around the source. Use null for map-wide broadcast.</param>
		/// <param name="targets">Specifies who will receive the packet.</param>
		public void Broadcast(Packet packet, IEntity source, BroadcastTargets targets)
			=> this.Broadcast(packet, source, this.VisibleRange, targets);

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		/// <param name="source">Source of the packet if it's only sent in a range around the source. Use null for map-wide broadcast.</param>
		/// <param name="range">The range around the source in which the packet is broadcasted.</param>
		/// <param name="targets">Specifies who will receive the packet.</param>
		public void Broadcast(Packet packet, IEntity source, int range, BroadcastTargets targets)
		{
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (source != null)
					{
						if (targets == BroadcastTargets.AllButSource && source == character)
							continue;

						if (!source.Position.InRange(character.Position, range))
							continue;
					}

					character.Connection.Send(packet);
				}
			}
		}
	}
}
