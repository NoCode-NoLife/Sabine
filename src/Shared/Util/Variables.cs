namespace Sabine.Shared.Util
{
	/// <summary>
	/// Holds temporary and permanent variables.
	/// </summary>
	public class VariableContainer
	{
		/// <summary>
		/// Temporary variables, which aren't saved.
		/// </summary>
		public Variables Temp { get; } = new Variables();

		/// <summary>
		/// Permanent variables, which are saved to the database.
		/// </summary>
		public Variables Perm { get; } = new Variables();
	}

	public class Variables : Yggdrasil.Util.Variables
	{
	}
}
