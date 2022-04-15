namespace Sabine.Shared.Const
{
	/// <summary>
	/// Result of a whisper request.
	/// </summary>
	public enum WhisperResult : byte
	{
		/// <summary>
		/// Positive response makes client display the whisper message
		/// the player sent.
		/// </summary>
		Okay = 0,

		/// <summary>
		/// Shows a message that the character doesn't exist.
		/// </summary>
		CharacterDoesntExist = 1,
	}
}
