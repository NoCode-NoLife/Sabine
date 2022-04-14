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
			packet.PutInt(serverTime);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character appear on the player's client.
		/// </summary>
		/// <param name="newCharacter"></param>
		public static void ZC_NOTIFY_STANDENTRY(PlayerCharacter player, ICharacter newCharacter)
		{
			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY);

			packet.PutInt(newCharacter.Handle);
			packet.PutShort((short)newCharacter.Speed);
			packet.PutByte((byte)newCharacter.ClassId);
			packet.PutByte((byte)newCharacter.Sex);
			packet.AddPackedPosition(newCharacter.Position, newCharacter.Direction);
			packet.PutShort(0);
			packet.PutByte((byte)newCharacter.HairId);
			packet.PutByte((byte)newCharacter.WeaponId);
			packet.PutByte(0); // Possibly a sprite option that wasn't implemented yet, like headgears.
			packet.PutByte((byte)newCharacter.State);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on clients of players  around it.
		/// </summary>
		/// <remarks>
		/// Currently the only known difference to ZC_NOTIFY_STANDENTRY
		/// is that this packet won't spawn character classes, as it
		/// fails to find the respective sprite's ACT file.
		/// ZC_NOTIFY_STANDENTRY meanwhile has a check for whether the
		/// class id is < 32, where the NPC sprites begin, and it seems
		/// to be handling both players and NPCs well.
		/// </remarks>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY_NPC(ICharacter character)
		{
			// Class id 32 appears to be a large shadow, possibly a warp,
			// and 63 seems to be an effect, that uses the speed field
			// as the effect id?

			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY_NPC);

			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);
			packet.PutByte((byte)character.ClassId);
			packet.PutByte((byte)character.Sex);
			packet.AddPackedPosition(character.Position, character.Direction);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes character appear on clients of players around it with
		/// a spawn effect.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_NEWENTRY(ICharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_NEWENTRY);

			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);
			packet.PutByte((byte)character.ClassId);
			packet.PutByte((byte)character.Sex);
			packet.AddPackedPosition(character.Position, character.Direction);
			packet.PutShort(0);
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes character who is currently moving appear on clients of
		/// players around it, moving between the given positions.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVEENTRY(ICharacter character, Position from, Position to)
		{
			var packet = new Packet(Op.ZC_NOTIFY_MOVEENTRY);

			packet.PutInt(character.Handle);
			packet.PutShort((short)character.Speed);
			packet.PutByte((byte)character.ClassId);
			packet.PutByte((byte)character.Sex);
			packet.AddPackedMove(from, to, 8, 8);
			packet.PutShort(0);
			packet.PutByte((byte)character.HairId);
			packet.PutByte((byte)character.WeaponId);
			packet.PutByte(0);
			packet.PutInt(DateTime.Now);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Removes character with the given handle from the player's client.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="handle"></param>
		/// <param name="type"></param>
		public static void ZC_NOTIFY_VANISH(PlayerCharacter player, int handle, DisappearType type)
		{
			var packet = new Packet(Op.ZC_NOTIFY_VANISH);
			packet.PutInt(handle);
			packet.PutByte((byte)type);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character move from one position to the other on clients
		/// of players around it.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVE(ICharacter character, Position from, Position to)
		{
			var packet = new Packet(Op.ZC_NOTIFY_MOVE);

			packet.PutInt(character.Handle);
			packet.AddPackedMove(from, to, 8, 8);
			packet.PutInt(DateTime.Now);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
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

			packet.PutInt(DateTime.Now);
			packet.AddPackedMove(from, to, 8, 8);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Stops character's movement. Doesn't work for players themselves.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="stopPos"></param>
		public static void ZC_STOPMOVE(ICharacter character, Position stopPos)
		{
			var packet = new Packet(Op.ZC_STOPMOVE);

			packet.PutInt(character.Handle);
			packet.PutShort((short)stopPos.X);
			packet.PutShort((short)stopPos.Y);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Sends the target's name to the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="target"></param>
		public static void ZC_ACK_REQNAME(PlayerCharacter character, ICharacter target)
		{
			var packet = new Packet(Op.ZC_ACK_REQNAME);

			packet.PutInt(target.Handle);
			packet.PutString(target.Username, 16);
			packet.PutString(target.Name, 16);

			character.Connection.Send(packet);
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

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
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
		public static void ZC_SPRITE_CHANGE(ICharacter character, SpriteType type, int value)
		{
			var packet = new Packet(Op.ZC_SPRITE_CHANGE);

			packet.PutInt(character.Handle);
			packet.PutByte((byte)type);
			packet.PutByte((byte)value);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
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

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
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

		/// <summary>
		/// Warps character to the given location.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mapStringId"></param>
		/// <param name="pos"></param>
		public static void ZC_NPCACK_MAPMOVE(PlayerCharacter character, string mapStringId, Position pos)
		{
			var mapFileName = mapStringId + ".gat";

			var packet = new Packet(Op.ZC_NPCACK_MAPMOVE);

			packet.PutString(mapFileName, 16);
			packet.PutShort((short)pos.X);
			packet.PutShort((short)pos.Y);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character do an action.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="handleSource"></param>
		/// <param name="handleTarget"></param>
		/// <param name="tick"></param>
		/// <param name="damage"></param>
		/// <param name="type"></param>
		public static void ZC_NOTIFY_ACT(PlayerCharacter character, int handleSource, int handleTarget, int tick, short damage, ActionType type)
		{
			var packet = new Packet(Op.ZC_NOTIFY_ACT);

			packet.PutInt(handleSource);
			packet.PutInt(handleTarget);
			packet.PutInt(tick);
			packet.PutShort(damage);
			packet.PutByte((byte)type);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Displays dialog on character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		/// <param name="message"></param>
		public static void ZC_SAY_DIALOG(PlayerCharacter character, int npcHandle, string message)
		{
			var packet = new Packet(Op.ZC_SAY_DIALOG);

			packet.PutInt(npcHandle);
			packet.PutString(message);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Shows a button to continue or end the dialog.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		public static void ZC_WAIT_DIALOG(PlayerCharacter character, int npcHandle)
		{
			var packet = new Packet(Op.ZC_WAIT_DIALOG);
			packet.PutInt(npcHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Closes the dialog window.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		public static void ZC_CLOSE_DIALOG(PlayerCharacter character, int npcHandle)
		{
			var packet = new Packet(Op.ZC_CLOSE_DIALOG);
			packet.PutInt(npcHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Closes the dialog window.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		/// <param name="optionsString">Options separated by colons.</param>
		public static void ZC_MENU_LIST(PlayerCharacter character, int npcHandle, string optionsString)
		{
			var packet = new Packet(Op.ZC_MENU_LIST);

			packet.PutInt(npcHandle);
			packet.PutString(optionsString);

			character.Connection.Send(packet);
		}
	}
}
