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
			// ZC_NOTIFY_EFFECT2, 0x01E1
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
