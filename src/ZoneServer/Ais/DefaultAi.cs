using System.Collections;

namespace Sabine.Zone.Ais
{
	/// <summary>
	/// Default AI to use for monsters.
	/// </summary>
	public class DefaultAi : MonsterAi
	{
		/// <summary>
		/// Called while the monster is in the idle state.
		/// </summary>
		/// <returns></returns>
		protected override IEnumerable Idle()
		{
			yield return this.Wait(3000, 10000);
			yield return this.Wander(5);
			yield return this.Say("woopwoop");
		}
	}
}
