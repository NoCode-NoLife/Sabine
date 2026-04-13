//--- Sabine Script ---------------------------------------------------------
// Payon Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Payon and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("alberta", "alberta_in")]
public class PayonBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Payon Town
		AddWarp(From("payon", 175, 189), To("pay_arche", 81, 22));
		AddWarp(From("pay_arche", 81, 17), To("payon", 175, 185));

		AddWarp(From("payon", 111, 173), To("payon_in01", 15, 68));
		AddWarp(From("payon_in01", 12, 68), To("payon", 108, 173));

		AddWarp(From("payon", 133, 81), To("payon_in01", 134, 171));
		AddWarp(From("payon_in01", 134, 168), To("payon", 129, 81));

		AddWarp(From("payon", 141, 91), To("payon_in01", 98, 173));
		AddWarp(From("payon_in01", 98, 170), To("payon", 141, 95));

		AddWarp(From("payon", 172, 133), To("payon_in01", 163, 108));
		AddWarp(From("payon_in01", 160, 108), To("payon", 169, 133));

		AddWarp(From("payon", 30, 176), To("payon_in01", 80, 63));
		AddWarp(From("payon_in01", 83, 63), To("payon", 30, 173));

		AddWarp(From("payon", 38, 174), To("payon_in01", 142, 45));
		AddWarp(From("payon_in01", 145, 45), To("payon", 38, 171));

		AddWarp(From("payon", 68, 173), To("payon_in01", 44, 138));
		AddWarp(From("payon_in01", 47, 138), To("payon", 71, 173));

		AddWarp(From("payon", 90, 167), To("payon_in01", 72, 89));
		AddWarp(From("payon_in01", 72, 86), To("payon", 90, 164));

		AddWarp(From("payon", 22, 174), To("payon_in01", 98, 25));
		AddWarp(From("payon_in01", 101, 25), To("payon", 22, 171));

		AddWarp(From("payon_in01", 122, 77), To("payon_in01", 80, 99));
		AddWarp(From("payon_in01", 83, 99), To("payon_in01", 124, 77));

		AddWarp(From("payon_in01", 111, 177), To("payon_in01", 129, 177));
		AddWarp(From("payon_in01", 126, 177), To("payon_in01", 108, 177));

		AddWarp(From("payon_in01", 113, 107), To("payon_in01", 63, 107));
		AddWarp(From("payon_in01", 60, 107), To("payon_in01", 110, 107));

		AddWarp(From("payon_in01", 113, 143), To("payon_in01", 63, 145));
		AddWarp(From("payon_in01", 60, 145), To("payon_in01", 110, 143));

		AddWarp(From("payon_in01", 113, 77), To("payon_in01", 63, 99));
		AddWarp(From("payon_in01", 60, 99), To("payon_in01", 110, 77));

		AddWarp(From("payon_in01", 122, 107), To("payon_in01", 80, 107));
		AddWarp(From("payon_in01", 83, 107), To("payon_in01", 125, 107));

		AddWarp(From("payon_in01", 124, 143), To("payon_in01", 80, 145));
		AddWarp(From("payon_in01", 83, 145), To("payon_in01", 127, 143));

		AddWarp(From("payon_in01", 130, 36), To("payon_in01", 70, 72));
		AddWarp(From("payon_in01", 70, 75), To("payon_in01", 130, 39));

		AddWarp(From("payon_in01", 143, 179), To("payon_in01", 167, 179));
		AddWarp(From("payon_in01", 164, 179), To("payon_in01", 140, 179));

		AddWarp(From("payon_in01", 167, 74), To("payon_in01", 168, 50));
		AddWarp(From("payon_in01", 168, 53), To("payon_in01", 167, 77));

		AddWarp(From("payon_in01", 170, 100), To("payon_in01", 170, 83));
		AddWarp(From("payon_in01", 170, 87), To("payon_in01", 170, 104));

		AddWarp(From("payon_in01", 170, 119), To("payon_in01", 170, 136));
		AddWarp(From("payon_in01", 170, 132), To("payon_in01", 170, 115));

		AddWarp(From("payon_in01", 22, 26), To("payon_in01", 30, 78));
		AddWarp(From("payon_in01", 30, 81), To("payon_in01", 22, 29));

		AddWarp(From("payon_in01", 26, 115), To("payon_in01", 30, 129));
		AddWarp(From("payon_in01", 30, 126), To("payon_in01", 26, 112));

		AddWarp(From("payon_in01", 26, 164), To("payon_in01", 30, 148));
		AddWarp(From("payon_in01", 30, 151), To("payon_in01", 26, 167));

		AddWarp(From("payon_in01", 30, 59), To("payon_in01", 52, 32));
		AddWarp(From("payon_in01", 52, 35), To("payon_in01", 30, 62));

		AddWarp(From("payon_in01", 60, 170), To("payon_in01", 72, 156));
		AddWarp(From("payon_in01", 72, 159), To("payon_in01", 60, 173));

		AddWarp(From("payon_in01", 70, 52), To("payon_in01", 86, 34));
		AddWarp(From("payon_in01", 86, 37), To("payon_in01", 70, 55));

		AddWarp(From("payon_in01", 72, 121), To("payon_in01", 72, 137));
		AddWarp(From("payon_in01", 72, 134), To("payon_in01", 72, 118));

		AddWarp(From("payon_in02", 10, 25), To("payon_in02", 72, 67));
		AddWarp(From("payon_in02", 75, 67), To("payon_in02", 13, 25));

		AddWarp(From("payon_in02", 35, 67), To("payon_in02", 55, 67));
		AddWarp(From("payon_in02", 52, 67), To("payon_in02", 32, 67));

		AddWarp(From("payon_in02", 61, 33), To("payon_in02", 73, 33));
		AddWarp(From("payon_in02", 70, 33), To("payon_in02", 58, 33));

		AddWarp(From("pay_arche", 145, 165), To("payon_in02", 64, 60));
		AddWarp(From("payon_in02", 64, 56), To("pay_arche", 141, 161));

		AddWarp(From("pay_arche", 92, 170), To("payon_in02", 50, 7));
		AddWarp(From("payon_in02", 50, 4), To("pay_arche", 92, 166));

		AddWarp(From("pay_arche", 71, 156), To("payon_in02", 82, 41));
		AddWarp(From("payon_in02", 82, 45), To("pay_arche", 74, 153));

		AddWarp(From("payon", 90, 25), To("pay_fild01", 333, 356));

		// Payon Dungeon
		AddWarp(From("pay_arche", 36, 131), To("pay_dun00", 21, 183));
		AddWarp(From("pay_dun00", 21, 186), To("pay_arche", 39, 131));

		AddWarp(From("pay_dun00", 184, 33), To("pay_dun01", 19, 33));
		AddWarp(From("pay_dun01", 15, 33), To("pay_dun00", 181, 33));

		AddWarp(From("pay_dun01", 286, 25), To("pay_dun02", 19, 63));
		AddWarp(From("pay_dun02", 16, 63), To("pay_dun01", 283, 28));

		// Payon Fields
		AddWarp(From("pay_fild02", 284, 108), To("pay_fild03", 20, 110));
		AddWarp(From("pay_fild03", 15, 110), To("pay_fild02", 280, 108));

		AddWarp(From("pay_fild01", 13, 152), To("moc_fild03", 299, 170));
		AddWarp(From("pay_fild01", 333, 361), To("payon", 90, 29));
		AddWarp(From("pay_fild01", 353, 14), To("pay_fild02", 160, 381));
		AddWarp(From("pay_fild03", 392, 63), To("alberta", 19, 233));
	}
}
