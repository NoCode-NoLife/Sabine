using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	public class SkillData
	{
		public SkillId ClassId { get; set; }
		public string StringId { get; set; }
		public SkillTargetType TargetType { get; set; }
		public int MaxLevel { get; set; }
		public int[] Range { get; set; }
		public int[] SpCost { get; set; }
		public int[] HpCost { get; set; }

		/// <summary>
		/// Returns the value for the given level. If the level is out of
		/// bounds, it will be clamped to the valid range, returning the
		/// highest or lowest value respectively.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public int GetRange(int level)
			=> this.Range[Math.Clamp(level - 1, 0, this.Range.Length - 1)];

		/// <summary>
		/// Returns the value for the given level. If the level is out of
		/// bounds, it will be clamped to the valid range, returning the
		/// highest or lowest value respectively.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public int GetSpCost(int level)
			=> this.SpCost[Math.Clamp(level - 1, 0, this.SpCost.Length - 1)];

		/// <summary>
		/// Returns the value for the given level. If the level is out of
		/// bounds, it will be clamped to the valid range, returning the
		/// highest or lowest value respectively.
		/// </summary>
		/// <param name="level"></param>
		/// <returns></returns>
		public int GetHpCost(int level)
			=> this.HpCost[Math.Clamp(level - 1, 0, this.HpCost.Length - 1)];
	}

	/// <summary>
	/// A skill database.
	/// </summary>
	public class SkillDb : DatabaseJsonIndexed<SkillId, SkillData>
	{
		/// <summary>
		/// Called to read an entry from the data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "stringId", "target", "maxLevel");

			var data = new SkillData();

			data.ClassId = (SkillId)entry.ReadInt("id");
			data.StringId = entry.ReadString("stringId");
			data.TargetType = entry.ReadEnum<SkillTargetType>("target");
			data.MaxLevel = entry.ReadInt("maxLevel");

			data.Range = ReadOptionalValueList(entry, "range");
			data.SpCost = ReadOptionalValueList(entry, "spCost");
			data.HpCost = ReadOptionalValueList(entry, "hpCost");

			this.AddOrReplace(data.ClassId, data);
		}

		/// <summary>
		/// Reads property that, if set, can be either a single value or a
		/// list of values. Returns an array of values, defaulting to an
		/// array with a single 0 if the property is not set.
		/// </summary>
		/// <param name="entry"></param>
		/// <param name="key"></param>
		/// <returns></returns>
		/// <exception cref="DatabaseWarningException"></exception>
		private static int[] ReadOptionalValueList(JObject entry, string key)
		{
			if (!entry.ContainsKey(key))
				return DefaultValueList;

			if (entry[key] is JValue value)
				return [(int)value];

			if (entry[key] is JArray array)
				return array.Select(static x => (int)x).ToArray();

			throw new DatabaseWarningException(null, $"Invalid format for '{key}' in skill data.");
		}

		private static readonly int[] DefaultValueList = [0];
	}
}
