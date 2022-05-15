using System.Collections;

#pragma warning disable IDE0009

namespace Sabine.Zone.Ais.Impl
{
	/// <summary>
	/// Type 01 AI: Passive.
	/// </summary>
	/// <remarks>
	/// Aegis: 02
	/// eAthena: MD_CANMOVE|MD_CANATTACK
	/// </remarks>
	[Ai("Type01")]
	public class Type01 : MonsterAi
	{
		protected override IEnumerable Idle()
		{
			yield return Wait(3000, 10000);
			yield return Wander(5);
		}
	}
}

#pragma warning restore IDE0009
