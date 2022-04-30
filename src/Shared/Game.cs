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
		public static int Version = 0;
	}

	/// <summary>
	/// A list of known packet versions.
	/// </summary>
	public static class Versions
	{
		// TODO: Just decided that custom version numbers would be a good
		//   idea, and now I'm confronted with the problem of what to name
		//   known versions, after I found that EP3 jRO and EP3 iRO don't
		//   have he same packet table. Granted, the difference is minor,
		//   but there is one, so... sub-episode versions? 3.1, 3.2, etc?
		//   Meh. Not ideal. I'll leave it like this for now, but the
		//   known versions should probably be the literal clients, like
		//   "euRO_20040512". The purpose of this class is to be able to
		//   easily reference clients you know after all.

		/// <summary>
		/// The packet version for the iRO Alpha client (2001-08-30).
		/// </summary>
		public const int Alpha = 100;

		/// <summary>
		/// The packet version for the iRO Beta 1 client (2002-02-20).
		/// </summary>
		public const int Beta1 = 200;

		/// <summary>
		/// The packet version for the jRO Beta 2 client (2002-08-09).
		/// </summary>
		public const int Beta2 = 300;

		/// <summary>
		/// The packet version for the jRO EP3 client (2003-05-27).
		/// </summary>
		public const int EP3 = 400;

		/// <summary>
		/// The packet version for the iRO EP3-5 client (2003-10-31).
		/// </summary>
		public const int EP3_2 = 500;

		/// <summary>
		/// The packet version for the euRO EP4 client (2004-05-12).
		/// </summary>
		public const int EP4 = 600;

		/// <summary>
		/// The packet version for the iRO EP8 client (2004-08-03).
		/// </summary>
		public const int EP8 = 700;
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
