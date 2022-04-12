using System;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;

namespace Sabine.Zone.World.Entities
{
	public class Monster : Npc
	{
		public MonsterData Data { get; }

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
