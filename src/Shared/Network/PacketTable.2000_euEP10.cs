namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion2000()
		{
			// Reference: euRO EP10 Ragexe.exe, dated 2007-03-05
			// Roughly equivalent to 2007-01-02aSakexe and eA20

			ChangeSize(Op.CZ_ENTER, 26);
			ChangeSize(Op.CZ_REQUEST_TIME, 8);
			ChangeSize(Op.CZ_REQUEST_MOVE, 8);
			ChangeSize(Op.CZ_REQUEST_ACT, -1);
			ChangeSize(Op.CZ_REQNAME, 11);
			ChangeSize(Op.CZ_CHANGE_DIRECTION, 11);
			ChangeSize(Op.CZ_ITEM_PICKUP, 8);
			ChangeSize(Op.CZ_ITEM_THROW, 10);
			ChangeSize(Op.CZ_USE_ITEM, 14);
			ChangeSize(Op.CZ_MOVE_ITEM_FROM_BODY_TO_STORE, 14);
			ChangeSize(Op.CZ_MOVE_ITEM_FROM_STORE_TO_BODY, 22);
			ChangeSize(Op.CZ_USE_SKILL, -1);
			ChangeSize(Op.CZ_USE_SKILL_TOGROUND, 22);
			ChangeSize(Op.CZ_USE_SKILL_TOGROUND_WITHTALKBOX, 102);
			ChangeSize(Op.CZ_REQNAME_BYGID, 15);
			ChangeSize(Op.ZC_STARSKILL, 32);

			Register(Op.ZC_TAEKWON_POINT, 0x0224, 10);
			Register(Op.CZ_TAEKWON_RANK, 0x0225, 2);
			Register(Op.ZC_TAEKWON_RANK, 0x0226, 282);
			Register(Op.ZC_GAME_GUARD, 0x0227, 18);
			Register(Op.CZ_ACK_GAME_GUARD, 0x0228, 18);
			Register(Op.ZC_STATE_CHANGE3, 0x0229, 15);
			Register(Op.ZC_NOTIFY_STANDENTRY3, 0x022A, 58);
			Register(Op.ZC_NOTIFY_NEWENTRY3, 0x022B, 57);
			Register(Op.ZC_NOTIFY_MOVEENTRY3, 0x022C, 64);
			Register(Op.CZ_COMMAND_MER, 0x022D, 5);
			Register(Op.ZC_PROPERTY_HOMUN, 0x022E, 71);
			Register(Op.ZC_FEED_MER, 0x022F, 5);
			Register(Op.ZC_CHANGESTATE_MER, 0x0230, 12);
			Register(Op.CZ_RENAME_MER, 0x0231, 26);
			Register(Op.CZ_REQUEST_MOVENPC, 0x0232, 9);
			Register(Op.CZ_REQUEST_ACTNPC, 0x0233, 11);
			Register(Op.CZ_REQUEST_MOVETOOWNER, 0x0234, 6);
			Register(Op.ZC_HOSKILLINFO_LIST, 0x0235, -1);
			Register(Op.ZC_KILLER_POINT, 0x0236, 10);
			Register(Op.CZ_KILLER_RANK, 0x0237, 2);
			Register(Op.ZC_KILLER_RANK, 0x0238, 282);
			Register(Op.ZC_HOSKILLINFO_UPDATE, 0x0239, 11);
			Register(Op.ZC_REQ_STORE_PASSWORD, 0x023A, 4);
			Register(Op.CZ_ACK_STORE_PASSWORD, 0x023B, 36);
			Register(Op.ZC_RESULT_STORE_PASSWORD, 0x023C, 6);
			Register(Op.AC_EVENT_RESULT, 0x023D, 6);
			Register(Op.HC_REQUEST_CHARACTER_PASSWORD, 0x023E, 8);
			Register(Op.CZ_MAIL_GET_LIST, 0x023F, 2);
			Register(Op.ZC_MAIL_REQ_GET_LIST, 0x0240, -1);
			Register(Op.CZ_MAIL_OPEN, 0x0241, 6);
			Register(Op.ZC_MAIL_REQ_OPEN, 0x0242, -1);
			Register(Op.CZ_MAIL_DELETE, 0x0243, 6);
			Register(Op.CZ_MAIL_GET_ITEM, 0x0244, 6);
			Register(Op.ZC_MAIL_REQ_GET_ITEM, 0x0245, 3);
			Register(Op.CZ_MAIL_RESET_ITEM, 0x0246, 4);
			Register(Op.CZ_MAIL_ADD_ITEM, 0x0247, 8);
			Register(Op.CZ_MAIL_SEND, 0x0248, -1);
			Register(Op.ZC_MAIL_REQ_SEND, 0x0249, 3);
			Register(Op.ZC_MAIL_RECEIVE, 0x024A, 70);
			Register(Op.CZ_AUCTION_CREATE, 0x024B, 4);
			Register(Op.CZ_AUCTION_ADD_ITEM, 0x024C, 8);
			Register(Op.CZ_AUCTION_ADD, 0x024D, 12);
			Register(Op.CZ_AUCTION_ADD_CANCEL, 0x024E, 6);
			Register(Op.CZ_AUCTION_BUY, 0x024F, 10);
			Register(Op.ZC_AUCTION_RESULT, 0x0250, 3);
			Register(Op.CZ_AUCTION_ITEM_SEARCH, 0x0251, 34);
			Register(Op.ZC_AUCTION_ITEM_REQ_SEARCH, 0x0252, -1);
			Register(Op.ZC_STARPLACE, 0x0253, 3);
			Register(Op.CZ_AGREE_STARPLACE, 0x0254, 3);
			Register(Op.ZC_ACK_MAIL_ADD_ITEM, 0x0255, 5);
			Register(Op.ZC_ACK_AUCTION_ADD_ITEM, 0x0256, 5);
			Register(Op.ZC_ACK_MAIL_DELETE, 0x0257, 8);
			Register(Op.CA_REQ_GAME_GUARD_CHECK, 0x0258, 2);
			Register(Op.AC_ACK_GAME_GUARD, 0x0259, 3);
			Register(Op.ZC_MAKINGITEM_LIST, 0x025A, -1);
			Register(Op.CZ_REQ_MAKINGITEM, 0x025B, 6);
			Register(Op.CZ_AUCTION_REQ_MY_INFO, 0x025C, 4);
			Register(Op.CZ_AUCTION_REQ_MY_SELL_STOP, 0x025D, 6);
			Register(Op.ZC_AUCTION_ACK_MY_SELL_STOP, 0x025E, 4);
			Register(Op.ZC_AUCTION_WINDOWS, 0x025F, 6);
			Register(Op.ZC_MAIL_WINDOWS, 0x0260, 6);
			Register(Op.AC_REQ_LOGIN_OLDEKEY, 0x0261, 11);
			Register(Op.AC_REQ_LOGIN_NEWEKEY, 0x0262, 11);
			Register(Op.AC_REQ_LOGIN_CARDPASS, 0x0263, 11);
			Register(Op.CA_ACK_LOGIN_OLDEKEY, 0x0264, 20);
			Register(Op.CA_ACK_LOGIN_NEWEKEY, 0x0265, 20);
			Register(Op.CA_ACK_LOGIN_CARDPASS, 0x0266, 30);
			Register(Op.AC_ACK_EKEY_FAIL_NOTEXIST, 0x0267, 4);
			Register(Op.AC_ACK_EKEY_FAIL_NOTUSESEKEY, 0x0268, 4);
			Register(Op.AC_ACK_EKEY_FAIL_NOTUSEDEKEY, 0x0269, 4);
			Register(Op.AC_ACK_EKEY_FAIL_AUTHREFUSE, 0x026A, 4);
			Register(Op.AC_ACK_EKEY_FAIL_INPUTEKEY, 0x026B, 4);
			Register(Op.AC_ACK_EKEY_FAIL_NOTICE, 0x026C, 4);
			Register(Op.AC_ACK_EKEY_FAIL_NEEDCARDPASS, 0x026D, 4);
			Register(Op.AC_ACK_FIRST_LOGIN, 0x026F, 2);
			Register(Op.AC_REQ_LOGIN_ACCOUNT_INFO, 0x0270, 2);
			Register(Op.CA_ACK_LOGIN_ACCOUNT_INFO, 0x0271, 40);
			Register(Op.AC_ACK_PT_ID_INFO, 0x0272, 44);
			Register(Op.CZ_REQ_MAIL_RETURN, 0x0273, 30);
			Register(Op.ZC_ACK_MAIL_RETURN, 0x0274, 8);
			Register(Op.CA_LOGIN_PCBANG, 0x0277, 84);
			Register(Op.ZC_NOTIFY_PCBANG, 0x0278, 2);
			Register(Op.CZ_HUNTINGLIST, 0x0279, 2);
			Register(Op.ZC_HUNTINGLIST, 0x027A, -1);
			Register(Op.ZC_PCBANG_EFFECT, 0x027B, 14);
			Register(Op.CA_LOGIN4, 0x027C, 60);
			Register(Op.ZC_PROPERTY_MERCE, 0x027D, 62);
			Register(Op.ZC_SHANDA_PROTECT, 0x027E, -1);
			Register(Op.CA_CLIENT_TYPE, 0x027F, 8);
			Register(Op.ZC_GANGSI_POINT, 0x0280, 12);
			Register(Op.CZ_GANGSI_RANK, 0x0281, 4);
			Register(Op.ZC_GANGSI_RANK, 0x0282, 284);
			Register(Op.ZC_AID, 0x0283, 6);
			Register(Op.ZC_NOTIFY_EFFECT3, 0x0284, 14);
			Register(Op.ZC_DEATH_QUESTION, 0x0285, 6);
			Register(Op.CZ_DEATH_QUESTION, 0x0286, 4);
			// ZC_PC_CASH_POINT_ITEMLIST, 0x0287
			// CZ_PC_BUY_CASH_POINT_ITEM, 0x0288
			// ZC_PC_CASH_POINT_UPDATE, 0x0289
			Register(Op.ZC_NPC_SHOWEFST_UPDATE, 0x028A, 18);
			// HC_CHARNOTBEENSELECTED, 0x028B
			// CH_SELECT_CHAR_GOINGTOBEUSED, 0x028C
			// CH_REQ_IS_VALID_CHARNAME, 0x028D
			// HC_ACK_IS_VALID_CHARNAME, 0x028E
			// CH_REQ_CHANGE_CHARNAME, 0x028F
			// HC_ACK_CHANGE_CHARNAME, 0x0290
			Register(Op.ZC_MSG, 0x0291, 4);
			Register(Op.CZ_STANDING_RESURRECTION, 0x0292, 2);
			Register(Op.ZC_BOSS_INFO, 0x0293, 70);
			Register(Op.ZC_READ_BOOK, 0x0294, 10);
			Register(Op.ZC_EQUIPMENT_ITEMLIST2, 0x0295, -1);
			Register(Op.ZC_STORE_EQUIPMENT_ITEMLIST2, 0x0296, -1);
			Register(Op.ZC_CART_EQUIPMENT_ITEMLIST2, 0x0297, -1);
			Register(Op.ZC_CASH_TIME_COUNTER, 0x0298, 8);
			Register(Op.ZC_CASH_ITEM_DELETE, 0x0299, 6);
			Register(Op.ZC_ITEM_PICKUP_ACK2, 0x029A, 27);
			Register(Op.ZC_MER_INIT, 0x029B, 70);
			Register(Op.ZC_MER_PROPERTY, 0x029C, 66);
			Register(Op.ZC_MER_SKILLINFO_LIST, 0x029D, -1);
			Register(Op.ZC_MER_SKILLINFO_UPDATE, 0x029E, 11);
			Register(Op.CZ_MER_COMMAND, 0x029F, 3);
			Register(Op.CZ_UNUSED_MER_USE_SKILL, 0x02A0, 10);
			Register(Op.CZ_UNUSED_MER_UPGRADE_SKILLLEVEL, 0x02A1, 4);
		}
	}
}
