namespace Sabine.Shared
{
	/// <summary>
	/// Global accessor for the application settings.
	/// </summary>
	public static class Game
	{
		/// <summary>
		/// The packet version to use.
		/// </summary>
		public static int Version = 2;
	}

	/// <summary>
	/// A list of known packet versions.
	/// </summary>
	public static class Versions
	{
		/// <summary>
		/// The packet version for the iRO Alpha client (2001-08-30).
		/// </summary>
		public const int Alpha = 0;

		/// <summary>
		/// The packet version for the iRO Beta 1 client (2002-02-20).
		/// </summary>
		public const int Beta1 = 2;

		/// <summary>
		/// The packet version for the jRO Beta 2 client (2002-08-09).
		/// </summary>
		public const int Beta2 = 3;
	}

	/// <summary>
	/// Sizes of various elements in packets.
	/// </summary>
	public static class Sizes
	{
		/// <summary>
		/// Size of fixed-sized character name strings.
		/// </summary>
		public static int CharacterNames = Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized item name strings.
		/// </summary>
		public static int ItemNames = Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized username strings.
		/// </summary>
		public static int Usernames = Game.Version < Versions.Beta1 ? 16 : 24;
	}
}
