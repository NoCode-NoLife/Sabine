using System;
using Sabine.Shared.Const;
using Sabine.Shared.Extensions;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// Packet senders.
	/// </summary>
	public static class Send
	{
		/// <summary>
		/// Accepts connection request, makes client load map.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ACCEPT_ENTER(ZoneConnection conn, PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_ACCEPT_ENTER);

			packet.PutInt(character.Id);
			packet.AddPackedPosition(character.Position, 0);
			packet.PutShort(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends server time to client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="serverTime"></param>
		public static void ZC_NOTIFY_TIME(ZoneConnection conn, DateTime serverTime)
		{
			var packet = new Packet(Op.ZC_NOTIFY_TIME);
			packet.PutInt(serverTime.GetUnixTimestamp());

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character appear.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY(ZoneConnection conn, PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY);

			packet.PutInt(character.SessionId);
			packet.PutShort((short)character.Speed);
			packet.PutByte((byte)character.JobId);
			packet.PutByte((byte)character.Sex);
			packet.AddPackedPosition(character.Position, 0);
			packet.PutShort(0);
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0);
			packet.PutByte(0); // dead_sit

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character appear with spawn effect.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_NEWENTRY(ZoneConnection conn, PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_NEWENTRY);

			packet.PutInt(character.SessionId);
			packet.PutShort((short)character.Speed);
			packet.PutByte((byte)character.JobId);
			packet.PutByte((byte)character.Sex);
			packet.AddPackedPosition(character.Position, 0);
			packet.PutShort(0);
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character move from one position to the other.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVE(ZoneConnection conn, PlayerCharacter character, Position from, Position to)
		{
			var packet = new Packet(Op.ZC_NOTIFY_MOVE);

			packet.PutInt(character.SessionId);
			packet.AddPackedMove(from, to, 8, 8);
			packet.PutInt(DateTime.Now.GetUnixTimestamp());

			conn.Send(packet);
		}

		/// <summary>
		/// Makes player's character move, as response to a move request.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_PLAYERMOVE(PlayerCharacter character, Position from, Position to)
		{
			var packet = new Packet(Op.ZC_NOTIFY_PLAYERMOVE);

			packet.PutInt(DateTime.Now.GetUnixTimestamp());
			packet.AddPackedMove(from, to, 8, 8);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends the character's name to the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ACK_REQNAME(ZoneConnection conn, PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_ACK_REQNAME);

			packet.PutInt(character.SessionId);
			packet.PutString(character.Username, 16);
			packet.PutString(character.Name, 16);

			conn.Send(packet);
		}

		/// <summary>
		/// Updates the given parameter on the client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public static void ZC_PAR_CHANGE(PlayerCharacter character, ParameterType type, int value)
		{
			var packet = new Packet(Op.ZC_PAR_CHANGE);

			packet.PutShort((short)type);
			packet.PutShort((short)value);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the given parameter on the client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public static void ZC_LONGPAR_CHANGE(PlayerCharacter character, ParameterType type, int value)
		{
			var packet = new Packet(Op.ZC_LONGPAR_CHANGE);

			packet.PutShort((short)type);
			packet.PutInt(value);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Response to stat change request.
		/// </summary>
		/// <remarks>
		/// Stat is only updated with the new value if the assigment
		/// was successful.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="success"></param>
		/// <param name="value"></param>
		public static void ZC_STATUS_CHANGE_ACK(PlayerCharacter character, ParameterType type, bool success, int value)
		{
			var packet = new Packet(Op.ZC_STATUS_CHANGE_ACK);

			packet.PutShort((short)type);
			packet.PutByte(success);
			packet.PutByte((byte)value);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates several stats and sub-stats on the character's client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_STATUS(PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_STATUS);

			packet.PutShort((short)character.StatPoints);
			packet.PutByte((byte)character.Str);
			packet.PutByte((byte)character.StrNeeded);
			packet.PutByte((byte)character.Agi);
			packet.PutByte((byte)character.AgiNeeded);
			packet.PutByte((byte)character.Vit);
			packet.PutByte((byte)character.VitNeeded);
			packet.PutByte((byte)character.Int);
			packet.PutByte((byte)character.IntNeeded);
			packet.PutByte((byte)character.Dex);
			packet.PutByte((byte)character.DexNeeded);
			packet.PutByte((byte)character.Luk);
			packet.PutByte((byte)character.LukNeeded);
			packet.PutByte((byte)character.AtkMin);
			packet.PutByte((byte)character.AtkMax);
			packet.PutByte((byte)character.Defense);
			packet.PutByte((byte)character.Matk);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends public chat packet to players around character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="message"></param>
		public static void ZC_NOTIFY_CHAT(PlayerCharacter character, string message)
		{
			var packet = new Packet(Op.ZC_NOTIFY_CHAT);

			packet.PutInt(character.SessionId);
			packet.PutString(message);

			character.Map.Broadcast(packet, character, false);
		}

		/// <summary>
		/// Sends chat packet to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="id"></param>
		/// <param name="message"></param>
		public static void ZC_NOTIFY_CHAT(PlayerCharacter character, int id, string message)
		{
			var packet = new Packet(Op.ZC_NOTIFY_CHAT);

			packet.PutInt(id);
			packet.PutString(message);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends public chat packet to character's client, displaying
		/// it above their head.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="message"></param>
		public static void ZC_NOTIFY_PLAYERCHAT(PlayerCharacter character, string message)
		{
			var packet = new Packet(Op.ZC_NOTIFY_PLAYERCHAT);
			packet.PutString(message);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Broadcasts message on all maps, displayed in the center
		/// of the game screen.
		/// </summary>
		/// <param name="message"></param>
		public static void ZC_BROADCAST(string message)
		{
			var packet = new Packet(Op.ZC_BROADCAST);
			packet.PutString(message);

			var maps = ZoneServer.Instance.World.Maps.GetAll();
			foreach (var map in maps)
				map.Broadcast(packet);
		}

		/// <summary>
		/// Updates the character's sprites for all players near them.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public static void ZC_SPRITE_CHANGE(PlayerCharacter character, SpriteType type, int value)
		{
			var packet = new Packet(Op.ZC_SPRITE_CHANGE);

			packet.PutInt(character.SessionId);
			packet.PutByte((byte)type);
			packet.PutByte((byte)value);

			character.Map.Broadcast(packet, character, true);
		}

		/// <summary>
		/// Sends the number of players who are online to the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="count"></param>
		public static void ZC_USER_COUNT(ZoneConnection conn, int count)
		{
			var packet = new Packet(Op.ZC_USER_COUNT);
			packet.PutInt(count);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character use the given emotion.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="emotion"></param>
		public static void ZC_EMOTION(PlayerCharacter character, EmotionId emotion)
		{
			var packet = new Packet(Op.ZC_EMOTION);
			packet.PutInt(character.SessionId);
			packet.PutByte((byte)emotion);

			character.Map.Broadcast(packet, character, true);
		}

		/// <summary>
		/// Sends item description to the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public static void ZC_REQ_ITEM_EXPLANATION_ACK(ZoneConnection conn, string name, string description)
		{
			var packet = new Packet(Op.ZC_REQ_ITEM_EXPLANATION_ACK);

			packet.PutString(name, 16);
			packet.PutString(description);

			conn.Send(packet);
		}
	}
}
