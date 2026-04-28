namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion900()
		{
			// Reference: bRO EP9 Ragexe.exe, dated 2006-01-31
			// Roughly equivalent to 2005-10-24aSakexe and eA19

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
			Register(Op.HC_REQUEST_CHARACTER_PASSWORD, 0x023E, 4);
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
		}
	}
}
