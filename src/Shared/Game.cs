namespace Sabine.Shared
{
	public static class Game
	{
		public static int Version = 2;
	}

	public static class Versions
	{
		public const int Alpha = 0;
		public const int Beta1 = 2;
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
