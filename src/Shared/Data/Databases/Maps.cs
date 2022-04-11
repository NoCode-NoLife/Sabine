using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	public class MapsData
	{
		public int Id { get; set; }
		public string StringId { get; set; }
		public string Name { get; set; }
	}

	public class MapsDb : DatabaseJsonIndexed<int, MapsData>
	{
		public MapsData Find(string stringId)
			=> this.Find(a => string.Compare(a.StringId, stringId, true) == 0);

		public bool TryFind(string stringId, out MapsData data)
		{
			data = this.Find(stringId);
			return data != null;
		}

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
