using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Network;
using Sabine.Zone.Network;
using Yggdrasil.Logging;
using Yggdrasil.Util;

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

		/// <summary>
		/// Kills the monster and removes it from the map with a death
		/// animation.
		/// </summary>
		/// <param name="killer"></param>
		public async override void Kill(Character killer)
		{
			base.Kill(killer);

			this.GiveExp(killer);
			this.GiveMvpExp(killer);

			// Removing the monster with a delay seems wonky, but if we
			// don't, the client will not display the damage for the last
			// hit and the monster will disappear before the hit animation
			// is even done. It feels good with a 1s delay though.
			// Especially with Porings, which pop at the height of their
			// hit animation. Alternative implementation: DisappearTime,
			// which will despawn the monster after X amount of time.
			await Task.Delay(1000);

			this.DropItems(killer);
			this.DropMvpItems(killer);

			this.Map.RemoveNpc(this);
		}

		/// <summary>
		/// Gives EXP to the character(s) that killed the monster.
		/// </summary>
		/// <param name="killer"></param>
		private void GiveExp(Character killer)
		{
			if (!(killer is PlayerCharacter playerCharacter))
				return;

			var baseExp = this.Data.BaseExp;
			var jobExp = this.Data.BaseExp;

			playerCharacter.GainBaseExp(baseExp);
			playerCharacter.GainJobExp(jobExp);
		}

		/// <summary>
		/// Gives EXP for killing an MVP if applicable.
		/// </summary>
		/// <param name="killer"></param>
		private void GiveMvpExp(Character killer)
		{
			if (!(killer is PlayerCharacter playerCharacter))
				return;

			var exp = this.Data.MvpExp;
			if (exp <= 0)
				return;

			playerCharacter.GainBaseExp(exp);

			Send.ZC_MVP(playerCharacter);
			Send.ZC_MVP_GETTING_SPECIAL_EXP(playerCharacter, exp);
		}

		/// <summary>
		/// Drops the monster's items.
		/// </summary>
		/// <param name="killer"></param>
		private void DropItems(Character killer)
			=> this.DropRandomItems(killer, this.Data.Drops);

		/// <summary>
		/// Drops the monster's MVP items.
		/// </summary>
		/// <param name="killer"></param>
		private void DropMvpItems(Character killer)
			=> this.DropRandomItems(killer, this.Data.MvpDrops);

		/// <summary>
		/// Drops the monster's items.
		/// </summary>
		/// <param name="killer"></param>
		private async void DropRandomItems(Character killer, IList<DropData> dropsData)
		{
			if (dropsData.Count == 0)
				return;

			var rnd = RandomProvider.Get();
			var map = this.Map;
			var pos = this.Position;

			for (var i = 0; i < dropsData.Count; ++i)
			{
				var dropData = dropsData[i];
				var dropRate = ZoneServer.Instance.Conf.World.ItemDropRate / 100f;
				var dropChance = dropData.Chance * dropRate;

				if (dropChance < rnd.Next(100))
					continue;

				await Task.Delay(100);

				var item = new Item(dropData.ItemId);
				var dropPos = pos.GetRandomInSquareRange(1);

				item.Drop(map, dropPos);
			}
		}
	}
}
