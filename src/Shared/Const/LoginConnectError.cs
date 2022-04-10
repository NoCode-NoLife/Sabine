namespace Sabine.Shared.Const
{
	/// <summary>
	/// Response code for refusing connection to login server.
	/// </summary>
	public enum LoginConnectError : byte
	{
		/// <summary>
		/// Shows message "Incorrect UserID".
		/// </summary>
		UserNotFound = 0,

		/// <summary>
		/// Shows message "Incorrect Password".
		/// </summary>
		PasswordIncorrect = 1,

		/// <summary>
		/// Shows message "ID expired".
		/// </summary>
		IdExpired = 2,

		/// <summary>
		/// Shows message "Access Denied".
		/// </summary>
		AccessDenied = byte.MaxValue,
	}
}
