using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;
using Yggdrasil.Util;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents an EXP table.
	/// </summary>
	public class ExpTableData
	{
		public ExpTableType Type { get; set; }
		public int MaxLevel { get; set; }
		public JobId[] Jobs { get; set; }
		public int[] Exp { get; set; }

		/// <summary>
		/// Returns the amount of EXP required to reach level+1.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public int GetExpNeededAtLevel(int level)
		{
			level = Math2.Clamp(1, this.MaxLevel, level);
			return this.Exp[level - 1];
		}
	}

	/// <summary>
	/// Defines the type of an EXP table.
	/// </summary>
	public enum ExpTableType
	{
		/// <summary>
		/// Base EXP.
		/// </summary>
		Base,

		/// <summary>
		/// Job EXP.
		/// </summary>
		Job,
	}

	/// <summary>
	/// An exp table database.
	/// </summary>
	public class ExpDb : DatabaseJson<ExpTableData>
	{
		/// <summary>
		/// Returns the amount of EXP necessary to reach the next level.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="jobId"></param>
		/// <param name="level"></param>
		public int GetExpNeeded(ExpTableType type, JobId jobId, int level)
		{
			var table = this.Find(a => a.Type == type && a.Jobs.Contains(jobId));
			if (table == null)
				throw new ArgumentException($"No EXP table found for type {type} and job {jobId}.");

			return table.GetExpNeededAtLevel(level);
		}

		/// <summary>
		/// Returns the max level for the given job.
		/// </summary>
		/// <param name="type"></param>
		/// <param name="jobId"></param>
		/// <returns></returns>
		public int GetMaxLevel(ExpTableType type, JobId jobId)
		{
			var table = this.Find(a => a.Type == type && a.Jobs.Contains(jobId));
			if (table == null)
				throw new ArgumentException($"No EXP table found for type {type} and job {jobId}.");

			return table.MaxLevel;
		}

		/// <summary>
		/// Called to read one entry from the database.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("type", "maxLevel", "jobs", "exp");

			var data = new ExpTableData();

			data.Type = entry.ReadEnum<ExpTableType>("type");
			data.MaxLevel = entry.ReadInt("maxLevel");
			data.Jobs = entry.ReadArray<JobId>("jobs");
			data.Exp = entry.ReadArray<int>("exp");

			if (data.Exp.Length < data.MaxLevel - 1)
				throw new DatabaseErrorException(null, "The exp table has less entries than the max level.");

			this.Add(data);
		}
	}
}
