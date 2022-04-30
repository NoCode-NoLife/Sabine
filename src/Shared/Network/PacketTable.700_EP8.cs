namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion700()
		{
			// Reference: iRO EP8 Ragexe.exe, dated 2004-08-03

			// A few of the packets that were taking up space in the table,
			// but weren't used yet, have been added/enabled. Though it
			// also created a few new gaps.

			Register(Op.ZC_AUTOSPELLLIST, 0x01CD, 30, false);
			Register(Op.CZ_SELECTAUTOSPELL, 0x01CE, 6, false);
			Register(Op.ZC_DEVOTIONLIST, 0x01CF, 28, false);
			Register(Op.ZC_SPIRITS, 0x01D0, 8, false);
			Register(Op.ZC_BLADESTOP, 0x01D1, 14, false);
			Register(Op.ZC_COMBODELAY, 0x01D2, 10, false);
			Register(Op.ZC_NOTIFY_EFFECT2, 0x01E1, 8, false);
			Register(Op.CA_CONNECT_INFO_CHANGED, 0x0200, 26);
			// ZC_FRIENDS_LIST, 0x201
			// CZ_ADD_FRIENDS, 0x202
			// CZ_DELETE_FRIENDS, 0x203
			Register(Op.CA_EXE_HASHCHECK, 0x0204, 18);
			Register(Op.ZC_DIVORCE, 0x0205, 26);
			// ZC_FRIENDS_STATE, 0x0206
			// ZC_REQ_ADD_FRIENDS, 0x0207
			// CZ_ACK_REQ_ADD_FRIENDS, 0x0208
			// ZC_ADD_FRIENDS_LIST, 0x0209
			// ZC_DELETE_FRIENDS, 0x020A
			// CH_EXE_HASHCHECK, 0x020B
			// CZ_EXE_HASHCHECK, 0x020C
			Register(Op.HC_BLOCK_CHARACTER, 0x020D, -1);
			Register(Op.ZC_STARSKILL, 0x020E, 24);
		}
	}
}
