using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Entities.Components.Characters;
using Yggdrasil.Ai.Enumerable;
using Yggdrasil.Logging;

namespace Sabine.Zone.Ais
{
	/// <summary>
	/// Controls a monster.
	/// </summary>
	public abstract class MonsterAi : EnumerableAi, ICharacterComponent
	{
		//private Position _initialPosition;
		//private bool _initialPositionSet;

		private bool _initiated;

		private readonly int _wanderMinDistance = 3;
		private readonly Dictionary<string, List<CallbackFunc>> _durings = new Dictionary<string, List<CallbackFunc>>();

		/// <summary>
		/// Returns the character this component belongs to.
		/// </summary>
		public Character Character { get; internal set; }

		/// <summary>
		/// Updates the AI, letting the character make a decision.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (this.Character.IsDead)
				return;

			// Monsters can chill if there's no player nearby.
			// TODO: Limit to visible range?
			if (this.Character.Map.PlayerCount == 0)
				return;

			//if (!_initialPositionSet)
			//{
			//	_initialPositionSet = true;
			//	_initialPosition = this.Character.Position;
			//}

			if (!_initiated)
			{
				this.Init();
				_initiated = true;
			}
			else
			{
				this.Update();
			}

			this.RunDuringCallbacks(this.CurrentRoutine);

			this.Heartbeat();
		}

		/// <summary>
		/// Returns the name of the currently curring routine.
		/// </summary>
		protected string CurrentRoutine { get; private set; }

		/// <summary>
		/// Starts given routine.
		/// </summary>
		/// <param name="routine"></param>
		protected override void StartRoutine(IEnumerable routine)
		{
			var type = routine.GetType();
			var name = type.GetType().Name;

			this.StartRoutine("Unknown", routine);
		}

		/// <summary>
		/// Starts given routine.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="routine"></param>
		protected void StartRoutine(string name, IEnumerable routine)
		{
			this.CurrentRoutine = name;
			base.StartRoutine(routine);
		}

		/// <summary>
		/// Sets up a callback that is called on every tick while a routine
		/// with the given name is active.
		/// </summary>
		/// <param name="routineName"></param>
		/// <param name="callback"></param>
		protected void During(string routineName, CallbackFunc callback)
		{
			if (!_durings.TryGetValue(routineName, out var list))
				_durings[routineName] = list = new List<CallbackFunc>();

			if (!list.Contains(callback))
				list.Add(callback);
		}

		/// <summary>
		/// Executes the given callbacks.
		/// </summary>
		/// <param name="callbacks"></param>
		private void RunDuringCallbacks(string routineName)
		{
			if (routineName == null)
				return;

			if (_durings.TryGetValue(routineName, out var callbacks))
				this.RunCallbacks(routineName, callbacks);
		}

		/// <summary>
		/// Executes the given callbacks.
		/// </summary>
		/// <param name="callbacks"></param>
		private void RunCallbacks(string routineName, List<CallbackFunc> callbacks)
		{
			if (callbacks == null || callbacks.Count == 0)
				return;

			var state = new CallbackState();

			foreach (var callback in callbacks)
			{
				try
				{
					callback(state);
				}
				catch (Exception ex)
				{
					Log.Error("MonsterAi.RunCallbacks: Error while running callbacks for '{0}': {1}", routineName, ex);
				}
			}
		}

		/// <summary>
		/// Called to select a state routine if none are active.
		/// </summary>
		protected override void Root()
			=> this.Start();

		/// <summary>
		/// Called before the AI is started for the first time.
		/// </summary>
		protected virtual void Init()
		{
		}

		/// <summary>
		/// Called when there are no active routines.
		/// </summary>
		protected abstract void Start();

		/// <summary>
		/// Called on every tick of the AI, before any actions take place.
		/// </summary>
		protected virtual void Update()
		{
		}

		/// <summary>
		/// Does nothing but wait.
		/// </summary>
		/// <returns></returns>
		private IEnumerable DummyRoutine()
		{
			foreach (var _ in this.Wait(9999999))
				yield return true;
		}

		/// <summary>
		/// Makes character wander around in range around its position.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		protected IEnumerable Wander(int range)
		{
			if (this.Character.IsStunned)
				yield break;

			if (range < _wanderMinDistance)
				range = _wanderMinDistance;

			if (!this.TryFindWanderPosition(range, out var pos))
				yield break;

			var moveTime = this.Character.Controller.MoveTo(pos);

			foreach (var _ in this.Wait(moveTime))
			{
				if (!this.Character.Controller.IsMoving)
					break;

				yield return true;
			}
		}

		/// <summary>
		/// Finds a passable position in range around the character that
		/// it can wander to.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		protected bool TryFindWanderPosition(int range, out Position wanderPos)
		{
			var curPos = this.Character.Position;
			wanderPos = new Position(0, 0);

			// TODO: Get a random passable from the tile data?
			for (var i = 0; i < 100; ++i)
			{
				wanderPos = curPos.GetRandomInRange(_wanderMinDistance, range);

				if (!this.Character.Map.IsPassable(wanderPos))
					continue;

				if (this.Character.Map.PathFinder.PathExists(curPos, wanderPos))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Returns the entity with the given handle on the character's map.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		protected bool TryGetEntity(int handle, out IEntity entity)
		{
			entity = this.Character.Map.GetEntity(handle);
			return entity != null;
		}

		/// <summary>
		/// Returns true if an entity with the given handle exists on the
		/// character's map.
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		protected bool EntityExists(int handle)
		{
			return this.TryGetEntity(handle, out _);
		}

		/// <summary>
		/// Makes character move to the given position.
		/// </summary>
		/// <param name="pos"></param>
		/// <returns></returns>
		protected IEnumerable MoveTo(Position pos)
		{
			var character = this.Character;

			if (!character.Map.IsPassable(pos))
				yield break;

			if (!character.Map.PathFinder.PathExists(character.Position, pos))
				yield break;

			var moveTime = character.Controller.MoveTo(pos);

			foreach (var _ in this.Wait(moveTime))
			{
				if (!this.Character.Controller.IsMoving)
					break;

				yield return true;
			}
		}

		/// <summary>
		/// Stops character's movement.
		/// </summary>
		protected void StopMove()
		{
			this.Character.Controller.StopMove();
		}

		/// <summary>
		/// Let's character say something.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		protected IEnumerable Say(string message)
		{
			Send.ZC_NOTIFY_CHAT(this.Character, message);
			yield break;
		}

		/// <summary>
		/// Let's character use an emote.
		/// </summary>
		/// <param name="emotionId"></param>
		/// <returns></returns>
		protected IEnumerable Emotion(EmotionId emotionId)
		{
			Send.ZC_EMOTION(this.Character, emotionId);
			yield break;
		}

		/// <summary>
		/// Tries to find a nearby item that the character can pick up
		/// and returns its handle via out. Returns false if no item was
		/// found.
		/// </summary>
		/// <param name="itemHandle"></param>
		/// <returns></returns>
		protected bool TryFindNearbyItem(out int itemHandle, out Position pos)
		{
			var character = this.Character;
			var characterPos = character.Position;
			var range = character.Map.VisibleRange / 2;

			var items = character.Map.GetItems(a => a.Position.InRange(characterPos, range));
			if (items.Length == 0)
			{
				itemHandle = 0;
				pos = Position.Zero;
				return false;
			}

			var item = items.OrderBy(a => a.Position.GetDistance(characterPos)).First();

			itemHandle = item.Handle;
			pos = item.Position;

			return true;
		}

		/// <summary>
		/// Makes character pick up the item with the given handle.
		/// </summary>
		/// <param name="itemHandle"></param>
		/// <returns></returns>
		protected IEnumerable PickUp(int itemHandle)
		{
			var item = this.Character.Map.GetItem(itemHandle);
			if (item == null)
				yield break;

			item.Map.RemoveItem(item);

			if (this.Character is Monster monster)
				monster.AddDropItem(item);
			else if (this.Character is PlayerCharacter player)
				player.Inventory.AddItem(item);
		}

		/// <summary>
		/// Follows and attacks the character with the given handle.
		/// Returns once the target is gone. One way... or another!
		/// </summary>
		/// <param name="handle"></param>
		/// <returns></returns>
		protected IEnumerable HuntDown(int handle)
		{
			var attacker = this.Character;
			var attackDelay = attacker.Parameters.AttackDelay;
			var attackRange = ((attacker as Monster)?.Data.AttackRange ?? 1);
			var chaseRange = (attacker as Monster)?.Data.ChaseRange ?? 12;

			while (true)
			{
				if (!attacker.Map.TryGetCharacter(handle, out var target) || target.IsDead)
					yield break;

				foreach (var _ in this.Wait(attackDelay))
					yield return true;

				while (!attacker.Position.InRange(target.Position, attackRange))
				{
					if (!attacker.Position.InRange(target.Position, chaseRange))
						yield break;

					attacker.Controller.MoveTo(target.Position);
					yield return true;
				}

				attacker.Controller.StopMove();
				attacker.StartAttacking(target, false);
			}
		}
	}
}
