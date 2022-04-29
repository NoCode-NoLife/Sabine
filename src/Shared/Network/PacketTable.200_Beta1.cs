namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion200()
		{
			// Reference: iRO Beta1 Ragexe.exe, dated 2002-02-20

			// Beta1 was still similar to Alpha in many ways and didn't
			// change too much. Name strings were increased from 16 to
			// 24, because someone finally realized that 16 characters
			// are a joke, top headgears were now used, though the packets
			// didn't need any changes for them, and there were a bunch
			// of new ops, many of them skill related, which were first
			// enabled in Beta1.

			ChangeSize(Op.CA_LOGIN, 54);
			ChangeSize(Op.CH_ENTER, 17);
			ChangeSize(Op.CH_MAKE_CHAR, 34);
			ChangeSize(Op.CH_DELETE_CHAR, 46);
			ChangeSize(Op.HC_ACCEPT_MAKECHAR, 92);
			ChangeSize(Op.CZ_ENTER, 19);
			ChangeSize(Op.ZC_NOTIFY_STANDENTRY, 26);
			ChangeSize(Op.ZC_NOTIFY_NEWENTRY, 25);
			ChangeSize(Op.ZC_NOTIFY_ACTENTRY, 30);
			ChangeSize(Op.ZC_NOTIFY_MOVEENTRY, 32);
			ChangeSize(Op.ZC_NOTIFY_STANDENTRY_NPC, 25);
			ChangeSize(Op.CZ_REQUEST_TIME, 6);
			ChangeSize(Op.ZC_NOTIFY_ACT, 27);
			ChangeSize(Op.ZC_ACK_REQNAME, 54);
			ChangeSize(Op.ZC_ITEM_ENTRY, 38);
			ChangeSize(Op.ZC_ITEM_FALL_ENTRY, 38);
			ChangeSize(Op.ZC_ITEM_PICKUP_ACK, 33);
			ChangeSize(Op.CZ_REQ_ITEM_EXPLANATION_BYNAME, 26);
			ChangeSize(Op.ZC_STATUS, 44);
			ChangeSize(Op.CZ_DISCONNECT_CHARACTER, 26);
			ChangeSize(Op.CZ_SETTING_WHISPER_PC, 27);
			ChangeSize(Op.ZC_MEMBER_NEWENTRY, 28);
			ChangeSize(Op.ZC_MEMBER_EXIT, 29);
			ChangeSize(Op.CZ_REQ_ROLE_CHANGE, 30);
			ChangeSize(Op.ZC_ROLE_CHANGE, 30);
			ChangeSize(Op.CZ_REQ_EXPEL_MEMBER, 16);
			ChangeSize(Op.ZC_REQ_EXCHANGE_ITEM, 26);
			ChangeSize(Op.ZC_ADD_EXCHANGE_ITEM, 30);
			ChangeSize(Op.ZC_ADD_ITEM_TO_STORE, 32);
			ChangeSize(Op.CZ_MAKE_GROUP, 26);
			ChangeSize(Op.ZC_ADD_SKILL, 39);

			Register(Op.ZC_NOTIFY_ACT_POSITION, 0x0027, 23);
			Register(Op.ZC_GROUPINFO_CHANGE, 0x009D, 6);
			Register(Op.CZ_CHANGE_GROUPEXPOPTION, 0x009E, 6);
			Register(Op.CZ_UPGRADE_SKILLLEVEL, 0x00AE, 4);
			Register(Op.CZ_USE_SKILL, 0x00AF, 10);
			Register(Op.ZC_NOTIFY_SKILL, 0x00B0, 31);
			Register(Op.ZC_NOTIFY_SKILL_POSITION, 0x00B1, 35);
			Register(Op.CZ_USE_SKILL_TOGROUND, 0x00B2, 10);
			Register(Op.ZC_NOTIFY_GROUNDSKILL, 0x00B3, 18);
			Register(Op.CZ_CANCEL_LOCKON, 0x00B4, 2);
			Register(Op.ZC_STATE_CHANGE, 0x00B5, 13);
			Register(Op.ZC_USE_SKILL, 0x00B6, 15);
			Register(Op.CZ_SELECT_WARPPOINT, 0x00B7, 20);
			Register(Op.ZC_WARPLIST, 0x00B8, 52);
			Register(Op.CZ_REMEMBER_WARPPOINT, 0x00B9, 2);
			Register(Op.ZC_ACK_REMEMBER_WARPPOINT, 0x00BA, 3);
			Register(Op.ZC_SKILL_ENTRY, 0x00BB, 11);
			Register(Op.ZC_SKILL_DISAPPEAR, 0x00BC, 6);
			Register(Op.ZC_NOTIFY_CARTITEM_COUNTINFO, 0x00BD, 14);
			Register(Op.ZC_CART_EQUIPMENT_ITEMLIST, 0x00BE, -1);
			Register(Op.ZC_CART_NORMAL_ITEMLIST, 0x00BF, -1);
			Register(Op.ZC_ADD_ITEM_TO_CART, 0x00C0, 32);
			Register(Op.ZC_DELETE_ITEM_FROM_CART, 0x00C1, 8);
			Register(Op.CZ_MOVE_ITEM_FROM_BODY_TO_CART, 0x00C2, 8);
			Register(Op.CZ_MOVE_ITEM_FROM_CART_TO_BODY, 0x00C3, 8);
			Register(Op.CZ_MOVE_ITEM_FROM_STORE_TO_CART, 0x00C4, 8);
			Register(Op.CZ_MOVE_ITEM_FROM_CART_TO_STORE, 0x00C5, 8);
			Register(Op.CZ_REQ_CARTOFF, 0x00C6, 2);
			Register(Op.ZC_CARTOFF, 0x00C7, 2);
			Register(Op.ZC_ACK_ADDITEM_TO_CART, 0x00C8, 3);
			Register(Op.ZC_OPENSTORE, 0x00C9, 4);
			Register(Op.CZ_REQ_CLOSESTORE, 0x00CA, 2);
			Register(Op.CZ_REQ_OPENSTORE, 0x00CB, -1);
			Register(Op.CZ_REQ_BUY_FROMMC, 0x00CC, 6);
			Register(Op.ZC_STORE_ENTRY, 0x00CD, 86);
			Register(Op.ZC_DISAPPEAR_ENTRY, 0x00CE, 6);
			Register(Op.ZC_PC_PURCHASE_ITEMLIST_FROMMC, 0x00CF, -1);
			Register(Op.CZ_PC_PURCHASE_ITEMLIST_FROMMC, 0x00D0, -1);
			Register(Op.ZC_PC_PURCHASE_RESULT_FROMMC, 0x00D1, 7);
			Register(Op.ZC_PC_PURCHASE_MYITEMLIST, 0x00D2, -1);
			Register(Op.ZC_DELETEITEM_FROM_MCSTORE, 0x00D3, 6);
			Register(Op.CZ_PKMODE_CHANGE, 0x00D4, 3);
			Register(Op.ZC_ATTACK_FAILURE_FOR_DISTANCE, 0x00D5, 16);
			Register(Op.ZC_ATTACK_RANGE, 0x00D6, 4);
			Register(Op.ZC_ACTION_FAILURE, 0x00D7, 4);
			Register(Op.ZC_EQUIP_ARROW, 0x00D8, 4);
			Register(Op.ZC_RECOVERY, 0x00D9, 6);
			Register(Op.ZC_USESKILL_ACK, 0x00DA, 24);
			Register(Op.CZ_ITEM_CREATE, 0x00DB, 26);
			Register(Op.CZ_MOVETO_MAP, 0x00DC, 18);
		}
	}
}
