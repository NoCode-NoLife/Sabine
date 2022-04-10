namespace Sabine.Shared.Const
{
	/// <summary>
	/// Specifies why a character couldn't be created.
	/// </summary>
	public enum CharCreateError : byte
	{
		/// <summary>
		/// The character named already existed.
		/// </summary>
		NameExistsAlready = 0,

		/// <summary>
		/// General error.
		/// </summary>
		Denied = 1,
	}
}
