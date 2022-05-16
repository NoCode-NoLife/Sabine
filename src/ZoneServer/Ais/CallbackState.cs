namespace Sabine.Zone.Ais
{
	/// <summary>
	/// An AI callback state.
	/// </summary>
	public struct CallbackState
	{
		/// <summary>
		/// Gets or sets whether a routine hook was handled, indicating to
		/// other callbacks that it's taken care of.
		/// </summary>
		public bool Handled;
	}

	/// <summary>
	/// An AI routine hook callback function.
	/// </summary>
	/// <param name="state"></param>
	public delegate void CallbackFunc(CallbackState state);
}
