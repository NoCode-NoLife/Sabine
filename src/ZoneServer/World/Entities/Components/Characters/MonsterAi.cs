using System;
using System.Collections;
using Sabine.Shared.World;
using Sabine.Zone.Network;
using Yggdrasil.Ai.Enumerable;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Controls a monster.
	/// </summary>
	public class MonsterAi : EnumerableAi, ICharacterComponent
	{
		//private Position _initialPosition;
		//private bool _initialPositionSet;

		private readonly int _wanderMinDistance = 3;

		/// <summary>
		/// Returns the character this component belongs to.
		/// </summary>
		public Character Character { get; set; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="monster"></param>
		public MonsterAi(Monster monster)
		{
			this.Character = monster;
		}

		/// <summary>
		/// Updates the AI, letting the character make a decision.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (this.Character.IsDead)
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
		private IEnumerable Idle()
		{
			foreach (var result in this.Wait(3000, 10000))
				yield return result;

			foreach (var result in this.Wander(5))
				yield return result;
		}

		/// <summary>
		/// Makes character wander around in range around its position.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		private IEnumerable Wander(int range)
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
		private bool TryFindWanderPosition(int range, out Position pos)
		{
			pos = new Position(0, 0);

			for (var i = 0; i < 100; ++i)
			{
				pos = this.Character.Position.GetRandomInRange(2, range);

				if (this.Character.Map.IsPassable(pos))
					return true;
			}

			return false;
		}

		/// <summary>
		/// Let's character say something.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		private IEnumerable Say(string message)
		{
			Send.ZC_NOTIFY_CHAT(this.Character, "wander...");
			yield break;
		}
	}
}
