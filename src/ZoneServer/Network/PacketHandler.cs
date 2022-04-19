using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Extensions;
using Sabine.Shared.Network;
using Sabine.Shared.Network.Helpers;
using Sabine.Shared.World;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Shops;
using Yggdrasil.Logging;

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
			var sessionId = packet.GetInt();
			var characterId = packet.GetInt();
			var accountId = packet.GetInt();
			var sex = packet.GetByte();

			var account = ZoneServer.Instance.Database.GetAccountById(accountId);
			if (account == null)
			{
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
				Log.Warning("CZ_ENTER: Map '{0}' not found for character '{1}'.", character.MapId, character.Name);

				var fallbackLocation = new Location(100036, 99, 81); // "prt_vilg02"

				if (!ZoneServer.Instance.World.Maps.TryGet(fallbackLocation.MapId, out map))
				{
					Log.Warning("CZ_ENTER: Fallback map not found either! Abort! Abort!!!");

					conn.Close();
					return;
				}

				character.SetLocation(fallbackLocation);
			}

			conn.Account = account;
			conn.Character = character;
			character.Connection = conn;

			Send.ZC_ACCEPT_ENTER(conn, character);

			map.AddCharacter(character);

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
				character.FinalizeWarp();
			else
				character.StartObserving();

			Send.ZC_STATUS(character);
			Send.ZC_PAR_CHANGE(character, ParameterType.Weight);
			Send.ZC_PAR_CHANGE(character, ParameterType.WeightMax);
			Send.ZC_PAR_CHANGE(character, ParameterType.SkillPoints);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.BaseExpNeeded);
			Send.ZC_LONGPAR_CHANGE(character, ParameterType.JobExpNeeded);

			var items = character.Inventory.GetItems();
			Send.ZC_NORMAL_ITEMLIST(character, items);
			Send.ZC_EQUIPMENT_ITEMLIST(character, items);
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

			character.MoveTo(toPos);

			// Spawn some NPCs to visualize the path the server calculated
			// for this move request.
			if (character.Vars.Temp.GetBool("Sabine.DebugPathEnabled"))
			{
				var path = character.Map.PathFinder.FindPath(fromPos, toPos);
				foreach (var pathPos in path)
				{
					var npc = new Npc(66);
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

			var character = conn.GetCurrentCharacter();

			if (ZoneServer.Instance.ChatCommands.TryExecute(character, text))
				return;

			text = string.Format("{0} : {1}", character.Name, text);

			Send.ZC_NOTIFY_CHAT(character, text);
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
			var target = ZoneServer.Instance.World.GetPlayerCharacter(a => a.Name == targetName);

			if (target == null)
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
				Log.Debug("CZ_REQNAME: User {0} requested the name of a character that doesn't exist.", conn.Account.Username);
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
			var itemData = SabineData.Items.Find(itemStringId);

			if (itemData == null)
			{
				Log.Warning("CZ_REQ_ITEM_EXPLANATION_BYNAME: Item data for '{0}' not found.", itemStringId);
				return;
			}

			// The client usually identifies items by their string id
			// and converts that to a Korean name to find the assets
			// for the item in the client. This works for the sprites
			// and the name display, but not the description. The
			// client sends the English string id for this request,
			// but if you send that name back, you get an error that
			// it can't find the texture for the item. Because of this,
			// we need to send back the Korean name in this instance.
			// The title of the item description window will be mangled
			// this way, but that's how it has to be.
			var name = itemData.KoreanName;

			// Generate a description. We could put proper descriptions
			// in a database, but this should work for now and it's kind
			// of fun that you can just generate them. It would be good
			// if someone could tell us what descriptions looked liked
			// in the alpha, because the client seems to not have
			// supported line-breaks.
			var sb = new StringBuilder();

			switch (itemData.Type)
			{
				case ItemType.Weapon:
				{
					sb.AppendFormat("Attack:^777777 {0}-{1}^000000", itemData.AttackMin, itemData.AttackMax);
					sb.AppendFormat(", Weight:^777777 {0:0.#}^000000", itemData.Weight / 10f);
					sb.AppendFormat(", Required Level:^777777 {0}^000000", itemData.RequiredLevel);
					sb.AppendFormat(", Jobs:^777777 {0}^000000", itemData.JobsAllowed);
					break;
				}
				case ItemType.Armor:
				{
					sb.AppendFormat(", Defense:^777777 {0}^000000", itemData.Defense);
					sb.AppendFormat(", Weight:^777777 {0:0.#}^000000", itemData.Weight / 10f);
					sb.AppendFormat(", Required Level:^777777 {0}^000000", itemData.RequiredLevel);
					sb.AppendFormat(", Jobs:^777777 {0}^000000", itemData.JobsAllowed);
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

			if (!(target is Npc npc))
			{
				Log.Debug("CZ_CONTACTNPC: User '{0}' tried to contact a non-NPC.", conn.Account.Username);
				return;
			}

			//Log.Debug("CZ_CONTACTNPC: " + npcHandle);

			//Send.ZC_SAY_DIALOG(character, npcHandle, "Hello, World!");
			//Task.Delay(5000).ContinueWith(_ => Send.ZC_SAY_DIALOG(character, npcHandle, "Goodbye, World!"));
			//Task.Delay(6000).ContinueWith(_ => Send.ZC_WAIT_DIALOG(character, npcHandle));
			//Task.Delay(8000).ContinueWith(_ => Send.ZC_MENU_LIST(character, npcHandle, "Option 1", "Option 2", "End"));

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
			var choice = packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var npc = character.Map.GetCharacter(npcHandle);

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
			var targetHandle = packet.GetInt();
			var action = (ActionType)packet.GetByte();

			var character = conn.GetCurrentCharacter();

			character.StopMove();

			switch (action)
			{
				case ActionType.SitDown:
				{
					character.SitDown();
					break;
				}
				case ActionType.StandUp:
				{
					character.StandUp();
					break;
				}
				case ActionType.Attack:
				{
					var target = character.Map.GetCharacter(targetHandle);
					if (target == null)
					{
						Log.Debug("CZ_REQUEST_ACT: User '{0}' tried to attack a character who doesn't exist.", conn.Account.Username);
						return;
					}

					Send.ZC_NOTIFY_ACT(character, character.Handle, target.Handle, DateTime.Now.GetUnixTimestamp(), 10, ActionType.Attack);
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
		/// Notification that the character rotated into a new direction.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CZ_CHANGE_DIRECTION)]
		public void CZ_CHANGE_DIRECTION(ZoneConnection conn, Packet packet)
		{
			var direction = (Direction)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			character.Direction = direction;

			Send.ZC_CHANGE_DIRECTION(character, direction);
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
				// a player should be unable to equip or use item unless
				// something is very wrong.

				Log.Debug("CZ_USE_ITEM: User '{0}' tried to equip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			character.Inventory.DecrementItem(item, 1);

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
			var itemInvId = packet.GetShort();
			var equipSlots = (EquipSlots)packet.GetByte();

			var character = conn.GetCurrentCharacter();
			var item = character.Inventory.GetItem(itemInvId);

			if (item == null)
			{
				// See CZ_USE_ITEM about negative responses.
				Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item they don't have.", conn.Account.Username);
				conn.Close();
				return;
			}

			if (!character.CanEquip(item))
			{
				Log.Debug("CZ_REQ_WEAR_EQUIP: User '{0}' tried to equip an item they can't equip.", conn.Account.Username);
				conn.Close();
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
			var shop = character.Vars.Temp.Get("Sabine.CurrentShop") as NpcShop;

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
	}
}
