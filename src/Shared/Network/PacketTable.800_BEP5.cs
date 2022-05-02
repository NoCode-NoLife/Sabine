namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion800()
		{
			// Reference: bRO EP5 Ragexe.exe, dated 2004-12-28

			ChangeSize(Op.CZ_LOCALBROADCAST, -1);
			ChangeSize(Op.CZ_INPUT_EDITDLGSTR, -1);

			Register(Op.CZ_REQMAKINGHOMUN, 0x01CA, 3, false);
			Register(Op.ZC_FRIENDS_LIST, 0x0201, -1, false);
			Register(Op.CZ_ADD_FRIENDS, 0x0202, 26, false);
			Register(Op.CZ_DELETE_FRIENDS, 0x0203, 10, false);
			Register(Op.ZC_FRIENDS_STATE, 0x0206, 11, false);
			Register(Op.ZC_REQ_ADD_FRIENDS, 0x0207, 34, false);
			Register(Op.CZ_ACK_REQ_ADD_FRIENDS, 0x0208, 14, false);
			Register(Op.ZC_ADD_FRIENDS_LIST, 0x0209, 36, false);
			Register(Op.ZC_DELETE_FRIENDS, 0x020A, 10, false);

			// CZ_REQ_PVPPOINT, 0x020F
			// ZC_ACK_PVPPOINT, 0x0210
			// ZH_MOVE_PVPWORLD, 0x0211
			Register(Op.CZ_REQ_GIVE_MANNER_BYNAME, 0x0212, 26);
			Register(Op.CZ_REQ_STATUS_GM, 0x0213, 26);
			Register(Op.ZC_ACK_STATUS_GM, 0x0214, 42);
			Register(Op.ZC_SKILLMSG, 0x0215, 6);
			Register(Op.ZC_BABYMSG, 0x0216, 6);
			Register(Op.CZ_BLACKSMITH_RANK, 0x0217, 2);
			Register(Op.CZ_ALCHEMIST_RANK, 0x0218, 2);
			Register(Op.ZC_BLACKSMITH_RANK, 0x0219, 282);
			Register(Op.ZC_ALCHEMIST_RANK, 0x021A, 282);
			Register(Op.ZC_BLACKSMITH_POINT, 0x021B, 10);
			Register(Op.ZC_ALCHEMIST_POINT, 0x021C, 10);
			Register(Op.CZ_LESSEFFECT, 0x021D, 6);
			Register(Op.ZC_LESSEFFECT, 0x021E, 6);
			Register(Op.ZC_NOTIFY_PKINFO, 0x021F, 66);
			Register(Op.ZC_NOTIFY_CRAZYKILLER, 0x0220, 10);
			Register(Op.ZC_NOTIFY_WEAPONITEMLIST, 0x0221, -1);
			Register(Op.CZ_REQ_WEAPONREFINE, 0x0222, 6);
			Register(Op.ZC_ACK_WEAPONREFINE, 0x0223, 8);
		}
	}
}
