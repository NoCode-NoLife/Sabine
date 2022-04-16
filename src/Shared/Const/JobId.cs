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
		Mage = 2,
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
		Mage = 0x04,
		Archer = 0x08,
		Acolyte = 0x10,
		Merchant = 0x20,
		Thief = 0x40,

		All = -1,
	}

	/// <summary>
	/// Extensions for the JobId enum.
	/// </summary>
	public static class JobIdExtensions
	{
		/// <summary>
		/// Returns true if the job matches the given filter, like when
		/// the job can use a certain item.
		/// </summary>
		/// <param name="jobId"></param>
		/// <param name="filter"></param>
		/// <returns></returns>
		public static bool Matches(this JobId jobId, JobFilter filter)
		{
			return (filter & (JobFilter)(1L << (int)jobId)) != 0;
		}
	}
}
