using System;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents a monster NPC.
	/// </summary>
	public class Monster : Npc
	{
		/// <summary>
		/// Return a reference to the monster's data.
		/// </summary>
		public MonsterData Data { get; }

		/// <summary>
		/// Creates new monster.
		/// </summary>
		/// <param name="monsterId"></param>
		/// <exception cref="ArgumentException"></exception>
		public Monster(int monsterId)
			: base(monsterId)
		{
			if (!SabineData.Monsters.TryFind(monsterId, out var data))
				throw new ArgumentException($"Data for monster '{monsterId}' not found.");

			this.ClassId = data.SpriteId;
			this.Name = data.Name;

			this.Data = data;
			this.ApplyData();
		}

		/// <summary>
		/// Modifies monster and its stats based on the loaded data.
		/// </summary>
		private void ApplyData()
		{
			this.Parameters.HpMax = this.Data.Hp;
			this.Parameters.Hp = this.Data.Hp;
			this.Parameters.SpMax = this.Data.Sp;
			this.Parameters.Sp = this.Data.Sp;
			this.Parameters.Str = this.Data.Str;
			this.Parameters.Agi = this.Data.Agi;
			this.Parameters.Vit = this.Data.Vit;
			this.Parameters.Int = this.Data.Int;
			this.Parameters.Dex = this.Data.Dex;
			this.Parameters.Luk = this.Data.Luk;
			this.Parameters.AttackMin = this.Data.AttackMin;
			this.Parameters.AttackMax = this.Data.AttackMax;
			this.Parameters.Defense = this.Data.Defense;
			this.Parameters.Speed = this.Data.Speed;
		}
	}
}
