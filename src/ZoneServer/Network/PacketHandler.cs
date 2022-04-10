using System;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;

namespace Sabine.Zone.Network
{
	public class PacketHandler : PacketHandler<ZoneConnection>
	{
		/// <summary>
		/// Login request sent upon connecting to the server.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ENTER)]
		public void CZ_ENTER(ZoneConnection conn, Packet packet)
		{
			var sessionId = packet.GetInt();
			var characterId = packet.GetInt();
			var accountId = packet.GetInt();
			var sex = packet.GetByte();

			conn.Account = new Account() { Id = accountId, SessionId = sessionId, Username = "admin", Password = "admin", Sex = Sex.Male, Authority = 99 };
			conn.Character = new PlayerCharacter() { Id = characterId, Name = "exec", Connection = conn };

			var map = ZoneServer.Instance.World.Maps.Get(conn.Character.MapName);
			if (map == null)
				throw new Exception($"Map '{conn.Character.MapName}' not found.");

			map.AddCharacter(conn.Character);

			Send.ZC_ACCEPT_ENTER(conn, conn.Character);
		}

		/// <summary>
		/// Notification that the client loaded the map and the character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_NOTIFY_ACTORINIT)]
		public void CZ_NOTIFY_ACTORINIT(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			Send.ZC_STATUS(character);
			Send.ZC_PAR_CHANGE(character, ParameterType.Weight, character.Weight);
			Send.ZC_PAR_CHANGE(character, ParameterType.WeightMax, character.WeightMax);
			Send.ZC_PAR_CHANGE(character, ParameterType.SkillPoints, character.SkillPoints);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.BaseExpNeeded, character.BaseExpNeeded);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.JobExpNeeded, character.JobExpNeeded);
		}

		/// <summary>
		/// Request for the current server time.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_TIME)]
		public void CZ_REQUEST_TIME(ZoneConnection conn, Packet packet)
		{
			Send.ZC_NOTIFY_TIME(conn, DateTime.Now);
		}

		/// <summary>
		/// Request to move to a new position.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_MOVE)]
		public void CZ_REQUEST_MOVE(ZoneConnection conn, Packet packet)
		{
			var toPos = (Position)packet.GetPackedPosition();

			var character = conn.GetCurrentCharacter();
			var fromPos = character.Position;
			character.Position = toPos;

			Log.Debug("{0} requests to move to {1},{2}.", character.Name, toPos.X, toPos.Y);

			Send.ZC_NOTIFY_PLAYERMOVE(character, fromPos, toPos);
		}

		/// <summary>
		/// Request to say something in chat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_CHAT)]
		public void CZ_REQUEST_CHAT(ZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var text = packet.GetString(len - sizeof(short) * 2);

			// The client sends the chat message in the format
			// "name : message", which is inconvenient for us.
			// We'll trim it down to the actual message.
			var index = text.IndexOf(':');
			if (index != -1)
				text = text.Substring(index + 1).Trim();

			var character = conn.GetCurrentCharacter();

			if (ZoneServer.Instance.ChatCommands.TryExecute(character, text))
				return;

			text = string.Format("{0} : {1}", character.Name, text);

			Send.ZC_NOTIFY_CHAT(character, text);
			Send.ZC_NOTIFY_PLAYERCHAT(character, text);
		}

		/// <summary>
		/// Request for a character's name when hovering over them.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQNAME)]
		public void CZ_REQNAME(ZoneConnection conn, Packet packet)
		{
			var characterId = packet.GetInt();

			var character = new PlayerCharacter() { Id = characterId, Name = "TestName" };

			Send.ZC_ACK_REQNAME(conn, character);
		}

		/// <summary>
		/// Request to increase a stat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_STATUS_CHANGE)]
		public void CZ_STATUS_CHANGE(ZoneConnection conn, Packet packet)
		{
			var type = (ParameterType)packet.GetShort();
			var change = (int)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var success = false;
			var value = 0;

			if (type < ParameterType.Str || type > ParameterType.Luk)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to assign points to invalid stat '{1}'.", conn.Account.Username, type);
				goto L_End;
			}

			var pointsNeeded = character.GetStatPointsNeeded(type);
			if (character.StatPoints < pointsNeeded)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to use more stat points than they have.", conn.Account.Username);
				goto L_End;
			}

			value = character.ModifyStat(type, change);
			character.ModifyStatPoints(-pointsNeeded);

			success = true;

		L_End:
			Send.ZC_STATUS_CHANGE_ACK(character, type, success, value);
		}

		/// <summary>
		/// Request for the amount of players online via the /who command.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_USER_COUNT)]
		public void CZ_REQ_USER_COUNT(ZoneConnection conn, Packet packet)
		{
			var count = ZoneServer.Instance.World.GetPlayerCount();
			Send.ZC_USER_COUNT(conn, count);
		}

		/// <summary>
		/// Request to use an emotion.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_EMOTION)]
		public void CZ_REQ_EMOTION(ZoneConnection conn, Packet packet)
		{
			var emotion = (EmotionId)packet.GetByte();

			if (!Enum.IsDefined(typeof(EmotionId), emotion))
			{
				Log.Warning("CZ_REQ_EMOTION: User '{0}' tried to use the invalid emotion '{1}'.", conn.Account.Username, emotion);
				return;
			}

			var character = conn.GetCurrentCharacter();
			Send.ZC_EMOTION(character, emotion);
		}

		/// <summary>
		/// Request for item description.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ITEM_EXPLANATION_BYNAME)]
		public void CZ_REQ_ITEM_EXPLANATION_BYNAME(ZoneConnection conn, Packet packet)
		{
			var itemName = packet.GetString(16);

			Send.ZC_REQ_ITEM_EXPLANATION_ACK(conn, itemName, "Foobar! Heals 9001 HP.");
		}
	}
}
