namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines the action taken when a character "restarts" after death.
	/// </summary>
	public enum RestartType : byte
	{
		/// <summary>
		/// The character will respawn at the last save point.
		/// </summary>
		SavePoint = 0,

		/// <summary>
		/// The character will be taken to the character selection screen.
		/// </summary>
		CharacterSelection = 1,
	}
}
