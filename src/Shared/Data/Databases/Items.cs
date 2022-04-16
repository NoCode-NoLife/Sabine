using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents an entry in the item data.
	/// </summary>
	public class ItemData
	{
		public int ClassId { get; set; }
		public string StringId { get; set; }
		public string Name { get; set; }
		public string KoreanName { get; set; }
		public ItemType Type { get; set; }
		public int Weight { get; set; }
		public int Price { get; set; }
		public int SellPrice { get; set; }
		public int Attack { get; set; }
		public int Defense { get; set; }
		public int AttackRange { get; set; }
		public JobFilter JobsAllowed { get; set; }
		public Sex SexAllowed { get; set; }
		public EquipSlots WearSlots { get; set; }
		public int RequiredLevel { get; set; }
		public int WeaponLevel { get; set; }
		public int LookId { get; set; }
	}

	/// <summary>
	/// An item database.
	/// </summary>
	public class ItemDb : DatabaseJsonIndexed<int, ItemData>
	{
		/// <summary>
		/// Returns the data for the entry with the given string id,
		/// or null if it wasn't found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <returns></returns>
		public ItemData Find(string stringId)
			=> this.Find(a => string.Compare(a.StringId, stringId, true) == 0);

		/// <summary>
		/// Returns the data for the entry with the given string id via
		/// out. Returns false if it wasn't found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool TryFind(string stringId, out ItemData data)
		{
			data = this.Find(stringId);
			return data != null;
		}

		/// <summary>
		/// Called to read an entry from the data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "name", "koreanName", "type", "weight");

			var data = new ItemData();

			data.ClassId = entry.ReadInt("id");
			data.StringId = entry.ReadString("name");
			data.Name = entry.ReadString("name").Replace("_", " ");
			data.KoreanName = entry.ReadString("koreanName");
			data.Type = entry.ReadEnum<ItemType>("type");
			data.Weight = entry.ReadInt("weight");
			data.Price = entry.ReadInt("price", 0);
			data.SellPrice = entry.ReadInt("sell", data.Price / 2);
			data.Attack = entry.ReadInt("attack", 0);
			data.Defense = entry.ReadInt("defense", 0);
			data.AttackRange = entry.ReadInt("range", 0);
			data.JobsAllowed = entry.ReadEnum("jobs", JobFilter.All);
			data.SexAllowed = entry.ReadEnum("sex", Sex.Any);
			data.WearSlots = entry.ReadEnum("equip", EquipSlots.None);
			data.RequiredLevel = entry.ReadInt("equipLevel", 0);
			data.WeaponLevel = entry.ReadInt("weaponLevel", 0);
			data.LookId = entry.ReadInt("look", 0);

			this.AddOrReplace(data.ClassId, data);
		}
	}
}
