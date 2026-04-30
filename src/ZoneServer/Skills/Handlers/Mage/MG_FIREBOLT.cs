using System;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Zone.Network;

namespace Sabine.Zone.Skills.Handlers.Mage
{
	/// <summary>
	/// Handler for the Mage skill "Firebolt".
	/// </summary>
	[SkillHandler(SkillId.MG_FIREBOLT)]
	public class MG_FIREBOLT : ITargetedSkillHandler
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

			var hitCount = skillLevel;

			var damage = Random.Shared.Next(attacker.Parameters.MagicAttackMin, attacker.Parameters.MagicAttackMax + 1);
			damage *= hitCount;

			target.TakeDamage(damage, attacker);
			target.StunEndTime = DateTime.Now.AddSeconds(1);

			Send.ZC_NOTIFY_SKILL(attacker, target, skill, skillLevel, damage, Game.GetTick(), attacker.Parameters.AttackMotionDelay, target.Parameters.DamageMotionDelay, hitCount, SkillHitEffect.MultiHit);
		}
	}
}
