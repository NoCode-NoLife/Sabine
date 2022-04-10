using Sabine.Shared.Network;
using Yggdrasil.Logging;

namespace Sabine.Auth.Network
{
	public class PacketHandler : PacketHandler<AuthConnection>
	{
		[PacketHandler(Op.CA_LOGIN)]
		public void CA_LOGIN(AuthConnection conn, Packet packet)
		{
			var username = packet.GetString(16);
			var password = packet.GetString(16);

			Log.Debug("Login request: '{0}:{1}'", username, password);

			//Send.AC_REFUSE_LOGIN(conn, LoginConnectError.UserNotFound);
			Send.AC_ACCEPT_LOGIN(conn);
		}
	}
}
