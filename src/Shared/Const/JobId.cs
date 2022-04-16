using System;

namespace Sabine.Shared.Const
{
	/// <summary>
	/// Ids of character's jobs.
	/// </summary>
	public enum JobId : byte
	{
		Novice = 0,
		Swordman = 1,
		Magician = 2,
		Archer = 3,
		Acolyte = 4,
		Merchant = 5,
		Thief = 6,
	}

	/// <summary>
	/// Bitmask of jobs something can apply to.
	/// </summary>
	[Flags]
	public enum JobFilter : long
	{
		Novice = 0x01,
		Swordman = 0x02,
		Magician = 0x04,
		Archer = 0x08,
		Acolyte = 0x10,
		Merchant = 0x20,
		Thief = 0x40,

		All = -1,
	}
}
