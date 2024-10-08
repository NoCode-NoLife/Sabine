﻿namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion100()
		{
			// Reference: iRO Alpha Ragexe.exe, dated 2001-08-30

			// Earliest client we have access to and the current base
			// level for our packet tables. Many packets didn't change
			// too much over the years, but the Alpha and Beta clients
			// currently represent the most drastic differences to newer
			// ones. In particular, they use "string ids" for items,
			// instead of their normal class ids, which makes handling
			// and multi-version support trickier. There are also packets
			// that weren't used anymore in later versions, such as a
			// request for an item's description, which was provided
			// by the server before they were moved to text files in
			// the client.

			Register(Op.CA_LOGIN, 0x0000, 34);
			Register(Op.CH_ENTER, 0x0001, 13);
			Register(Op.CH_SELECT_CHAR, 0x0002, 3);
			Register(Op.CH_MAKE_CHAR, 0x0003, 26);
			Register(Op.CH_DELETE_CHAR, 0x0004, 6);
			Register(Op.AC_ACCEPT_LOGIN, 0x0005, -1);
			Register(Op.AC_REFUSE_LOGIN, 0x0006, 3);
			Register(Op.HC_ACCEPT_ENTER, 0x0007, -1);
			Register(Op.HC_REFUSE_ENTER, 0x0008, 3);
			Register(Op.HC_ACCEPT_MAKECHAR, 0x0009, 76);
			Register(Op.HC_REFUSE_MAKECHAR, 0x000A, 3);
			Register(Op.HC_ACCEPT_DELETECHAR, 0x000B, 2);
			Register(Op.HC_REFUSE_DELETECHAR, 0x000C, 3);
			Register(Op.HC_NOTIFY_ZONESVR, 0x000D, 28);
			Register(Op.CZ_ENTER, 0x000E, 15);
			Register(Op.ZC_ACCEPT_ENTER, 0x000F, 11);
			Register(Op.ZC_REFUSE_ENTER, 0x0010, 3);
			Register(Op.ZC_NOTIFY_INITCHAR, 0x0011, -1); // Unused in alpha
			Register(Op.ZC_NOTIFY_UPDATECHAR, 0x0012, 9); // Unused in alpha
			Register(Op.ZC_NOTIFY_UPDATEPLAYER, 0x0013, 5); // Unused in alpha
			Register(Op.ZC_NOTIFY_STANDENTRY, 0x0014, 19);
			Register(Op.ZC_NOTIFY_NEWENTRY, 0x0015, 18);
			Register(Op.ZC_NOTIFY_ACTENTRY, 0x0016, 23); // Unused in alpha
			Register(Op.ZC_NOTIFY_MOVEENTRY, 0x0017, 25);
			Register(Op.ZC_NOTIFY_STANDENTRY_NPC, 0x0018, 18);
			Register(Op.CZ_NOTIFY_ACTORINIT, 0x0019, 2);
			Register(Op.CZ_REQUEST_TIME, 0x001A, 2);
			Register(Op.ZC_NOTIFY_TIME, 0x001B, 6);
			Register(Op.ZC_NOTIFY_VANISH, 0x001C, 7);
			Register(Op.SC_NOTIFY_BAN, 0x001D, 3);
			Register(Op.CZ_REQUEST_QUIT, 0x001E, 2); // Presumably unused in alpha
			Register(Op.ZC_ACCEPT_QUIT, 0x001F, 2); // Unused in alpha
			Register(Op.ZC_REFUSE_QUIT, 0x0020, 2); // Unused in alpha
			Register(Op.CZ_REQUEST_MOVE, 0x0021, 5);
			Register(Op.ZC_NOTIFY_MOVE, 0x0022, 16);
			Register(Op.ZC_NOTIFY_PLAYERMOVE, 0x0023, 12);
			Register(Op.ZC_STOPMOVE, 0x0024, 10);
			Register(Op.CZ_REQUEST_ACT, 0x0025, 7);
			Register(Op.ZC_NOTIFY_ACT, 0x0026, 17);
			Register(Op.CZ_REQUEST_CHAT, 0x0027, -1);
			Register(Op.ZC_NOTIFY_CHAT, 0x0028, -1);
			Register(Op.ZC_NOTIFY_PLAYERCHAT, 0x0029, -1);
			Register(Op.SERVER_ENTRY_ACK, 0x002A, -1); // Not in client's packet table
			Register(Op.CZ_CONTACTNPC, 0x002B, 7);
			Register(Op.ZC_NPCACK_MAPMOVE, 0x002C, 22);
			Register(Op.ZC_NPCACK_SERVERMOVE, 0x002D, 28);
			Register(Op.ZC_NPCACK_ENABLE, 0x002E, 2);
			Register(Op.CZ_REQNAME, 0x002F, 6);
			Register(Op.ZC_ACK_REQNAME, 0x0030, 38);
			Register(Op.CZ_WHISPER, 0x0031, -1);
			Register(Op.ZC_WHISPER, 0x0032, -1);
			Register(Op.ZC_ACK_WHISPER, 0x0033, 3);
			Register(Op.CZ_BROADCAST, 0x0034, -1);
			Register(Op.ZC_BROADCAST, 0x0035, -1);
			Register(Op.CZ_CHANGE_DIRECTION, 0x0036, 3);
			Register(Op.ZC_CHANGE_DIRECTION, 0x0037, 7);
			Register(Op.ZC_ITEM_ENTRY, 0x0038, 30);
			Register(Op.ZC_ITEM_FALL_ENTRY, 0x0039, 30);
			Register(Op.CZ_ITEM_PICKUP, 0x003A, 6);
			Register(Op.ZC_ITEM_PICKUP_ACK, 0x003B, 25);
			Register(Op.ZC_ITEM_DISAPPEAR, 0x003C, 6);
			Register(Op.CZ_ITEM_THROW, 0x003D, 6);
			Register(Op.ZC_NORMAL_ITEMLIST, 0x003E, -1);
			Register(Op.ZC_EQUIPMENT_ITEMLIST, 0x003F, -1);
			Register(Op.ZC_STORE_NORMAL_ITEMLIST, 0x0040, -1);
			Register(Op.ZC_STORE_EQUIPMENT_ITEMLIST, 0x0041, -1);
			Register(Op.CZ_USE_ITEM, 0x0042, 8);
			Register(Op.ZC_USE_ITEM_ACK, 0x0043, 7);
			Register(Op.CZ_REQ_WEAR_EQUIP, 0x0044, 5);
			Register(Op.ZC_REQ_WEAR_EQUIP_ACK, 0x0045, 6);
			Register(Op.CZ_REQ_TAKEOFF_EQUIP, 0x0046, 4);
			Register(Op.ZC_REQ_TAKEOFF_EQUIP_ACK, 0x0047, 6);
			Register(Op.CZ_REQ_ITEM_EXPLANATION_BYNAME, 0x0048, 18);
			Register(Op.ZC_REQ_ITEM_EXPLANATION_ACK, 0x0049, -1);
			Register(Op.ZC_ITEM_THROW_ACK, 0x004A, 6);
			Register(Op.ZC_PAR_CHANGE, 0x004B, 6);
			Register(Op.ZC_LONGPAR_CHANGE, 0x004C, 8);
			Register(Op.CZ_RESTART, 0x004D, 3);
			Register(Op.ZC_RESTART_ACK, 0x004E, 3);
			Register(Op.ZC_SAY_DIALOG, 0x004F, -1);
			Register(Op.ZC_WAIT_DIALOG, 0x0050, 6);
			Register(Op.ZC_CLOSE_DIALOG, 0x0051, 6);
			Register(Op.ZC_MENU_LIST, 0x0052, -1);
			Register(Op.CZ_CHOOSE_MENU, 0x0053, 7);
			Register(Op.CZ_REQ_NEXT_SCRIPT, 0x0054, 6);
			Register(Op.CZ_REQ_STATUS, 0x0055, 2);
			Register(Op.CZ_STATUS_CHANGE, 0x0056, 5);
			Register(Op.ZC_STATUS_CHANGE_ACK, 0x0057, 6);
			Register(Op.ZC_STATUS, 0x0058, 20);
			Register(Op.ZC_STATUS_CHANGE, 0x0059, 5);
			Register(Op.CZ_REQ_EMOTION, 0x005A, 3);
			Register(Op.ZC_EMOTION, 0x005B, 7);
			Register(Op.CZ_REQ_USER_COUNT, 0x005C, 2);
			Register(Op.ZC_USER_COUNT, 0x005D, 6);
			Register(Op.ZC_SPRITE_CHANGE, 0x005E, 8);
			Register(Op.ZC_SELECT_DEALTYPE, 0x005F, 6);
			Register(Op.CZ_ACK_SELECT_DEALTYPE, 0x0060, 7);
			Register(Op.ZC_PC_PURCHASE_ITEMLIST, 0x0061, -1);
			Register(Op.ZC_PC_SELL_ITEMLIST, 0x0062, -1);
			Register(Op.CZ_PC_PURCHASE_ITEMLIST, 0x0063, -1);
			Register(Op.CZ_PC_SELL_ITEMLIST, 0x0064, -1);
			Register(Op.ZC_PC_PURCHASE_RESULT, 0x0065, 3);
			Register(Op.ZC_PC_SELL_RESULT, 0x0066, 3);
			Register(Op.CZ_DISCONNECT_CHARACTER, 0x0067, 18);
			Register(Op.ZC_ACK_DISCONNECT_CHARACTER, 0x0068, 3);
			Register(Op.CZ_DISCONNECT_ALL_CHARACTER, 0x0069, 2);
			Register(Op.CZ_SETTING_WHISPER_PC, 0x006A, 19);
			Register(Op.CZ_SETTING_WHISPER_STATE, 0x006B, 3);
			Register(Op.ZC_SETTING_WHISPER_PC, 0x006C, 4);
			Register(Op.ZC_SETTING_WHISPER_STATE, 0x006D, 4);
			Register(Op.CZ_REQ_WHISPER_LIST, 0x006E, 2);
			Register(Op.ZC_WHISPER_LIST, 0x006F, -1);
			Register(Op.CZ_CREATE_CHATROOM, 0x0070, -1);
			Register(Op.ZC_ACK_CREATE_CHATROOM, 0x0071, 3);
			Register(Op.ZC_ROOM_NEWENTRY, 0x0072, -1);
			Register(Op.ZC_DESTROY_ROOM, 0x0073, 6);
			Register(Op.CZ_REQ_ENTER_ROOM, 0x0074, 14);
			Register(Op.ZC_REFUSE_ENTER_ROOM, 0x0075, 3);
			Register(Op.ZC_ENTER_ROOM, 0x0076, -1);
			Register(Op.ZC_MEMBER_NEWENTRY, 0x0077, 20);
			Register(Op.ZC_MEMBER_EXIT, 0x0078, 21);
			Register(Op.CZ_CHANGE_CHATROOM, 0x0079, -1);
			Register(Op.ZC_CHANGE_CHATROOM, 0x007A, -1);
			Register(Op.CZ_REQ_ROLE_CHANGE, 0x007B, 22);
			Register(Op.ZC_ROLE_CHANGE, 0x007C, 22);
			Register(Op.CZ_REQ_EXPEL_MEMBER, 0x007D, 18);
			Register(Op.CZ_EXIT_ROOM, 0x007E, 2);
			Register(Op.CZ_REQ_EXCHANGE_ITEM, 0x007F, 6);
			Register(Op.ZC_REQ_EXCHANGE_ITEM, 0x0080, 18);
			Register(Op.CZ_ACK_EXCHANGE_ITEM, 0x0081, 3);
			Register(Op.ZC_ACK_EXCHANGE_ITEM, 0x0082, 3);
			Register(Op.CZ_ADD_EXCHANGE_ITEM, 0x0083, 8);
			Register(Op.ZC_ADD_EXCHANGE_ITEM, 0x0084, 22);
			Register(Op.ZC_ACK_ADD_EXCHANGE_ITEM, 0x0085, 5);
			Register(Op.CZ_CONCLUDE_EXCHANGE_ITEM, 0x0086, 2);
			Register(Op.ZC_CONCLUDE_EXCHANGE_ITEM, 0x0087, 3);
			Register(Op.CZ_CANCEL_EXCHANGE_ITEM, 0x0088, 2);
			Register(Op.ZC_CANCEL_EXCHANGE_ITEM, 0x0089, 2);
			Register(Op.CZ_EXEC_EXCHANGE_ITEM, 0x008A, 2);
			Register(Op.ZC_EXEC_EXCHANGE_ITEM, 0x008B, 3);
			Register(Op.ZC_EXCHANGEITEM_UNDO, 0x008C, 2);
			Register(Op.ZC_NOTIFY_STOREITEM_COUNTINFO, 0x008D, 6);
			Register(Op.CZ_MOVE_ITEM_FROM_BODY_TO_STORE, 0x008E, 8);
			Register(Op.ZC_ADD_ITEM_TO_STORE, 0x008F, 24);
			Register(Op.CZ_MOVE_ITEM_FROM_STORE_TO_BODY, 0x0090, 8);
			Register(Op.ZC_DELETE_ITEM_FROM_STORE, 0x0091, 8);
			Register(Op.CZ_CLOSE_STORE, 0x0092, 2);
			Register(Op.ZC_CLOSE_STORE, 0x0093, 2);
			Register(Op.CZ_MAKE_GROUP, 0x0094, 22);
			Register(Op.ZC_ACK_MAKE_GROUP, 0x0095, 3);
			Register(Op.ZC_GROUP_LIST, 0x0096, -1);
			Register(Op.CZ_REQ_JOIN_GROUP, 0x0097, 6);
			Register(Op.ZC_ACK_REQ_JOIN_GROUP, 0x0098, 27);
			Register(Op.ZC_REQ_JOIN_GROUP, 0x0099, 30);
			Register(Op.CZ_JOIN_GROUP, 0x009A, 10);
			Register(Op.CZ_REQ_LEAVE_GROUP, 0x009B, 2);
			Register(Op.CZ_REQ_EXPEL_GROUP_MEMBER, 0x009C, 30);
			Register(Op.ZC_ADD_MEMBER_TO_GROUP, 0x009D, 79);
			Register(Op.ZC_DELETE_MEMBER_FROM_GROUP, 0x009E, 31);
			Register(Op.ZC_NOTIFY_HP_TO_GROUPM, 0x009F, 10);
			Register(Op.ZC_NOTIFY_POSITION_TO_GROUPM, 0x00A0, 10);
			Register(Op.CZ_REQUEST_CHAT_PARTY, 0x00A1, -1);
			Register(Op.ZC_NOTIFY_CHAT_PARTY, 0x00A2, -1);
			Register(Op.ZC_MVP_GETTING_ITEM, 0x00A3, 18);
			Register(Op.ZC_MVP_GETTING_SPECIAL_EXP, 0x00A4, 6);
			Register(Op.ZC_MVP, 0x00A5, 6);
			Register(Op.ZC_THROW_MVPITEM, 0x00A6, 2);
			Register(Op.ZC_SKILLINFO_UPDATE, 0x00A7, 9);
			Register(Op.ZC_SKILLINFO_LIST, 0x00A8, -1);
			Register(Op.ZC_ACK_TOUSESKILL, 0x00A9, 6);
			Register(Op.ZC_ADD_SKILL, 0x00AA, 33);
		}
	}
}
