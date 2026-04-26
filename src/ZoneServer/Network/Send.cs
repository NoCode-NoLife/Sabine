using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Net;
using Sabine.Shared;
using Sabine.Shared.Configuration.Files;
using Sabine.Shared.Const;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.Network.Helpers;
using Sabine.Zone.Skills;
using Sabine.Zone.World.Actors;
using Sabine.Zone.World.Chats;
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
		/// Sends data necessary to initialize the connection on newer
		/// clients.
		/// </summary>
		/// <param name="conn"></param>
		public static void InitConnection(ZoneConnection conn)
		{
			var buffer = ArrayPool<byte>.Shared.Rent(sizeof(int));
			BinaryPrimitives.WriteInt32LittleEndian(buffer, conn.Account.Id);

			conn.Send(buffer, sizeof(int), static (data, len, type) => ArrayPool<byte>.Shared.Return(data));
		}

		/// <summary>
		/// Accepts connection request, makes client load map.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="character"></param>
		public static void ZC_ACCEPT_ENTER(ZoneConnection conn, PlayerCharacter character)
		{
			using var packet = Packet.Rent(Op.ZC_ACCEPT_ENTER);

			packet.PutInt(character.Id);
			packet.AddPackedPosition(character.Position, character.Direction);
			packet.PutShort(0);

			conn.Send(packet);
		}

		/// <summary>
		/// Sends server time to client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="tick"></param>
		public static void ZC_NOTIFY_TIME(ZoneConnection conn, int tick)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_TIME);
			packet.PutInt(tick);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character appear on the player's client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY(PlayerCharacter player, IStandEntry character)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_STANDENTRY);
			packet.AddStandEntry(character);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on the clients around it.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_STANDENTRY(IStandEntry character)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_STANDENTRY);
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
		public static void ZC_NOTIFY_STANDENTRY_NPC(IStandEntry character)
		{
			// In the alpha, class id 32 is a warp, in the form of a round
			// shadow sprite, and 63 seems to be an effect, that uses the
			// speed field as the effect id.

			using var packet = Packet.Rent(Op.ZC_NOTIFY_STANDENTRY_NPC);
			packet.AddStandEntryNpc(character);

			character.Map.Broadcast(packet, character, BroadcastTargets.AllButSource);
		}

		/// <summary>
		/// Makes NPC appear on the player's client.
		/// </summary>
		/// <param name="player"></param>
		public static void ZC_NOTIFY_STANDENTRY_NPC(PlayerCharacter player, IStandEntry character)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_STANDENTRY_NPC);
			packet.AddStandEntryNpc(character);

			player.Connection.Send(packet);
		}

		/// <summary>
		/// Makes character appear on clients of players around it with
		/// a spawn effect.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_NOTIFY_NEWENTRY(IStandEntry character)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_NEWENTRY);
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
		public static void ZC_NOTIFY_MOVEENTRY(IStandEntry character, Position from, Position to)
		{
			// Same as the others, but with a packed move instead of the
			// character's packed position.

			using var packet = Packet.Rent(Op.ZC_NOTIFY_MOVEENTRY);
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
		public static void ZC_NOTIFY_MOVEENTRY(PlayerCharacter player, IStandEntry character, Position from, Position to)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_MOVEENTRY);
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
			using var packet = Packet.Rent(Op.ZC_NOTIFY_VANISH);
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
			using var packet = Packet.Rent(Op.ZC_NOTIFY_VANISH);
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
			using var packet = Packet.Rent(Op.ZC_NOTIFY_MOVE);

			packet.PutInt(character.Handle);
			packet.AddPackedMove(from, to, 8, 8);
			packet.PutInt(Game.GetTick());

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
			using var packet = Packet.Rent(Op.ZC_NOTIFY_PLAYERMOVE);

			packet.PutInt(Game.GetTick());
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
			using var packet = Packet.Rent(Op.ZC_STOPMOVE);

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
			using var packet = Packet.Rent(Op.ZC_ACK_REQNAME);

			packet.PutInt(target.Handle);

			// The first string is displayed in parantheses after the
			// character name. It seems like it's intended for the account
			// name, because that's what the client displays for the
			// player character itself. This might indicate that they had
			// planned a "team name" kind of feature, similar to ToS, to
			// have a common identifier between characters on one account.
			// Not a terrible idea, but sending the account names of other
			// players is not exactly ideal, so... maybe let's not do
			// that. If we leave it empty, the client will just not
			// display it. However, maybe we could add a display name for
			// the accounts, which could be used here. Also: 16-24 free
			// bytes for monster HP!

			var secondaryName = ""; // target.Username
			var mainName = target.Name;

			if (target is Monster)
			{
				var hpDisplayType = ZoneServer.Instance.Conf.World.DisplayMonsterHp;

				switch (hpDisplayType)
				{
					case DisplayMonsterHpType.Percentage:
					{
						// Round to ceiling in case percentage falls below
						// 1% and clamp it to 0~100, in case the calculation
						// result ends up above 100. (Floats ftw, am I right?)
						secondaryName = string.Format("{0}%", Math2.Clamp(0, 100, Math.Ceiling(100f / target.Parameters.HpMax * target.Parameters.Hp)));
						break;
					}
					case DisplayMonsterHpType.Actual:
					{
						secondaryName = string.Format("{0}/{1}", target.Parameters.Hp, target.Parameters.HpMax);
						break;
					}
				}
			}

			// Append secName to targetName if the client doesn't support
			// a secondary name anymore.
			if (Game.Version > Versions.Alpha && !string.IsNullOrWhiteSpace(secondaryName))
				mainName = string.Format("{0} ({1})", mainName, secondaryName);

			// This is still sent in Beta1, but the client doesn't display
			// it anymore.
			if (Game.Version < Versions.Beta2)
				packet.PutString(secondaryName, Sizes.CharacterNames);

			packet.PutString(mainName, Sizes.CharacterNames);

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
				if (!ZoneServer.Instance.Data.Features.IsEnabled("JobLevels"))
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

			using var packet = Packet.Rent(Op.ZC_PAR_CHANGE);

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

			using var packet = Packet.Rent(Op.ZC_LONGPAR_CHANGE);

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
			using var packet = Packet.Rent(Op.ZC_STATUS_CHANGE_ACK);

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
			using var packet = Packet.Rent(Op.ZC_STATUS);

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

			if (Game.Version < Versions.Beta1)
			{
				packet.PutByte((byte)character.Parameters.AttackMin);
				packet.PutByte((byte)character.Parameters.AttackMax);
				packet.PutByte((byte)character.Parameters.Defense);
				packet.PutByte((byte)character.Parameters.MagicAttack);
			}
			else
			{
				packet.PutShort((short)character.Parameters.Attack);
				packet.PutShort((short)character.Parameters.AttackBonus);
				packet.PutShort((short)character.Parameters.MagicAttackMax);
				packet.PutShort((short)character.Parameters.MagicAttackMin);
				packet.PutShort((short)character.Parameters.MeleeDefense);
				packet.PutShort((short)character.Parameters.MeleeDefenseBonus);
				packet.PutShort((short)character.Parameters.MagicDefense);
				packet.PutShort((short)character.Parameters.MagicDefenseBonus);
				packet.PutShort((short)character.Parameters.Hit);
				packet.PutShort((short)character.Parameters.Flee);
				packet.PutShort((short)character.Parameters.FleeBonus);
				packet.PutShort((short)character.Parameters.Critical);
				packet.PutShort((short)character.Parameters.Aspd);
				packet.PutShort((short)character.Parameters.AspdBonus);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends public chat packet to players around character.
		/// </summary>
		/// <param name="character">The character who is the source of the chat message.</param>
		/// <param name="message">The chat message to send.</param>
		public static void ZC_NOTIFY_CHAT(Character character, string message)
			=> ZC_NOTIFY_CHAT(new SightBroadcastSender(character, BroadcastTargets.AllButSource), character.Handle, message);

		/// <summary>
		/// Sends chat packet to character's client.
		/// </summary>
		/// <param name="character">The character to send the packet to.</param>
		/// <param name="authorHandle">The handle of the author of the chat message.</param>
		/// <param name="message">The chat message to send.</param>
		public static void ZC_NOTIFY_CHAT(PlayerCharacter character, int authorHandle, string message)
			=> ZC_NOTIFY_CHAT(new SingleConnectionSender(character), authorHandle, message);

		/// <summary>
		/// Sends public chat packet for the author's message via sender.
		/// </summary>
		/// <param name="sender">The sender to use for sending the packet.</param>
		/// <param name="authorHandle">The source of the chat message.</param>
		/// <param name="message">The chat message to send.</param>
		public static void ZC_NOTIFY_CHAT<TSender>(TSender sender, int authorHandle, string message) where TSender : ISender
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_CHAT);

			packet.PutInt(authorHandle);
			packet.PutString(message);

			sender.Send(packet);
		}

		/// <summary>
		/// Displays message to the character's client as their own
		/// message, displaying it above their head and/or in a special
		/// color inside a chat.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="message"></param>
		public static void ZC_NOTIFY_PLAYERCHAT(PlayerCharacter character, string message)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_PLAYERCHAT);
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
			using var packet = Packet.Rent(Op.ZC_WHISPER);

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
			using var packet = Packet.Rent(Op.ZC_ACK_WHISPER);
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
			using var packet = Packet.Rent(Op.ZC_BROADCAST);
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
			using var packet = Packet.Rent(Op.ZC_SPRITE_CHANGE);

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
			using var packet = Packet.Rent(Op.ZC_USER_COUNT);
			packet.PutInt(count);

			conn.Send(packet);
		}

		/// <summary>
		/// Makes character use the given emotion.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="emotionId"></param>
		public static void ZC_EMOTION(Character character, EmotionId emotionId)
		{
			using var packet = Packet.Rent(Op.ZC_EMOTION);

			packet.PutInt(character.Handle);
			packet.PutByte((byte)emotionId);

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
			using var packet = Packet.Rent(Op.ZC_REQ_ITEM_EXPLANATION_ACK);

			packet.PutString(name, Sizes.ItemNames);
			packet.PutString(description);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the character's direction on other players' clients.
		/// </summary>
		/// <remarks>
		/// Doesn't affect the character's direction on their own client.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="direction"></param>
		/// <param name="headTurn"></param>
		public static void ZC_CHANGE_DIRECTION(Character character, Direction direction, HeadTurn headTurn)
		{
			using var packet = Packet.Rent(Op.ZC_CHANGE_DIRECTION);

			packet.PutInt(character.Handle);

			if (Game.Version >= Versions.Beta2)
			{
				packet.PutByte((byte)headTurn);
				packet.PutByte(0);
			}

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

			using var packet = Packet.Rent(Op.ZC_NPCACK_MAPMOVE);

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
		public static void ZC_NPCACK_SERVERMOVE(PlayerCharacter character, string mapStringId, Position pos, string ip, int port)
		{
			var mapFileName = mapStringId + ".gat";

			using var packet = Packet.Rent(Op.ZC_NPCACK_SERVERMOVE);

			packet.PutString(mapFileName, 16);
			packet.PutShort((short)pos.X);
			packet.PutShort((short)pos.Y);
			packet.PutInt(IPAddress.Parse(ip));
			packet.PutShort((short)port);

			character.Connection.Send(packet);
		}

		public static class ZC_NOTIFY_ACT
		{
			/// <summary>
			/// Makes character do an action.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="handleSource"></param>
			/// <param name="handleTarget"></param>
			/// <param name="tick"></param>
			/// <param name="damage"></param>
			/// <param name="type"></param>
			/// <param name="delay1"></param>
			/// <param name="delay2"></param>
			public static void Attack(Character character, int handleSource, int handleTarget, int tick, ActionType type, int damage, int delay1, int delay2)
			{
				// Cap the damage, as the alpha client crashes if the damage
				// is greater than 999. 0 is displayed as "Miss", while
				// negative numbers become 0 damage.
				// Beta1 doesn't crash, regardless of the damage value, but
				// caps the displayed damage at 9999.
				if (Game.Version < Versions.Beta1)
					damage = Math2.Clamp(-1, 999, damage);

				Raw(character, handleSource, handleTarget, tick, type, damage, delay1, delay2, 0, 0);
			}

			/// <summary>
			/// Makes character do an action.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="handleSource"></param>
			/// <param name="type"></param>
			public static void Simple(Character character, int handleSource, ActionType type)
				=> Raw(character, handleSource, 0, 0, type, 0, 0, 0, 0, 0);

			/// <summary>
			/// Makes character do an action.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="handleSource"></param>
			/// <param name="handleTarget"></param>
			/// <param name="tick"></param>
			/// <param name="type"></param>
			/// <param name="arg1"></param>
			/// <param name="arg2"></param>
			/// <param name="arg3"></param>
			/// <param name="arg4"></param>
			public static void Raw(Character character, int handleSource, int handleTarget, int tick, ActionType type, int arg1, int arg2, int arg3, int arg4, int arg5)
			{
				using var packet = Packet.Rent(Op.ZC_NOTIFY_ACT);

				packet.PutInt(handleSource);
				packet.PutInt(handleTarget);
				packet.PutInt(tick);

				if (Game.Version >= Versions.Beta1)
				{
					packet.PutInt(arg2);
					packet.PutInt(arg3);
				}

				packet.PutShort((short)arg1);

				if (Game.Version >= Versions.Beta1)
					packet.PutShort((short)arg4);

				packet.PutByte((byte)type);

				if (Game.Version >= Versions.Beta2)
					packet.PutShort((short)arg5);

				character.Map.Broadcast(packet, character, BroadcastTargets.All);
			}
		}

		/// <summary>
		/// Informs the character that their attack failed due to the
		/// target being out of range, making them get closer.
		/// </summary>
		/// <param name="character">The character whose attack failed.</param>
		/// <param name="target">The target character that is out of range.</param>
		/// <param name="range">The range into which the character should get to attack.</param>
		public static void ZC_ATTACK_FAILURE_FOR_DISTANCE(PlayerCharacter character, Character target, int range)
		{
			using var packet = Packet.Rent(Op.ZC_ATTACK_FAILURE_FOR_DISTANCE);

			packet.PutInt(target.Handle);
			packet.PutShort((short)target.Position.X);
			packet.PutShort((short)target.Position.Y);
			packet.PutShort((short)character.Position.X);
			packet.PutShort((short)character.Position.Y);
			packet.PutShort((short)range);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Displays dialog on character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		/// <param name="message"></param>
		public static void ZC_SAY_DIALOG(PlayerCharacter character, int npcHandle, string message)
		{
			using var packet = Packet.Rent(Op.ZC_SAY_DIALOG);

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
			using var packet = Packet.Rent(Op.ZC_WAIT_DIALOG);
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
			using var packet = Packet.Rent(Op.ZC_CLOSE_DIALOG);
			packet.PutInt(npcHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Displays a list of options to choose from during a dialog.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		/// <param name="optionsString">Options separated by colons.</param>
		public static void ZC_MENU_LIST(PlayerCharacter character, int npcHandle, string optionsString)
		{
			using var packet = Packet.Rent(Op.ZC_MENU_LIST);

			packet.PutInt(npcHandle);
			packet.PutString(optionsString);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes client close the connection and display a message
		/// for why this disconnect was requested.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="reason"></param>
		public static void SC_NOTIFY_BAN(ZoneConnection conn, DisconnectReason reason)
		{
			using var packet = Packet.Rent(Op.SC_NOTIFY_BAN);
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
			using var packet = Packet.Rent(Op.ZC_ITEM_PICKUP_ACK);

			packet.PutShort((short)item.InventoryId);
			packet.PutShort((short)amount);

			if (Game.Version < Versions.Beta2)
			{
				packet.PutString(item.StringId, Sizes.ItemNames);
				packet.PutByte((byte)item.Type);
				packet.PutByte((byte)item.GetSlotsFor(character));
			}
			else
			{
				packet.PutShort((short)item.ClassId);
				packet.PutByte(item.IsIdentified);
				packet.PutByte(0);   // Attribute
				packet.PutByte(0);   // Refine
				packet.PutShort(0);  // Card1
				packet.PutShort(0);  // Card2
				packet.PutShort(0);  // Card3
				packet.PutShort(0);  // Card4
				packet.PutShort((short)item.GetSlotsFor(character));
				packet.PutByte((byte)item.Type);
			}

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
			using var packet = Packet.Rent(Op.ZC_NORMAL_ITEMLIST);

			foreach (var item in items)
			{
				if (item.Type.IsEquip())
					continue;

				packet.AddNormalItem(item);
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
			using var packet = Packet.Rent(Op.ZC_STORE_NORMAL_ITEMLIST);

			foreach (var item in items)
			{
				if (item.Type.IsEquip())
					continue;

				packet.AddNormalItem(item);
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
			using var packet = Packet.Rent(Op.ZC_PC_PURCHASE_ITEMLIST);

			foreach (var item in items)
			{
				packet.PutInt(item.Price);

				if (Game.Version >= Versions.Beta1)
					packet.PutInt(item.Price); // Skill-bonus adjusted price

				packet.PutByte(0);

				if (Game.Version < Versions.Beta2)
					packet.PutString(item.StringId, Sizes.ItemNames);
				else
					packet.PutShort((short)item.ClassId);
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
			using var packet = Packet.Rent(Op.ZC_PC_SELL_ITEMLIST);

			foreach (var item in items)
			{
				var sellPrice = item.Data.SellPrice;

				// The alpha client halves the price sent here, so we have
				// to multiply our sell price to get it to display the
				// correct amount. We could also send the buy price,
				// but that would potentially cause unexpected behavior
				// if someone were to set a selling price for an item
				// that's not half of the buy price.
				if (Game.Version < Versions.Beta1)
					sellPrice *= 2;

				packet.PutShort((short)item.InventoryId);
				packet.PutInt(sellPrice);

				if (Game.Version >= Versions.Beta1)
					packet.PutInt(sellPrice); // Skill-bonus adjusted price
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
			using var packet = Packet.Rent(Op.ZC_EQUIPMENT_ITEMLIST);

			foreach (var item in items)
			{
				if (!item.Type.IsEquip())
					continue;

				packet.AddEquipItem(item, item.GetSlotsFor(character));
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
			using var packet = Packet.Rent(Op.ZC_STORE_EQUIPMENT_ITEMLIST);

			foreach (var item in items)
			{
				if (!item.Type.IsEquip())
					continue;

				packet.AddEquipItem(item, item.GetSlotsFor(character));
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
			using var packet = Packet.Rent(Op.ZC_ITEM_ENTRY);
			packet.AddEntryItem(item);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes item appear for players around it, dropping to the ground.
		/// </summary>
		/// <param name="item"></param>
		public static void ZC_ITEM_FALL_ENTRY(Item item)
		{
			using var packet = Packet.Rent(Op.ZC_ITEM_FALL_ENTRY);
			packet.AddEntryItem(item);

			item.Map.Broadcast(packet, item, BroadcastTargets.All);
		}

		/// <summary>
		/// Makes item with given handle disappear for the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="handle"></param>
		public static void ZC_ITEM_DISAPPEAR(PlayerCharacter character, int handle)
		{
			using var packet = Packet.Rent(Op.ZC_ITEM_DISAPPEAR);
			packet.PutInt(handle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Makes item disappear on clients around it.
		/// </summary>
		/// <param name="item"></param>
		public static void ZC_ITEM_DISAPPEAR(Item item)
		{
			using var packet = Packet.Rent(Op.ZC_ITEM_DISAPPEAR);
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
			using var packet = Packet.Rent(Op.ZC_ITEM_THROW_ACK);

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
			using var packet = Packet.Rent(Op.ZC_USE_ITEM_ACK);

			packet.PutShort((short)invId);
			packet.PutShort((short)newAmount);
			packet.PutByte(true);

			character.Connection.Send(packet);
		}

		public static class ZC_REQ_WEAR_EQUIP_ACK
		{
			/// <summary>
			/// Successful response to equip request, makes character
			/// equip the item in the given slot.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="invId"></param>
			/// <param name="equipSlot"></param>
			public static void Success(PlayerCharacter character, int invId, EquipSlots equipSlot)
				=> Raw(character, invId, equipSlot, true);

			/// <summary>
			/// Negative response to equip request, displaying error
			/// message.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="invId"></param>
			public static void Fail(PlayerCharacter character, int invId)
				=> Raw(character, invId, EquipSlots.None, false);

			/// <summary>
			/// Response to equip request.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="invId"></param>
			/// <param name="equipSlot"></param>
			/// <param name="success"></param>
			public static void Raw(PlayerCharacter character, int invId, EquipSlots equipSlot, bool success)
			{
				using var packet = Packet.Rent(Op.ZC_REQ_WEAR_EQUIP_ACK);

				packet.PutShort((short)invId);

				if (Game.Version < Versions.Beta2)
					packet.PutByte((byte)equipSlot);
				else
					packet.PutShort((short)equipSlot);

				packet.PutByte(success);

				character.Connection.Send(packet);
			}
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
			using var packet = Packet.Rent(Op.ZC_REQ_TAKEOFF_EQUIP_ACK);

			packet.PutShort((short)invId);

			if (Game.Version < Versions.Beta2)
				packet.PutByte((byte)equipSlot);
			else
				packet.PutShort((short)equipSlot);

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
			using var packet = Packet.Rent(Op.ZC_CLOSE_STORE);
			character.Connection.Send(packet);
		}

		/// <summary>
		/// Shows menu to select whether to buy or sell items.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="npcHandle"></param>
		public static void ZC_SELECT_DEALTYPE(PlayerCharacter character, int npcHandle)
		{
			using var packet = Packet.Rent(Op.ZC_SELECT_DEALTYPE);
			packet.PutInt(npcHandle);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Displays MVP animation above the character's head.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_MVP(Character character)
		{
			using var packet = Packet.Rent(Op.ZC_MVP);
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
			using var packet = Packet.Rent(Op.ZC_MVP_GETTING_SPECIAL_EXP);
			packet.PutInt(expAmount);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends response to restart request to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="type"></param>
		public static void ZC_RESTART_ACK(PlayerCharacter character, RestartType type)
		{
			using var packet = Packet.Rent(Op.ZC_RESTART_ACK);
			packet.PutByte((byte)type);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends response to disconnect request to character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="s1"></param>
		public static void ZC_ACK_REQ_DISCONNECT(PlayerCharacter character, int s1)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_REQ_DISCONNECT);
			packet.PutShort((short)s1);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends a list of skills the character has to the client,
		/// refreshing the skill list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skills"></param>
		public static void ZC_SKILLINFO_LIST(PlayerCharacter character, IEnumerable<Skill> skills)
		{
			using var packet = Packet.Rent(Op.ZC_SKILLINFO_LIST);

			foreach (var skill in skills)
				packet.AddSkill(skill);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Adds the given skill to the character's skill list.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void ZC_ADD_SKILL(PlayerCharacter character, Skill skill)
		{
			using var packet = Packet.Rent(Op.ZC_ADD_SKILL);
			packet.AddSkill(skill);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Updates the skill on the character's client.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skill"></param>
		public static void ZC_SKILLINFO_UPDATE(PlayerCharacter character, Skill skill)
		{
			using var packet = Packet.Rent(Op.ZC_SKILLINFO_UPDATE);

			packet.PutShort((short)skill.Id);
			packet.PutShort((short)skill.Level);
			packet.PutShort((short)skill.SpCost);
			packet.PutByte(skill.CanBeLeveled);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Informs the client about success or failure of a skill usage
		/// request.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="skillId"></param>
		/// <param name="result"></param>
		/// <param name="failType"></param>
		/// <param name="failReason"></param>
		public static void ZC_ACK_TOUSESKILL(PlayerCharacter character, SkillId skillId, SkillUseResult result, SkillFailType failType, SkillFailReason failReason)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_TOUSESKILL);

			if (Game.Version <= Versions.Alpha)
			{
				// The alpha client checks for success and displays a
				// message on fail.

				packet.PutShort((short)skillId);
				packet.PutByte((byte)result);
				packet.PutByte((byte)failReason);
			}
			else
			{
				// Beta2+ first checks for success. On fail it checks the
				// fail type and either displays skill specific fail
				// reasons, or generic ones.

				packet.PutShort((short)skillId);
				packet.PutInt((int)failReason);
				packet.PutByte((byte)result);
				packet.PutByte((byte)failType);
			}

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends notification about a trade request to character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="traderName"></param>
		public static void ZC_REQ_EXCHANGE_ITEM(PlayerCharacter character, string traderName)
		{
			using var packet = Packet.Rent(Op.ZC_REQ_EXCHANGE_ITEM);
			packet.PutString(traderName, Sizes.CharacterNames);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends response about a trade request to character, opening
		/// trade window on accept.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="response"></param>
		public static void ZC_ACK_EXCHANGE_ITEM(PlayerCharacter character, TradingResponse response)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_EXCHANGE_ITEM);
			packet.PutByte((byte)response);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Cancels active trade on character's client.
		/// </summary>
		/// <param name="character"></param>
		public static void ZC_CANCEL_EXCHANGE_ITEM(PlayerCharacter character)
		{
			using var packet = Packet.Rent(Op.ZC_CANCEL_EXCHANGE_ITEM);

			character.Connection.Send(packet);
		}

		public static class ZC_ADD_EXCHANGE_ITEM
		{
			/// <summary>
			/// Adds item to trade window, on the side of the trading partner.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="item"></param>
			/// <param name="amount"></param>
			public static void Item(PlayerCharacter character, Item item, int amount)
			{
				using var packet = Packet.Rent(Op.ZC_ADD_EXCHANGE_ITEM);

				packet.PutInt(amount);
				packet.PutString(item.StringId, Sizes.ItemNames);

				character.Connection.Send(packet);
			}

			/// <summary>
			/// Adds zeny to trade window, on the side of the trading
			/// partner.
			/// </summary>
			/// <param name="character"></param>
			/// <param name="amount"></param>
			public static void Zeny(PlayerCharacter character, int amount)
			{
				using var packet = Packet.Rent(Op.ZC_ADD_EXCHANGE_ITEM);

				packet.PutInt(amount);
				packet.PutString("money", Sizes.ItemNames);

				character.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Sends response to request to add item to trade, adding the
		/// item to the own side on success.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="invId"></param>
		/// <param name="result"></param>
		public static void ZC_ACK_ADD_EXCHANGE_ITEM(PlayerCharacter character, int invId, TradingSuccess result)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_ADD_EXCHANGE_ITEM);

			packet.PutShort((short)invId);
			packet.PutByte((byte)result);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies the character about the given side requesting to
		/// lock in the trade.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="side"></param>
		public static void ZC_CONCLUDE_EXCHANGE_ITEM(PlayerCharacter character, TradingSide side)
		{
			using var packet = Packet.Rent(Op.ZC_CONCLUDE_EXCHANGE_ITEM);
			packet.PutByte((byte)side);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies the character about the result of the trade finish
		/// request.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public static void ZC_EXEC_EXCHANGE_ITEM(PlayerCharacter character, TradingSuccess result)
		{
			using var packet = Packet.Rent(Op.ZC_EXEC_EXCHANGE_ITEM);
			packet.PutByte((byte)result);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies the character about the result of the chat room
		/// creation request.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public static void ZC_ACK_CREATE_CHATROOM(PlayerCharacter character, ChatRoomSuccess result)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_CREATE_CHATROOM);
			packet.PutByte((byte)result);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Creates a new chat room on all clients close to the chat,
		/// displaying it above the owner's head.
		/// </summary>
		/// <param name="room"></param>
		public static void ZC_ROOM_NEWENTRY(ChatRoom room)
		{
			using var packet = Packet.Rent(Op.ZC_ROOM_NEWENTRY);

			packet.PutInt(room.OwnerHandle);
			packet.PutInt(room.Id);
			packet.PutShort((short)room.Limit);
			packet.PutShort((short)room.MemberCount);
			packet.PutByte((byte)room.Privacy);
			packet.PutString(room.Title, false);

			room.Map.Broadcast(packet);
		}

		/// <summary>
		/// Updates the chat room on the character's client in respons to
		/// a change request.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="room"></param>
		public static void ZC_CHANGE_CHATROOM(PlayerCharacter character, ChatRoom room)
		{
			using var packet = Packet.Rent(Op.ZC_CHANGE_CHATROOM);

			packet.PutInt(room.OwnerHandle);
			packet.PutInt(room.Id);
			packet.PutShort((short)room.Limit);
			packet.PutShort((short)room.MemberCount);
			packet.PutByte((byte)room.Privacy);
			packet.PutString(room.Title, false);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Removes chat room from all clients close to the chat, no
		/// longer displaying it above the owner's head.
		/// </summary>
		/// <param name="room"></param>
		public static void ZC_DESTROY_ROOM(ChatRoom room)
		{
			using var packet = Packet.Rent(Op.ZC_DESTROY_ROOM);
			packet.PutInt(room.Id);

			room.Map.Broadcast(packet);
		}

		/// <summary>
		/// Notifies the character about the reason they weren't able to
		/// join a chat room.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="reason"></param>
		public static void ZC_REFUSE_ENTER_ROOM(PlayerCharacter character, ChatRoomRefuseReason reason)
		{
			using var packet = Packet.Rent(Op.ZC_REFUSE_ENTER_ROOM);
			packet.PutByte((byte)reason);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Sends information about the chat they're entering to character.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="newMember"></param>
		public static void ZC_ENTER_ROOM(ChatRoom room, PlayerCharacter newMember)
		{
			using var packet = Packet.Rent(Op.ZC_ENTER_ROOM);

			packet.PutInt(room.Id);

			using var members = room.GetMembers();
			foreach (var member in members)
			{
				packet.PutInt((int)member.Role);
				packet.PutString(member.Name, Sizes.CharacterNames);
			}

			newMember.Connection.Send(packet);
		}

		/// <summary>
		/// Notifies the chat members about a new member joining.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="newMember"></param>
		public static void ZC_MEMBER_NEWENTRY(ChatRoom room, PlayerCharacter newMember)
		{
			using var packet = Packet.Rent(Op.ZC_MEMBER_NEWENTRY);

			packet.PutShort((short)room.MemberCount);
			packet.PutString(newMember.Name, Sizes.CharacterNames);

			using var members = room.GetMembers();
			foreach (var member in members)
			{
				if (room.Map.TryGetPlayer(member.Handle, out var memberCharacter))
					memberCharacter.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Notifies the chat members about a member leaving.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="formerMember"></param>
		/// <param name="reason"></param>
		public static void ZC_MEMBER_EXIT(ChatRoom room, PlayerCharacter formerMember, MemberExitReason reason)
		{
			using var packet = Packet.Rent(Op.ZC_MEMBER_EXIT);

			packet.PutShort((short)room.MemberCount);
			packet.PutString(formerMember.Name, Sizes.CharacterNames);
			packet.PutByte((byte)reason);

			formerMember.Connection.Send(packet);

			using var members = room.GetMembers();
			foreach (var member in members)
			{
				if (room.Map.TryGetPlayer(member.Handle, out var memberCharacter))
					memberCharacter.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Updates the given members role in the chat, moving the owner
		/// to the top.
		/// </summary>
		/// <param name="room"></param>
		/// <param name="memberName"></param>
		/// <param name="role"></param>
		public static void ZC_ROLE_CHANGE(ChatRoom room, string memberName, ChatRoomRole role)
		{
			using var packet = Packet.Rent(Op.ZC_ROLE_CHANGE);

			packet.PutInt((int)role);
			packet.PutString(memberName, Sizes.CharacterNames);

			using var members = room.GetMembers();
			foreach (var member in members)
			{
				if (room.Map.TryGetPlayer(member.Handle, out var memberCharacter))
					memberCharacter.Connection.Send(packet);
			}
		}

		/// <summary>
		/// Sends result of the party creation request to the character,
		/// opening the party window on success and/or displaying a
		/// message about the result.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="result"></param>
		public static void ZC_ACK_MAKE_GROUP(PlayerCharacter character, PartyCreationResult result)
		{
			using var packet = Packet.Rent(Op.ZC_ACK_MAKE_GROUP);
			packet.PutByte((byte)result);

			character.Connection.Send(packet);
		}

		/// <summary>
		/// Displays effect on character, for them and the characters
		/// nearby.
		/// </summary>
		/// <remarks>
		/// First seen in i20030430, where the client handles 5 effects,
		/// from 0 to 4. Based on their usage in eAthena, those might be:
		/// 
		/// - 0: BaseLevelUp
		/// - 1: JobLevelUp
		/// - 2: RefineFail
		/// - 3: RefineUp
		/// - 4: ?
		/// 
		/// Unfortunately no clients that were tested did anything when
		/// these were sent on their own.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="effectId"></param>
		public static void ZC_NOTIFY_EFFECT(PlayerCharacter character, int effectId)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_EFFECT);

			packet.PutInt(character.Handle);
			packet.PutInt((int)effectId);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}

		/// <summary>
		/// Displays effect on character, for them and the characters
		/// nearby.
		/// </summary>
		/// <remarks>
		/// First seen in eu20040512, these effects appear to be more
		/// general purpose than the original ZC_NOTIFY_EFFECT and make
		/// up the vast majority of effects in use.
		/// </remarks>
		/// <param name="character"></param>
		/// <param name="effectId"></param>
		public static void ZC_NOTIFY_EFFECT2(PlayerCharacter character, EffectId effectId)
		{
			using var packet = Packet.Rent(Op.ZC_NOTIFY_EFFECT2);

			packet.PutInt(character.Handle);
			packet.PutInt((int)effectId);

			character.Map.Broadcast(packet, character, BroadcastTargets.All);
		}
	}
}
