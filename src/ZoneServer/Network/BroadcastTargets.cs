namespace Sabine.Zone.Network
{
	/// <summary>
	/// Specifies who should receive a broadcasted packet.
	/// </summary>
	public enum BroadcastTargets
	{
		/// <summary>
		/// Everyone receives the packet.
		/// </summary>
		All,

		/// <summary>
		/// Everyone but the source entity receives the packet.
		/// </summary>
		AllButSource,
	}
}
