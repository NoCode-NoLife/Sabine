using System.Net;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;

namespace Sabine.Auth.Network
{
	internal static class Send
	{
		public static void AC_REFUSE_LOGIN(AuthConnection conn, LoginConnectError errorCode)
		{
			var packet = new Packet(Op.AC_REFUSE_LOGIN);
			packet.PutByte((byte)errorCode);

			conn.Send(packet);
		}

		public static void AC_ACCEPT_LOGIN(AuthConnection conn, Account account)
		{
			var packet = new Packet(Op.AC_ACCEPT_LOGIN);

			packet.PutInt(account.Id);
			packet.PutByte((byte)account.Sex);
			packet.PutInt(account.SessionId);

			// for server in servers
			{
				var charIp = AuthServer.Instance.Conf.Char.ServerIp;
				var charPort = AuthServer.Instance.Conf.Char.BindPort;
				var charName = AuthServer.Instance.Conf.Char.Name;

				packet.PutInt(IPAddress.Parse(charIp));
				packet.PutShort((short)charPort);
				packet.PutString(charName, 20);
			}

			conn.Send(packet);
		}
	}
}
