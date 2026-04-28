using System;

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
		public static int Version { get; set; } = Versions.Alpha;

		/// <summary>
		/// Returns the servers current tick time, indicating how many
		/// milliseconds have passed since a certain time.
		/// </summary>
		/// <returns></returns>
		public static int GetTick()
			=> Environment.TickCount;
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
		/// Sabine packet version 100, based on iRO Alpha 2001-08-30.
		/// </summary>
		public const int Alpha = 100;

		/// <summary>
		/// Sabine packet version 190, based on iRO Beta1 2001-12-14.
		/// </summary>
		public const int S190 = 190;

		/// <summary>
		/// Sabine packet version 200, based on iRO Beta 1 2002-02-20.
		/// </summary>
		public const int Beta1 = 200;

		/// <summary>
		/// Sabine packet version 300, based on jRO Beta 2 2002-08-09.
		/// </summary>
		public const int Beta2 = 300;

		/// <summary>
		/// Sabine packet version 350, based on iRO EP4 2003-04-30.
		/// </summary>
		public const int S350 = 350;

		/// <summary>
		/// Sabine packet version 400, based on jRO EP3 2003-05-27.
		/// </summary>
		public const int S400 = 400;

		/// <summary>
		/// Sabine packet version 500, based on iRO EP6 2003-10-31.
		/// </summary>
		public const int S500 = 500;

		/// <summary>
		/// Sabine packet version 600, based on euRO EP5 2004-05-12.
		/// </summary>
		public const int S600 = 600;

		/// <summary>
		/// Sabine packet version 700, based on iRO EP8 2004-08-03.
		/// </summary>
		public const int S700 = 700;

		/// <summary>
		/// Sabine packet version 800, based on bRO EP8 2004-12-28.
		/// </summary>
		public const int S800 = 800;

		/// <summary>
		/// Sabine packet version 900, based on bRO EP9 2006-01-31.
		/// </summary>
		public const int S900 = 900;

		/// <summary>
		/// Sabine packet version 2000, based on euRO EP10.4 2007-03-05.
		/// </summary>
		public const int S2000 = 2000;
	}

	/// <summary>
	/// Sizes of various elements in packets.
	/// </summary>
	public static class Sizes
	{
		/// <summary>
		/// Size of fixed-sized character name strings.
		/// </summary>
		public static int CharacterNames => Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized item name strings.
		/// </summary>
		public static int ItemNames => Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized username strings.
		/// </summary>
		public static int Usernames => Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized party name strings.
		/// </summary>
		public static int PartyNames => Game.Version < Versions.Beta1 ? 16 : 24;

		/// <summary>
		/// Size of fixed-sized skill name strings.
		/// </summary>
		public static int SkillNames => 24;
	}
}
