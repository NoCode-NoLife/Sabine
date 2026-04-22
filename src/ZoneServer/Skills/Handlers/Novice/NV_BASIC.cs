using System;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Zone.Network;
using Sabine.Zone.World.Actors;

namespace Sabine.Zone.Skills.Handlers.Novice
{
	/// <summary>
	/// Handler for the Novice skill "Basic Skill".
	/// </summary>
	public static class NV_BASIC
	{
		/// <summary>
		/// Returns true if the character can use the given ability.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="ability"></param>
		/// <returns></returns>
		/// <exception cref="ArgumentException"></exception>
		public static bool CanUse(Character character, BasicSkillAbility ability)
		{
			// There were no skills before Beta1, so we'll allow everything
			if (Game.Version < Versions.Beta1)
				return true;

			if (!character.Skills.TryGet(SkillId.NV_BASIC, out var skill))
				return false;

			switch (ability)
			{
				case BasicSkillAbility.Trade: return skill.Level >= 1;
				case BasicSkillAbility.UseEmotes: return skill.Level >= 2;
				case BasicSkillAbility.Sit: return skill.Level >= 3;
				case BasicSkillAbility.CreateChatRoom: return skill.Level >= 4;
				case BasicSkillAbility.JoinParty: return skill.Level >= 5;
				case BasicSkillAbility.UseStorage: return skill.Level >= 6;
				case BasicSkillAbility.CreateParty: return skill.Level >= 7;
				case BasicSkillAbility.KillPlayers: return skill.Level >= 8;
				case BasicSkillAbility.JudgePlayers: return skill.Level >= 8;
				case BasicSkillAbility.ChangeJob: return skill.Level >= 9;

				default: throw new ArgumentException($"Unknown ability: {ability}", nameof(ability));
			}
		}

		/// <summary>
		/// Checks if the character can use the given ability. If not,
		/// true is returned and a skill use fail message is sent to the
		/// character's client.
		/// </summary>
		/// <example>
		/// if (NV_BASIC.TryFail(character, BasicSkillAbility.UseEmotes))
		///		return; // ZC_ACK_TOUSESKILL sent
		///		
		/// Send.ZC_EMOTION(character, emotion);
		/// </example>
		/// <param name="character"></param>
		/// <param name="ability"></param>
		/// <returns></returns>
		public static bool TryFail(PlayerCharacter character, BasicSkillAbility ability)
		{
			if (CanUse(character, ability))
				return false;

			var failReason = SkillFailReason.SkillFailed;

			switch (ability)
			{
				case BasicSkillAbility.Trade: failReason = SkillFailReason.BasicLvLow_NoTrade; break;
				case BasicSkillAbility.UseEmotes: failReason = SkillFailReason.BasicLvLow_NoEmote; break;
				case BasicSkillAbility.Sit: failReason = SkillFailReason.BasicLvLow_NoSitting; break;
				case BasicSkillAbility.CreateChatRoom: failReason = SkillFailReason.BasicLvLow_NoChatRoom; break;
				case BasicSkillAbility.JoinParty: failReason = SkillFailReason.BasicLvLow_NoParty; break;
				case BasicSkillAbility.UseStorage: break;
				case BasicSkillAbility.CreateParty: failReason = SkillFailReason.BasicLvLow_NoParty; break;
				case BasicSkillAbility.KillPlayers: failReason = SkillFailReason.BasicLvLow_NoPK; break;
				case BasicSkillAbility.JudgePlayers: break;
				case BasicSkillAbility.ChangeJob: break;
			}

			Send.ZC_ACK_TOUSESKILL(character, SkillId.NV_BASIC, SkillUseResult.Fail, SkillFailType.SkillSpecific, failReason);
			return true;
		}
	}

	/// <summary>
	/// Basic abilities unlocked by the Basic Skills Novice skill.
	/// </summary>
	public enum BasicSkillAbility
	{
		/// <summary>
		/// Allows players to trade with others via the trading system.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 1.
		/// </remarks>
		Trade,

		/// <summary>
		/// Allows players the usage of emotes.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 2.
		/// </remarks>
		UseEmotes,

		/// <summary>
		/// Allows players to sit.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 3.
		/// </remarks>
		Sit,

		/// <summary>
		/// Allows players to create chat rooms.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 4.
		/// </remarks>
		CreateChatRoom,

		/// <summary>
		/// Allows players to join parties.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 5.
		/// </remarks>
		JoinParty,

		/// <summary>
		/// Allows players to use storage.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 6.
		/// </remarks>
		UseStorage,

		/// <summary>
		/// Allows players to create parties of their own.
		/// </summary>
		/// <remarks>
		/// Typically unlocked at level 7.
		/// </remarks>
		CreateParty,

		/// <summary>
		/// Allows players to engage in open-world PK (PvP combat)
		/// </summary>
		/// <remarks>
		/// Unlocked at level 8, according to the skill description in the
		/// beta1 client.
		/// </remarks>
		KillPlayers,

		/// <summary>
		/// Allows players to use the alignment system.
		/// </summary>
		/// <remarks>
		/// Unlocked at level 8, according to the skill descriptions on
		/// fan pages around the time of beta2.
		/// </remarks>
		JudgePlayers,

		/// <summary>
		/// Allows players to change their job to a first job.
		/// </summary>
		ChangeJob,
	}
}
