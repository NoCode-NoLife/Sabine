namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines what a character wants to do.
	/// </summary>
	public enum ActionType : byte
	{
		/// <summary>
		/// Attack once? Only seen in Alpha so far.
		/// </summary>
		Attack = 0,

		// 1

		/// <summary>
		/// Sit down.
		/// </summary>
		SitDown = 2,

		/// <summary>
		/// Stand up.
		/// </summary>
		StandUp = 3,

		// 4
		// 5
		// 6

		/// <summary>
		/// Attack continiously? Sent from Beta1 onwards.
		/// </summary>
		AutoAttack = 7,
	}
}
