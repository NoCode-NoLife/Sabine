using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents a map's data.
	/// </summary>
	public class MapsData
	{
		public int Id { get; set; }
		public string StringId { get; set; }
		public string Name { get; set; }
	}

	/// <summary>
	/// A map database.
	/// </summary>
	public class MapsDb : DatabaseJsonIndexed<int, MapsData>
	{
		/// <summary>
		/// Returns the data for the map with the given string id,
		/// or null if it wasn't found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <returns></returns>
		public MapsData Find(string stringId)
			=> this.Find(a => string.Compare(a.StringId, stringId, true) == 0);

		/// <summary>
		/// Returns the data for the map with the given string id via
		/// out. Returns false if the map wasn't found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool TryFind(string stringId, out MapsData data)
		{
			data = this.Find(stringId);
			return data != null;
		}

		/// <summary>
		/// Called to read an entry from the map database file.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			var data = new MapsData();

			data.Id = entry.ReadInt("id");
			data.StringId = entry.ReadString("stringId");
			data.Name = entry.ReadString("name");

			this.AddOrReplace(data.Id, data);
		}
	}
}
