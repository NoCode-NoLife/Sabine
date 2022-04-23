namespace Sabine.Shared.Const
{
	/// <summary>
	/// Parameter types used in packets that handle certain ranges
	/// of stats and other parameters.
	/// </summary>
	public enum ParameterType : short
	{
		Speed = 0,
		BaseExp = 1, // Long
		JobExp = 2, // Long
		Karma = 3,
		Manner = 4,
		Hp = 5,
		HpMax = 6,
		Sp = 7,
		SpMax = 8,
		StatPoints = 9,
		/// 10
		BaseLevel = 11,
		SkillPoints = 12,
		Str = 13,
		Agi = 14,
		Vit = 15,
		Int = 16,
		Dex = 17,
		Luk = 18,
		/// 19
		Zeny = 20, // Long
		/// 21
		BaseExpNeeded = 22, // Long
		JobExpNeeded = 23, // Long
		Weight = 24,
		WeightMax = 25,
		AttackMin = 32,
		AttackMax = 33,
		Defense = 34,
		MagicAttack = 35,

		// This is where the parameters supported by the alpha client end,
		// but it's fine to send newer ones, as they are simply ignored.

		JobLevel = 55,
	}

	/// <summary>
	/// Extensions for ParameterType enum.
	/// </summary>
	public static class ParameterTypeExtensions
	{
		/// <summary>
		/// Returns true if the given paramter is a "long" parameter and
		/// is sent with an int, instead of a short.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsLong(this ParameterType type)
		{
			return
				type == ParameterType.BaseExp ||
				type == ParameterType.JobExp ||
				type == ParameterType.Zeny ||
				type == ParameterType.BaseExpNeeded ||
				type == ParameterType.JobExpNeeded;
		}
	}
}
