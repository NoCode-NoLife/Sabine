using Sabine.Shared.Const;
using Sabine.Shared.World;

namespace Sabine.Char.Database
{
	public class Character
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public JobId JobId { get; set; }
		public int Zeny { get; set; }

		public string MapName { get; set; } = "prt_vilg01";
		public Position Position { get; set; } = new Position(100, 80);
		public int Speed { get; set; } = 200;

		public int BaseLevel { get; set; } = 1;
		public int JobLevel { get; set; } = 1;
		public int BaseExp { get; set; }
		public int JobExp { get; set; }

		public int Hp { get; set; } = 40;
		public int HpMax { get; set; } = 40;
		public int Sp { get; set; } = 11;
		public int SpMax { get; set; } = 11;

		public int Str { get; set; } = 1;
		public int Agi { get; set; } = 1;
		public int Vit { get; set; } = 1;
		public int Int { get; set; } = 1;
		public int Dex { get; set; } = 1;
		public int Luk { get; set; } = 1;
		public int StatPoints { get; set; }

		public int Slot { get; set; }
		public int HairId { get; set; }
		public int WeaponId { get; set; }
	}
}
