using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Maps;
using Shared.Const;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public class PlayerCharacter : ICharacter, IUpdateable
	{
		private readonly object _visibilityUpdateSyncLock = new object();
		private readonly HashSet<int> _visibleEntities = new HashSet<int>();

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
		/// Returns the character's class id.
		/// </summary>
		int ICharacter.ClassId => (int)this.JobId;

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
		/// Gets or sets the character's speed.
		/// </summary>
		public int Speed { get; set; } = 200;

		/// <summary>
		/// Gets or sets the character's hair.
		/// </summary>
		public int HairId { get; set; }

		/// <summary>
		/// Gets or sets the character's weapon.
		/// </summary>
		public int WeaponId { get; set; }

		/// <summary>
		/// Gets or sets how many HP the character currently has.
		/// </summary>
		public int Hp { get; set; } = 40;

		/// <summary>
		/// Gets or sets the character's maximum amount of HP.
		/// </summary>
		public int HpMax { get; set; } = 40;

		/// <summary>
		/// Gets or sets how many SP the character currently has.
		/// </summary>
		public int Sp { get; set; } = 40;

		/// <summary>
		/// Gets or sets the character's maximum amount of SP.
		/// </summary>
		public int SpMax { get; set; } = 40;

		/// <summary>
		/// Gets or sets the character's STR stat.
		/// </summary>
		public int Str { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's AGI stat.
		/// </summary>
		public int Agi { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's VIT stat.
		/// </summary>
		public int Vit { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's INT stat.
		/// </summary>
		public int Int { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's DEX stat.
		/// </summary>
		public int Dex { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's LUK stat.
		/// </summary>
		public int Luk { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's stat points.
		/// </summary>
		public int StatPoints { get; set; } = 10;

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the STR stat by one.
		/// </summary>
		public int StrNeeded => (1 + (this.Str + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the AGI stat by one.
		/// </summary>
		public int AgiNeeded => (1 + (this.Agi + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the VIT stat by one.
		/// </summary>
		public int VitNeeded => (1 + (this.Vit + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the INT stat by one.
		/// </summary>
		public int IntNeeded => (1 + (this.Int + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the DEX stat by one.
		/// </summary>
		public int DexNeeded => (1 + (this.Dex + 9) / 10);

		/// <summary>
		/// Returns the amount of stat points necessary to increase
		/// the LUK stat by one.
		/// </summary>
		public int LukNeeded => (1 + (this.Luk + 9) / 10);

		/// <summary>
		/// Returns the character's current min attack.
		/// </summary>
		public int AtkMin { get; set; } = 10;

		/// <summary>
		/// Returns the character's current max attack.
		/// </summary>
		public int AtkMax { get; set; } = 15;

		/// <summary>
		/// Returns the character's current magic attack.
		/// </summary>
		public int Matk { get; set; } = 1;

		/// <summary>
		/// Returns the character's current defense.
		/// </summary>
		public int Defense { get; set; } = 7;

		/// <summary>
		/// Returns the character's current weight.
		/// </summary>
		public int Weight { get; set; } = 10;

		/// <summary>
		/// Returns the character's current max weight.
		/// </summary>
		public int WeightMax { get; set; } = 20000;

		/// <summary>
		/// Returns the number of skill points the character has to assign.
		/// </summary>
		public int SkillPoints { get; set; } = 10;

		/// <summary>
		/// Gets or sets the character's base experience points.
		/// </summary>
		public int BaseExp { get; set; } = 10;

		/// <summary>
		/// Gets or sets the character's job experience points.
		/// </summary>
		public int JobExp { get; set; }

		/// <summary>
		/// Returns the amount of experience points necessary to reach
		/// the next base level.
		/// </summary>
		public int BaseExpNeeded { get; set; } = 9;

		/// <summary>
		/// Returns the amount of experience points necessary to reach
		/// the next job level.
		/// </summary>
		public int JobExpNeeded { get; set; }

		/// <summary>
		/// Gets or sets the character's current base level.
		/// </summary>
		public int BaseLevel { get; set; } = 1;

		/// <summary>
		/// Gets or sets the character's current base level.
		/// </summary>
		public int JobLevel { get; set; } = 1;

		/// <summary>
		/// Gets or sets how many Zeny the character has.
		/// </summary>
		public int Zeny { get; set; }

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
		/// Returns the value for the given stat.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStat(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Str: return this.Str;
				case ParameterType.Agi: return this.Agi;
				case ParameterType.Vit: return this.Vit;
				case ParameterType.Int: return this.Int;
				case ParameterType.Dex: return this.Dex;
				case ParameterType.Luk: return this.Luk;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Modifies the given stat and updates the client.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="modifier"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int ModifyStat(ParameterType type, int modifier)
		{
			if (type < ParameterType.Str || type > ParameterType.Luk)
				throw new ArgumentException($"Invalid stat type '{type}'.");

			var newValue = 0;
			switch (type)
			{
				case ParameterType.Str: newValue = this.Str += modifier; break;
				case ParameterType.Agi: newValue = this.Agi += modifier; break;
				case ParameterType.Vit: newValue = this.Vit += modifier; break;
				case ParameterType.Int: newValue = this.Int += modifier; break;
				case ParameterType.Dex: newValue = this.Dex += modifier; break;
				case ParameterType.Luk: newValue = this.Luk += modifier; break;
			}

			Send.ZC_PAR_CHANGE(this, type, newValue);

			return newValue;
		}

		/// <summary>
		/// Returns the amount of stat points needed to increase the
		/// given stat by one.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public int GetStatPointsNeeded(ParameterType type)
		{
			switch (type)
			{
				case ParameterType.Str: return this.StrNeeded;
				case ParameterType.Agi: return this.AgiNeeded;
				case ParameterType.Vit: return this.VitNeeded;
				case ParameterType.Int: return this.IntNeeded;
				case ParameterType.Dex: return this.DexNeeded;
				case ParameterType.Luk: return this.LukNeeded;

				default:
					throw new ArgumentException($"Invalid stat type '{type}'.");
			}
		}

		/// <summary>
		/// Modifies the character's stat points and updates the client.
		/// </summary>
		/// <param name="modifier"></param>
		public void ModifyStatPoints(int modifier)
		{
			this.StatPoints += modifier;

			Send.ZC_PAR_CHANGE(this, ParameterType.StatPoints, this.StatPoints);
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
		/// Stops character's movement.
		/// </summary>
		public void StopMove()
		{
			var pos = this.Position; // TODO: Calculate current position
			Send.ZC_STOPMOVE(this, pos);
		}

		/// <summary>
		/// Updates character.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			this.UpdateVisibility();
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

					Send.ZC_NOTIFY_STANDENTRY(this, entity);
				}

				foreach (var handle in disappeared)
				{
					if (handle == this.Handle)
						continue;

					Send.ZC_NOTIFY_VANISH(this, handle, DisappearType.Vanish);
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
	}
}
