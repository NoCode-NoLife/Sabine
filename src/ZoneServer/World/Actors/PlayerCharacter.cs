using System;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.L10N;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Actors.Components.Characters;
using Shared.Const;
using Yggdrasil.Collections;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Actors
{
	/// <summary>
	/// Represents a player character.
	/// </summary>
	public class PlayerCharacter : Character
	{
		private readonly object _visibilitySyncLock = new();
		private readonly InOutTracker<IActor> _visibleActors = new();

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
		public override IdentityId IdentityId
		{
			get => (IdentityId)this.JobId;
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
		/// Gets or sets the character's save location, where they respawn
		/// upon death.
		/// </summary>
		public Location SaveLocation { get; set; }

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
		/// Gets or sets the id of the chat room the character is in.
		/// </summary>
		public int ChatRoomId { get; set; }

		/// <summary>
		/// Gets or sets the item class id currently designated as ammo
		/// for the character.
		/// </summary>
		public int AmmoClassId { get; set; }

		/// <summary>
		/// Creates a new character.
		/// </summary>
		public PlayerCharacter(JobId jobId)
		{
			this.JobId = jobId;
			this.Inventory = new Inventory(this);

			this.Parameters = new PlayerCharacterParameters(this);
			this.Components.Add(new RegenComponent(this));

			this.LoadJobData(jobId);
		}

		/// <summary>
		/// Loads the data for the given job.
		/// </summary>
		/// <param name="jobId"></param>
		/// <exception cref="ArgumentException"></exception>
		private void LoadJobData(JobId jobId)
		{
			if (!ZoneServer.Instance.Data.Jobs.TryFind(jobId, out var jobData))
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
		/// Sends a debug message to the character's client that is
		/// displayed in the chat log.
		/// </summary>
		/// <param name="format"></param>
		/// <param name="args"></param>
		public void DebugMessage(string format, params object[] args)
		{
			if (args.Length > 0)
				format = string.Format(format, args);

			var message = string.Format(Localization.Get("[Debug] : {0}"), format);

			Send.ZC_NOTIFY_CHAT(this, 0, message);
		}

		/// <summary>
		/// Warps character to given location.
		/// </summary>
		/// <param name="location"></param>
		public override void Warp(Location location)
		{
			if (this.IsWarping)
			{
				if (location != this.WarpLocation)
					throw new InvalidOperationException("A warp is already in progress.");

				// If we get two warp calls for the same location, we're going
				// to assume it was accidental and ignore the second one.
				Log.Debug("Encountered a double warp. Stacktrace: {0}", Environment.StackTrace);
				return;
			}

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

			this.Map.RemovePlayer(this);
			this.SetLocation(this.WarpLocation);
			map.AddPlayer(this);

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
		/// Starts updating of visible entities. A visibility update is
		/// executed when this method is called.
		/// </summary>
		public void StartObserving()
		{
			lock (_visibilitySyncLock)
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
			lock (_visibilitySyncLock)
			{
				if (!this.IsObserving)
					return;

				this.IsObserving = false;
				this.RemoveVisibleActors();
			}
		}

		/// <summary>
		/// Updates visible entities around character.
		/// </summary>
		internal void UpdateVisibility()
		{
			lock (_visibilitySyncLock)
			{
				if (!this.IsObserving)
					return;

				_visibleActors.Begin();

				this.Map.GetVisibleActors(this, _visibleActors.UpdateList);

				_visibleActors.Update();

				foreach (var actor in _visibleActors.Added)
				{
					switch (actor)
					{
						case Character character:
						{
							Send.ZC_NOTIFY_STANDENTRY(this, character);

							// TODO: Cache chat ownership on player?
							if (character is PlayerCharacter player)
							{
								if (player.ChatRoomId != 0 && ZoneServer.Instance.World.ChatRooms.TryGet(player.ChatRoomId, out var room))
								{
									if (room.IsOwner(player))
										Send.ZC_ROOM_NEWENTRY(room);
								}
							}

							break;
						}

						case Item item:
						{
							Send.ZC_ITEM_ENTRY(this, item);
							break;
						}
					}
				}

				foreach (var actor in _visibleActors.Removed)
				{
					if (actor is Item)
						Send.ZC_ITEM_DISAPPEAR(this, actor.Handle);
					else
						Send.ZC_NOTIFY_VANISH(this, actor.Handle, DisappearType.Vanish);
				}

				_visibleActors.End();
			}
		}

		/// <summary>
		/// Adds actor to list of character's visible entities without
		/// updating the client.
		/// </summary>
		/// <remarks>
		/// AddVisibleEntity and RemoveVisibleEntity are to be used in
		/// cases where an outside source needs to control an entity's
		/// appear or disappear packets.
		/// </remarks>
		/// <param name="actor"></param>
		internal void AddVisibleActor(IActor actor)
		{
			if (!this.IsObserving)
				return;

			lock (_visibilitySyncLock)
				_visibleActors.InjectItem(actor);
		}

		/// <summary>
		/// Removes entity from list of character's visible entities
		/// without updating the client.
		/// </summary>
		/// <remarks>
		/// AddVisibleEntity and RemoveVisibleEntity are to be used in
		/// cases where an outside source needs to control an entity's
		/// appear or disappear packets.
		/// </remarks>
		/// <param name="actor"></param>
		internal void RemoveVisibleActor(IActor actor)
		{
			lock (_visibilitySyncLock)
			{
				if (!this.IsObserving)
					return;

				_visibleActors.EjectItem(actor);
			}
		}

		/// <summary>
		/// Clears the list of visible actors and updates the client.
		/// </summary>
		private void RemoveVisibleActors()
		{
			lock (_visibilitySyncLock)
			{
				foreach (var actor in _visibleActors.Current)
				{
					if (actor is Item)
						Send.ZC_ITEM_DISAPPEAR(this, actor.Handle);
					else
						Send.ZC_NOTIFY_VANISH(this, actor.Handle, DisappearType.Vanish);
				}

				_visibleActors.ClearItems();
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

			if (Game.Version < Versions.EP3_2)
				Send.ZC_SPRITE_CHANGE(this, type, lookId);
			else
				Send.ZC_SPRITE_CHANGE2(this, type, lookId, 0);
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

			if (Game.Version < Versions.EP3_2)
				Send.ZC_SPRITE_CHANGE(this, SpriteType.Class, (int)jobId);
			else
				Send.ZC_SPRITE_CHANGE2(this, SpriteType.Class, (int)jobId, 0);

			// Send a BaseLevel change packet to get the level up animation
			Send.ZC_PAR_CHANGE(this, ParameterType.BaseLevel);

			this.Skills.UpdateClassSkills();
		}

		/// <summary>
		/// Returns the character's current attack range, based on its
		/// state and equipped items.
		/// </summary>
		/// <returns></returns>
		public override int GetAttackRange()
		{
			// Range is 3 for normal attacks and 16 for ranged
			// in the alpha client. This is hardcoded, based on
			// the type of the item that was equipped.
			if (Game.Version < Versions.Beta1)
			{
				if (this.Inventory.RightHand?.Type == ItemType.RangedWeapon)
					return 16;

				return 3;
			}

			// If you're coming here to check on the attack range of bows
			// in Beta1, I can tell you that the behavior you're
			// witnessing appears correct. While their range was hardcoded
			// to 16 in the alpha, all the data I saw suggests that all
			// bows had a range of 5 in the beta. That's what 2003 servers
			// used and the db websites of the time don't mention ranges
			// or range differences. But the range can be changed in the
			// item data, so all is good in the world.

			return this.Inventory.RightHand?.Data.AttackRange ?? 3;
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
			var maxLevel = ZoneServer.Instance.Data.ExpTables.GetMaxLevel(ExpTableType.Base, this.JobId);
			var levelsGained = 0;
			var statPointsGained = 0;

			exp = Math.Max(0, Math2.AddChecked(exp, amount));

			while (level < maxLevel && exp >= expNeeded)
			{
				exp -= expNeeded;

				level++;
				levelsGained++;
				statPointsGained += (level / 5) + 2;

				expNeeded = ZoneServer.Instance.Data.ExpTables.GetExpNeeded(ExpTableType.Base, this.JobId, level);
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
			if (!ZoneServer.Instance.Data.Features.IsEnabled("JobLevels"))
				return;

			var exp = this.Parameters.JobExp;
			var level = this.Parameters.JobLevel;
			var expNeeded = this.Parameters.JobExpNeeded;
			var maxLevel = ZoneServer.Instance.Data.ExpTables.GetMaxLevel(ExpTableType.Job, this.JobId);
			var levelsGained = 0;

			exp = Math2.AddChecked(exp, amount);
			if (exp < 0)
				exp = 0;

			while (level < maxLevel && exp >= expNeeded)
			{
				exp -= expNeeded;

				level++;
				levelsGained++;

				expNeeded = ZoneServer.Instance.Data.ExpTables.GetExpNeeded(ExpTableType.Job, this.JobId, level);
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
			this.Parameters.Modify(ParameterType.Sp, amount);
		}

		/// <summary>
		/// Kills the character.
		/// </summary>
		/// <param name="killer"></param>
		public override void Kill(Character killer)
		{
			base.Kill(killer);

			Send.ZC_NOTIFY_VANISH(this, DisappearType.StrikedDead);
		}
	}
}
