using System.Net;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;

namespace Sabine.Auth.Network
{
	/// <summary>
	/// Packet senders.
	/// </summary>
	public static class Send
	{
		/// <summary>
		/// Shows error message on the client about why the login request
		/// was refused.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="errorCode"></param>
		public static void AC_REFUSE_LOGIN(AuthConnection conn, LoginConnectError errorCode)
		{
			var packet = new Packet(Op.AC_REFUSE_LOGIN);
			packet.PutByte((byte)errorCode);

			conn.Send(packet);
		}

		/// <summary>
		/// Accepts login request, making the client display a list of
		/// available char servers.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="account"></param>
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
