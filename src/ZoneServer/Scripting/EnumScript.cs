using System;
using Yggdrasil.Scripting;
using Yggdrasil.Versioning.ManagedEnum;

namespace Sabine.Zone.Scripting
{
	/// <summary>
	/// Script class dedicated to setting up enum mappings.
	/// </summary>
	public class EnumScript<TEnum> : IScript where TEnum : struct, Enum
	{
		/// <summary>
		/// Initializes script.
		/// </summary>
		/// <returns></returns>
		public bool Init()
		{
			if (!ScriptRequirementAttribute.ValidateAll(this))
				return true;

			this.Load();
			return true;
		}

		/// <summary>
		/// Called when the script is being initialized.
		/// </summary>
		public virtual void Load()
		{
		}

		/// <summary>
		/// Inserts the given enum value at the end of the enum mapping.
		/// </summary>
		/// <param name="enumKey"></param>
		protected void Define(TEnum enumKey)
		{
			MEnum<TEnum>.Shared.InsertValue(enumKey);
		}

		/// <summary>
		/// Inserts the given enum value into the enum mapping,
		/// potentially shifting other values.
		/// </summary>
		/// <param name="enumKey"></param>
		/// <param name="value"></param>
		protected void Define(TEnum enumKey, int value)
		{
			MEnum<TEnum>.Shared.InsertValue(enumKey, value);
		}
	}
}
