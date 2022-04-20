using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents a monster's data.
	/// </summary>
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
		public int AttackRange { get; set; }
		public int ViewRange { get; set; }
		public int ChaseRange { get; set; }
		public int AttackMin { get; set; }
		public int AttackMax { get; set; }
		public int Defense { get; set; }
		public int MagicDefense { get; set; }
		public int Str { get; set; }
		public int Agi { get; set; }
		public int Vit { get; set; }
		public int Int { get; set; }
		public int Dex { get; set; }
		public int Luk { get; set; }
		public SizeType Size { get; set; }
		public RaceType Race { get; set; }
		public ElementType Element { get; set; }
		public int ElementLevel { get; set; }
		public int Mode { get; set; }
		public int Speed { get; set; }
		public int AttackDelay { get; set; }
		public int AttackMotion { get; set; }
		public int DamageMotion { get; set; }

		public List<DropData> Drops { get; set; } = new List<DropData>();
		public List<DropData> MvpDrops { get; set; } = new List<DropData>();
	}

	/// <summary>
	/// Represents a drop item.
	/// </summary>
	public class DropData
	{
		public int ItemId { get; set; }
		public int Chance { get; set; }
	}

	/// <summary>
	/// A monster database.
	/// </summary>
	public class MonsterDb : DatabaseJsonIndexed<int, MonsterData>
	{
		/// <summary>
		/// Returns the data for the monster with the given name,
		/// or null if it wasn't found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MonsterData Find(string name)
			=> this.Find(a => string.Compare(a.Name, name, true) == 0);

		/// <summary>
		/// Returns the data for the monster with the given name via
		/// out. Returns false if the map wasn't found.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="data"></param>
		/// <returns></returns>
		public bool TryFind(string name, out MonsterData data)
		{
			data = this.Find(name);
			return data != null;
		}

		/// <summary>
		/// Called to read an entry from the monster database file.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "name", "spriteId");

			var data = new MonsterData();

			data.Id = entry.ReadInt("id");
			data.Name = entry.ReadString("name");
			data.SpriteId = entry.ReadInt("spriteId");
			data.Level = entry.ReadInt("level", 1);
			data.Hp = entry.ReadInt("hp", 50);
			data.Sp = entry.ReadInt("sp", 0);
			data.BaseExp = entry.ReadInt("exp", 0);
			data.JobExp = entry.ReadInt("jexp", 0);
			data.AttackRange = entry.ReadInt("attackRange", 1);
			data.ViewRange = entry.ReadInt("viewRange", 10);
			data.ChaseRange = entry.ReadInt("chaseRange", 12);
			data.AttackMin = entry.ReadInt("attackMin", 1);
			data.AttackMax = entry.ReadInt("attackMax", 1);
			data.Defense = entry.ReadInt("def", 0);
			data.MagicDefense = entry.ReadInt("mdef", 0);
			data.Str = entry.ReadInt("str", 1);
			data.Agi = entry.ReadInt("agi", 1);
			data.Vit = entry.ReadInt("vit", 1);
			data.Int = entry.ReadInt("int", 1);
			data.Dex = entry.ReadInt("dex", 1);
			data.Luk = entry.ReadInt("luk", 1);
			data.Size = entry.ReadEnum("size", SizeType.Small);
			data.Race = entry.ReadEnum("race", RaceType.Formless);
			data.Element = entry.ReadEnum("element", ElementType.Neutral);
			data.ElementLevel = entry.ReadInt("elementLevel", 1);
			data.Mode = entry.ReadInt("mode");
			data.Speed = entry.ReadInt("speed", 200);
			data.AttackDelay = entry.ReadInt("attackDelay", 1500);
			data.AttackMotion = entry.ReadInt("attackMotion", 0);
			data.DamageMotion = entry.ReadInt("damageMotion", 0);

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
