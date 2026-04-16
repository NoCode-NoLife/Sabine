namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines the type of a response to a trading request.
	/// </summary>
	public enum TradingResponse : byte
	{
		/// <summary>
		/// Character is too far away.
		/// </summary>
		TooFar = 0,

		/// <summary>
		/// Character not found.
		/// </summary>
		NotFound = 1,

		/// <summary>
		/// Request failed.
		/// </summary>
		Fail = 2,

		/// <summary>
		/// Request accepted.
		/// </summary>
		Accept = 3,

		/// <summary>
		/// Request rejected.
		/// </summary>
		Cancel = 4,
	}

	/// <summary>
	/// Defines which side of the trade was affected.
	/// </summary>
	public enum TradingSide : byte
	{
		/// <summary>
		/// Own side.
		/// </summary>
		Self = 0,

		/// <summary>
		/// Trading partner's side.
		/// </summary>
		Partner = 1,
	}

	/// <summary>
	/// Defines the result of a trading operation.
	/// </summary>
	public enum TradingSuccess : byte
	{
		/// <summary>
		/// Operation was executed successfully.
		/// </summary>
		Success = 0,

		/// <summary>
		/// Operation failed.
		/// </summary>
		Fail = 1,
	}
}
