using System.Collections;

#pragma warning disable IDE0009

namespace Sabine.Zone.Ais.Impl
{
	/// <summary>
	/// AI Type 01: Passive
	/// </summary>
	/// <remarks>
	/// Aegis: 01
	/// Athena: 0x0081 (MD_CANMOVE|MD_CANATTACK)
	/// </remarks>
	[Ai("Type01")]
	public class Type01 : MonsterAi
	{
		protected override void Start()
		{
			this.StartRoutine("Idle", this.Idle());
		}

		protected IEnumerable Idle()
		{
			yield return Wait(3000, 10000);
			yield return Wander(5);
		}
	}
}

#pragma warning restore IDE0009
