using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	public class SkillTreeJobData
	{
		public JobId JobId { get; set; }
		public List<SkillTreeSkillData> Skills { get; set; } = new();
	}

	public class SkillTreeSkillData
	{
		public SkillId SkillId { get; set; }
		public int MaxLevel { get; set; }
		public List<SkillTreePrerequisiteData> Prerequisites { get; set; } = new();
	}

	public class SkillTreePrerequisiteData
	{
		public SkillId SkillId { get; set; }
		public int MinLevel { get; set; }
	}

	/// <summary>
	/// A skill tree database.
	/// </summary>
	public class SkillTreeDb(JobDb jobDb, SkillDb skillDb) : DatabaseJsonIndexed<JobId, SkillTreeJobData>
	{
		private readonly JobDb _jobDb = jobDb;
		private readonly SkillDb _skillDb = skillDb;

		/// <summary>
		/// Called to read an entry from the data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("job", "skills");

			var jobStringId = entry.ReadString("job");

			var jobData = _jobDb.Entries.Values.FirstOrDefault(a => a.Name == jobStringId);
			if (jobData == null)
				throw new DatabaseErrorException(null, $"Unknown job name '{jobStringId}'.");

			var treeJobData = new SkillTreeJobData();

			treeJobData.JobId = jobData.Id;

			foreach (JObject skill in entry["skills"])
			{
				skill.AssertNotMissing("id", "maxLevel");

				var skillStringId = skill.ReadString("id");

				var skillData = _skillDb.Entries.Values.FirstOrDefault(a => a.StringId == skillStringId);
				if (skillData == null)
					throw new DatabaseErrorException(null, $"Unknown skill id '{skillStringId}'.");

				var treeSkillData = new SkillTreeSkillData();

				treeSkillData.SkillId = skillData.ClassId;
				treeSkillData.MaxLevel = skill.ReadInt("maxLevel");

				if (skill.ContainsKey("prerequisites"))
				{
					foreach (JProperty prerequisite in skill["prerequisites"])
					{
						skillStringId = prerequisite.Name;

						var prereqSkillData = _skillDb.Entries.Values.FirstOrDefault(a => a.StringId == skillStringId);
						if (prereqSkillData == null)
							throw new DatabaseErrorException(null, $"Unknown prerequisite skill id '{skillStringId}'.");

						var treePrereqData = new SkillTreePrerequisiteData();

						treePrereqData.SkillId = prereqSkillData.ClassId;
						treePrereqData.MinLevel = (int)prerequisite.Value;

						treeSkillData.Prerequisites.Add(treePrereqData);
					}
				}

				treeJobData.Skills.Add(treeSkillData);
			}

			this.AddOrReplace(treeJobData.JobId, treeJobData);
		}
	}
}
