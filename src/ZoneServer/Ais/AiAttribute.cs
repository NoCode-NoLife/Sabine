using System;

namespace Sabine.Zone.Ais
{
	/// <summary>
	/// Specifies the AI types that an AI handles.
	/// </summary>
	public class AiAttribute : Attribute
	{
		/// <summary>
		/// Returns the names of the AI types the AI handles.
		/// </summary>
		public string[] Names { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="names"></param>
		public AiAttribute(params string[] names)
		{
			this.Names = names;
		}
	}
}
