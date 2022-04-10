using Sabine.Shared.Network;

namespace Sabine.Auth.Network
{
	public class AuthConnection : Connection
	{
		protected override void OnPacketReceived(Packet packet)
		{
			AuthServer.Instance.PacketHandler.Handle(this, packet);
		}
	}
}
