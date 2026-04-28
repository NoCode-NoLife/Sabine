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
		public string Name { get; set; }
		public ItemType Type { get; set; }
		public int Weight { get; set; }
		public int Price { get; set; }
		public int SellPrice { get; set; }
		public int Attack { get; set; }
		public int AttackMin { get; set; }
		public int AttackMax { get; set; }
		public int Defense { get; set; }
		public int MagicDefense { get; set; }
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
		/// Called to read an entry from the data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			// This works, and is probably a clean enough solution, but I
			// wish this wouldn't have to be in the db itself.
			// VersionedDatabaseJsonIndexed incoming?
			if (entry.ContainsKey("versionMin") || entry.ContainsKey("versionMax"))
			{
				var versionMin = entry.ReadInt("versionMin", 0);
				var versionMax = entry.ReadInt("versionMax", int.MaxValue);

				if (Game.Version < versionMin || Game.Version > versionMax)
					return;

				if (entry.ContainsKey("entries"))
				{
					foreach (JObject subEntry in entry["entries"])
						this.ReadEntry(subEntry);
					return;
				}
			}

			entry.AssertNotMissing("id", "name", "type", "weight");

			var data = new ItemData();

			data.ClassId = entry.ReadInt("id");
			data.Name = entry.ReadString("name");
			data.Type = entry.ReadEnum<ItemType>("type");
			data.Weight = entry.ReadInt("weight");
			data.Price = entry.ReadInt("price", 0);
			data.SellPrice = entry.ReadInt("sell", data.Price / 2);
			data.Attack = entry.ReadInt("attack", 0);
			data.AttackMin = entry.ReadInt("attackMin", 0);
			data.AttackMax = entry.ReadInt("attackMax", 0);
			data.Defense = entry.ReadInt("defense", 0);
			data.MagicDefense = entry.ReadInt("magicDefense", 0);
			data.AttackRange = entry.ReadInt("range", 0);
			data.JobsAllowed = entry.ReadEnum("jobs", JobFilter.All);
			data.SexAllowed = entry.ReadEnum("sex", Sex.Any);
			data.WearSlots = entry.ReadEnum("equip", EquipSlots.None);
			data.RequiredLevel = entry.ReadInt("equipLevel", 1);
			data.WeaponLevel = entry.ReadInt("weaponLevel", 0);
			data.LookId = entry.ReadInt("look", 0);

			if (Game.Version >= Versions.Beta2)
			{
				if (data.WearSlots == EquipSlots.Accessory1 || data.WearSlots == EquipSlots.Accessory2)
					data.WearSlots = EquipSlots.Accessories;
			}

			// Force headgear slots to Head for Beta1, since it couldn't
			// handle multiple headgear slots yet.
			if (Game.Version < Versions.Beta2)
			{
				if ((data.WearSlots & EquipSlots.HeadMiddle) != 0)
					data.WearSlots = EquipSlots.Head;
				else if ((data.WearSlots & EquipSlots.HeadTop) != 0)
					data.WearSlots = EquipSlots.Head;
			}

			this.AddOrReplace(data.ClassId, data);
		}
	}
}
