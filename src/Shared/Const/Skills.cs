namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines how and on what a skill can be used.
	/// </summary>
	public enum SkillTargetType : short
	{
		Passive = 0,
		Enemy = 1,
		Place = 2,
		Self = 4,
		Friend = 16,
		Trap = 32
	}

	/// <summary>
	/// The result of a skill use attempt.
	/// </summary>
	public enum SkillUseResult : byte
	{
		/// <summary>
		/// Skill use failed.
		/// </summary>
		Fail = 0,

		/// <summary>
		/// Skill was successfully used.
		/// </summary>
		Success = 1,
	}

	/// <summary>
	/// Specifies a general reason for skill failure.
	/// </summary>
	public enum SkillFailType : byte
	{
		// Beta1
		//-------------------------------------------------------------------

		/// <summary>
		/// Displays a message specific to a skill, defined in
		/// SkillFailReason.
		/// </summary>
		SkillSpecific = 0,

		/// <summary>
		/// Displays message about not having enough SP.
		/// </summary>
		NotEnoughSp = 1,

		/// <summary>
		/// Displays message about not having enough HP.
		/// </summary>
		NotEnoughHp = 2,

		/// <summary>
		/// Displays message about not having any warp points defined.
		/// </summary>
		NoWarpPoint = 3,

		/// <summary>
		/// Displays message about the skill being on cooldown. (?)
		/// </summary>
		Cooldown = 4,

		/// <summary>
		/// Displays message about not having enough money to use the skill.
		/// </summary>
		NoMoney = 5,

		/// <summary>
		/// Displays message about the skill not being usable with the
		/// current weapon.
		/// </summary>
		WrongWeapon = 6,

		/// <summary>
		/// Displays message about not having the required gems to use the
		/// skill.
		/// </summary>
		NoRedGem = 7,

		/// <summary>
		/// Displays message about not having the required gems to use the
		/// skill.
		/// </summary>
		NoBlueGem = 8,

		/// <summary>
		/// Displays a generic failure message.
		/// </summary>
		GenericFail = 9,
	}

	/// <summary>
	/// Specifies a more specific reason for skill failure used for skill
	/// specific reasons.
	/// </summary>
	public enum SkillFailReason : byte
	{
		// Alpha/Beta1
		//-------------------------------------------------------------------

		/// <summary>
		/// Displays message about not being able to trade due to a low
		/// Basic Skill level.
		/// </summary>
		BasicLvLow_NoTrade = 0,

		/// <summary>
		/// Displays message about not being able to use emotes due to a
		/// low Basic Skill level.
		/// </summary>
		BasicLvLow_NoEmote = 1,

		/// <summary>
		/// Displays message about not being able to sit due to a low
		/// Basic Skill level.
		/// </summary>
		BasicLvLow_NoSitting = 2,

		/// <summary>
		/// Displays message about not being able to create a chat room
		/// due to a low Basic Skill level.
		/// </summary>
		BasicLvLow_NoChatRoom = 3,

		/// <summary>
		/// Displays message about not being able to create a party due to
		/// a low Basic Skill level.
		/// </summary>
		BasicLvLow_NoParty = 4,

		/// <summary>
		/// Displays message about not being able to shout due to a low
		/// Basic Skill level.
		/// </summary>
		BasicLvLow_NoShout = 5,

		/// <summary>
		/// Displays message about not being able to enable PK due to a
		/// low Basic Skill level.
		/// </summary>
		BasicLvLow_NoPK = 6,

		// Beta1
		//-------------------------------------------------------------------

		/// <summary>
		/// Displays message about an unsufficient skill level.
		/// </summary>
		LowSkillLevel = 27,

		/// <summary>
		/// Displays message about failing to steal.
		/// </summary>
		StealFailed = 50,

		/// <summary>
		/// Displays message about failing to apply poison.
		/// </summary>
		PoisonFailed = 52,

		// Custom
		//-------------------------------------------------------------------

		/// <summary>
		/// Custom reason that's not actually handled by the client.
		/// </summary>
		SkillFailed = 255,
	}

}
