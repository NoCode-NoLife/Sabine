using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Configuration.Files;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.L10N;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities.Components.Characters;
using Shared.Const;
using Yggdrasil.Scheduling;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public class PlayerCharacter : Character
	{
		private readonly object _visibilityUpdateSyncLock = new object();
		private readonly HashSet<int> _visibleEntities = new HashSet<int>();

		/// <summary>
		/// Gets or sets the connection that controls this player.
		/// </summary>
		public ZoneConnection Connection { get; internal set; } = new DummyConnection();

		/// <summary>
		/// Returns the character's variable container.
		/// </summary>
		public VariableContainer Vars { get; } = new VariableContainer();

		/// <summary>
		/// Returns a reference to the character's inventory.
		/// </summary>
		public Inventory Inventory { get; }

		/// <summary>
		/// Returns this character's username.
		/// </summary>
		public override string Username => this.Connection.Account.Username;

		/// <summary>
		/// Returns the character's handle.
		/// </summary>
		public override int Handle
		{
			get => this.Connection.Account.Id;
			protected set => throw new NotSupportedException();
		}

		/// <summary>
		/// Gets or sets this character's id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets this character's name.
		/// </summary>
		public override string Name { get; set; }

		/// <summary>
		/// Returns this character's/account's sex.
		/// </summary>
		public override Sex Sex => this.Connection.Account.Sex;

		/// <summary>
		/// Gets or sets this character's job.
		/// </summary>
		public JobId JobId { get; private set; }

		/// <summary>
		/// Returns a reference to the character's job's data.
		/// </summary>
		public JobData JobData { get; private set; }

		/// <summary>
		/// Returns the character's class id, which is equal to its
		/// current job id.
		/// </summary>
		public override int ClassId
		{
			get => (int)this.JobId;
			protected set => throw new NotSupportedException();
		}

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
		public PlayerCharacter(JobId jobId)
		{
			this.JobId = jobId;
			this.Inventory = new Inventory(this);

			this.Parameters = new PlayerCharacterParameters(this);

			this.LoadJobData(jobId);
		}

		/// <summary>
		/// Loads the data for the given job.
		/// </summary>
		/// <param name="jobId"></param>
		/// <exception cref="ArgumentException"></exception>
		private void LoadJobData(JobId jobId)
		{
			if (!SabineData.Jobs.TryFind(jobId, out var jobData))
				throw new ArgumentException($"No data found for job {jobId}.");

			this.JobData = jobData;
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
		/// Warps character to the given location.
		/// </summary>
		/// <param name="mapStringId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public void Warp(string mapStringId, int x, int y)
		{
			if (!SabineData.Maps.TryFind(mapStringId, out var mapData))
				throw new ArgumentException($"Map '{mapStringId}' not found.");

			this.Warp(mapData.Id, new Position(x, y));
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
		public override void Warp(Location location)
		{
			if (this.IsWarping)
				throw new InvalidOperationException("A warp is already in progress.");

			if (!ZoneServer.Instance.World.Maps.TryGet(location.MapId, out var map))
				throw new ArgumentException($"Map '{location.MapId}' not found.");

			this.IsWarping = true;
			this.WarpLocation = location;

			this.Controller.StopMove();
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
		/// Makes character sit down.
		/// </summary>
		public void SitDown()
		{
			if (this.State != CharacterState.Standing)
				return;

			this.State = CharacterState.Sitting;
			Send.ZC_NOTIFY_ACT.Simple(this, this.Handle, ActionType.SitDown);
		}

		/// <summary>
		/// Makes character stand up.
		/// </summary>
		public void StandUp()
		{
			if (this.State != CharacterState.Sitting)
				return;

			this.State = CharacterState.Standing;
			Send.ZC_NOTIFY_ACT.Simple(this, this.Handle, ActionType.StandUp);
		}

		/// <summary>
		/// Updates character and its components.
		/// </summary>
		/// <param name="elapsed"></param>
		public override void Update(TimeSpan elapsed)
		{
			base.Update(elapsed);
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

			var visibleEntities = this.Map.GetVisibleEntities(this);

			lock (_visibilityUpdateSyncLock)
			{
				var appeared = visibleEntities.Where(a => !_visibleEntities.Contains(a.Handle));
				var disappeared = _visibleEntities.Where(a => !visibleEntities.Exists(b => b.Handle == a));

				foreach (var entity in appeared)
				{
					if (entity == this)
						continue;

					switch (entity)
					{
						case Character character: Send.ZC_NOTIFY_STANDENTRY(this, character); break;
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
		/// Adds entity to list of character's visible entities without
		/// updating the client.
		/// </summary>
		/// <remarks>
		/// AddVisibleEntity and RemoveVisibleEntity are to be used in
		/// cases where an outside source needs to control an entity's
		/// appear or disappear packets.
		/// </remarks>
		/// <param name="handle"></param>
		internal void AddVisibleEntity(IEntity entity)
		{
			if (!this.IsObserving)
				return;

			lock (_visibilityUpdateSyncLock)
				_visibleEntities.Add(entity.Handle);
		}

		/// <summary>
		/// Removes entity from list of character's visible entities without
		/// updating the client.
		/// </summary>
		/// <param name="entity"></param>
		internal void RemoveVisibleEntity(IEntity entity)
		{
			if (!this.IsObserving)
				return;

			lock (_visibilityUpdateSyncLock)
				_visibleEntities.Remove(entity.Handle);
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
			this.LoadJobData(jobId);

			this.Inventory.CheckEquipRequirements();
			this.Inventory.RefreshClient();
			this.Parameters.RecalculateAll();
			this.Heal();

			Send.ZC_SPRITE_CHANGE(this, SpriteType.Class, (int)jobId);

			// Send a BaseLevel change packet to get the level up animation
			Send.ZC_PAR_CHANGE(this, ParameterType.BaseLevel);
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
		/// Increases the character's exp by the given amount and levels
		/// them up if possible.
		/// </summary>
		/// <param name="amount"></param>
		public void GainBaseExp(int amount)
		{
			var exp = this.Parameters.BaseExp;
			var level = this.Parameters.BaseLevel;
			var expNeeded = this.Parameters.BaseExpNeeded;
			var maxLevel = SabineData.ExpTables.GetMaxLevel(ExpTableType.Base, this.JobId);
			var levelsGained = 0;
			var statPointsGained = 0;

			exp = Math.Max(0, Math2.AddChecked(exp, amount));

			while (level < maxLevel && exp >= expNeeded)
			{
				exp -= expNeeded;

				level++;
				levelsGained++;
				statPointsGained += (level / 5) + 2;

				expNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Base, this.JobId, level);
			}

			if (levelsGained != 0)
			{
				this.Parameters.Set(ParameterType.BaseLevel, level);
				this.Parameters.Set(ParameterType.BaseExpNeeded, expNeeded);
				this.Parameters.Modify(ParameterType.StatPoints, statPointsGained);
				this.Parameters.Modify(ParameterType.SkillPoints, levelsGained);
			}

			this.Parameters.Set(ParameterType.BaseExp, exp);
		}

		/// <summary>
		/// Increases the character's exp by the given amount and levels
		/// them up if possible.
		/// </summary>
		/// <param name="amount"></param>
		public void GainJobExp(int amount)
		{
			// Don't give any job EXP if the feature is disabled
			if (!SabineData.Features.IsEnabled("JobLevels"))
				return;

			var exp = this.Parameters.JobExp;
			var level = this.Parameters.JobLevel;
			var expNeeded = this.Parameters.JobExpNeeded;
			var maxLevel = SabineData.ExpTables.GetMaxLevel(ExpTableType.Job, this.JobId);
			var levelsGained = 0;

			exp = Math2.AddChecked(exp, amount);
			if (exp < 0)
				exp = 0;

			while (level < maxLevel && exp >= expNeeded)
			{
				exp -= expNeeded;

				level++;
				levelsGained++;

				expNeeded = SabineData.ExpTables.GetExpNeeded(ExpTableType.Job, this.JobId, level);
			}

			if (levelsGained != 0)
			{
				this.Parameters.Set(ParameterType.JobLevel, level);
				this.Parameters.Set(ParameterType.JobExpNeeded, expNeeded);
				this.Parameters.Modify(ParameterType.SkillPoints, levelsGained);

				// The alpha client offers no way to update the job level.
				// It's only set once, on login, based on the data given
				// to it by the char server, so we should inform the player
				// about reaching the next level if job leveling is enabled.
				this.ServerMessage(Localization.Get("You have reached job level {0}."), level);
			}

			this.Parameters.Set(ParameterType.JobExp, exp);
		}

		/// <summary>
		/// Restores characters HP and SP and heals any negative status
		/// effects.
		/// </summary>
		public void Heal()
		{
			this.Heal(this.Parameters.HpMax, this.Parameters.SpMax);
		}

		/// <summary>
		/// Heals the given amount of HP and SP.
		/// </summary>
		/// <param name="amountHp"></param>
		/// <param name="amountSp"></param>
		public void Heal(int amountHp, int amountSp)
		{
			this.HealHp(amountHp);
			this.HealSp(amountSp);
		}

		/// <summary>
		/// Heals the given amount of HP.
		/// </summary>
		/// <param name="amount"></param>
		public void HealHp(int amount)
		{
			this.Parameters.Modify(ParameterType.Hp, amount);
		}

		/// <summary>
		/// Heals the given amount of SP.
		/// </summary>
		/// <param name="amount"></param>
		public void HealSp(int amount)
		{
			this.Parameters.Modify(ParameterType.Hp, amount);
		}
	}
}
