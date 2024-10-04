using System.Collections.Generic;
using System.Net;
using Sabine.Char.Database;
using Sabine.Char.Network.Helpers;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Network;

namespace Sabine.Char.Network
{
	/// <summary>
	/// Packet senders.
	/// </summary>
	public static class Send
	{
		/// <summary>
		/// Shows error messages about why the login request was refused.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="errorCode"></param>
		public static void HC_REFUSE_ENTER(CharConnection conn, CharConnectError errorCode)
		{
			var packet = new Packet(Op.HC_REFUSE_ENTER);
			packet.PutByte((byte)errorCode);

			conn.Send(packet);
		}

		/// <summary>
		/// Accepts login request and makes client transition to the
		/// character selection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="characters"></param>
		public static void HC_ACCEPT_ENTER(CharConnection conn, IEnumerable<Character> characters)
		{
			var packet = new Packet(Op.HC_ACCEPT_ENTER);

			// The client expects '(len - 4) % 106 + 2' bytes here, meaning
			// a minimum of 2. It's unknown what exactly these bytes do.
			if (Game.Version >= Versions.EP4)
				packet.PutShort(0);

			foreach (var character in characters)
				packet.AddCharacter(character);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends zone server connection information to the client,
		/// making it connect to it.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="characterId"></param>
		/// <param name="mapStringId"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public static void HC_NOTIFY_ZONESVR(CharConnection conn, int characterId, string mapStringId, string ip, int port)
		{
			var mapFileName = mapStringId + ".gat";

			var packet = new Packet(Op.HC_NOTIFY_ZONESVR);

			packet.PutInt(characterId);
			packet.PutString(mapFileName, 16);
			packet.PutInt(IPAddress.Parse(ip));
			packet.PutShort((short)port);

			conn.Send(packet);
		}

		/// <summary>
		/// Shows error message about why the character creation request
		/// was refused.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="error"></param>
		public static void HC_REFUSE_MAKECHAR(CharConnection conn, CharCreateError error)
		{
			var packet = new Packet(Op.HC_REFUSE_MAKECHAR);
			packet.PutByte((byte)error);

			conn.Send(packet);
		}

		/// <summary>
		/// Accepts character creation request, adding the character to
		/// the selectable ones.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void HC_ACCEPT_MAKECHAR(CharConnection conn, Character character)
		{
			var packet = new Packet(Op.HC_ACCEPT_MAKECHAR);
			packet.AddCharacter(character);

			conn.Send(packet);
		}

		/// <summary>
		/// Accepts character deletion request, removing it from the
		/// selectable characters on the client.
		/// </summary>
		/// <param name="conn"></param>
		public static void HC_ACCEPT_DELETECHAR(CharConnection conn)
		{
			var packet = new Packet(Op.HC_ACCEPT_DELETECHAR);
			conn.Send(packet);
		}

		/// <summary>
		/// Shows an error message for why a character deletion request
		/// was refused.
		/// </summary>
		/// <param name="conn"></param>
		public static void HC_REFUSE_DELETECHAR(CharConnection conn)
		{
			var packet = new Packet(Op.HC_REFUSE_DELETECHAR);
			packet.PutByte(0); // Doesn't seem to do anything.

			conn.Send(packet);
		}
	}
}
