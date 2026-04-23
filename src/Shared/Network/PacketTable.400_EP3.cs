namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion400()
		{
			// Reference: jRO EP3 Ragexe.exe, dated 2003-05-27

			ChangeSize(Op.ZC_MSG_STATE_CHANGE, 9);

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
