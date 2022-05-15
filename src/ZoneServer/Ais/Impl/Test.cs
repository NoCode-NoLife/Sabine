using System.Collections;
using Sabine.Shared.Const;

#pragma warning disable IDE0009

namespace Sabine.Zone.Ais.Impl
{
	/// <summary>
	/// Default AI to use for monsters.
	/// </summary>
	[Ai("Test")]
	public class Test : MonsterAi
	{
		/// <summary>
		/// Called while the monster is in the idle state.
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable Idle()
		{
			yield return Wait(3000, 10000);

			SwitchRandom();
			if (Case(75))
			{
				yield return Emotion(EmotionId.MusicNote);
				yield return Wander(3);
			}
			else if (Case(20))
			{
				yield return Say("Whoop, whoop!");
				yield return Wander(6);
			}
			else if (Case(5))
			{
				yield return Say("...");
			}
		}
	}
}

#pragma warning restore IDE0009
