namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadBeta2()
		{
			ShiftAll(0x64);

			ChangeSize(Op.CA_LOGIN, 55);
			ChangeSize(Op.CH_MAKE_CHAR, 37);
			ChangeSize(Op.AC_REFUSE_LOGIN, 23);
			ChangeSize(Op.HC_ACCEPT_MAKECHAR, 108);
			ChangeSize(Op.ZC_NOTIFY_STANDENTRY, 52);
			ChangeSize(Op.ZC_NOTIFY_NEWENTRY, 51);
			ChangeSize(Op.ZC_NOTIFY_ACTENTRY, 56);
			ChangeSize(Op.ZC_NOTIFY_MOVEENTRY, 58);
			ChangeSize(Op.ZC_NOTIFY_STANDENTRY_NPC, 41);
			ChangeSize(Op.ZC_NOTIFY_ACT, 29);
			ChangeSize(Op.ZC_ACK_REQNAME, 30);
			ChangeSize(Op.CZ_CHANGE_DIRECTION, 5);
			ChangeSize(Op.ZC_CHANGE_DIRECTION, 9);
			ChangeSize(Op.ZC_ITEM_ENTRY, 17);
			ChangeSize(Op.ZC_ITEM_FALL_ENTRY, 17);
			ChangeSize(Op.ZC_ITEM_PICKUP_ACK, 23);
			ChangeSize(Op.CZ_REQ_WEAR_EQUIP, 6);
			ChangeSize(Op.ZC_REQ_WEAR_EQUIP_ACK, 7);
			ChangeSize(Op.ZC_REQ_TAKEOFF_EQUIP_ACK, 7);
			ChangeSize(Op.ZC_PAR_CHANGE, 8);
			ChangeSize(Op.CZ_DISCONNECT_CHARACTER, 6);
			ChangeSize(Op.CZ_REQ_EXPEL_MEMBER, 26);
			ChangeSize(Op.ZC_ADD_EXCHANGE_ITEM, 19);
			ChangeSize(Op.ZC_ADD_ITEM_TO_STORE, 21);
			ChangeSize(Op.ZC_MVP_GETTING_ITEM, 4);
			ChangeSize(Op.ZC_SKILLINFO_UPDATE, 11);
			ChangeSize(Op.ZC_ACK_TOUSESKILL, 10);
			ChangeSize(Op.ZC_WARPLIST, 68);
			ChangeSize(Op.ZC_SKILL_ENTRY, 16);
			ChangeSize(Op.ZC_ADD_ITEM_TO_CART, 21);
			ChangeSize(Op.CZ_MOVETO_MAP, 22);

			// The packet CZ_REQ_ITEM_EXPLANATION_BYNAME was removed from
			// the table, though this didn't affect the ops.

			//Register(?, 0x0000, 10); // It's in the packet table, but is it used?
			Register(Op.ZC_COUPLESTATUS, 0x0141, 14);
			Register(Op.ZC_OPEN_EDITDLG, 0x0142, 6);
			Register(Op.CZ_INPUT_EDITDLG, 0x0143, 10);
			Register(Op.ZC_COMPASS, 0x0144, 23);
			Register(Op.ZC_SHOW_IMAGE, 0x0145, 19);
			Register(Op.CZ_CLOSE_DIALOG, 0x0146, 6);
			Register(Op.ZC_AUTORUN_SKILL, 0x0147, 39);
			Register(Op.ZC_RESURRECTION, 0x0148, 6);
			Register(Op.CZ_REQ_GIVE_MANNER_POINT, 0x0149, 7);
			Register(Op.ZC_ACK_GIVE_MANNER_POINT, 0x014A, 6);
			Register(Op.ZC_NOTIFY_MANNER_POINT_GIVEN, 0x014B, 27);
			Register(Op.ZC_MYGUILD_BASIC_INFO, 0x014C, -1);
			Register(Op.CZ_REQ_GUILD_MENUINTERFACE, 0x014D, 2);
			Register(Op.ZC_ACK_GUILD_MENUINTERFACE, 0x014E, 6);
			Register(Op.CZ_REQ_GUILD_MENU, 0x014F, 6);
			Register(Op.ZC_GUILD_INFO, 0x0150, 110);
			Register(Op.CZ_REQ_GUILD_EMBLEM_IMG, 0x0151, 6);
			Register(Op.ZC_GUILD_EMBLEM_IMG, 0x0152, -1);
			Register(Op.CZ_REGISTER_GUILD_EMBLEM_IMG, 0x0153, -1);
			Register(Op.ZC_MEMBERMGR_INFO, 0x0154, -1);
			Register(Op.CZ_REQ_CHANGE_MEMBERPOS, 0x0155, -1);
			Register(Op.ZC_ACK_REQ_CHANGE_MEMBERS, 0x0156, -1);
			Register(Op.CZ_REQ_OPEN_MEMBER_INFO, 0x0157, 6);
			Register(Op.ZC_ACK_OPEN_MEMBER_INFO, 0x0158, -1);
			Register(Op.CZ_REQ_LEAVE_GUILD, 0x0159, 54);
			Register(Op.ZC_ACK_LEAVE_GUILD, 0x015A, 66);
			Register(Op.CZ_REQ_BAN_GUILD, 0x015B, 54);
			Register(Op.ZC_ACK_BAN_GUILD, 0x015C, 90);
			Register(Op.CZ_REQ_DISORGANIZE_GUILD, 0x015D, 42);
			Register(Op.ZC_ACK_DISORGANIZE_GUILD_RESULT, 0x015E, 6);
			Register(Op.ZC_ACK_DISORGANIZE_GUILD, 0x015F, 42);
			Register(Op.ZC_POSITION_INFO, 0x0160, -1);
			Register(Op.CZ_REG_CHANGE_GUILD_POSITIONINFO, 0x0161, -1);
			Register(Op.ZC_GUILD_SKILLINFO, 0x0162, -1);
			Register(Op.ZC_BAN_LIST, 0x0163, -1);
			Register(Op.ZC_OTHER_GUILD_LIST, 0x0164, -1);
			Register(Op.CZ_REQ_MAKE_GUILD, 0x0165, 30);
			Register(Op.ZC_POSITION_ID_NAME_INFO, 0x0166, -1);
			Register(Op.ZC_RESULT_MAKE_GUILD, 0x0167, 3);
			Register(Op.CZ_REQ_JOIN_GUILD, 0x0168, 14);
			Register(Op.ZC_ACK_REQ_JOIN_GUILD, 0x0169, 3);
			Register(Op.ZC_REQ_JOIN_GUILD, 0x016A, 30);
			Register(Op.CZ_JOIN_GUILD, 0x016B, 10);
			Register(Op.ZC_UPDATE_GDID, 0x016C, 43);
			Register(Op.ZC_UPDATE_CHARSTAT, 0x016D, 14);
			Register(Op.CZ_GUILD_NOTICE, 0x016E, 186);
			Register(Op.ZC_GUILD_NOTICE, 0x016F, 182);
			Register(Op.CZ_REQ_ALLY_GUILD, 0x0170, 14);
			Register(Op.ZC_REQ_ALLY_GUILD, 0x0171, 30);
			Register(Op.CZ_ALLY_GUILD, 0x0172, 10);
			Register(Op.ZC_ACK_REQ_ALLY_GUILD, 0x0173, 3);
			Register(Op.ZC_ACK_CHANGE_GUILD_POSITIONINFO, 0x0174, -1);
			Register(Op.CZ_REQ_GUILD_MEMBER_INFO, 0x0175, 6);
			Register(Op.ZC_ACK_GUILD_MEMBER_INFO, 0x0176, 106);
			Register(Op.ZC_ITEMIDENTIFY_LIST, 0x0177, -1);
			Register(Op.CZ_REQ_ITEMIDENTIFY, 0x0178, 4);
			Register(Op.ZC_ACK_ITEMIDENTIFY, 0x0179, 5);
			Register(Op.CZ_REQ_ITEMCOMPOSITION_LIST, 0x017A, 4);
			Register(Op.ZC_ITEMCOMPOSITION_LIST, 0x017B, -1);
			Register(Op.CZ_REQ_ITEMCOMPOSITION, 0x017C, 6);
			Register(Op.ZC_ACK_ITEMCOMPOSITION, 0x017D, 7);
			Register(Op.CZ_GUILD_CHAT, 0x017E, -1);
			Register(Op.ZC_GUILD_CHAT, 0x017F, -1);
			Register(Op.CZ_REQ_HOSTILE_GUILD, 0x0180, 6);
			Register(Op.ZC_ACK_REQ_HOSTILE_GUILD, 0x0181, 3);
			Register(Op.ZC_MEMBER_ADD, 0x0182, 106);
			Register(Op.CZ_REQ_DELETE_RELATED_GUILD, 0x0183, 10);
			Register(Op.ZC_DELETE_RELATED_GUILD, 0x0184, 10);
			Register(Op.ZC_ADD_RELATED_GUILD, 0x0185, 34);
			Register(Op.COLLECTORDEAD, 0x0186, -1);
			Register(Op.PING, 0x0187, 6);
			Register(Op.ZC_ACK_ITEMREFINING, 0x0188, 8);
			Register(Op.ZC_NOTIFY_MAPINFO, 0x0189, 4);
			Register(Op.CZ_REQ_DISCONNECT, 0x018A, 4);
			Register(Op.ZC_ACK_REQ_DISCONNECT, 0x018B, 4);
			Register(Op.ZC_MONSTER_INFO, 0x018C, 29);
			Register(Op.ZC_MAKABLEITEMLIST, 0x018D, -1);
			Register(Op.CZ_REQMAKINGITEM, 0x018E, 10);
			Register(Op.ZC_ACK_REQMAKINGITEM, 0x018F, 6);
			Register(Op.CZ_USE_SKILL_TOGROUND_WITHTALKBOX, 0x0190, 90);
			Register(Op.ZC_TALKBOX_CHATCONTENTS, 0x0191, 86);
			Register(Op.ZC_UPDATE_MAPINFO, 0x0192, 24);
			Register(Op.CZ_REQNAME_BYGID, 0x0193, 6);
			Register(Op.ZC_ACK_REQNAME_BYGID, 0x0194, 30);
			Register(Op.ZC_ACK_REQNAMEALL, 0x0195, 102);
			Register(Op.ZC_MSG_STATE_CHANGE, 0x0196, 8);
			Register(Op.CZ_RESET, 0x0197, 4);
			Register(Op.CZ_CHANGE_MAPTYPE, 0x0198, 8);
		}
	}
}
