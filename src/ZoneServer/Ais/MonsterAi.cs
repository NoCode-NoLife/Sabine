using System;
using System.Collections;
using Sabine.Shared.Const;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Entities.Components.Characters;
using Yggdrasil.Ai.Enumerable;

namespace Sabine.Zone.Ais
{
	/// <summary>
	/// Controls a monster.
	/// </summary>
	public abstract class MonsterAi : EnumerableAi, ICharacterComponent
	{
		//private Position _initialPosition;
		//private bool _initialPositionSet;

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

			this.Heartbeat();
		}

		/// <summary>
		/// Called to select a stat routine if none are active.
		/// </summary>
		protected override void Root()
		{
			this.StartRoutine(this.Idle());
		}

		/// <summary>
		/// Runs the idle state routine.
		/// </summary>
		/// <returns></returns>
		protected abstract IEnumerable Idle();

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

			this.Character.Controller.MoveTo(pos);

			while (this.Character.Controller.IsMoving)
				yield return true;
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
	}
}
