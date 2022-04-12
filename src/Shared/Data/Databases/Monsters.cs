using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	public class MonsterData
	{
		public int Id { get; set; }
		public int SpriteId { get; set; }
		public string Name { get; set; }
		public int Level { get; set; }
		public int Hp { get; set; }
		public int Sp { get; set; }
		public int BaseExp { get; set; }
		public int JobExp { get; set; }
		public int Range1 { get; set; }
		public int Range2 { get; set; }
		public int Range3 { get; set; }
		public int AtkMin { get; set; }
		public int AtkMax { get; set; }
		public int Defense { get; set; }
		public int MagicDefense { get; set; }
		public int Str { get; set; }
		public int Agi { get; set; }
		public int Vit { get; set; }
		public int Int { get; set; }
		public int Dex { get; set; }
		public int Luk { get; set; }
		public int Scale { get; set; }
		public int Race { get; set; }
		public int Element { get; set; }
		public int Mode { get; set; }
		public int Speed { get; set; }
		public int AttackDelay { get; set; }
		public int AttackMotion { get; set; }
		public int DamageMotion { get; set; }

		public List<DropData> Drops { get; set; } = new List<DropData>();
		public List<DropData> MvpDrops { get; set; } = new List<DropData>();
	}

	public class DropData
	{
		public int ItemId { get; set; }
		public int Chance { get; set; }
	}

	public class MonsterDb : DatabaseJsonIndexed<int, MonsterData>
	{
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "spriteId", "name");

			var data = new MonsterData();

			data.Id = entry.ReadInt("id");
			data.SpriteId = entry.ReadInt("spriteId");
			data.Name = entry.ReadString("name");
			data.Level = entry.ReadInt("level");
			data.Hp = entry.ReadInt("hp");
			data.Sp = entry.ReadInt("sp");
			data.BaseExp = entry.ReadInt("exp");
			data.JobExp = entry.ReadInt("jexp");
			data.Range1 = entry.ReadInt("range1", 1);
			data.Range2 = entry.ReadInt("range2", 10);
			data.Range3 = entry.ReadInt("range3", 12);
			data.AtkMin = entry.ReadInt("atkMin", 1);
			data.AtkMax = entry.ReadInt("atkMax", 1);
			data.Defense = entry.ReadInt("def", 0);
			data.MagicDefense = entry.ReadInt("mdef", 0);
			data.Str = entry.ReadInt("str", 1);
			data.Agi = entry.ReadInt("agi", 1);
			data.Vit = entry.ReadInt("vit", 1);
			data.Int = entry.ReadInt("int", 1);
			data.Dex = entry.ReadInt("dex", 1);
			data.Luk = entry.ReadInt("luk", 1);
			data.Scale = entry.ReadInt("scale", 1);
			data.Race = entry.ReadInt("race");
			data.Element = entry.ReadInt("element");
			data.Mode = entry.ReadInt("mode");
			data.Speed = entry.ReadInt("speed");
			data.AttackDelay = entry.ReadInt("adelay");
			data.AttackMotion = entry.ReadInt("amotion");
			data.DamageMotion = entry.ReadInt("dmotion");

			if (entry.ContainsKey("drops"))
			{
				foreach (JObject dropEntry in entry["drops"])
				{
					dropEntry.AssertNotMissing("itemId", "chance");

					var drop = new DropData();

					drop.ItemId = dropEntry.ReadInt("itemId");
					drop.Chance = dropEntry.ReadInt("chance");

					data.Drops.Add(drop);
				}
			}

			if (entry.ContainsKey("mvpDrops"))
			{
				foreach (JObject dropEntry in entry["drops"])
				{
					dropEntry.AssertNotMissing("itemId", "chance");

					var drop = new DropData();

					drop.ItemId = dropEntry.ReadInt("itemId");
					drop.Chance = dropEntry.ReadInt("chance");

					data.MvpDrops.Add(drop);
				}
			}

			this.AddOrReplace(data.Id, data);
		}
	}
}
