using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Text;
using Sabine.Shared.Util;
using static System.Net.Mime.MediaTypeNames;

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
		Knight = 0x80,
		Priest = 0x100,
		Wizard = 0x200,
		Blacksmith = 0x400,
		Hunter = 0x800,
		Assassin = 0x1000,
		// 0x2000
		Crusader = 0x4000,
		Monk = 0x8000,
		Sage = 0x10000,
		Rogue = 0x20000,
		Alchemist = 0x40000,
		BardDancer = 0x80000,

		AllSwordman = Swordman | Knight | Crusader,
		AllMage = Mage | Wizard | Sage,
		AllArcher = Archer | Hunter | BardDancer,
		AllAcolyte = Acolyte | Priest | Monk,
		AllMerchant = Merchant | Blacksmith | Alchemist,
		AllThief = Thief | Assassin | Rogue,

		AllButNovice = All & ~Novice,

		All = -1,
	}

	/// <summary>
	/// Extensions for the JobFilter enum.
	/// </summary>
	public static class JobFilterExtensions
	{
		private readonly static Dictionary<JobFilter, string> Cache = new();

		public static string ToReadableString(this JobFilter filter)
		{
			if (Cache.TryGetValue(filter, out var cached))
				return cached;

			var result = "";

			if (filter == JobFilter.All)
			{
				result = Localization.Get("All");
			}
			else
			{
				var sb = new StringBuilder();
				var max = JobFilter.BardDancer;

				if (Game.Version < Versions.Beta2)
					max = JobFilter.Thief;

				for (var i = 1; i <= (long)max; i <<= 1)
				{
					if ((filter & (JobFilter)i) != 0)
					{
						if (sb.Length > 0)
							sb.Append(", ");

						string name;

						switch ((JobFilter)i)
						{
							case JobFilter.Novice: name = Localization.Get("Novice"); break;
							case JobFilter.Swordman: name = Localization.Get("Swordman"); break;
							case JobFilter.Mage: name = Localization.Get("Mage"); break;
							case JobFilter.Archer: name = Localization.Get("Archer"); break;
							case JobFilter.Acolyte: name = Localization.Get("Acolyte"); break;
							case JobFilter.Merchant: name = Localization.Get("Merchant"); break;
							case JobFilter.Thief: name = Localization.Get("Thief"); break;
							case JobFilter.Knight: name = Localization.Get("Knight"); break;
							case JobFilter.Priest: name = Localization.Get("Priest"); break;
							case JobFilter.Wizard: name = Localization.Get("Wizard"); break;
							case JobFilter.Blacksmith: name = Localization.Get("Blacksmith"); break;
							case JobFilter.Hunter: name = Localization.Get("Hunter"); break;
							case JobFilter.Assassin: name = Localization.Get("Assassin"); break;
							case JobFilter.Crusader: name = Localization.Get("Crusader"); break;
							case JobFilter.Monk: name = Localization.Get("Monk"); break;
							case JobFilter.Sage: name = Localization.Get("Sage"); break;
							case JobFilter.Rogue: name = Localization.Get("Rogue"); break;
							case JobFilter.Alchemist: name = Localization.Get("Alchemist"); break;
							case JobFilter.BardDancer: name = Localization.Get("Bard/Dancer"); break;

							default: continue;
						}

						sb.Append(name);
					}
				}

				result = sb.ToString();
			}

			return Cache[filter] = result;
		}
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
