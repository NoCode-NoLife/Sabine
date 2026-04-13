using System;
using System.Linq;
using Sabine.Shared.Data;

namespace Sabine.Zone.Scripting
{
	/// <summary>
	/// Base class for attributes that specify requirements for a script
	/// to be loaded.
	/// </summary>
	/// <remarks>
	/// Works only with script classes that inherit from <see
	/// cref="GeneralScript"/> or otherwise validate script requirements
	/// themselves by calling <see cref="ValidateAll"/> in their Load
	/// method.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public abstract class ScriptRequirementAttribute : Attribute
	{
		/// <summary>
		/// Returns true if the requirement is met and the script can be
		/// loaded.
		/// </summary>
		/// <returns></returns>
		public abstract bool Validate();

		/// <summary>
		/// Returns true if all requirements specified by attributes on
		/// the given type are met and the script can be loaded.
		/// </summary>
		/// <param name="obj">The object to validate.</param>
		/// <returns>True if all requirements are met.</returns>
		public static bool ValidateAll(object obj)
		{
			var attributes = obj.GetType().GetCustomAttributes(typeof(ScriptRequirementAttribute), false).OfType<ScriptRequirementAttribute>();

			if (attributes.Any())
			{
				foreach (var attribute in attributes)
				{
					if (!attribute.Validate())
						return false;
				}
			}

			return true;
		}
	}

	/// <summary>
	/// Indicates that a script requires the specified maps to be present
	/// in order to load.
	/// </summary>
	/// <param name="mapStringIds"></param>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequiresMapsAttribute(params string[] mapStringIds) : ScriptRequirementAttribute
	{
		public string[] MapStringIds { get; } = mapStringIds;

		/// <summary>
		/// Returns true if all required maps are present in the world.
		/// </summary>
		/// <returns></returns>
		public override bool Validate()
		{
			foreach (var mapStringId in this.MapStringIds)
			{
				if (!ZoneServer.Instance.World.Maps.TryGetByStringId(mapStringId, out _))
					return false;
			}

			return true;
		}
	}

	/// <summary>
	/// Indicates that a script requires the specified features to be
	/// enabled in order to load.
	/// </summary>
	/// <param name="features"></param>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
	public class RequiresFeaturesAttribute(params string[] features) : ScriptRequirementAttribute
	{
		public string[] Features { get; } = features;

		/// <summary>
		/// Returns true if all required features are enabled.
		/// </summary>
		/// <returns></returns>
		public override bool Validate()
		{
			foreach (var feature in this.Features)
			{
				if (!SabineData.Features.IsEnabled(feature))
					return false;
			}

			return true;
		}
	}
}
