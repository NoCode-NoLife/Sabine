using Sabine.Shared.Const;
using Sabine.Shared.World;

namespace Sabine.Char.Database
{
	/// <summary>
	/// A character's data in the database.
	/// </summary>
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public JobId JobId { get; set; }
		public int Zeny { get; set; }

		public Location Location { get; set; }
		public int Speed { get; set; } = 200;

		public int BaseLevel { get; set; } = 1;
		public int JobLevel { get; set; } = 1;
		public int BaseExp { get; set; }
		public int JobExp { get; set; }

		public int Hp { get; set; }
		public int HpMax { get; set; }
		public int Sp { get; set; }
		public int SpMax { get; set; }

		public int Str { get; set; }
		public int Agi { get; set; }
		public int Vit { get; set; }
		public int Int { get; set; }
		public int Dex { get; set; }
		public int Luk { get; set; }
		public int StatPoints { get; set; }

		public int Slot { get; set; }
		public int HairId { get; set; }
		public int WeaponId { get; set; }
		public int HeadTopId { get; set; }
	}
}
