using System;
using System.Buffers;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Events.Args;
using Sabine.Zone.Scripting;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.Skills.Handlers.Novice;
using Sabine.Zone.World.Actors;
using Sabine.Zone.World.Chats;
using Sabine.Zone.World.Maps;
using Sabine.Zone.World.Shops;
using Yggdrasil.Collections;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// Packet handler methods.
	/// </summary>
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
			int accountId, sessionId, characterId, tick;

			if (Game.Version < Versions.Beta1)
			{
				accountId = packet.GetInt();
				characterId = packet.GetInt();
				sessionId = packet.GetInt();
			}
			else if (Game.Version < Versions.S2000)
			{
				accountId = packet.GetInt();
				characterId = packet.GetInt();
				sessionId = packet.GetInt();

				// This isn't sessionId2. Looks like it might be a tick?
				tick = packet.GetInt();
			}
			else
			{
				// It seems like this structure changed wildly over the
				// years, going by eA's packet db, though they always only
				// used the same five fields, and I don't see what the
				// rest of the data would be either. Was this an
				// obfuscation attempt by Gravity?

				packet.Skip(2);
				accountId = packet.GetInt();
				packet.Skip(1);
				characterId = packet.GetInt();
				packet.Skip(4);
				sessionId = packet.GetInt();
				tick = packet.GetInt();
			}

			var sex = packet.GetByte();

			var account = ZoneServer.Instance.Database.GetAccountById(accountId);
			if (account == null)
			{
				Log.Debug("CZ_ENTER: Account '{0}' not found.", accountId);
				conn.Close();
				return;
			}

			if (sessionId != account.SessionId)
			{
				Log.Warning("CZ_ENTER: User '{0}' tried to log in with an invalid session id.", account.Username);
				conn.Close();
				return;
			}

			var character = ZoneServer.Instance.Database.GetCharacter(account, characterId);
			if (character == null)
			{
				Log.Warning("CZ_ENTER: User '{0}' tried to log in with a character that doesn't exist ({1}).", account.Username, characterId);
				conn.Close();
				return;
			}

			if (!ZoneServer.Instance.World.Maps.TryGet(character.MapId, out var map))
			{
				Log.Warning("CZ_ENTER: Map '{0}' not found for character '{1}'. Closing connection for char server to fix the location.", character.MapId, character.Name);

				conn.Close();
				return;
			}

			if (character.SaveLocation.IsZero)
				character.SaveLocation = new Location(100036, 99, 81);

			conn.Account = account;
			conn.Character = character;
			character.Connection = conn;

			ZoneServer.Instance.ServerEvents.PlayerLoggedIn.Raise(new PlayerEventArgs(character));

			// Starting some time after beta 1, the client expects the raw
			// account id to be sent upon connection, or it won't react to
			// any packets...?
			if (Game.Version >= Versions.Beta2)
				Send.InitConnection(conn);

			Send.ZC_ACCEPT_ENTER(conn, character);

			map.AddPlayer(character);

			Log.Info("User '{0}' logged in.", account.Username);
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

			if (character.IsWarping)
			{
				character.FinalizeWarp();
				return;
			}

			character.StartObserving();

			// Send all stats/parameters to the client that it didn't
			// get from the char server yet. Also send a few that
			// might've changed during character loading.
			Send.ZC_STATUS(character);
			Send.ZC_PAR_CHANGE(character, ParameterType.WeightMax);
			Send.ZC_PAR_CHANGE(character, ParameterType.Weight);
			Send.ZC_PAR_CHANGE(character, ParameterType.SkillPoints);
			Send.ZC_PAR_CHANGE(character, ParameterType.HpMax);
			Send.ZC_PAR_CHANGE(character, ParameterType.Hp);
			Send.ZC_PAR_CHANGE(character, ParameterType.SpMax);
			Send.ZC_PAR_CHANGE(character, ParameterType.Sp);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.BaseExpNeeded);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.JobExpNeeded);

			var items = character.Inventory.GetItems();
			Send.ZC_NORMAL_ITEMLIST(character, items);
			Send.ZC_EQUIPMENT_ITEMLIST(character, items);

			if (character.Inventory.Ammo != null)
				Send.ZC_EQUIP_ARROW(character, character.Inventory.Ammo);

			var skills = character.Skills.GetAll();
			Send.ZC_SKILLINFO_LIST(character, skills);

			if (character.IsDead)
				Send.ZC_NOTIFY_VANISH(character, DisappearType.StrikedDead);

			ZoneServer.Instance.ServerEvents.PlayerReady.Raise(new PlayerEventArgs(character));
		}

		/// <summary>
		/// Request for the current server time.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_TIME)]
		public void CZ_REQUEST_TIME(ZoneConnection conn, Packet packet)
		{
			var clientTime = -1;

			if (Game.Version >= Versions.Beta1)
				clientTime = packet.GetInt();

			Send.ZC_NOTIFY_TIME(conn, Game.GetTick());
		}

		/// <summary>
		/// Request to move to a new position.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_MOVE)]
		public void CZ_REQUEST_MOVE(ZoneConnection conn, Packet packet)
		{
			// Three byte that don't seem to change or contain useful
			// information?
			if (Game.Version >= Versions.S2000)
				packet.Skip(3);

			var toPos = (Position)packet.GetPackedPosition();

			var character = conn.GetCurrentCharacter();
			var fromPos = character.Position;

			character.Controller.MoveTo(toPos);

			// Spawn some NPCs to visualize the path the server calculated
			// for this move request.
			if (character.Vars.Temp.GetBool("Sabine.DebugPathEnabled"))
			{
				var path = character.Map.PathFinder.FindPath(fromPos, toPos);
				foreach (var pathPos in path)
				{
					var npc = new Npc(IdentityId.JT_1_F_01);
					npc.Warp(character.Map.Id, pathPos);

					Task.Delay(5000).ContinueWith(_ => character.Map.RemoveNpc(npc));
				}
			}
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

			if (Game.Version >= Versions.S600)
			{
				// What is this...? It's part of the message, and the client
				// doesn't display it when it's sent back to it, but what
				// does it do? Looks like there's no reference to it in
				// eAthena either. Maybe it's a euRO thing? Something to
				// do with languages?
				if (text.StartsWith("|00"))
					text = text.Substring("|00".Length);
			}

			var character = conn.GetCurrentCharacter();

			if (ZoneServer.Instance.ChatCommands.TryExecute(character, text))
				return;

			text = string.Format("{0} : {1}", character.Name, text);

			// The client uses the same packets for displaying chat
			// messages publically and inside chat rooms, forcing us to
			// filter who we send which messages to. If the character is
			// in a chat room, they will only get messages from that chat
			// room. If they're not in a chat, they will only get messages
			// from characters around them who are also not in a chat. For
			// this purpose, we consider chat room 0 to be the public
			// chat, to quickly match characters who are in the same
			// "chat".

			using var sameChatCharacters = PooledList<PlayerCharacter>.Rent();
			character.Map.GetPlayers(sameChatCharacters, character, static (sourceCharacter, character) =>
			{
				if (sourceCharacter == character)
					return false;

				var inRange = sourceCharacter.Position.InRange(character.Position, sourceCharacter.Map.VisibleRange);
				if (!inRange)
					return false;

				var sameRoom = sourceCharacter.ChatRoomId == character.ChatRoomId;
				if (!sameRoom)
					return false;

				return true;
			});

			Send.ZC_NOTIFY_CHAT(new ListBroadcastSender(sameChatCharacters), character.Handle, text);
			Send.ZC_NOTIFY_PLAYERCHAT(character, text);
		}

		/// <summary>
		/// Request to send a whisper chat message to another character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_WHISPER)]
		public void CZ_WHISPER(ZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var targetName = packet.GetString(16);
			var message = packet.GetString(len - 4 - 16);

			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Maps.TryGetPlayerByName(targetName, out var target))
			{
				Send.ZC_ACK_WHISPER(character, WhisperResult.CharacterDoesntExist);
				return;
			}

			Send.ZC_WHISPER(target, character.Name, message);
			Send.ZC_ACK_WHISPER(character, WhisperResult.Okay);
		}

		/// <summary>
		/// Request for a character's name when hovering over them.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQNAME)]
		public void CZ_REQNAME(ZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var target = character.Map.GetCharacter(handle);

			if (target == null)
			{
				// Don't warn, since this can easily happen when the client
				// requests the name for a character that just disappeared.
				//Log.Debug("CZ_REQNAME: User {0} requested the name of a character that doesn't exist.", conn.Account.Username);
				return;
			}

			Send.ZC_ACK_REQNAME(character, target);
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
			var parameters = character.Parameters;

			var success = false;
			var value = 0;

			if (type < ParameterType.Str || type > ParameterType.Luk)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to assign points to invalid stat '{1}'.", conn.Account.Username, type);
				goto L_End;
			}

			var pointsNeeded = parameters.GetStatPointsNeeded(type);
			if (parameters.StatPoints < pointsNeeded)
			{
				Log.Debug("CZ_STATUS_CHANGE: User '{0}' tried to use more stat points than they have.", conn.Account.Username);
				goto L_End;
			}

			value = parameters.Modify(type, change);
			parameters.Modify(ParameterType.StatPoints, -pointsNeeded);

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

			if (NV_BASIC.TryFail(character, BasicSkillAbility.UseEmotes))
				return;

			Send.ZC_EMOTION(character, emotion);
		}

		/// <summary>
		/// Request for an item's description.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ITEM_EXPLANATION_BYNAME)]
		public void CZ_REQ_ITEM_EXPLANATION_BYNAME(ZoneConnection conn, Packet packet)
		{
			var itemStringId = packet.GetString(16);

			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.Data.ItemNames.TryFind(a => a.AlphaName == itemStringId || a.BetaName == itemStringId, out var itemNameData))
			{
				Log.Warning("CZ_REQ_ITEM_EXPLANATION_BYNAME: Item name data for '{0}' not found.", itemStringId);
				return;
			}

			if (!ZoneServer.Instance.Data.Items.TryFind(itemNameData.Id, out var itemData))
			{
				Log.Warning("CZ_REQ_ITEM_EXPLANATION_BYNAME: Item data for '{0}' not found.", itemStringId);
				return;
			}

			// The alpha client usually identifies items by their string
			// id and converts that to a Korean name to find the assets
			// for the item in the client. This works for the sprites
			// and the name display, but not the description. The
			// client sends the English string id for this request,
			// but if you send that name back, you get an error that
			// it can't find the texture for the item. Because of this,
			// we need to send back the Korean name in this instance.
			// The title of the item description window will be mangled
			// this way, but that's how it has to be.
			var name = itemNameData.KoreanName;

			// Generate a description. We could put proper descriptions
			// in a database, but this should work for now and it's kind
			// of fun that you can just generate them. It would be good
			// if someone could tell us what descriptions looked liked
			// in the alpha, because the client seems to have no support
			// for line-breaks.
			var sb = new StringBuilder();

			switch (itemData.Type)
			{
				case ItemType.Weapon:
				case ItemType.RangedWeapon:
				{
					sb.AppendFormat("Attack:^777777 {0}-{1}^000000", itemData.AttackMin, itemData.AttackMax);
					sb.AppendFormat(", Weight:^777777 {0:0.#}^000000", itemData.Weight / 10f);
					sb.AppendFormat(", Required Level:^777777 {0}^000000", itemData.RequiredLevel);
					sb.AppendFormat(", Jobs:^777777 {0}^000000", itemData.JobsAllowed.ToReadableString());
					break;
				}
				case ItemType.Armor:
				{
					sb.AppendFormat("Defense:^777777 {0}^000000", itemData.Defense);
					sb.AppendFormat(", Weight:^777777 {0:0.#}^000000", itemData.Weight / 10f);
					sb.AppendFormat(", Required Level:^777777 {0}^000000", itemData.RequiredLevel);
					sb.AppendFormat(", Jobs:^777777 {0}^000000", itemData.JobsAllowed.ToReadableString());
					break;
				}
				default:
				{
					sb.AppendFormat("Weight:^777777 {0:0.#}^000000", itemData.Weight / 10f);
					break;
				}
			}

			Send.ZC_REQ_ITEM_EXPLANATION_ACK(character, name, sb.ToString());
		}

		/// <summary>
		/// Request for starting a dialog with an NPC.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CONTACTNPC)]
		public void CZ_CONTACTNPC(ZoneConnection conn, Packet packet)
		{
			var targetHandle = packet.GetInt();
			var b1 = packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var target = character.Map.GetCharacter(targetHandle);

			if (target == null)
			{
				Log.Debug("CZ_CONTACTNPC: User '{0}' tried to contact a non-existent target.", conn.Account.Username);
				return;
			}

			if (target is not Npc npc)
			{
				Log.Debug("CZ_CONTACTNPC: User '{0}' tried to contact a non-NPC.", conn.Account.Username);
				return;
			}

			if (npc.DialogFunc == null)
				return;

			conn.CurrentDialog = new Dialog(character, npc);
			conn.CurrentDialog.Start();
		}

		/// <summary>
		/// Chooses a menu item during a dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHOOSE_MENU)]
		public void CZ_CHOOSE_MENU(ZoneConnection conn, Packet packet)
		{
			var npcHandle = packet.GetInt();
			var choice = (int)packet.GetSByte();

			var character = conn.GetCurrentCharacter();
			var npc = character.Map.GetCharacter(npcHandle);

			if (character.IsDead)
			{
				// This packet is sent if the player clicks outside of the
				// respawn dialog after death.
				return;
			}

			// 0xFF is sent when there's no menu to choose anything from,
			// so it's presumably a cancel action.

			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_CHOOSE_MENU: User '{0}' tried to choose a menu item without being in a dialog.", conn.Account.Username);
				return;
			}

			conn.CurrentDialog.Resume(choice.ToString());
		}

		/// <summary>
		/// Request to continue a paused dialog.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_NEXT_SCRIPT)]
		public void CZ_REQ_NEXT_SCRIPT(ZoneConnection conn, Packet packet)
		{
			var npcHandle = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var npc = character.Map.GetCharacter(npcHandle);

			if (conn.CurrentDialog == null)
			{
				Log.Debug("CZ_CHOOSE_MENU: User '{0}' tried to choose a menu item without being in a dialog.", conn.Account.Username);
				return;
			}

			conn.CurrentDialog.Resume(null);
		}

		/// <summary>
		/// Request to do an action, such as sitting down or attacking.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQUEST_ACT)]
		public void CZ_REQUEST_ACT(ZoneConnection conn, Packet packet)
		{
			int targetHandle;
			ActionType action;

			if (Game.Version < Versions.S2000)
			{
				targetHandle = packet.GetInt();
				action = (ActionType)packet.GetByte();
			}
			else
			{
				// It's currently unknown whether this is exclusive to
				// euRO, but the eu20070305 client obfuscates this packet
				// by using a dynamic size and writing the handle to a
				// position somewhere in the middle of otherwise garbage
				// data. 

				var len = packet.GetShort();

				var handleOffset = (len % 2 == 0) ? (4 + (len - 20) / 2) : ((len + 27) / 2);
				var fillerLen1 = handleOffset - 4;
				var fillerLen2 = len - handleOffset - 8;

				packet.Skip(fillerLen1);

				targetHandle = packet.GetInt();

				packet.Skip(fillerLen2);

				action = (ActionType)packet.GetInt();
			}

			var character = conn.GetCurrentCharacter();

			character.Controller.StopMove();

			switch (action)
			{
				case ActionType.SitDown:
				{
					if (NV_BASIC.TryFail(character, BasicSkillAbility.Sit))
						return;

					character.SitDown();
					break;
				}
				case ActionType.StandUp:
				{
					character.StandUp();
					break;
				}
				case ActionType.Attack:
				case ActionType.AutoAttack:
				{
					// So far, I've seen Attack only on Alpha, and newer
					// clients used AutoAttack. To not actually keep at-
					// tacking when not intended, the client sends the
					// packet CZ_CANCEL_LOCKON right after the ACT packet.

					if (!character.Map.TryGetCharacter(targetHandle, out var target))
					{
						Log.Debug("CZ_REQUEST_ACT: User '{0}' tried to attack a character who doesn't exist.", conn.Account.Username);
						return;
					}

					var attackRange = character.GetAttackRange();

					if (character.Vars.Temp.GetBool("Sabine.DebugMode", false))
					{
						var distance = character.Position.GetDistance(target.Position);
						var inRange = character.Position.InRange(target.Position, attackRange);

						character.DebugMessage("Attack Range: {0}, Distance: {1}, InRange: {2}", attackRange, distance, inRange);
					}

					if (!character.Position.InRange(target.Position, attackRange))
					{
						// The alpha client does its own range checks and
						// the distance fail packet doesn't exist yet.
						// It's safe to assume that nothing more but
						// stopping the attack is expected from us for
						// the alpha here.
						if (Game.Version >= Versions.Beta1)
							Send.ZC_ATTACK_FAILURE_FOR_DISTANCE(character, target, attackRange - 1);
						return;
					}

					var autoAttack = action == ActionType.AutoAttack;
					if (Game.Version < Versions.Beta1)
						autoAttack = false;

					character.StartAttacking(target, autoAttack);
					break;
				}
				default:
				{
					Log.Debug("CZ_REQUEST_ACT: Unknown action '{0}'.", action);
					break;
				}
			}
		}

		/// <summary>
		/// Cancels the lock state on a target, to make the player character
		/// stop attacking it.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CANCEL_LOCKON)]
		public void CZ_CANCEL_LOCKON(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();
			character.StopAttacking();
		}

		/// <summary>
		/// Notification that the character rotated into a new direction.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_DIRECTION)]
		public void CZ_CHANGE_DIRECTION(ZoneConnection conn, Packet packet)
		{
			var headTurn = HeadTurn.Straight;
			var bodyDir = Direction.South;

			if (Game.Version < Versions.Beta2)
			{
				bodyDir = (Direction)packet.GetByte();
			}
			else if (Game.Version < Versions.S2000)
			{
				// Beta2 added the ability to turn the head in addition to
				// the body

				headTurn = (HeadTurn)packet.GetByte();
				var b2 = packet.GetByte();
				bodyDir = (Direction)packet.GetByte();
			}
			else
			{
				packet.Skip(5);
				headTurn = (HeadTurn)packet.GetByte();
				var b2 = packet.GetByte();
				packet.Skip(1);
				bodyDir = (Direction)packet.GetByte();
			}

			var character = conn.GetCurrentCharacter();

			character.Direction = bodyDir;
			character.HeadTurn = headTurn;

			Send.ZC_CHANGE_DIRECTION(character, bodyDir, headTurn);
		}

		/// <summary>
		/// Request to pick up an item.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_PICKUP)]
		public void CZ_ITEM_PICKUP(ZoneConnection conn, Packet packet)
		{
			var itemHandle = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var item = character.Map.GetItem(itemHandle);

			if (item == null)
			{
				Log.Debug("CZ_ITEM_PICKUP: User '{0}' tried to pick up a non-existing item.", conn.Account.Username);
				return;
			}

			item.Map.RemoveItem(item);
			character.Inventory.AddItem(item);
		}

		/// <summary>
		/// Request to drop items from a stack.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ITEM_THROW)]
		public void CZ_ITEM_THROW(ZoneConnection conn, Packet packet)
		{
			var itemInvId = packet.GetShort();
			var amount = packet.GetShort();

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				Log.Debug("CZ_ITEM_THROW: User '{0}' tried to drop an item they don't have.", conn.Account.Username);
				return;
			}

			if (amount <= 0 || amount > item.Amount)
			{
				// The client doesn't send a drop request if you put in
				// a 0 or more than you have.
				Log.Debug("CZ_ITEM_THROW: User '{0}' tried to drop an invalid amount.", conn.Account.Username);
				return;
			}

			var removedAmount = character.Inventory.DecrementItem(item, amount);
			var dropItem = new Item(item.ClassId, removedAmount);

			character.Drop(dropItem);
		}

		/// <summary>
		/// Request to use an item from the inventory.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_USE_ITEM)]
		public void CZ_USE_ITEM(ZoneConnection conn, Packet packet)
		{
			var itemInvId = packet.GetShort();
			var clientTick = packet.GetInt();

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				// Both ZC_USE_ITEM_ACK and ZC_REQ_WEAR_EQUIP_ACK appear
				// to have a success parameter, with the client ignoring
				// the entire packet if it's false. However, the client
				// also seems to be waiting for a positive response, and
				// if you send a negative one, or nothing at all, it will
				// not send any more requests to use or equip any item
				// until the next relog.
				// In the case of using an item we can let it slight and
				// simply not apply any effects, but for equipping we
				// would either need to go through with it and then reverse
				// it, or simply assume that the player is cheating and
				// disconnect them. There's probably no legit reason for
				// why a player should be unable to equip or use an item
				// unless something is very wrong.

				Log.Debug("CZ_USE_ITEM: User '{0}' tried to equip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			if (!ItemScript.TryGetScript(item.ClassId, out var script))
			{
				character.ServerMessage(Localization.Get("This item has not been implemented yet."));
				Log.Debug("CZ_USE_ITEM: No script found for item '{0}'.", item.ClassId);
			}
			else
			{
				var result = script.OnUse(character, item);
				if (result == ItemUseResult.Okay)
					character.Inventory.DecrementItem(item, 1);
			}

			Send.ZC_USE_ITEM_ACK(character, itemInvId, item.Amount);
		}

		/// <summary>
		/// Request to equip an item from the inventory.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_WEAR_EQUIP)]
		public void CZ_REQ_WEAR_EQUIP(ZoneConnection conn, Packet packet)
		{
			int itemInvId;
			EquipSlots equipSlots;

			if (Game.Version < Versions.Beta2)
			{
				itemInvId = packet.GetShort();
				equipSlots = (EquipSlots)packet.GetByte();
			}
			else
			{
				itemInvId = packet.GetShort();
				equipSlots = (EquipSlots)packet.GetShort();
			}

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				// See CZ_USE_ITEM about negative responses.
				Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			if (character.Vars.Temp.GetBool("Sabine.DebugMode", false))
			{
				character.DebugMessage("Equip {0} on {1}", item.Data.Name, equipSlots);
			}

			if (!character.CanEquip(item))
			{
				if (Game.Version < Versions.Beta1)
				{
					Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item they can't equip.", conn.Account.Username);
					conn.Close();
					return;
				}

				if (character.Parameters.BaseLevel < item.Data.RequiredLevel)
					character.ServerMessage(Localization.Get("You need to be at east level {0} to equip this item."), item.Data.RequiredLevel);

				Send.ZC_REQ_WEAR_EQUIP_ACK.Fail(character, itemInvId);
				return;
			}

			if (item.Type == ItemType.Ammo)
			{
				character.Inventory.EquipAmmo(item);
				return;
			}

			if (equipSlots == EquipSlots.None)
			{
				Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item in None (Item: {1} ({2}), Data Slots: {3}).", conn.Account.Username, item.Data.Name, item.ClassId, item.Data.WearSlots);

				if (Game.Version < Versions.Beta1)
				{
					conn.Close();
					return;
				}

				Send.ZC_REQ_WEAR_EQUIP_ACK.Fail(character, itemInvId);
				return;
			}

			if (item.Data.WearSlots != equipSlots)
			{
				if (Game.Version < Versions.Beta1)
				{
					Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item in an invalid slot (Item: {1}, Request: {2}).", conn.Account.Username, item.Data.WearSlots, equipSlots);
					conn.Close();
					return;
				}

				Send.ZC_REQ_WEAR_EQUIP_ACK.Fail(character, itemInvId);
				return;
			}

			character.Inventory.EquipItem(item, equipSlots);
		}

		/// <summary>
		/// Request to unequip an item and move it to the inventory.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_TAKEOFF_EQUIP)]
		public void CZ_REQ_TAKEOFF_EQUIP(ZoneConnection conn, Packet packet)
		{
			var itemInvId = packet.GetShort();

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				// See CZ_USE_ITEM about negative responses.
				Log.Debug("ZC_REQ_TAKEOFF_EQUIP_ACK: User '{0}' tried to unequip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			character.Inventory.UnequipItem(item);
		}

		/// <summary>
		/// Request to drop an item on the ground.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.ZC_ITEM_THROW_ACK)]
		public void ZC_ITEM_THROW_ACK(ZoneConnection conn, Packet packet)
		{
			var itemInvId = packet.GetShort();

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				// See CZ_USE_ITEM about negative responses.
				Log.Debug("ZC_REQ_TAKEOFF_EQUIP_ACK: User '{0}' tried to unequip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			character.Inventory.UnequipItem(item);
		}

		/// <summary>
		/// Notification that the player wants to close the storage.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CLOSE_STORE)]
		public void CZ_CLOSE_STORE(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();
			Send.ZC_CLOSE_STORE(character);
		}

		/// <summary>
		/// Response to server's query about whether the player wants to
		/// buy or sell items.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ACK_SELECT_DEALTYPE)]
		public void CZ_ACK_SELECT_DEALTYPE(ZoneConnection conn, Packet packet)
		{
			var handle = packet.GetInt();
			var option = (ShopActionType)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var shop = character.Vars.Temp.Get<NpcShop>("Sabine.CurrentShop", null);

			if (shop == null)
			{
				Log.Warning("CZ_ACK_SELECT_DEALTYPE: User '{0}' tried to open a shop without one being active.", conn.Account.Username);
				return;
			}

			if (option == ShopActionType.Buy)
			{
				var items = shop.GetItems();
				Send.ZC_PC_PURCHASE_ITEMLIST(character, items);
			}
			else
			{
				var items = character.Inventory.GetItems();
				Send.ZC_PC_SELL_ITEMLIST(character, items);
			}
		}

		/// <summary>
		/// Request to buy items from a shop.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PC_PURCHASE_ITEMLIST)]
		public void CZ_PC_PURCHASE_ITEMLIST(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();
			var shop = character.Vars.Temp.Get("Sabine.CurrentShop") as NpcShop;
			var zenyCost = 0;

			var len = packet.GetShort();

			var count = (len - 4) / 18;
			var buyItems = new Dictionary<ShopItem, int>();

			for (var i = 0; i < count; ++i)
			{
				var amount = packet.GetShort();
				var stringId = packet.GetString(16);

				var item = shop.GetItem(stringId);
				if (item == null)
				{
					Log.Warning("CZ_PC_PURCHASE_ITEMLIST: User '{0}' tried to buy an item that doesn't exist in the shop.", conn.Account.Username);
					return;
				}

				buyItems[item] = amount;
				zenyCost += item.Price * amount;
			}

			if (character.Parameters.Zeny < zenyCost)
			{
				Log.Debug("CZ_PC_PURCHASE_ITEMLIST: User '{0}' didn't have enough money to buy the selected items.", conn.Account.Username);
				return;
			}

			foreach (var entry in buyItems)
			{
				var shopItem = entry.Key;
				var amount = entry.Value;

				var newItem = new Item(shopItem.ClassId, amount);
				character.Inventory.AddItem(newItem);
			}

			character.Parameters.Modify(ParameterType.Zeny, -zenyCost);
		}

		/// <summary>
		/// Request to sell items to a shop.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_PC_SELL_ITEMLIST)]
		public void CZ_PC_SELL_ITEMLIST(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			var len = packet.GetShort();

			var count = (len - 4) / 4;
			var sellItems = new Dictionary<Item, int>();

			for (var i = 0; i < count; ++i)
			{
				var invId = packet.GetShort();
				var amount = packet.GetShort();

				var item = character.Inventory.GetItem(invId);
				if (item == null)
				{
					Log.Warning("CZ_PC_SELL_ITEMLIST: User '{0}' tried to sell an item they don't have.", conn.Account.Username);
					return;
				}

				if (item.Amount < amount)
				{
					Log.Warning("CZ_PC_SELL_ITEMLIST: User '{0}' tried to sell more items than they have.", conn.Account.Username);
					return;
				}

				sellItems[item] = amount;
			}

			var gainZeny = 0;

			foreach (var entry in sellItems)
			{
				var item = entry.Key;
				var amount = entry.Value;

				character.Inventory.DecrementItem(item, amount);
				gainZeny += item.Data.SellPrice * amount;
			}

			character.Parameters.Modify(ParameterType.Zeny, gainZeny);
		}

		/// <summary>
		/// Request to go back to the character server.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_RESTART)]
		public void CZ_RESTART(ZoneConnection conn, Packet packet)
		{
			var type = (RestartType)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			Send.ZC_RESTART_ACK(character, type);

			if (type == RestartType.SavePoint)
			{
				character.Heal();
				character.Warp(character.SaveLocation);
			}
		}

		/// <summary>
		/// Request to be allowed to close the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_DISCONNECT)]
		public void CZ_REQ_DISCONNECT(ZoneConnection conn, Packet packet)
		{
			var s1 = packet.GetShort();

			var character = conn.GetCurrentCharacter();
			Send.ZC_ACK_REQ_DISCONNECT(character, 0); // 1 = wait 10 seconds
		}

		/// <summary>
		/// Request to increase a skill's level.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_UPGRADE_SKILLLEVEL)]
		public void CZ_UPGRADE_SKILLLEVEL(ZoneConnection conn, Packet packet)
		{
			var skillId = (SkillId)packet.GetShort();

			var character = conn.GetCurrentCharacter();

			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_UPGRADE_SKILLLEVEL: User '{0}' tried to upgrade a skill they don't have.", conn.Account.Username);
				return;
			}

			if (!skill.CanBeLeveled)
			{
				Log.Warning("CZ_UPGRADE_SKILLLEVEL: User '{0}' tried to upgrade a skill that can't be leveled any more.", conn.Account.Username);
				return;
			}

			if (character.Parameters.SkillPoints < 1)
			{
				Log.Warning("CZ_UPGRADE_SKILLLEVEL: User '{0}' tried to upgrade a skill without having enough skill points.", conn.Account.Username);
				return;
			}

			skill.LevelUp();
			character.Parameters.Modify(ParameterType.SkillPoints, -1);

			character.Skills.UpdateClassSkills();
		}

		/// <summary>
		/// Request to use a skill on a target.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_USE_SKILL)]
		public void CZ_USE_SKILL(ZoneConnection conn, Packet packet)
		{
			var level = packet.GetShort();
			var skillId = (SkillId)packet.GetShort();
			var targetHandle = packet.GetInt();

			var character = conn.GetCurrentCharacter();

			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_USE_SKILL: User '{0}' tried to use skill '{1}', which they don't have.", conn.Account.Username, skillId);
				return;
			}

			if (skill.Level == 0)
			{
				Log.Warning("CZ_USE_SKILL: User '{0}' tried to use skill '{1}' at level 0.", conn.Account.Username, skillId);
				return;
			}

			if (!character.Map.TryGetCharacter(targetHandle, out var target))
			{
				character.ServerMessage(Localization.Get("Target not found."));
				return;
			}

			// Clamp level, but don't warn about invalid values, since the
			// requested level may be too high if the skill changed after
			// it was hotkeyed.
			level = Math2.Clamp(1, skill.Level, level);

			Send.ZC_NOTIFY_PLAYERCHAT(character, skill.Data.StringId + "!!!");

			switch (skillId)
			{
				case SkillId.SM_BASH:
				{
					if (!character.TrySpendSp(skill.SpCost))
					{
						character.ServerMessage(Localization.Get("Not enough SP."));
						return;
					}

					character.Controller.StopMove();

					var attacker = character;
					var damage = level * 5;

					var attackMotionDelay = attacker.Parameters.AttackMotionDelay;
					var damageMotionDelay = target.Parameters.DamageMotionDelay;

					target.TakeDamage(damage, character);

					Send.ZC_NOTIFY_ACT.Attack(attacker, attacker.Handle, target.Handle, Game.GetTick(), ActionType.Attack, damage, attackMotionDelay, damageMotionDelay);
					break;
				}
			}
		}

		/// <summary>
		/// Request to use a skill targeting the ground.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_USE_SKILL_TOGROUND)]
		public void CZ_USE_SKILL_TOGROUND(ZoneConnection conn, Packet packet)
		{
			var level = packet.GetShort();
			var skillId = (SkillId)packet.GetShort();
			var x = packet.GetShort();
			var y = packet.GetShort();

			var character = conn.GetCurrentCharacter();

			if (!character.Skills.TryGet(skillId, out var skill))
			{
				Log.Warning("CZ_USE_SKILL: User '{0}' tried to use skill '{1}', which they don't have.", conn.Account.Username, skillId);
				return;
			}

			if (skill.Level == 0)
			{
				Log.Warning("CZ_USE_SKILL: User '{0}' tried to use skill '{1}' at level 0.", conn.Account.Username, skillId);
				return;
			}

			var targetPos = new Position(x, y);

			// Clamp level, but don't warn about invalid values, since the
			// requested level may be too high if the skill changed after
			// it was hotkeyed.
			level = Math2.Clamp(1, skill.Level, level);

			Send.ZC_NOTIFY_PLAYERCHAT(character, skill.Data.StringId + "!!!");

			switch (skillId)
			{
				case SkillId.MG_FIREWALL:
				{
					if (!character.InUseRange(skill, targetPos))
					{
						character.ServerMessage(Localization.Get("Too far away."));
						return;
					}

					if (!character.TrySpendSp(skill.SpCost))
					{
						character.ServerMessage(Localization.Get("Not enough SP."));
						return;
					}

					character.Controller.StopMove();

					var npc = new Npc(IdentityId.JT_1_F_01);
					npc.Warp(character.Map.Id, targetPos);

					Task.Delay(3000).ContinueWith(__ => character.Map.RemoveNpc(npc));
					break;
				}
			}
		}

		/// <summary>
		/// Request to start a trade with another player.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_EXCHANGE_ITEM)]
		public void CZ_REQ_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var targetHandle = packet.GetInt();

			var character = conn.GetCurrentCharacter();

			if (!character.Map.TryGetPlayer(targetHandle, out var partner))
			{
				character.ServerMessage(Localization.Get("Character not found."));
				return;
			}

			if (ZoneServer.Instance.World.Trades.TryGetTrade(character, out var existingTrade))
			{
				character.ServerMessage(Localization.Get("You're already trading."));
				return;
			}

			if (NV_BASIC.TryFail(character, BasicSkillAbility.Trade))
				return;

			ZoneServer.Instance.World.Trades.InitiateTrade(character, partner);
		}

		/// <summary>
		/// Response to a trade request from the receiving character.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ACK_EXCHANGE_ITEM)]
		public void CZ_ACK_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var response = (TradingResponse)packet.GetByte();

			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
			{
				character.ServerMessage(Localization.Get("You're not in a trade."));
				return;
			}

			trade.Acknowledge(character, response);
		}

		/// <summary>
		/// Request to add an item to the list of trade items.
		/// </summary>
		/// <remarks>
		/// Sent automatically for Zeny with inventory id 0 upon conluding
		/// the trade. For items it's sent when they're added to the list.
		/// </remarks>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_ADD_EXCHANGE_ITEM)]
		public void CZ_ADD_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var invId = packet.GetShort();
			var amount = packet.GetInt();

			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
			{
				character.ServerMessage(Localization.Get("You're not in a trade."));
				return;
			}

			if (invId == 0)
				trade.SetZeny(character, amount);
			else
				trade.AddItem(character, invId, amount);
		}

		/// <summary>
		/// Request to cancel active trade.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CANCEL_EXCHANGE_ITEM)]
		public void CZ_CANCEL_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
			{
				character.ServerMessage(Localization.Get("You're not in a trade."));
				return;
			}

			trade.RequestCancellation(character);
		}

		/// <summary>
		/// Request to lock in active trade.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CONCLUDE_EXCHANGE_ITEM)]
		public void CZ_CONCLUDE_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
			{
				character.ServerMessage(Localization.Get("You're not in a trade."));
				return;
			}

			trade.Conclude(character);
		}

		/// <summary>
		/// Request to finish active trade.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXEC_EXCHANGE_ITEM)]
		public void CZ_EXEC_EXCHANGE_ITEM(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.Trades.TryGetTrade(character, out var trade))
			{
				character.ServerMessage(Localization.Get("You're not in a trade."));
				return;
			}

			trade.Complete(character);
		}

		/// <summary>
		/// Request to create a chat room.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CREATE_CHATROOM)]
		public void CZ_CREATE_CHATROOM(ZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var limit = packet.GetShort();
			var privacy = (ChatPrivacy)packet.GetByte();
			var password = packet.GetString(8);

			var titleLen = packet.GetRemainingLength();
			var title = packet.GetString(titleLen);

			var character = conn.GetCurrentCharacter();

			if (NV_BASIC.TryFail(character, BasicSkillAbility.CreateChatRoom))
				return;

			limit = Math.Clamp(limit, 1, 20);

			if (privacy < ChatPrivacy.Private || privacy > ChatPrivacy.Public)
				privacy = ChatPrivacy.Public;

			var room = new ChatRoom(character, title, limit, privacy, password);
			room.AddMember(character);

			ZoneServer.Instance.World.ChatRooms.Add(room);

			Send.ZC_ACK_CREATE_CHATROOM(character, ChatRoomSuccess.Success);
		}

		/// <summary>
		/// Request to modify a chat room.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_CHATROOM)]
		public void CZ_CHANGE_CHATROOM(ZoneConnection conn, Packet packet)
		{
			var len = packet.GetShort();
			var limit = packet.GetShort();
			var privacy = (ChatPrivacy)packet.GetByte();
			var password = packet.GetString(8);

			var titleLen = packet.GetRemainingLength();
			var title = packet.GetString(titleLen);

			var character = conn.GetCurrentCharacter();

			limit = Math.Clamp(limit, 1, 20);

			if (privacy < ChatPrivacy.Private || privacy > ChatPrivacy.Public)
				privacy = ChatPrivacy.Public;

			if (character.ChatRoomId == 0)
			{
				Log.Debug("CZ_CHANGE_CHATROOM: User '{0}' tried to change a chat room but isn't in a chat room.", conn.Account.Username);
				return;
			}

			if (!ZoneServer.Instance.World.ChatRooms.TryGet(character.ChatRoomId, out var room))
			{
				Log.Debug("CZ_CHANGE_CHATROOM: User '{0}' tried to change a chat room but their chat room doesn't exist.", conn.Account.Username);
				return;
			}

			if (!room.IsOwner(character))
			{
				Log.Debug("CZ_CHANGE_CHATROOM: User '{0}' tried to change a chat room but is not the owner.", conn.Account.Username);
				return;
			}

			room.ChangeSettings(title, limit, privacy, password);

			// For some reason the client requires a separate update for
			// the sender of the settings update, with the exact same
			// information as the room update we have to broadcast anyway.
			Send.ZC_CHANGE_CHATROOM(character, room);
		}

		/// <summary>
		/// Request to create a chat room.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ENTER_ROOM)]
		public void CZ_REQ_ENTER_ROOM(ZoneConnection conn, Packet packet)
		{
			var roomId = packet.GetInt();
			var password = packet.GetString(8);

			var character = conn.GetCurrentCharacter();

			if (!ZoneServer.Instance.World.ChatRooms.TryGet(roomId, out var room))
			{
				Log.Debug("CZ_REQ_ENTER_ROOM: User '{0}' tried to enter a chat room that doesn't exist.", conn.Account.Username);
				return;
			}

			if (room.IsFull)
			{
				Send.ZC_REFUSE_ENTER_ROOM(character, ChatRoomRefuseReason.Full);
				return;
			}

			if (room.IsBanned(character))
			{
				Send.ZC_REFUSE_ENTER_ROOM(character, ChatRoomRefuseReason.Banned);
				return;
			}

			if (room.Privacy == ChatPrivacy.Private && password != room.Password)
			{
				Send.ZC_REFUSE_ENTER_ROOM(character, ChatRoomRefuseReason.WrongPassword);
				return;
			}

			room.AddMember(character);
		}

		/// <summary>
		/// Request from player to exit the current chat room.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_EXIT_ROOM)]
		public void CZ_EXIT_ROOM(ZoneConnection conn, Packet packet)
		{
			var character = conn.GetCurrentCharacter();

			if (character.ChatRoomId == 0)
			{
				Log.Debug("CZ_EXIT_ROOM: User '{0}' tried to exit a chat room but isn't in one.", conn.Account.Username);
				return;
			}

			if (!ZoneServer.Instance.World.ChatRooms.TryGet(character.ChatRoomId, out var room))
			{
				Log.Debug("CZ_EXIT_ROOM: User '{0}' tried to exit a chat room that doesn't exist.", conn.Account.Username);
				return;
			}

			if (!room.IsMember(character))
			{
				Log.Debug("CZ_EXIT_ROOM: User '{0}' tried to exit a chat room but isn't a member.", conn.Account.Username);
				return;
			}

			room.RemoveMember(character, MemberExitReason.Left);
		}

		/// <summary>
		/// Request to switch ownership of a chat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_ROLE_CHANGE)]
		public void CZ_REQ_ROLE_CHANGE(ZoneConnection conn, Packet packet)
		{
			var role = (ChatRoomRole)packet.GetInt();
			var newOwnerName = packet.GetString(Sizes.CharacterNames);

			var character = conn.GetCurrentCharacter();

			// Based on the packet names it can be assumed that more than
			// two roles were intended to be usable at some point, but the
			// client only sends this packet to change ownership now.
			if (role != ChatRoomRole.Owner)
			{
				Log.Debug("CZ_REQ_ROLE_CHANGE: User '{0}' tried to change a chat member's role to an invalid one ({1}).", conn.Account.Username, role);
				return;
			}

			if (character.ChatRoomId == 0)
			{
				Log.Debug("CZ_REQ_ROLE_CHANGE: User '{0}' tried to change chat room ownership but isn't in a chat room.", conn.Account.Username);
				return;
			}

			if (!ZoneServer.Instance.World.ChatRooms.TryGet(character.ChatRoomId, out var room))
			{
				Log.Debug("CZ_REQ_ROLE_CHANGE: User '{0}' tried to change chat room ownership but their chat room doesn't exist.", conn.Account.Username);
				return;
			}

			if (!room.IsOwner(character))
			{
				Log.Debug("CZ_REQ_ROLE_CHANGE: User '{0}' tried to change chat room ownership but is not the owner.", conn.Account.Username);
				return;
			}

			if (!room.IsMember(newOwnerName))
			{
				Log.Debug("CZ_REQ_ROLE_CHANGE: User '{0}' tried to change chat room ownership to '{1}', who isn't in the chat room.", conn.Account.Username, newOwnerName);
				return;
			}

			room.ChangeOwner(newOwnerName);
		}

		/// <summary>
		/// Request to switch ownership of a chat.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_EXPEL_MEMBER)]
		public void CZ_REQ_EXPEL_MEMBER(ZoneConnection conn, Packet packet)
		{
			var memberName = packet.GetString(Sizes.CharacterNames);

			var character = conn.GetCurrentCharacter();

			if (character.ChatRoomId == 0)
			{
				Log.Debug("CZ_REQ_EXPEL_MEMBER: User '{0}' tried to expel member '{1}' but isn't in a chat room.", conn.Account.Username, memberName);
				return;
			}

			if (!ZoneServer.Instance.World.ChatRooms.TryGet(character.ChatRoomId, out var room))
			{
				Log.Debug("CZ_REQ_EXPEL_MEMBER: User '{0}' tried to expel member '{1}' but their chat room doesn't exist.", conn.Account.Username, memberName);
				return;
			}

			if (!room.IsOwner(character))
			{
				Log.Debug("CZ_REQ_EXPEL_MEMBER: User '{0}' tried to expel member '{1}' but is not the owner.", conn.Account.Username, memberName);
				return;
			}

			if (!room.IsMember(memberName))
			{
				Log.Debug("CZ_REQ_EXPEL_MEMBER: User '{0}' tried to expel member '{1}', who isn't in the chat room.", conn.Account.Username, memberName);
				return;
			}

			if (!character.Map.TryGetPlayerByName(memberName, out var memberCharacter))
			{
				Log.Debug("CZ_REQ_EXPEL_MEMBER: User '{0}' tried to expel member '{1}', but they couldn't be found.", conn.Account.Username, memberName);
				return;
			}

			room.RemoveMember(memberCharacter, MemberExitReason.Kicked);
		}

		/// <summary>
		/// Request to create a party, sent when using the /organize
		/// command.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_MAKE_GROUP)]
		public void CZ_MAKE_GROUP(ZoneConnection conn, Packet packet)
		{
			var partyName = packet.GetString(Sizes.PartyNames);

			var character = conn.GetCurrentCharacter();

			if (NV_BASIC.TryFail(character, BasicSkillAbility.CreateParty))
				return;

			character.ServerMessage(Localization.Get("This feature has not been implemented yet."));

			//Send.ZC_ACK_MAKE_GROUP(character, PartyCreationResult.Success);
		}

		/// <summary>
		/// Request to leave the current party.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_REQ_LEAVE_GROUP)]
		public void CZ_REQ_LEAVE_GROUP(ZoneConnection conn, Packet packet)
		{
		}
	}
}
