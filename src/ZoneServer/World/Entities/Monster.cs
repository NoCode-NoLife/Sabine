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
		}
	}
}
