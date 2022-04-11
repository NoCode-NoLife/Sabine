using Sabine.Shared.Data.Databases;

namespace Sabine.Shared.Data
{
	/// <summary>
	/// Interface providing access to all of the server's data.
	/// </summary>
	public static class SabineData
	{
		/// <summary>
		/// db/maps.txt
		/// </summary>
		public static MapsDb Maps { get; } = new MapsDb();
	}
}
