namespace Sabine.Shared.Const
{
	/// <summary>
	/// Specifies why a disconnect was requested.
	/// </summary>
	public enum DisconnectReason : byte
	{
		/// <summary>
		/// Doesn't display a message and simply closes the connection.
		/// </summary>
		NoReason = 0,

		/// <summary>
		/// Displays the message "Server Off" before closing the connection.
		/// </summary>
		ServerOff = 1,

		/// <summary>
		/// Displays the message "Double login probibited" before closing
		/// the connection.
		/// </summary>
		DoubleLoginProbibited = 2,
	}
}
