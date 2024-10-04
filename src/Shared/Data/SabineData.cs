using Sabine.Shared.Data.Databases;

namespace Sabine.Shared.Data
{
	/// <summary>
	/// Interface providing access to all of the server's data.
	/// </summary>
	public static class SabineData
	{
		/// <summary>
		/// db/exp.txt
		/// </summary>
		public static ExpDb ExpTables { get; } = new();

		/// <summary>
		/// db/features.txt
		/// </summary>
		public static FeatureDb Features { get; } = new();

		/// <summary>
		/// db/items.txt
		/// </summary>
		public static ItemDb Items { get; } = new();

		/// <summary>
		/// db/item_names.txt
		/// </summary>
		public static ItemNameDb ItemNames { get; } = new();

		/// <summary>
		/// db/jobs.txt
		/// </summary>
		public static JobDb Jobs { get; } = new();

		/// <summary>
		/// db/map_cache.dat
		/// </summary>
		public static MapCacheDb MapCache { get; } = new();

		/// <summary>
		/// db/maps.txt
		/// </summary>
		public static MapsDb Maps { get; } = new();

		/// <summary>
		/// db/monsters.txt
		/// </summary>
		public static MonsterDb Monsters { get; } = new();
	}
}
