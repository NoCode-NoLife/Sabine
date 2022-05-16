using System;
using System.Collections;
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
	}
}
