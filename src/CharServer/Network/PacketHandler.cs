using System;
using System.Linq;
using Sabine.Char.Database;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Network;
using Sabine.Shared.World;
using Yggdrasil.Logging;

namespace Sabine.Char.Network
{
	/// <summary>
	/// Packet handler methods.
	/// </summary>
	public class PacketHandler : PacketHandler<CharConnection>
	{
		/// <summary>
		/// Login request, first packet sent upon connection.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CH_ENTER)]
		public void CH_ENTER(CharConnection conn, Packet packet)
		{
			var sessionId = packet.GetInt();
			var accountId = packet.GetInt();
			var b1 = packet.GetByte(); // 1? language?
			var b2 = packet.GetByte(); // 0?
			var sex = packet.GetByte();

			//if(b1 != 1)
			//{
			//	Send.HC_REFUSE_ENTER(conn, CharConnectError.LanguageIncorrect);
			//	conn.Close();
			//	return;
			//}

			var account = CharServer.Instance.Database.GetAccountById(accountId);
			if (account == null)
			{
				Send.HC_REFUSE_ENTER(conn, CharConnectError.AccessDenied);
				conn.Close(1);
				return;
			}

			if (sessionId != account.SessionId)
			{
				Log.Warning("CH_ENTER: User '{0}' tried to log in with an invalid session id.", account.Username);
				Send.HC_REFUSE_ENTER(conn, CharConnectError.AccessDenied);
				conn.Close(1);
				return;
			}

			var characters = CharServer.Instance.Database.GetCharacters(account);

			conn.Account = account;
			conn.Characters.AddRange(characters);

			Send.HC_ACCEPT_ENTER(conn, characters);

			Log.Info("User '{0}' logged in.", account.Username);
		}

		/// <summary>
		/// Request to log in with the selected character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CH_SELECT_CHAR)]
		public void CH_SELECT_CHAR(CharConnection conn, Packet packet)
		{
			var slot = packet.GetByte();

			var character = conn.Characters.FirstOrDefault(a => a.Slot == slot);
			if (character == null)
			{
				Log.Warning("CH_SELECT_CHAR: User '{0}' tried to select a non-existing character.", conn.Account.Username);
				return;
			}

			if (!SabineData.Maps.TryFind(character.Location.MapId, out var mapData))
			{
				Log.Error("CH_SELECT_CHAR: Character '{0}' is on an invalid map ({1}).", character.Name, character.Location.MapId);
				return;
			}

			var zoneServerIp = CharServer.Instance.Conf.Zone.ServerIp;
			var zoneServerPort = CharServer.Instance.Conf.Zone.BindPort;

			Send.HC_NOTIFY_ZONESVR(conn, character.Id, mapData.StringId, zoneServerIp, zoneServerPort);
		}

		/// <summary>
		/// Request to create a new character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CH_MAKE_CHAR)]
		public void CH_MAKE_CHAR(CharConnection conn, Packet packet)
		{
			var character = new Character();

			character.Name = packet.GetString(16);
			character.Str = packet.GetByte();
			character.Agi = packet.GetByte();
			character.Vit = packet.GetByte();
			character.Int = packet.GetByte();
			character.Dex = packet.GetByte();
			character.Luk = packet.GetByte();
			character.Slot = packet.GetByte();
			character.HairId = packet.GetByte();

			var account = conn.Account;
			var db = CharServer.Instance.Database;

			var isSlotValid = character.Slot >= 0 && character.Slot <= 2;
			if (!isSlotValid)
			{
				Log.Warning("CH_MAKE_CHAR: User '{0}' tried to create a character in an invalid slot.", account.Username);
				Send.HC_REFUSE_MAKECHAR(conn, CharCreateError.Denied);
				return;
			}

			var statDistribution = character.Str + character.Agi + character.Vit + character.Int + character.Dex + character.Luk;
			if (statDistribution != 30)
			{
				Log.Warning("CH_MAKE_CHAR: User '{0}' tried to create a character with an invalid stat distribution ({1}).", account.Username, statDistribution);
				Send.HC_REFUSE_MAKECHAR(conn, CharCreateError.Denied);
				return;
			}

			var slotInUse = conn.Characters.Any(a => a.Slot == character.Slot);
			if (slotInUse)
			{
				Log.Warning("CH_MAKE_CHAR: User '{0}' tried to create a character in an occupied slot.", account.Username);
				Send.HC_REFUSE_MAKECHAR(conn, CharCreateError.Denied);
				return;
			}

			if (db.CharacterNameExists(character.Name))
			{
				Send.HC_REFUSE_MAKECHAR(conn, CharCreateError.NameExistsAlready);
				return;
			}

			if (!SabineData.Maps.TryFind(CharServer.Instance.Conf.Char.StartMapStringId, out var mapData))
			{
				Log.Error("CH_MAKE_CHAR: Unknown start map '{0}'.", CharServer.Instance.Conf.Char.StartMapStringId);
				Send.HC_REFUSE_MAKECHAR(conn, CharCreateError.Denied);
				return;
			}

			character.Hp = character.HpMax = (int)(40 * (1 + character.Vit / 100.0));
			character.Sp = character.SpMax = (int)(10 * (1 + character.Int / 100.0));
			character.Zeny = CharServer.Instance.Conf.Char.StartZeny;
			character.Location = new Location(mapData.Id, CharServer.Instance.Conf.Char.StartPosition);

			db.CreateCharacter(account, ref character);
			conn.Characters.Add(character);

			Send.HC_ACCEPT_MAKECHAR(conn, character);

			Log.Info("User '{0}' created character '{1}'.", account.Username, character.Name);
		}

		/// <summary>
		/// Request to delete a character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CH_DELETE_CHAR)]
		public void CH_DELETE_CHAR(CharConnection conn, Packet packet)
		{
			var characterId = packet.GetInt();

			var character = conn.Characters.FirstOrDefault(a => a.Id == characterId);
			if (character == null)
			{
				Send.HC_REFUSE_DELETECHAR(conn);
				Log.Warning("CH_DELETE_CHAR: User '{0}' tried to selete a non-existing character.", conn.Account.Username);
				return;
			}

			var deletedFromDb = CharServer.Instance.Database.RemoveCharacter(character.Id);
			if (!deletedFromDb)
			{
				Send.HC_REFUSE_DELETECHAR(conn);
				Log.Debug("CH_DELETE_CHAR: Deletion of character '{1}' of user '{0}' failed.", conn.Account.Username, characterId);
				return;
			}

			conn.Characters.Remove(character);

			Send.HC_ACCEPT_DELETECHAR(conn);

			Log.Info("User '{0}' deleted character '{1}'.", conn.Account.Username, character.Name);
		}
	}
}
