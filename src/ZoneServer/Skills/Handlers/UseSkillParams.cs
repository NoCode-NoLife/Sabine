using Sabine.Shared.World;
using Sabine.Zone.World.Actors;

namespace Sabine.Zone.Skills.Handlers
{
	/// <summary>
	/// Represents the parameters necessary for using a targeted skill.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="target"></param>
	/// <param name="skill"></param>
	/// <param name="skillLevel"></param>
	public readonly struct UseSkillParams(Character character, Character target, Skill skill, int skillLevel)
	{
		/// <summary>
		/// Returns the character that used the skill.
		/// </summary>
		public readonly Character Character = character;

		/// <summary>
		/// Returns the target of the skill. Returns null if no target was
		/// specified.
		/// </summary>
		public readonly Character Target = target;

		/// <summary>
		/// Returns the skill being used.
		/// </summary>
		public readonly Skill Skill = skill;

		/// <summary>
		/// Returns the level the skill is to be used at.
		/// </summary>
		public readonly int SkillLevel = skillLevel;

		/// <summary>
		/// Returns true if the skill has a non-null target.
		/// </summary>
		public bool HasTarget => Target != null;
	}

	/// <summary>
	/// Represents the parameters necessary for using a skill aimed at the
	/// ground.
	/// </summary>
	/// <param name="character"></param>
	/// <param name="targetPosition"></param>
	/// <param name="skill"></param>
	/// <param name="skillLevel"></param>
	public readonly struct UseGroundSkillParams(Character character, Position targetPosition, Skill skill, int skillLevel)
	{
		/// <summary>
		/// Returns the character that used the skill.
		/// </summary>
		public readonly Character Character = character;

		/// <summary>
		/// Returns the target of the skill. Returns null if no target was
		/// specified.
		/// </summary>
		public readonly Position TargetPosition = targetPosition;

		/// <summary>
		/// Returns the skill being used.
		/// </summary>
		public readonly Skill Skill = skill;

		/// <summary>
		/// Returns the level the skill is to be used at.
		/// </summary>
		public readonly int SkillLevel = skillLevel;
	}
}
