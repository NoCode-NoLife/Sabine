using System.Collections;
using Sabine.Shared.Const;
using Sabine.Shared.World;

#pragma warning disable IDE0009

namespace Sabine.Zone.Ais.Impl
{
	/// <summary>
	/// AI Type 02: Passive Looter
	/// </summary>
	/// <remarks>
	/// Aegis: 02
	/// Athena: 0x0083 (MD_CANMOVE|MD_LOOTER|MD_CANATTACK)
	/// </remarks>
	[Ai("Type02")]
	public class Type02 : MonsterAi
	{
		private int _targetItemHandle;

		protected override void Init()
		{
			During("Idle", CheckNearbyItems);
			During("PickUpItem", CheckTargetItem);
		}

		protected override void Start()
		{
			StartRoutine("Idle", Idle());
		}

		private IEnumerable Idle()
		{
			while (true)
			{
				yield return Wait(3000, 10000);
				yield return Wander(5);
			}
		}

		private IEnumerable PickUpItem(int handle, Position pos)
		{
			_targetItemHandle = handle;

			if (Chance(15))
				yield return Emotion(EmotionId.MusicNote);

			yield return MoveTo(pos);
			yield return PickUp(handle);
		}

		private void CheckNearbyItems(CallbackState state)
		{
			if (TryFindNearbyItem(out var handle, out var pos))
				StartRoutine("PickUpItem", PickUpItem(handle, pos));
		}

		private void CheckTargetItem(CallbackState state)
		{
			if (!EntityExists(_targetItemHandle))
			{
				_targetItemHandle = 0;

				StopMove();
				StartRoutine("Idle", Idle());
			}
		}
	}
}

#pragma warning restore IDE0009
