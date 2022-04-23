using System;
using System.Collections;
using Sabine.Shared.World;
using Yggdrasil.Ai.Enumerable;

namespace Sabine.Zone.World.Entities.Components.Characters
{
	/// <summary>
	/// Controls a monster.
	/// </summary>
	public class MonsterAi : EnumerableAi, ICharacterComponent
	{
		private Position _initialPosition;
		private readonly int _wanderAwayDistance = 10;

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
			_initialPosition = this.Character.Position;
		}

		/// <summary>
		/// Updates the AI, letting the character make a decision.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			if (this.Character.IsDead)
				return;

			if (this.Character.IsStunned)
			{
				this.ClearRoutine();
				return;
			}

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
			var pos = this.FindWanderPosition(range);
			this.Character.Controller.MoveTo(pos);

			while (this.Character.Controller.IsMoving)
				yield return true;
		}

		/// <summary>
		/// Finds a passable position in range around the character that
		/// it can wander towards.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		private Position FindWanderPosition(int range)
		{
			for (var i = 0; i < 100; ++i)
			{
				var pos = this.Character.Position.GetRandomInSquareRange(range);
				if (pos == this.Character.Position)
					continue;

				if (!pos.InSquareRange(_initialPosition, _wanderAwayDistance))
					continue;

				if (this.Character.Map.IsPassable(pos))
					return pos;
			}

			return this.Character.Position;
		}
	}
}
