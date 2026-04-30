using System;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Zone.Network;

namespace Sabine.Zone.Skills.Handlers.Swordman
{
	/// <summary>
	/// Handler for the Swordman skill "Bash".
	/// </summary>
	[SkillHandler(SkillId.SM_BASH)]
	public class SM_BASH : ITargetedSkillHandler
	{
		/// <summary>
		/// Handles the skill.
		/// </summary>
		/// <param name="parameters"></param>
		public void Handle(UseSkillParams parameters)
		{
			var attacker = parameters.Character;
			var target = parameters.Target;
			var skill = parameters.Skill;
			var skillLevel = parameters.SkillLevel;

			if (!attacker.TrySpendSp(skill.SpCost))
			{
				Send.ZC_ACK_TOUSESKILL(attacker, skill.Id, SkillUseResult.Fail, SkillFailType.NotEnoughSp, SkillFailReason.SkillFailed);
				return;
			}

			attacker.Controller.StopMove();
			target.Controller.StopMove();

			var damage = Random.Shared.Next(attacker.Parameters.AttackMin, attacker.Parameters.AttackMax + 1);
			damage = (int)(damage * (1 + (skillLevel * 0.3f)));

			target.TakeDamage(damage, attacker);
			target.StunEndTime = DateTime.Now.AddSeconds(1);

			Send.ZC_NOTIFY_SKILL(attacker, target, skill, skillLevel, damage, Game.GetTick(), attacker.Parameters.AttackMotionDelay, target.Parameters.DamageMotionDelay, 1, SkillHitEffect.SingleHit);
		}
	}
}
