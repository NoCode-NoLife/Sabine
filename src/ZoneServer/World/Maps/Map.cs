using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Network;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Actors;
using Sabine.Zone.World.Maps.PathFinding;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// Represents a map in the world.
	/// </summary>
	public class Map : IUpdateable
	{
		private readonly Dictionary<int, PlayerCharacter> _players = new();
		private readonly Dictionary<int, Npc> _npcs = new();
		private readonly Dictionary<int, Item> _items = new();
		private readonly List<IUpdateable> _updateEntities = new();

		/// <summary>
		/// Returns a reference to the Limbo map. See Limbo class for
		/// more information.
		/// </summary>
		public static readonly Limbo Limbo = new();

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
				lock (_players)
					return _players.Count;
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
			this.CacheData = ZoneServer.Instance.Data.MapCache.Find(this.StringId);
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
			this.UpdateCharacters(elapsed);
			this.RemoveDroppedItems();
		}

		/// <summary>
		/// Runs Update on all characters.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateCharacters(TimeSpan elapsed)
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

				lock (_players)
					_updateEntities.AddRange(_players.Values);

				foreach (var entity in _updateEntities)
					entity.Update(elapsed);

				_updateEntities.Clear();
			}
		}

		/// <summary>
		/// Removes overdue items.
		/// </summary>
		private void RemoveDroppedItems()
		{
			List<Item> items = null;
			var now = DateTime.Now;

			lock (_items)
			{
				foreach (var item in _items.Values)
				{
					if (now < item.DropDisappearTime)
						continue;

					if (items == null)
						items = new List<Item>();

					items.Add(item);
				}
			}

			if (items == null)
				return;

			foreach (var item in items)
				this.RemoveItem(item);
		}

		/// <summary>
		/// Returns the entity with the given handle.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public IActor GetEntity(int handle)
		{
			var result = new List<IActor>();

			lock (_items)
			{
				if (_items.TryGetValue(handle, out var entity))
					return entity;
			}

			lock (_npcs)
			{
				if (_npcs.TryGetValue(handle, out var entity))
					return entity;
			}

			lock (_players)
			{
				if (_players.TryGetValue(handle, out var entity))
					return entity;
			}

			return null;
		}

		/// <summary>
		/// Returns a list of entities that the given entity can see.
		/// </summary>
		/// <param name="entity"></param>
		/// <returns></returns>
		public List<IActor> GetVisibleEntities(IActor entity)
		{
			var result = new List<IActor>();

			lock (_items)
				result.AddRange(_items.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			lock (_npcs)
				result.AddRange(_npcs.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			lock (_players)
				result.AddRange(_players.Values.Where(a => a.Position.InRange(entity.Position, this.VisibleRange)));

			return result;
		}

		/// <summary>
		/// Adds entity to visible entities on all players.
		/// </summary>
		/// <param name="entity"></param>
		private void AddVisibleEntity(IActor entity)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
					character.AddVisibleEntity(entity);
			}
		}

		/// <summary>
		/// Removes entity from visible entities on all players.
		/// </summary>
		/// <param name="entity"></param>
		private void RemoveVisibleEntity(IActor entity)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
					character.RemoveVisibleEntity(entity);
			}
		}

		/// <summary>
		/// Adds character to this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void AddPlayer(PlayerCharacter character)
		{
			lock (_players)
			{
				if (_players.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' already exists on the map.");

				_players[character.Id] = character;
				character.Map = this;
			}

			this.AddVisibleEntity(character);
			Send.ZC_NOTIFY_NEWENTRY(character);
		}

		/// <summary>
		/// Removes character from this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void RemovePlayer(PlayerCharacter character)
		{
			lock (_players)
			{
				if (!_players.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' doesn't exists on the map.");

				_players.Remove(character.Id);
			}

			// Cancel any trades on removal, so they get cancelled on
			// disconnect, warp, etc.
			if (ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
				trade.Cancel();

			// Remove the character from any chat room they might be in,
			// so they get removed on disconnect, warp, etc.
			if (character.ChatRoomId != 0 && ZoneServer.Instance.World.ChatRooms.TryGet(character.ChatRoomId, out var room))
				room.RemoveMember(character, MemberExitReason.Left);

			Send.ZC_NOTIFY_VANISH(character, DisappearType.Vanish);
			this.RemoveVisibleEntity(character);

			character.Map = null;
		}

		/// <summary>
		/// Returns the player character with the given handle via out.
		/// Returns false if the character wasn't found or isn't a
		/// player character.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool TryGetPlayer(int handle, out PlayerCharacter player)
		{
			lock (_players)
			{
				if (_players.TryGetValue(handle, out var character))
				{
					player = character;
					return true;
				}
			}

			player = null;
			return false;
		}

		/// <summary>
		/// Returns the player character with the given id via out.
		/// Returns false if the character wasn't found or isn't a
		/// player character.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool TryGetPlayerById(int id, out PlayerCharacter player)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
				{
					if (character.Id == id)
					{
						player = character;
						return true;
					}
				}
			}

			player = null;
			return false;
		}

		/// <summary>
		/// Returns the player character with the given name via out.
		/// Returns false if the character wasn't found or isn't a
		/// player character.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool TryGetPlayerByName(string name, out PlayerCharacter player)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
				{
					if (character.Name == name)
					{
						player = character;
						return true;
					}
				}
			}

			player = null;
			return false;
		}

		/// <summary>
		/// Adds the characters on the map that match the predicate to the
		/// given list.
		/// </summary>
		/// <typeparam name="TState"></typeparam>
		/// <param name="result">The list to add matching characters to.</param>
		/// <param name="state">A state that is passed to the predicate for determining matches.</param>
		/// <param name="predicate">The predicate characters need to match to be added to the list.</param>
		public void GetPlayers<TState>(List<PlayerCharacter> result, TState state, Func<TState, PlayerCharacter, bool> predicate)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
				{
					if (predicate(state, character))
						result.Add(character);
				}
			}
		}

		/// <summary>
		/// Returns the player or NPC with the given handle. Returns null
		/// if not matching character was found.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public Character GetCharacter(int handle)
		{
			lock (_players)
			{
				var character = _players.Values.FirstOrDefault(a => a.Handle == handle);
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
		/// Returns the character with the given handle via out, returns
		/// false if the character wasn't found.
		/// </summary>
		/// <param name="handle"></param>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool TryGetCharacter(int handle, out Character character)
		{
			character = this.GetCharacter(handle);
			return character != null;
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
			}

			this.AddVisibleEntity(npc);

			// Use a NEWENTRY for NPCs, so they get a spawn effect, but
			// STANDENTRY for monsters, to just make them appear.
			if (npc is Monster)
				Send.ZC_NOTIFY_STANDENTRY(npc);
			else
				Send.ZC_NOTIFY_NEWENTRY(npc);
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
			}

			this.RemoveVisibleEntity(npc);

			if (npc.IsDead)
				Send.ZC_NOTIFY_VANISH(npc, DisappearType.StrikedDead);
			else
				Send.ZC_NOTIFY_VANISH(npc, DisappearType.Vanish);

			npc.Map = null;
		}

		/// <summary>
		/// Returns the NPC with the given handle, or null if it doesn't
		/// exist.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		public Npc GetNpc(int handle)
		{
			lock (_npcs)
			{
				_npcs.TryGetValue(handle, out var npc);
				return npc;
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
		/// Adds all trigger areas containing the given position to the
		/// result list.
		/// </summary>
		/// <param name="position"></param>
		/// <param name="result"></param>
		public void GetTriggerAreas(Position position, IList<TriggerArea> result)
		{
			lock (_npcs)
			{
				foreach (var npc in _npcs.Values)
				{
					if (npc.TriggerArea == null)
						continue;

					if (npc.TriggerArea.Contains(position))
						result.Add(npc.TriggerArea);
				}
			}
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
			}

			Send.ZC_ITEM_FALL_ENTRY(item);
			this.AddVisibleEntity(item);
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
			}

			Send.ZC_ITEM_DISAPPEAR(item);
			this.RemoveVisibleEntity(item);

			item.Map = null;
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
		/// Returns a list of items that match the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Item[] GetItems(Func<Item, bool> predicate)
		{
			lock (_items)
				return _items.Values.Where(predicate).ToArray();
		}

		/// <summary>
		/// Returns a random position on the map that characters can walk
		/// on.
		/// </summary>
		/// <returns></returns>
		public Position GetRandomWalkablePosition()
		{
			var rnd = RandomProvider.Get();

			var width = this.CacheData.Width;
			var height = this.CacheData.Height;

			// Try to find a random position
			for (var i = 0; i < 100; ++i)
			{
				var x = rnd.Next(1, width);
				var y = rnd.Next(1, height);

				if (this.CacheData.IsPassable(x, y))
					return new Position(x, y);
			}

			// If the random search failed, go overboard and get all
			// passable tiles to choose a random one from all of them.
			// The data format should probably be changed a little,
			// because we need to find a lot of random positions,
			// and a simple list of walkable tiles would be mighty
			// helpful.
			var tiles = new List<MapCacheTile>();

			for (var y = 0; y < height; ++y)
			{
				for (var x = 0; x < width; ++x)
				{
					if (this.CacheData.Tiles[x, y].IsWalkable)
						tiles.Add(this.CacheData.Tiles[x, y]);
				}
			}

			if (tiles.Count == 0)
				throw new InvalidDataException("No walkable position found.");

			var rndTile = tiles[rnd.Next(tiles.Count)];
			return new Position(rndTile.X, rndTile.Y);
		}

		/// <summary>
		/// Returns true if the given tile can be walked on.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		public bool IsPassable(Position pos)
		{
			return this.CacheData.IsPassable(pos.X, pos.Y);
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
		public void Broadcast(Packet packet, IActor source, BroadcastTargets targets)
			=> this.Broadcast(packet, source, this.VisibleRange, targets);

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		/// <param name="source">Source of the packet if it's only sent in a range around the source. Use null for map-wide broadcast.</param>
		/// <param name="range">The range around the source in which the packet is broadcasted.</param>
		/// <param name="targets">Specifies who will receive the packet.</param>
		public void Broadcast(Packet packet, IActor source, int range, BroadcastTargets targets)
		{
			lock (_players)
			{
				foreach (var character in _players.Values)
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
