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
		protected override void Start()
		{
			StartRoutine("Idle", Idle());
		}

		protected override void Update()
		{
			if (CurrentRoutine == "Idle")
			{
				if (TryFindNearbyItem(out var handle, out var pos))
					StartRoutine("PickUpItem", PickUpItem(handle, pos));
			}
		}

		protected override IEnumerable Idle()
		{
			yield return Wait(3000, 10000);
			yield return Wander(5);
		}

		private IEnumerable PickUpItem(int handle, Position pos)
		{
			if (Random(50) < 100)
				yield return Emotion(EmotionId.MusicNote);

			yield return MoveTo(pos);
			yield return PickUp(handle);
		}
	}
}

#pragma warning restore IDE0009
