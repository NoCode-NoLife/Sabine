namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion500()
		{
			ChangeSize(Op.ZC_NOTIFY_STANDENTRY, 54);
			ChangeSize(Op.ZC_NOTIFY_NEWENTRY, 53);
			ChangeSize(Op.ZC_NOTIFY_ACTENTRY, 58);
			ChangeSize(Op.ZC_NOTIFY_MOVEENTRY, 60);
			ChangeSize(Op.ZC_RESURRECTION, 8);
			ChangeSize(Op.CZ_REQ_GIVE_MANNER_POINT, 9);
			ChangeSize(Op.ZC_MSG_STATE_CHANGE, 9);

			Register(Op.ZC_NOTIFY_MAPPROPERTY, 0x0199, 4);
			Register(Op.ZC_NOTIFY_RANKING, 0x019A, 14);
			Register(Op.ZC_NOTIFY_EFFECT, 0x019B, 10);
			Register(Op.CZ_LOCALBROADCAST, 0x019C, 4);
			Register(Op.CZ_CHANGE_EFFECTSTATE, 0x019D, 6);
			Register(Op.ZC_START_CAPTURE, 0x019E, 2);
			Register(Op.CZ_TRYCAPTURE_MONSTER, 0x019F, 6);
			Register(Op.ZC_TRYCAPTURE_MONSTER, 0x01A0, 3);
			Register(Op.CZ_COMMAND_PET, 0x01A1, 3);
			Register(Op.ZC_PROPERTY_PET, 0x01A2, 35);
			Register(Op.ZC_FEED_PET, 0x01A3, 5);
			Register(Op.ZC_CHANGESTATE_PET, 0x01A4, 11);
			Register(Op.CZ_RENAME_PET, 0x01A5, 26);
			Register(Op.ZC_PETEGG_LIST, 0x01A6, -1);
			Register(Op.CZ_SELECT_PETEGG, 0x01A7, 4);
			Register(Op.CZ_PETEGG_INFO, 0x01A8, 4);
			Register(Op.CZ_PET_ACT, 0x01A9, 6);
			Register(Op.ZC_PET_ACT, 0x01AA, 10);
			Register(Op.ZC_PAR_CHANGE_USER, 0x01AB, 12);
			Register(Op.ZC_SKILL_UPDATE, 0x01AC, 6);
			Register(Op.ZC_MAKINGARROW_LIST, 0x01AD, -1);
			Register(Op.CZ_REQ_MAKINGARROW, 0x01AE, 4);
			Register(Op.CZ_REQ_CHANGECART, 0x01AF, 4);
			Register(Op.ZC_NPCSPRITE_CHANGE, 0x01B0, 11);
			Register(Op.ZC_SHOWDIGIT, 0x01B1, 7);
			Register(Op.CZ_REQ_OPENSTORE2, 0x01B2, -1);
			Register(Op.ZC_SHOW_IMAGE2, 0x01B3, 67);
			Register(Op.ZC_CHANGE_GUILD, 0x01B4, 12);
			Register(Op.SC_BILLING_INFO, 0x01B5, 18);
			Register(Op.ZC_GUILD_INFO2, 0x01B6, 114);
			Register(Op.CZ_GUILD_ZENY, 0x01B7, 6);
			Register(Op.ZC_GUILD_ZENY_ACK, 0x01B8, 3);
			Register(Op.ZC_DISPEL, 0x01B9, 6);
			Register(Op.CZ_REMOVE_AID, 0x01BA, 26);
			Register(Op.CZ_SHIFT, 0x01BB, 26);
			Register(Op.CZ_RECALL, 0x01BC, 26);
			Register(Op.CZ_RECALL_GID, 0x01BD, 26);
			Register(Op.AC_ASK_PNGAMEROOM, 0x01BE, 2);
			Register(Op.CA_REPLY_PNGAMEROOM, 0x01BF, 3);
			Register(Op.CZ_REQ_REMAINTIME, 0x01C0, 2);
			Register(Op.ZC_REPLY_REMAINTIME, 0x01C1, 14);
			Register(Op.ZC_INFO_REMAINTIME, 0x01C2, 10);
			Register(Op.ZC_BROADCAST2, 0x01C3, -1);
			Register(Op.ZC_ADD_ITEM_TO_STORE2, 0x01C4, 22);
			Register(Op.ZC_ADD_ITEM_TO_CART2, 0x01C5, 22);
			Register(Op.CS_REQ_ENCRYPTION, 0x01C6, 4);
			Register(Op.SC_ACK_ENCRYPTION, 0x01C7, 2);
			Register(Op.ZC_USE_ITEM_ACK2, 0x01C8, 13);
			Register(Op.ZC_SKILL_ENTRY2, 0x01C9, 97);
			// CZ_REQMAKINGHOMUN, 0x01CA
			Register(Op.CZ_MONSTER_TALK, 0x01CB, 9);
			Register(Op.ZC_MONSTER_TALK, 0x01CC, 9);
			// ZC_AUTOSPELLLIST, 0x01CD
			// CZ_SELECTAUTOSPELL, 0x01CE
			// ZC_DEVOTIONLIST, 0x01CF
			// ZC_SPIRITS, 0x01D0
			// ZC_BLADESTOP, 0x01D1
			// ZC_COMBODELAY, 0x01D2
			Register(Op.ZC_SOUND, 0x01D3, 35);
			Register(Op.ZC_OPEN_EDITDLGSTR, 0x01D4, 6);
			Register(Op.CZ_INPUT_EDITDLGSTR, 0x01D5, 8);
			Register(Op.ZC_NOTIFY_MAPPROPERTY2, 0x01D6, 4);
			Register(Op.ZC_SPRITE_CHANGE2, 0x01D7, 11);
			Register(Op.ZC_NOTIFY_STANDENTRY2, 0x01D8, 54);
			Register(Op.ZC_NOTIFY_NEWENTRY2, 0x01D9, 53);
			Register(Op.ZC_NOTIFY_MOVEENTRY2, 0x01DA, 60);
			Register(Op.CA_REQ_HASH, 0x01DB, 2);
			Register(Op.AC_ACK_HASH, 0x01DC, -1);
			Register(Op.CA_LOGIN2, 0x01DD, 47);
			Register(Op.ZC_NOTIFY_SKILL2, 0x01DE, 33);
			Register(Op.CZ_REQ_ACCOUNTNAME, 0x01DF, 6);
			Register(Op.ZC_ACK_ACCOUNTNAME, 0x01E0, 30);
			// ZC_NOTIFY_EFFECT2, 0x01E0
			Register(Op.ZC_REQ_COUPLE, 0x01E2, 34);
			Register(Op.CZ_JOIN_COUPLE, 0x01E3, 14);
			Register(Op.ZC_START_COUPLE, 0x01E4, 2);
			Register(Op.CZ_REQ_JOIN_COUPLE, 0x01E5, 6);
			Register(Op.ZC_COUPLENAME, 0x01E6, 26);
			Register(Op.CZ_DORIDORI, 0x01E7, 2);
			Register(Op.CZ_MAKE_GROUP2, 0x01E8, 28);
			Register(Op.ZC_ADD_MEMBER_TO_GROUP2, 0x01E9, 81);
			Register(Op.ZC_CONGRATULATION, 0x01EA, 6);
			Register(Op.ZC_NOTIFY_POSITION_TO_GUILDM, 0x01EB, 10);
			Register(Op.ZC_GUILD_MEMBER_MAP_CHANGE, 0x01EC, 26);
			Register(Op.CZ_CHOPOKGI, 0x01ED, 2);
			Register(Op.ZC_NORMAL_ITEMLIST2, 0x01EE, -1);
			Register(Op.ZC_CART_NORMAL_ITEMLIST2, 0x01EF, -1);
			Register(Op.ZC_STORE_NORMAL_ITEMLIST2, 0x01F0, -1);
			Register(Op.AC_NOTIFY_ERROR, 0x01F1, -1);
			Register(Op.ZC_UPDATE_CHARSTAT2, 0x01F2, 20);
			Register(Op.ZC_NOTIFY_EFFECT2, 0x01F3, 10);
			Register(Op.ZC_REQ_EXCHANGE_ITEM2, 0x01F4, 32);
			Register(Op.ZC_ACK_EXCHANGE_ITEM2, 0x01F5, 9);
			Register(Op.ZC_REQ_BABY, 0x01F6, 34);
			Register(Op.CZ_JOIN_BABY, 0x01F7, 14);
			Register(Op.ZC_START_BABY, 0x01F8, 2);
			Register(Op.CZ_REQ_JOIN_BABY, 0x01F9, 6);
			Register(Op.CA_LOGIN3, 0x01FA, 48);
			Register(Op.CH_DELETE_CHAR2, 0x01FB, 56);
			Register(Op.ZC_REPAIRITEMLIST, 0x01FC, -1);
			Register(Op.CZ_REQ_ITEMREPAIR, 0x01FD, 4);
			Register(Op.ZC_ACK_ITEMREPAIR, 0x01FE, 5);
			Register(Op.ZC_HIGHJUMP, 0x01FF, 10);
		}
	}
}
