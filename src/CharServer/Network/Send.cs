using System.Collections.Generic;
using System.Net;
using Sabine.Char.Database;
using Sabine.Char.Network.Helpers;
using Sabine.Shared.Const;
using Sabine.Shared.Network;

namespace Sabine.Char.Network
{
	internal static class Send
	{
		public static void HC_REFUSE_ENTER(CharConnection conn, CharConnectError errorCode)
		{
			var packet = new Packet(Op.HC_REFUSE_ENTER);
			packet.PutByte((byte)errorCode);

			conn.Send(packet);
		}

		public static void HC_ACCEPT_ENTER(CharConnection conn, IEnumerable<Character> characters)
		{
			var packet = new Packet(Op.HC_ACCEPT_ENTER);

			foreach (var character in characters)
				packet.AddCharacter(character);

			conn.Send(packet);
		}

		public static void HC_NOTIFY_ZONESVR(CharConnection conn, int characterId, string mapName, string ip, int port)
		{
			mapName += ".gat";

			var packet = new Packet(Op.HC_NOTIFY_ZONESVR);

			packet.PutInt(characterId);
			packet.PutString(mapName, 16);
			packet.PutInt(IPAddress.Parse(ip));
			packet.PutShort((short)port);

			conn.Send(packet);
		}

		public static void HC_REFUSE_MAKECHAR(CharConnection conn, CharCreateError error)
		{
			var packet = new Packet(Op.HC_REFUSE_MAKECHAR);
			packet.PutByte((byte)error);

			conn.Send(packet);
		}

		public static void HC_ACCEPT_MAKECHAR(CharConnection conn, Character character)
		{
			var packet = new Packet(Op.HC_ACCEPT_MAKECHAR);
			packet.AddCharacter(character);

			conn.Send(packet);
		}
	}
}
