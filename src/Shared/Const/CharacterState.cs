namespace Shared.Const
{
	/// <summary>
	/// Specifies a character's state.
	/// </summary>
	public enum CharacterState : byte
	{
		/// <summary>
		/// Character is standing.
		/// </summary>
		Standing = 0,

		/// <summary>
		/// Character is lying on the floor, dead.
		/// </summary>
		Dead = 1,

		/// <summary>
		/// Character is sitting.
		/// </summary>
		Sitting = 2,
	}
}
