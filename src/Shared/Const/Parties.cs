namespace Sabine.Shared.Const
{
	/// <summary>
	/// Result for a party creation attempt.
	/// </summary>
	public enum PartyCreationResult : byte
	{
		/// <summary>
		/// Party was created successfully.
		/// </summary>
		Success = 0,

		/// <summary>
		/// Party name already exists.
		/// </summary>
		NameAlreadyExists = 1,

		/// <summary>
		/// Player is already in a party.
		/// </summary>
		AlreadyInParty = 2,
	}
}
