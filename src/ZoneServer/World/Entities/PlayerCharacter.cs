using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.L10N;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities.CharacterComponents;
using Sabine.Zone.World.Maps;
using Shared.Const;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public class PlayerCharacter : ICharacter, IUpdateable
	{
		private readonly object _visibilityUpdateSyncLock = new object();
		private readonly HashSet<int> _visibleEntities = new HashSet<int>();

		private readonly Queue<Position> _pathQueue = new Queue<Position>();
		private Position _nextDestination;
		private TimeSpan _currentMoveTime;
		private bool _moving;

		/// <summary>
		/// Gets or sets the connection that controls this player.
		/// </summary>
		public ZoneConnection Connection { get; set; } = new DummyConnection();

		/// <summary>
		/// Returns the character's variable container.
		/// </summary>
		public VariableContainer Vars { get; } = new VariableContainer();

		/// <summary>
		/// Returns this character's username.
		/// </summary>
		public string Username => this.Connection.Account.Username;

		/// <summary>
		/// Returns this character's session id.
		/// </summary>
		public int SessionId => this.Connection.Account.SessionId;

		/// <summary>
		/// Returns the character's handle.
		/// </summary>
		public int Handle => this.SessionId;

		/// <summary>
		/// Gets or sets this character's id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets this character's name.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Returns this character's/account's sex.
		/// </summary>
		public Sex Sex => this.Connection.Account.Sex;

		/// <summary>
		/// Gets or sets this character's job.
		/// </summary>
		public JobId JobId { get; set; } = JobId.Novice;

		/// <summary>
		/// Returns the character's speed from its parameters.
		/// </summary>
		public int Speed => this.Parameters.Speed;

		/// <summary>
		/// Returns the character's class id, which is equal to its
		/// current job id.
		/// </summary>
		public int ClassId => (int)this.JobId;

		/// <summary>
		/// Gets or sets the character's hair.
		/// </summary>
		public int HairId { get; set; }

		/// <summary>
		/// Gets or sets the character's weapon.
		/// </summary>
		public int WeaponId { get; set; }

		/// <summary>
		/// Returns a reference to the character's parameters/stats.
		/// </summary>
		public Parameters Parameters { get; }

		/// <summary>
		/// Returns a reference to the character's inventory.
		/// </summary>
		public Inventory Inventory { get; }

		/// <summary>
		/// Gets or sets the character's state.
		/// </summary>
		public CharacterState State { get; set; }

		/// <summary>
		/// Gets or sets the id of the map the character is on.
		/// </summary>
		public int MapId { get; set; } = 100036;

		/// <summary>
		/// Gets or sets the character's current position.
		/// </summary>
		public Position Position { get; set; } = new Position(99, 81);

		/// <summary>
		/// Gets or sets the direction the character is looking in.
		/// </summary>
		public Direction Direction { get; set; } = Direction.North;

		/// <summary>
		/// Returns true if character is warping to a new location.
		/// </summary>
		public bool IsWarping { get; private set; }

		/// <summary>
		/// Returns the position the character is warping towards while
		/// IsWarping is true.
		/// </summary>
		public Location WarpLocation { get; private set; }

		/// <summary>
		/// Returns a reference to the map the character is currently on.
		/// </summary>
		public Map Map
		{
			get => _map;
			set => _map = value ?? Map.Limbo;
		}
		private Map _map = Map.Limbo;

		/// <summary>
		/// Gets or sets whether the character is currently observing
		/// its surroundings, actively updating the visible entities.
		/// </summary>
		public bool IsObserving { get; protected set; }

		/// <summary>
		/// Gets or sets the player's selected language.
		/// </summary>
		public string SelectedLanguage
		{
			get => _selectedLanguage;
			set
			{
				_selectedLanguage = value;
				this.Localizer = ZoneServer.Instance.Localization.Get(value);
			}
		}
		private string _selectedLanguage;

		/// <summary>
		/// Returns the localizer for the player's selected language.
		/// </summary>
		public Localizer Localizer
		{
			get
			{
				if (_localizer == null)
					_localizer = ZoneServer.Instance.Localization.GetDefault();

				return _localizer;
			}
			private set => _localizer = value;
		}
		private Localizer _localizer;

		/// <summary>
		/// Creates a new character.
		/// </summary>
		public PlayerCharacter()
		{
			this.Parameters = new Parameters(this);
			this.Inventory = new Inventory(this);
		}

		/// <summary>
		/// Sends a server message to the character's client that is
		/// displayed in the chat log.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void ServerMessage(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			var message = string.Format(Localization.Get("[Server] : {0}"), format);

			Send.ZC_NOTIFY_CHAT(this, 0, message);
		}

		/// <summary>
		/// Warps character to given location.
		/// </summary>
		/// <param name="mapId"></param>
		/// <param name="pos"></param>
		public void Warp(int mapId, Position pos)
			=> this.Warp(new Location(mapId, pos));

		/// <summary>
		/// Warps character to given location.
		/// </summary>
		/// <param name="location"></param>
		public void Warp(Location location)
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(location.MapId, out var map))
				throw new ArgumentException($"Map '{location.MapId}' not found.");

			this.IsWarping = true;
			this.WarpLocation = location;

			this.StopObserving();

			Send.ZC_NPCACK_MAPMOVE(this, map.StringId, location.Position);
		}

		/// <summary>
		/// Finalizes a warp, actually moving the character to the
		/// new location.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void FinalizeWarp()
		{
			if (!this.IsWarping)
				throw new InvalidOperationException("No warp in process that could be finalized.");

			if (!ZoneServer.Instance.World.Maps.TryGet(this.WarpLocation.MapId, out var map))
				throw new ArgumentException($"Map '{this.WarpLocation.MapId}' not found.");

			this.Map.RemoveCharacter(this);
			this.SetLocation(this.WarpLocation);
			map.AddCharacter(this);

			this.IsWarping = false;
			this.StartObserving();
		}

		/// <summary>
		/// Sets character's map id and position.
		/// </summary>
		/// <param name="location"></param>
		public void SetLocation(Location location)
		{
			this.MapId = location.MapId;
			this.Position = location.Position;
		}

		/// <summary>
		/// Returns the character's location.
		/// </summary>
		/// <returns></returns>
		public Location GetLocation()
			=> new Location(this.MapId, this.Position);

		/// <summary>
		/// Makes character sit down.
		/// </summary>
		public void SitDown()
		{
			if (this.State != CharacterState.Standing)
				return;

			this.State = CharacterState.Sitting;
			Send.ZC_NOTIFY_ACT(this, this.Handle, 0, 0, 0, ActionType.SitDown);
		}

		/// <summary>
		/// Makes character stand up.
		/// </summary>
		public void StandUp()
		{
			if (this.State != CharacterState.Sitting)
				return;

			this.State = CharacterState.Standing;
			Send.ZC_NOTIFY_ACT(this, this.Handle, 0, 0, 0, ActionType.StandUp);
		}

		/// <summary>
		/// Updates character.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.UpdateVisibility();
			this.UpdateMovement(elapsed);
		}

		/// <summary>
		/// Starts updating of visible entities. A visibility update is
		/// executed when this method is called.
		/// </summary>
		public void StartObserving()
		{
			lock (_visibilityUpdateSyncLock)
			{
				if (this.IsObserving)
					return;

				this.IsObserving = true;
				this.UpdateVisibility();
			}
		}

		/// <summary>
		/// Stops updating of visible entities. A visibility update is
		/// executed when this method is called.
		/// </summary>
		public void StopObserving()
		{
			lock (_visibilityUpdateSyncLock)
			{
				if (!this.IsObserving)
					return;

				this.IsObserving = false;
				this.RemoveVisibleEntities();
			}
		}

		/// <summary>
		/// Updates visible entities around character.
		/// </summary>
		public void UpdateVisibility()
		{
			if (!this.IsObserving)
				return;

			lock (_visibilityUpdateSyncLock)
			{
				var visibleEntities = this.Map.GetVisibleEntities(this);

				var appeared = visibleEntities.Where(a => !_visibleEntities.Contains(a.Handle));
				var disappeared = _visibleEntities.Where(a => !visibleEntities.Exists(b => b.Handle == a));

				foreach (var entity in appeared)
				{
					if (entity == this)
						continue;

					switch (entity)
					{
						case ICharacter character: Send.ZC_NOTIFY_STANDENTRY(this, character); break;
						case Item item: Send.ZC_ITEM_ENTRY(this, item); break;
					}
				}

				foreach (var handle in disappeared)
				{
					if (handle == this.Handle)
						continue;

					if (handle < 0x6000_0000)
						Send.ZC_NOTIFY_VANISH(this, handle, DisappearType.Vanish);
					else
						Send.ZC_ITEM_DISAPPEAR(this, handle);
				}

				// To remember the visible entities for the next run we store
				// their ids. There might be some cases where it would be
				// useful to have the actual references, but we can still
				// get those if we need to, and this way there's no chance
				// for any memory leaks because we're storing objects
				// that reference each other.

				_visibleEntities.Clear();
				_visibleEntities.UnionWith(visibleEntities.Select(a => a.Handle));
			}
		}

		/// <summary>
		/// Adds handle to the character's visible entities.
		/// </summary>
		/// <param name="handle"></param>
		public void MarkVisible(int handle)
		{
			lock (_visibilityUpdateSyncLock)
				_visibleEntities.Add(handle);
		}

		/// <summary>
		/// Clears the list of visible entities and updates the client.
		/// </summary>
		private void RemoveVisibleEntities()
		{
			lock (_visibilityUpdateSyncLock)
			{
				foreach (var handle in _visibleEntities)
					Send.ZC_NOTIFY_VANISH(this, handle, DisappearType.Vanish);

				_visibleEntities.Clear();
			}
		}

		/// <summary>
		/// Returns true if the character is able to equip the given item,
		/// based on its level and job requirements.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool CanEquip(Item item)
		{
			if (item.Data.SexAllowed != Sex.Any && item.Data.SexAllowed != this.Sex)
				return false;

			// There's no mention of required levels in the GameFAQs
			// alpha guide. Did they not exist? Maybe finding enough
			// Zeny was challenge enough?
			if (this.Parameters.BaseLevel < item.Data.RequiredLevel)
				return false;

			if (!this.JobId.Matches(item.Data.JobsAllowed))
				return false;

			return true;
		}

		/// <summary>
		/// Changes the character's look and updates the client.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="lookId"></param>
		public void ChangeLook(SpriteType type, int lookId)
		{
			switch (type)
			{
				case SpriteType.Hair: this.HairId = lookId; break;
				case SpriteType.Weapon: this.WeaponId = lookId; break;
				default:
					throw new ArgumentException($"Unsupported type '{type}'.");
			}

			Send.ZC_SPRITE_CHANGE(this, type, lookId);
		}

		/// <summary>
		/// Changes character's job and updates the client.
		/// </summary>
		/// <param name="jobId"></param>
		public void ChangeJob(JobId jobId)
		{
			this.JobId = jobId;
			Send.ZC_SPRITE_CHANGE(this, SpriteType.Class, (int)jobId);

			this.Inventory.RefreshClient();
		}

		/// <summary>
		/// Drops item in range of the character.
		/// </summary>
		/// <param name="item"></param>
		public void Drop(Item item)
		{
			var rnd = RandomProvider.Get();
			var pos = this.Position;

			pos.X += rnd.Next(-1, 2);
			pos.Y += rnd.Next(-1, 2);

			item.MapId = this.MapId;
			item.Position = pos;

			this.Map.AddItem(item);
		}

		/// <summary>
		/// Returns the character's current attack range, based on its
		/// state and equipped items.
		/// </summary>
		/// <returns></returns>
		public int GetAttackRange()
		{
			// Range is 3 for normal attacks and 16 for ranged
			// in the alpha client. This is hardcoded, based on
			// the type of the item that was equipped.

			if (this.Inventory.RightHand?.Type == ItemType.RangedWeapon)
				return 16;

			return 3;
		}

		/// <summary>
		/// Makes character move to the given position.
		/// </summary>
		/// <param name="toPos"></param>
		public void MoveTo(Position toPos)
		{
			var character = this;
			var fromPos = this.Position;

			// If we're currently moving, the start position needs to be
			// the next tile we move to, or the client will rubber-band
			// back to the tile it was just on.
			if (_moving)
				fromPos = _nextDestination;

			// Clear the current movement queue and fill it with the
			// new path.
			var path = this.Map.PathFinder.FindPath(fromPos, toPos);

			_pathQueue.Clear();
			foreach (var pos in path)
				_pathQueue.Enqueue(pos);

			Log.Debug("Moving from {0} to {1}.", fromPos, toPos);
			for (var i = 0; i < path.Length; i++)
			{
				var pos = path[i];
				Log.Debug("  {0}: {1}", i + 1, pos);
			}

			// Start the move to the next tile
			this.NextMovement();

			// Notify the clients
			Send.ZC_NOTIFY_PLAYERMOVE(character, fromPos, toPos);
			Send.ZC_NOTIFY_MOVE(character, fromPos, toPos);
		}

		/// <summary>
		/// Stops character's current movement.
		/// </summary>
		public Position StopMove()
		{
			if (!_moving)
				return this.Position;

			var stopPos = _nextDestination;
			_pathQueue.Clear();
			_moving = false;

			// The client doesn't react to ZC_STOPMOVE for its controlled
			// character, so we need to send a move to make it stop.
			// Unless that's not desired? Maybe there should a be
			// forceStop parameter or something? TBD.

			Send.ZC_STOPMOVE(this, stopPos);
			Send.ZC_NOTIFY_PLAYERMOVE(this, this.Position, stopPos);

			return stopPos;
		}

		/// <summary>
		/// Updates the character's position based on its current movement.
		/// </summary>
		/// <param name="elapsed"></param>
		private void UpdateMovement(TimeSpan elapsed)
		{
			// Do nothing if we're not currently moving to a new tile
			if (!_moving)
				return;

			// Wait until enough time has passed to reach the next tile
			_currentMoveTime -= elapsed;
			if (_currentMoveTime > TimeSpan.Zero)
				return;

			// Update current position
			this.Position = _nextDestination;
			Log.Debug("  now at {0}", this.Position);

			// Start next move if there's still something left in the queue
			if (_pathQueue.Count != 0)
			{
				this.NextMovement();
				return;
			}

			_moving = false;
		}

		/// <summary>
		/// Starts server-side move to the next tile in the character's
		/// movement queue.
		/// </summary>
		/// <exception cref="InvalidOperationException"></exception>
		private void NextMovement()
		{
			if (_pathQueue.Count == 0)
				throw new InvalidOperationException("Path queue is empty.");

			_nextDestination = _pathQueue.Dequeue();

			var movingStright = this.Position.X == _nextDestination.X || this.Position.Y == _nextDestination.Y;
			var speed = (float)this.Parameters.Speed;

			// Speed appears to match diagonal movement, but it needs
			// to be adjusted for straight moves, or the character will
			// move slower on the server than on the client.
			// Athena uses 'speed * 14 / 10' to slow down diagonal movement
			// instead, so are characters possibly moving faster on the
			// alpha client? That would explain why Athena's default walk
			// speed value of 150 seems way too fast for the alpha client.
			// Meanwhile, the client seems to use '10 * speed / 13'...? But
			// our 'speed / 1.8f' seems to work well for the moment and gives
			// us ~111 for 200 speed, while the client's formula gives us
			// ~153, which is way off. This needs more research.
			if (movingStright)
				speed = speed / 1.8f;

			_currentMoveTime = TimeSpan.FromMilliseconds(speed);
			_moving = true;
		}
	}
}
