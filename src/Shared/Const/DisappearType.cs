namespace Sabine.Shared.Const
{
	/// <summary>
	/// Specifies how a character disappears from a map.
	/// </summary>
	public enum DisappearType : byte
	{
		/// <summary>
		/// It simply disappears.
		/// </summary>
		Vanish = 0,

		/// <summary>
		/// It falls to the ground dead (doesn't disappear).
		/// </summary>
		StrikedDead = 1,

		/// <summary>
		/// Disappears with heal/warp effect.
		/// </summary>
		Effect = 2,
	}
}
