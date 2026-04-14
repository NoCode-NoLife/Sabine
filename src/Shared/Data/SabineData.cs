using Sabine.Shared.Data.Databases;

namespace Sabine.Shared.Data
{
	/// <summary>
	/// Interface providing access to all of the server's data.
	/// </summary>
	public class SabineData
	{
		/// <summary>
		/// db/exp.txt
		/// </summary>
		public ExpDb ExpTables { get; } = new();

		/// <summary>
		/// db/features.txt
		/// </summary>
		public FeatureDb Features { get; } = new();

		/// <summary>
		/// db/items.txt
		/// </summary>
		public ItemDb Items { get; } = new();

		/// <summary>
		/// db/item_names.txt
		/// </summary>
		public ItemNameDb ItemNames { get; } = new();

		/// <summary>
		/// db/jobs.txt
		/// </summary>
		public JobDb Jobs { get; } = new();

		/// <summary>
		/// db/map_cache.dat
		/// </summary>
		public MapCacheDb MapCache { get; } = new();

		/// <summary>
		/// db/maps.txt
		/// </summary>
		public MapsDb Maps { get; } = new();

		/// <summary>
		/// db/monsters.txt
		/// </summary>
		public MonsterDb Monsters { get; } = new();
	}
}
