using System;
using System.Collections.Generic;
using System.Net;
using Sabine.Char.Database;
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
			{
				packet.PutInt(character.Id);
				packet.PutInt(character.BaseExp);
				packet.PutInt(character.Zeny);
				packet.PutInt(character.JobExp);
				packet.PutInt(0); // ?
				packet.PutInt(0); // ?
				packet.PutInt(0); // ?
				packet.PutShort((short)character.StatPoints);
				packet.PutShort((short)character.Hp);
				packet.PutShort((short)character.HpMax);
				packet.PutShort((short)character.Sp);
				packet.PutShort((short)character.SpMax);
				packet.PutShort((short)character.Speed);
				packet.PutShort(0); // Karma
				packet.PutShort(0); // Manner
				packet.PutString(character.Name, 16);
				packet.PutByte((byte)character.JobId);
				packet.PutByte((byte)character.BaseLevel);
				packet.PutByte((byte)character.JobLevel);
				packet.PutByte((byte)character.Str);
				packet.PutByte((byte)character.Agi);
				packet.PutByte((byte)character.Vit);
				packet.PutByte((byte)character.Int);
				packet.PutByte((byte)character.Dex);
				packet.PutByte((byte)character.Luk);
				packet.PutByte((byte)character.Slot);
				packet.PutByte(0); // Gap
				packet.PutByte((byte)character.HairId);
				packet.PutByte((byte)character.WeaponId);
				packet.PutByte(0); // ?
			}

			conn.Send(packet);
		}

		public static void HC_NOTIFY_ZONESVR(CharConnection conn, int characterId, string mapName, string ip, int port)
		{
			var ipInt = BitConverter.ToInt32(IPAddress.Parse(ip).GetAddressBytes(), 0);
			mapName += ".gat";

			var packet = new Packet(Op.HC_NOTIFY_ZONESVR);

			packet.PutInt(characterId);
			packet.PutString(mapName, 16);
			packet.PutInt(ipInt);
			packet.PutShort((short)port);

			conn.Send(packet);
		}
	}
}
