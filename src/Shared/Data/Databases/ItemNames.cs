using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents a map's data.
	/// </summary>
	public class ItemNameData
	{
		public int Id { get; set; }
		public string AlphaName { get; set; }
		public string BetaName { get; set; }
		public string KoreanName { get; set; }
	}

	/// <summary>
	/// An item name database.
	/// </summary>
	public class ItemNameDb : DatabaseJsonIndexed<int, ItemNameData>
	{
		/// <summary>
		/// Called to read an entry from the map database file.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id");

			var data = new ItemNameData();

			data.Id = entry.ReadInt("id");
			data.AlphaName = entry.ReadString("alphaName", null);
			data.BetaName = entry.ReadString("betaName", null);
			data.KoreanName = entry.ReadString("koreanName", null);

			if (string.IsNullOrWhiteSpace(data.AlphaName))
				data.AlphaName = null;

			if (string.IsNullOrWhiteSpace(data.BetaName))
				data.BetaName = null;

			if (string.IsNullOrWhiteSpace(data.KoreanName))
				data.KoreanName = null;

			this.AddOrReplace(data.Id, data);
		}
	}
}
