namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion500()
		{
			// Reference: iRO EP3 Ragexe.exe, dated 2003-10-31

			// No changes from jRO EP3 to iRO EP3, which might be to be
			// expected, but there were a few new packets, which change
			// the client's behavior slightly, as it was using hashed
			// passwords, utilizing different packets for the login. It
			// also required an initial packet being sent by the char and
			// zone servers, otherwise the client wouldn't send a login
			// packet and disconnect after a few seconds.

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
			Register(Op.ZC_NOTIFY_STANDENTRY2, 0x01D8, 52);
			Register(Op.ZC_NOTIFY_NEWENTRY2, 0x01D9, 51);
			Register(Op.ZC_NOTIFY_MOVEENTRY2, 0x01DA, 58);
			Register(Op.CA_REQ_HASH, 0x01DB, 2);
			Register(Op.AC_ACK_HASH, 0x01DC, -1);
			Register(Op.CA_LOGIN2, 0x01DD, 47);
			Register(Op.ZC_NOTIFY_SKILL2, 0x01DE, 33);
			Register(Op.CZ_REQ_ACCOUNTNAME, 0x01DF, 6);
			Register(Op.ZC_ACK_ACCOUNTNAME, 0x01E0, 30);
		}
	}
}
