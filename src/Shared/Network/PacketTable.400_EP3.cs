namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion400()
		{
			// Reference: jRO EP3 Ragexe.exe, dated 2003-05-27

			// Barely any changes, but quite a few new packets.

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
		}
	}
}
