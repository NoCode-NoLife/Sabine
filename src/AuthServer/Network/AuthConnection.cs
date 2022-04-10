using Sabine.Shared.Network;

namespace Sabine.Auth.Network
{
	/// <summary>
	/// Represents a connection to a client.
	/// </summary>
	public class AuthConnection : Connection
	{
		/// <summary>
		/// Called when a packet was send by the client.
		/// </summary>
		/// <param name="packet"></param>
		protected override void OnPacketReceived(Packet packet)
		{
			AuthServer.Instance.PacketHandler.Handle(this, packet);
		}
	}
}
