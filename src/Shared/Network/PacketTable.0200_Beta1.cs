namespace Sabine.Shared.Network
{
	public static partial class PacketTable
	{
		private static void LoadVersion200()
		{
			// Reference: iRO Beta1 Ragexe.exe, dated 2002-02-20

			// As the first and cleanest Beta1 client we found, this is
			// typically the one we refer to as "Beta1". Notable changes
			// are that the client now sent a version on login and that
			// the UI had three head slots, even though still only one was
			// usable.

			ChangeSize(Op.CA_LOGIN, 54);
			ChangeSize(Op.CH_DELETE_CHAR, 46);
			ChangeSize(Op.ZC_NOTIFY_ACT, 27);

			Register(Op.ZC_GROUPINFO_CHANGE, 0x009D, 6);
			Register(Op.CZ_CHANGE_GROUPEXPOPTION, 0x009E, 6);
		}
	}
}
