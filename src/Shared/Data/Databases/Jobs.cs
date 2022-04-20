using Newtonsoft.Json.Linq;
using Sabine.Shared.Const;
using Yggdrasil.Data.JSON;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents a job's data.
	/// </summary>
	public class JobData
	{
		public JobId Id { get; set; }
		public string Name { get; set; }

		public JobModifiersData Modifiers { get; set; } = new JobModifiersData();
		public JobWeaponDelaysData WeaponDelays { get; set; } = new JobWeaponDelaysData();
	}

	/// <summary>
	/// A job's stat modifiers.
	/// </summary>
	public class JobModifiersData
	{
		public int Weight { get; set; }
		public float HpFactor { get; set; }
		public float HpMultiplier { get; set; }
		public float SpFactor { get; set; }
	}

	/// <summary>
	/// A job's weapon delays.
	/// </summary>
	public class JobWeaponDelaysData
	{
		public float BareHand { get; set; }
		public float Dagger { get; set; }
		public float Sword { get; set; }
		public float Bow { get; set; }
		public float Spear { get; set; }
		public float Axe { get; set; }
		public float Mace { get; set; }
		public float Rod { get; set; }
	}

	/// <summary>
	/// A job database.
	/// </summary>
	public class JobDb : DatabaseJsonIndexed<JobId, JobData>
	{
		/// <summary>
		/// Called to read one entry from the job data.
		/// </summary>
		/// <param name="entry"></param>
		protected override void ReadEntry(JObject entry)
		{
			entry.AssertNotMissing("id", "name", "modifiers", "weaponDelays");

			var modifiersObj = (JObject)entry["modifiers"];
			modifiersObj.AssertNotMissing("weight", "hpFactor", "hpMultiplier", "spFactor");

			var weaponDelaysObj = (JObject)entry["weaponDelays"];
			weaponDelaysObj.AssertNotMissing("bareHand", "dagger", "sword", "bow", "spear", "axe", "mace", "rod");

			var data = new JobData();

			data.Id = entry.ReadEnum<JobId>("id");
			data.Name = entry.ReadString("name");

			data.Modifiers.Weight = modifiersObj.ReadInt("weight");
			data.Modifiers.HpFactor = modifiersObj.ReadInt("hpFactor");
			data.Modifiers.HpMultiplier = modifiersObj.ReadInt("hpMultiplier");
			data.Modifiers.SpFactor = modifiersObj.ReadInt("spFactor");

			data.WeaponDelays.BareHand = weaponDelaysObj.ReadFloat("bareHand");
			data.WeaponDelays.Dagger = weaponDelaysObj.ReadFloat("dagger");
			data.WeaponDelays.Sword = weaponDelaysObj.ReadFloat("sword");
			data.WeaponDelays.Bow = weaponDelaysObj.ReadFloat("bow");
			data.WeaponDelays.Spear = weaponDelaysObj.ReadFloat("spear");
			data.WeaponDelays.Axe = weaponDelaysObj.ReadFloat("axe");
			data.WeaponDelays.Mace = weaponDelaysObj.ReadFloat("mace");
			data.WeaponDelays.Rod = weaponDelaysObj.ReadFloat("rod");

			this.AddOrReplace(data.Id, data);
		}
	}
}
