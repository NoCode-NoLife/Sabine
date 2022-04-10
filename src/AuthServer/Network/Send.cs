using System;
using System.Net;
using Sabine.Shared.Const;
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

		public static void AC_ACCEPT_LOGIN(AuthConnection conn)
		{
			var packet = new Packet(Op.AC_ACCEPT_LOGIN);

			packet.PutInt(0x12345678); // AccountId
			packet.PutByte(1); // sex: 0=f, 1=m
			packet.PutInt(int.MaxValue); // SessionId

			// for server in servers
			{
				var charIp = AuthServer.Instance.Conf.Char.ServerIp;
				var charPort = AuthServer.Instance.Conf.Char.BindPort;
				var charName = AuthServer.Instance.Conf.Char.Name;

				var charIpInt = BitConverter.ToInt32(IPAddress.Parse(charIp).GetAddressBytes(), 0);

				packet.PutInt(charIpInt);
				packet.PutShort((short)charPort);
				packet.PutString(charName, 20);
			}

			conn.Send(packet);
		}
	}
}
