using System;
using System.Collections.Generic;
using System.Net;
using Sabine.Shared;
using Sabine.Shared.Configuration.Files;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.Network.Helpers;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Shops;
using Yggdrasil.Util;

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
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY(PlayerCharacter player, IEntryCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY);
			packet.AddStandEntry(character);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on the clients around it.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY(IEntryCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY);
			packet.AddStandEntry(character);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes NPC appear on clients of players  around it.
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
		public static void ZC_NOTIFY_STANDENTRY_NPC(IEntryCharacter character)
		{
			// In the alpha, class id 32 is a warp, in the form of a round
			// shadow sprite, and 63 seems to be an effect, that uses the
			// speed field as the effect id.

			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY_NPC);
			packet.AddStandEntryNpc(character);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes NPC appear on the player's client.
		/// </summary>
		/// <param name="player"></param>
		public static void ZC_NOTIFY_STANDENTRY_NPC(PlayerCharacter player, IEntryCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_STANDENTRY_NPC);
			packet.AddStandEntryNpc(character);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on clients of players around it with
		/// a spawn effect.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_NEWENTRY(IEntryCharacter character)
		{
			var packet = new Packet(Op.ZC_NOTIFY_NEWENTRY);
			packet.AddNewEntry(character);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes character who is currently moving appear on clients of
		/// players around it, moving between the given positions.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVEENTRY(IEntryCharacter character, Position from, Position to)
		{
			// Same as the others, but with a packed move instead of the
			// character's packed position.

			var packet = new Packet(Op.ZC_NOTIFY_MOVEENTRY);
			packet.AddMoveEntry(character, from, to);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes character who is currently moving appear on player's
		/// client, moving between the given positions.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVEENTRY(PlayerCharacter player, IEntryCharacter character, Position from, Position to)
		{
			var packet = new Packet(Op.ZC_NOTIFY_MOVEENTRY);
			packet.AddMoveEntry(character, from, to);

			player.Connection.Send(packet);
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
		/// Removes character from clients around it.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		public static void ZC_NOTIFY_VANISH(Character character, DisappearType type)
		{
			var packet = new Packet(Op.ZC_NOTIFY_VANISH);
			packet.PutInt(character.Handle);
			packet.PutByte((byte)type);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Makes character move from one position to the other on clients
		/// of players around it.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		public static void ZC_NOTIFY_MOVE(Character character, Position from, Position to)
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
		/// Stops character's movement on the clients of players nearby.
		/// </summary>
		/// <remarks>
		/// The (alpha) client doesn't react to this packet for its
		/// controlled character, only others.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="stopPos"></param>
		public static void ZC_STOPMOVE(Character character, Position stopPos)
		{
			var packet = new Packet(Op.ZC_STOPMOVE);

			packet.PutInt(character.Handle);
			packet.PutShort((short)stopPos.X);
			packet.PutShort((short)stopPos.Y);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Sends the target's name to the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="target"></param>
		public static void ZC_ACK_REQNAME(PlayerCharacter character, Character target)
		{
			var packet = new Packet(Op.ZC_ACK_REQNAME);

			packet.PutInt(target.Handle);

			if (Game.Version < Versions.Beta2)
			{
				// The first string is displayed in parantheses after the
				// character name. It seems like it's intended for the
				// account name, because that's what the client displays
				// for the player character itself. This might indicate
				// that they had planned a "team name" kind of feature,
				// similar to ToS, to have a common identifier between
				// characters on one account. Not a terrible idea, but
				// sending the account names of other players is not
				// exactly ideal, so... maybe let's not do that.
				// However, maybe we could add a display name for the
				// accounts, which could be used here.
				// Also: 16-24 free bytes for monster HP!

				var secName = "";

				if (target is Monster)
				{
					var hpDisplayType = ZoneServer.Instance.Conf.World.DisplayMonsterHp;

					switch (hpDisplayType)
					{
						case DisplayMonsterHpType.Percentage:
							secName = string.Format("{0:0}%", 100f / target.Parameters.HpMax * target.Parameters.Hp);
							break;

						case DisplayMonsterHpType.Actual:
							secName = string.Format("{0}/{1}", target.Parameters.Hp, target.Parameters.HpMax);
							break;
					}
				}

				packet.PutString(secName, Sizes.CharacterNames); // target.Username
			}

			packet.PutString(target.Name, Sizes.CharacterNames);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the given parameter on the client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		public static void ZC_PAR_CHANGE(PlayerCharacter character, ParameterType type)
		{
			var value = character.Parameters.Get(type);
			ZC_PAR_CHANGE(character, type, value);
		}

		/// <summary>
		/// Updates the given parameter on the client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		public static void ZC_LONGPAR_CHANGE(PlayerCharacter character, ParameterType type)
		{
			var value = character.Parameters.Get(type);

			// Always display job level and EXP as 0 if the feature
			// isn't enabled
			if (type == ParameterType.JobExp || type == ParameterType.JobExpNeeded)
			{
				if (!SabineData.Features.IsEnabled("JobLevels"))
					value = 0;
			}

			ZC_LONGPAR_CHANGE(character, type, value);
		}

		/// <summary>
		/// Updates the given parameter on the client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		/// <param name="value"></param>
		public static void ZC_PAR_CHANGE(PlayerCharacter character, ParameterType type, int value)
		{
			if (type.IsLong())
				throw new ArgumentException($"Parameter type '{type}' should be sent using ZC_LONGPAR_CHANGE.");

			if (type == ParameterType.Weight || type == ParameterType.WeightMax)
				value /= 10;

			var packet = new Packet(Op.ZC_PAR_CHANGE);

			packet.PutShort((short)type);

			// Apparently Gravity thought it was a good idea to upgrade
			// ZC_PAR_CHANGE to integers, but keep two separate packets
			// for some reason. The two packets still handle specific
			// parameters, you can't use one for both.
			if (Game.Version < Versions.Beta2)
				packet.PutShort((short)value);
			else
				packet.PutInt(value);

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
			if (!type.IsLong())
				throw new ArgumentException($"Parameter type '{type}' should be sent using ZC_PAR_CHANGE.");

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

			packet.PutShort((short)character.Parameters.StatPoints);
			packet.PutByte((byte)character.Parameters.Str);
			packet.PutByte((byte)character.Parameters.StrNeeded);
			packet.PutByte((byte)character.Parameters.Agi);
			packet.PutByte((byte)character.Parameters.AgiNeeded);
			packet.PutByte((byte)character.Parameters.Vit);
			packet.PutByte((byte)character.Parameters.VitNeeded);
			packet.PutByte((byte)character.Parameters.Int);
			packet.PutByte((byte)character.Parameters.IntNeeded);
			packet.PutByte((byte)character.Parameters.Dex);
			packet.PutByte((byte)character.Parameters.DexNeeded);
			packet.PutByte((byte)character.Parameters.Luk);
			packet.PutByte((byte)character.Parameters.LukNeeded);
			packet.PutByte((byte)character.Parameters.AttackMin);
			packet.PutByte((byte)character.Parameters.AttackMax);
			packet.PutByte((byte)character.Parameters.Defense);
			packet.PutByte((byte)character.Parameters.MagicAttack);

			if (Game.Version >= Versions.Beta1)
			{
				packet.PutEmpty(24);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends public chat packet to players around character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="message"></param>
		public static void ZC_NOTIFY_CHAT(Character character, string message)
		{
			var packet = new Packet(Op.ZC_NOTIFY_CHAT);

			packet.PutInt(character.Handle);
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
		/// Sends a whisper chat message to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="fromName"></param>
		/// <param name="message"></param>
		public static void ZC_WHISPER(PlayerCharacter character, string fromName, string message)
		{
			var packet = new Packet(Op.ZC_WHISPER);

			packet.PutString(fromName, 16);
			packet.PutString(message);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends a whisper chat message to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public static void ZC_ACK_WHISPER(PlayerCharacter character, WhisperResult result)
		{
			var packet = new Packet(Op.ZC_ACK_WHISPER);
			packet.PutByte((byte)result);

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
		public static void ZC_SPRITE_CHANGE(Character character, SpriteType type, int value)
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

			packet.PutInt(character.Handle);
			packet.PutByte((byte)emotion);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Sends item description to the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="name"></param>
		/// <param name="description"></param>
		public static void ZC_REQ_ITEM_EXPLANATION_ACK(PlayerCharacter character, string name, string description)
		{
			var packet = new Packet(Op.ZC_REQ_ITEM_EXPLANATION_ACK);

			packet.PutString(name, Sizes.ItemNames);
			packet.PutString(description);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the character's direction.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="direction"></param>
		/// <exception cref="NotImplementedException"></exception>
		public static void ZC_CHANGE_DIRECTION(Character character, Direction direction)
		{
			var packet = new Packet(Op.ZC_CHANGE_DIRECTION);

			packet.PutInt(character.Handle);
			packet.PutByte((byte)direction);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
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
		/// Warps character to the given location and makes the client
		/// connect to another zone server to continue.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="mapStringId"></param>
		/// <param name="pos"></param>
		/// <param name="ip"></param>
		/// <param name="port"></param>
		public static void ZC_NPCACK_MAPMOVE(PlayerCharacter character, string mapStringId, Position pos, string ip, int port)
		{
			var mapFileName = mapStringId + ".gat";

			var packet = new Packet(Op.ZC_NPCACK_SERVERMOVE);

			packet.PutString(mapFileName, 16);
			packet.PutShort((short)pos.X);
			packet.PutShort((short)pos.Y);
			packet.PutInt(IPAddress.Parse(ip));
			packet.PutShort((short)port);

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
		public static void ZC_NOTIFY_ACT(PlayerCharacter character, int handleSource, int handleTarget, int tick, int damage, ActionType type)
		{
			// Cap the damage, as the alpha client crashes if the damage
			// is greater than 999. 0 is displayed as "Miss", while
			// negative numbers become 0 damage.
			damage = Math2.Clamp(-1, 999, damage);

			var packet = new Packet(Op.ZC_NOTIFY_ACT);

			packet.PutInt(handleSource);
			packet.PutInt(handleTarget);
			packet.PutInt(tick);
			packet.PutShort((short)damage);
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

		/// <summary>
		/// Makes client close the connection and displays a message
		/// for why this disconnect was requested.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		/// <param name="optionsString"></param>
		public static void SC_NOTIFY_BAN(ZoneConnection conn, DisconnectReason reason)
		{
			var packet = new Packet(Op.SC_NOTIFY_BAN);
			packet.PutByte((byte)reason);

			conn.Send(packet);
		}

		/// <summary>
		/// Either makes given item appear in character's inventory or
		/// displays a message for why they couldn't pick up an item.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <param name="result"></param>
		public static void ZC_ITEM_PICKUP_ACK(PlayerCharacter character, Item item, PickUpResult result)
			=> ZC_ITEM_PICKUP_ACK(character, item, item.Amount, result);

		/// <summary>
		/// Adds item to inventory with the given amount. If the item
		/// already exists, the amount is added to the existing stack.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		/// <param name="result"></param>
		public static void ZC_ITEM_PICKUP_ACK(PlayerCharacter character, Item item, int amount, PickUpResult result)
		{
			var packet = new Packet(Op.ZC_ITEM_PICKUP_ACK);

			var wearSlots = item.WearSlots;
			if (!character.CanEquip(item))
				wearSlots = EquipSlots.None;

			packet.PutShort((short)item.InventoryId);
			packet.PutShort((short)amount);
			packet.PutString(item.StringId, Sizes.ItemNames);
			packet.PutByte((byte)item.Type);
			packet.PutByte((byte)wearSlots);
			packet.PutByte((byte)result);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the character's item and etc inventory tabs using
		/// the given list of items, filtering it for normal items.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_NORMAL_ITEMLIST(PlayerCharacter character, IEnumerable<Item> items)
		{
			var packet = new Packet(Op.ZC_NORMAL_ITEMLIST);

			foreach (var item in items)
			{
				if (item.Type.IsEquip())
					continue;

				if (Game.Version < Versions.Beta2)
				{
					var size = 6 + Sizes.ItemNames;
					if (Game.Version >= Versions.Beta1)
						size += 4;

					// The first byte contains the size of the item
					// struct plus the size byte, which the client
					// memcpys for handling. It's currently unclear
					// why this size byte is necessary, but it's
					// working this way.
					packet.PutByte((byte)size);

					packet.PutByte((byte)item.Type);
					packet.PutShort((short)item.InventoryId);
					packet.PutShort((short)item.Amount);

					if (Game.Version >= Versions.Beta1)
					{
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
					}

					packet.PutString(item.StringId, Sizes.ItemNames);
				}
				else
				{
					packet.PutShort((short)item.InventoryId);
					packet.PutShort((short)item.ClassId);
					packet.PutByte((byte)item.Type);
					packet.PutByte(item.IsIdentified);
					packet.PutShort((short)item.Amount);
					packet.PutShort(0);
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Opens storage window if it's not open yet and fills it with
		/// the given items, filtering for non-equip.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_STORE_NORMAL_ITEMLIST(PlayerCharacter character, IEnumerable<Item> items)
		{
			var packet = new Packet(Op.ZC_STORE_NORMAL_ITEMLIST);

			foreach (var item in items)
			{
				if (item.Type.IsEquip())
					continue;

				var size = 6 + Sizes.ItemNames;
				if (Game.Version >= Versions.Beta1)
					size += 4;

				packet.PutByte((byte)size);

				packet.PutByte((byte)item.Type);
				packet.PutShort((short)item.InventoryId);
				packet.PutShort((short)item.Amount);

				if (Game.Version >= Versions.Beta1)
				{
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
				}

				packet.PutString(item.StringId, Sizes.ItemNames);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends a list of purchasable items to the character's client,
		/// making it open a window where the player can do so.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_PC_PURCHASE_ITEMLIST(PlayerCharacter character, IEnumerable<ShopItem> items)
		{
			var packet = new Packet(Op.ZC_PC_PURCHASE_ITEMLIST);

			foreach (var item in items)
			{
				packet.PutInt(item.Price);
				packet.PutByte(0);
				packet.PutString(item.StringId, Sizes.ItemNames);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends a list of items the character can sell to the client,
		/// making it open a window where the player can do so.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_PC_SELL_ITEMLIST(PlayerCharacter character, IEnumerable<Item> items)
		{
			var packet = new Packet(Op.ZC_PC_SELL_ITEMLIST);

			foreach (var item in items)
			{
				// The client halves the price sent here, so we have
				// to multiply our sell price to get it to display the
				// correct amount. We could also send the buy price,
				// but that would potentially cause unexpected behavior
				// if someone were to set a selling price for an item
				// that's not half of the buy price.

				var sellPrice = item.Data.SellPrice * 2;

				packet.PutShort((short)item.InventoryId);
				packet.PutInt(sellPrice);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the character's equip inventory tab using the given
		/// list of items, filtering it for equip items.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_EQUIPMENT_ITEMLIST(PlayerCharacter character, IEnumerable<Item> items)
		{
			var packet = new Packet(Op.ZC_EQUIPMENT_ITEMLIST);

			foreach (var item in items)
			{
				if (!item.Type.IsEquip())
					continue;

				var wearSlots = item.WearSlots;
				if (!character.CanEquip(item))
					wearSlots = EquipSlots.None;

				if (Game.Version < Versions.Beta2)
				{
					var size = 6 + Sizes.ItemNames;
					if (Game.Version >= Versions.Beta1)
						size += 16;

					// The first byte contains the size of the item
					// struct plus the size byte, which the client
					// memcpys for handling. It's currently unclear
					// why this size byte is necessary, but it's
					// working this way.
					packet.PutByte((byte)size);

					packet.PutByte((byte)item.Type);
					packet.PutByte((byte)wearSlots);
					packet.PutShort((short)item.InventoryId);
					packet.PutByte((byte)item.EquippedOn);

					if (Game.Version >= Versions.Beta1)
					{
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
						packet.PutByte(0);
					}

					packet.PutString(item.StringId, Sizes.ItemNames);
				}
				else
				{
					packet.PutShort((short)item.InventoryId);
					packet.PutShort((short)item.ClassId);
					packet.PutByte((byte)item.Type);
					packet.PutByte(item.IsIdentified);
					packet.PutShort((short)wearSlots);
					packet.PutShort((short)item.EquippedOn);
					packet.PutByte(0);   // Attribute
					packet.PutByte(0);   // Refine
					packet.PutShort(0);  // Card1
					packet.PutShort(0);  // Card2
					packet.PutShort(0);  // Card3
					packet.PutShort(0);  // Card4
				}
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Opens storage window if it's not open yet and fills it with
		/// the given items, filtering for equip.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="items"></param>
		public static void ZC_STORE_EQUIPMENT_ITEMLIST(PlayerCharacter character, IEnumerable<Item> items)
		{
			var packet = new Packet(Op.ZC_STORE_EQUIPMENT_ITEMLIST);

			foreach (var item in items)
			{
				if (!item.Type.IsEquip())
					continue;

				var wearSlots = item.WearSlots;
				if (!character.CanEquip(item))
					wearSlots = EquipSlots.None;

				var size = 6 + Sizes.ItemNames;
				if (Game.Version >= Versions.Beta1)
					size += 16;

				packet.PutByte((byte)size);

				packet.PutByte((byte)item.Type);
				packet.PutByte((byte)wearSlots);
				packet.PutShort((short)item.InventoryId);
				packet.PutByte((byte)item.EquippedOn);

				if (Game.Version >= Versions.Beta1)
				{
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
				}

				packet.PutString(item.StringId, Sizes.ItemNames);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes item appear for player, lying on the ground.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="item"></param>
		public static void ZC_ITEM_ENTRY(PlayerCharacter character, Item item)
		{
			var packet = new Packet(Op.ZC_ITEM_ENTRY);

			packet.PutInt(item.Handle);
			packet.PutShort((short)item.Position.X);
			packet.PutShort((short)item.Position.Y);
			packet.PutShort((short)item.Amount);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutString(item.StringId, Sizes.ItemNames);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes item appear for players around it, dropping to the ground.
		/// </summary>
		/// <param name="item"></param>
		public static void ZC_ITEM_FALL_ENTRY(Item item)
		{
			var packet = new Packet(Op.ZC_ITEM_FALL_ENTRY);

			packet.PutInt(item.Handle);
			packet.PutShort((short)item.Position.X);
			packet.PutShort((short)item.Position.Y);
			packet.PutByte(0);
			packet.PutByte(0);
			packet.PutShort((short)item.Amount);
			packet.PutString(item.StringId, Sizes.ItemNames);

			item.Map.Broadcast(packet, item, BroadcastTargets.All);
		}

		/// <summary>
		/// Makes item with given handle disappear for the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="handle"></param>
		public static void ZC_ITEM_DISAPPEAR(PlayerCharacter character, int handle)
		{
			var packet = new Packet(Op.ZC_ITEM_DISAPPEAR);
			packet.PutInt(handle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes item disappear on clients around it.
		/// </summary>
		/// <param name="item"></param>
		public static void ZC_ITEM_DISAPPEAR(Item item)
		{
			var packet = new Packet(Op.ZC_ITEM_DISAPPEAR);
			packet.PutInt(item.Handle);

			item.Map.Broadcast(packet, item, BroadcastTargets.All);
		}

		/// <summary>
		/// Response to item drop request, which updates the item stack
		/// that was changed.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="invId">Inventory id of the item that changed or was dropped.</param>
		/// <param name="removeAmount">Amount to remove from the item stack. If it reaches 0, the item disappears.</param>
		public static void ZC_ITEM_THROW_ACK(PlayerCharacter character, int invId, int removeAmount)
		{
			var packet = new Packet(Op.ZC_ITEM_THROW_ACK);

			packet.PutShort((short)invId);
			packet.PutShort((short)removeAmount);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Response to item use request, updates the item's amount.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="invId"></param>
		/// <param name="newAmount"></param>
		public static void ZC_USE_ITEM_ACK(PlayerCharacter character, int invId, int newAmount)
		{
			var packet = new Packet(Op.ZC_USE_ITEM_ACK);

			packet.PutShort((short)invId);
			packet.PutShort((short)newAmount);
			packet.PutByte(true);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Response to equip request, makes character equip the item
		/// in the given slot.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="invId"></param>
		/// <param name="equipSlot"></param>
		public static void ZC_REQ_WEAR_EQUIP_ACK(PlayerCharacter character, int invId, EquipSlots equipSlot)
		{
			var packet = new Packet(Op.ZC_REQ_WEAR_EQUIP_ACK);

			packet.PutShort((short)invId);
			packet.PutByte((byte)equipSlot);
			packet.PutByte(true);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Response to unequip request, moves the item out of the given
		/// slot and into the inventory.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="invId"></param>
		/// <param name="equipSlot"></param>
		public static void ZC_REQ_TAKEOFF_EQUIP_ACK(PlayerCharacter character, int invId, EquipSlots equipSlot)
		{
			var packet = new Packet(Op.ZC_REQ_TAKEOFF_EQUIP_ACK);

			packet.PutShort((short)invId);
			packet.PutByte((byte)equipSlot);
			packet.PutByte(true);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Response to storage closing notification. Closes the storage
		/// window on the character's client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CLOSE_STORE(PlayerCharacter character)
		{
			var packet = new Packet(Op.ZC_CLOSE_STORE);
			character.Connection.Send(packet);
		}

		/// <summary>
		/// Shows menu to select whether to buy or sell items.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		public static void ZC_SELECT_DEALTYPE(PlayerCharacter character, int npcHandle)
		{
			var packet = new Packet(Op.ZC_SELECT_DEALTYPE);
			packet.PutInt(npcHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Displays MVP animation above the character's head.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_MVP(Character character)
		{
			var packet = new Packet(Op.ZC_MVP);
			packet.PutInt(character.Handle);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Displays message to the player, telling them that they were
		/// the MVP and that they'll receive an EXP bonus.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_MVP_GETTING_SPECIAL_EXP(PlayerCharacter character, int expAmount)
		{
			var packet = new Packet(Op.ZC_MVP_GETTING_SPECIAL_EXP);
			packet.PutInt(expAmount);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends response to restart request to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="b1"></param>
		public static void ZC_RESTART_ACK(PlayerCharacter character, int b1)
		{
			var packet = new Packet(Op.ZC_RESTART_ACK);
			packet.PutByte((byte)b1);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends response to disconnect request to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="s1"></param>
		public static void ZC_ACK_REQ_DISCONNECT(PlayerCharacter character, int s1)
		{
			var packet = new Packet(Op.ZC_ACK_REQ_DISCONNECT);
			packet.PutShort((short)s1);

			character.Connection.Send(packet);
		}
	}
}
