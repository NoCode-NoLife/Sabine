using System;
using System.Text;
using Sabine.Shared.Const;
using Sabine.Shared.Data.Databases;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Skills
{
	public class Skill
	{
		/// <summary>
		/// Returns the character the skill belongs to.
		/// </summary>
		public Character Character { get; }

		/// <summary>
		/// Returns the skill's id.
		/// </summary>
		public SkillId Id { get; }

		/// <summary>
		/// Returns the skill's current level.
		/// </summary>
		public int Level { get; set; }

		/// <summary>
		/// Returns the skill's static data.
		/// </summary>
		public SkillData Data { get; }

		/// <summary>
		/// Returns the skill's range for the current level.
		/// </summary>
		public int Range
		{
			get
			{
				// Some skills appear to have a dynamic range that is
				// relative to the character's normal attack range, such
				// as Double Strafe, but it's currently unclear if it was
				// always like that or how this should be set up.
				//
				// eAthena defines negative ranges that it applies as
				// modifiers to the attack range, while other, older,
				// emulators defined static ranges.
				//
				// We'll just use the data as is for now, until we have
				// more info.

				return this.Data.GetRange(this.Level);
			}
		}


		/// <summary>
		/// Returns the skill's SP cost for the current level.
		/// </summary>
		public int SpCost => this.Data.GetSpCost(this.Level);

		/// <summary>
		/// Returns the skill's HP cost for the current level.
		/// </summary>
		public int HpCost => this.Data.GetHpCost(this.Level);

		/// <summary>
		/// Returns true if the skill can be leveled up.
		/// </summary>
		/// <remarks>
		/// Only checks whether the skill can technically be leveled up,
		/// such as whether the current level is below the maximum. It
		/// doesn't check whether the character meets any other
		/// requirements, such as having enough skill points.
		/// </remarks>
		public bool CanBeLeveled => this.Level < this.Data.MaxLevel;

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="owner"></param>
		/// <param name="skillId"></param>
		/// <param name="level"></param>
		/// <exception cref="ArgumentException">
		/// Thrown if the skill data for the given id couldn't be found.
		/// </exception>
		public Skill(Character owner, SkillId skillId, int level)
		{
			if (!ZoneServer.Instance.Data.Skills.TryFind(skillId, out var data))
				throw new ArgumentException($"Data for skill '{skillId}' not found.");

			this.Character = owner;
			this.Id = skillId;
			this.Level = level;
			this.Data = data;
		}

		/// <summary>
		/// Increases the skill's level by 1, up to the max level.
		/// </summary>
		public void LevelUp()
		{
			this.Level = Math.Min(this.Level + 1, this.Data.MaxLevel);

			if (this.Character is PlayerCharacter pc)
				Send.ZC_SKILLINFO_UPDATE(pc, this);
		}
	}
}
