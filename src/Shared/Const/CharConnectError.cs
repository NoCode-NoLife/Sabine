namespace Sabine.Shared.Const
{
	/// <summary>
	/// Response code for refusing connection to char server.
	/// </summary>
	public enum CharConnectError : byte
	{
		/// <summary>
		/// Shows message "Client language is different from server".
		/// </summary>
		LanguageIncorrect = 0,

		/// <summary>
		/// Shows message "Access Denied".
		/// </summary>
		AccessDenied = byte.MaxValue,
	}
}
