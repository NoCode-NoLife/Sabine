using System.Linq;
using Sabine.Shared.Network;
using Yggdrasil.Logging;

namespace Sabine.Char.Network
{
	public class PacketHandler : PacketHandler<CharConnection>
	{
		[PacketHandler(Op.CH_ENTER)]
		public void CH_ENTER(CharConnection conn, Packet packet)
		{
			var sessionId = packet.GetInt();
			var accountId = packet.GetInt();
			var b1 = packet.GetByte(); // 1? language?
			var b2 = packet.GetByte(); // 0?
			var sex = packet.GetByte();

			Log.Debug("Login request: {0:X8}, {1:X8}", sessionId, accountId);

			//Send.HC_REFUSE_ENTER(conn, CharConnectError.LanguageIncorrect);
			Send.HC_ACCEPT_ENTER(conn, conn.Characters);
		}

		[PacketHandler(Op.CH_SELECT_CHAR)]
		public void CH_SELECT_CHAR(CharConnection conn, Packet packet)
		{
			var slot = packet.GetByte();

			var character = conn.Characters.FirstOrDefault(a => a.Slot == slot);
			if (character == null)
			{
				Log.Warning("CH_SELECT_CHAR: User '{0}' tried to select a non-existing character slot.", conn.Account.Username);
				return;
			}

			Log.Debug("Character selected: {0}", character.Name);

			Send.HC_NOTIFY_ZONESVR(conn, character.Id, character.MapName, "127.0.0.1", 7002);
		}
	}
}
