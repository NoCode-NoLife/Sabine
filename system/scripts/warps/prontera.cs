//--- Sabine Script ---------------------------------------------------------
// Prontera Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Prontera and its surrounding maps.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class PronteraWarpsScript : GeneralScript
{
	public override void Load()
	{
		// Prontera North Field
		AddWarp(From("prt_fild01", 112, 22), To("prt_vilg02", 100, 173));
		AddWarp(From("prt_vilg02", 100, 176), To("prt_fild01", 112, 26));

		// Prontera North
		AddWarp(From("prt_vilg02", 100, 23), To("prt_vilg00", 100, 173));
		AddWarp(From("prt_vilg00", 100, 176), To("prt_vilg02", 100, 26));

		AddWarp(From("prt_vilg00", 176, 100), To("prt_fild04", 18, 156));
		AddWarp(From("prt_fild04", 15, 156), To("prt_vilg00", 173, 100));

		AddWarp(From("prt_vilg02", 71, 74), To("prt_intr02", 14, 126));
		AddWarp(From("prt_intr02", 10, 126), To("prt_vilg02", 73, 77));

		AddWarp(From("prt_vilg02", 153, 50), To("prt_intr02", 80, 149));
		AddWarp(From("prt_intr02", 80, 153), To("prt_vilg02", 150, 53));

		AddWarp(From("prt_vilg02", 161, 100), To("prt_intr02", 166, 20));
		AddWarp(From("prt_intr02", 162, 20), To("prt_vilg02", 157, 100));

		AddWarp(From("prt_vilg02", 127, 119), To("prt_intr02", 50, 72));
		AddWarp(From("prt_intr02", 46, 72), To("prt_vilg02", 124, 116));

		AddWarp(From("prt_vilg02", 135, 87), To("prt_intr02", 178, 109));
		AddWarp(From("prt_intr02", 178, 113), To("prt_vilg02", 131, 90));

		AddWarp(From("prt_vilg02", 111, 63), To("prt_intr02", 118, 98));
		AddWarp(From("prt_intr02", 114, 98), To("prt_vilg02", 107, 63));

		AddWarp(From("prt_vilg02", 77, 160), To("prt_intr02", 14, 20));
		AddWarp(From("prt_intr02", 10, 20), To("prt_vilg02", 73, 160));

		AddWarp(From("prt_vilg02", 143, 151), To("prt_intr02", 136, 40));
		AddWarp(From("prt_intr02", 132, 40), To("prt_vilg02", 140, 148));

		AddWarp(From("prt_vilg02", 49, 99), To("prt_intr02", 95, 46));
		AddWarp(From("prt_intr02", 99, 46), To("prt_vilg02", 52, 102));

		AddWarp(From("prt_vilg02", 86, 142), To("prt_intr02", 61, 30));
		AddWarp(From("prt_intr02", 65, 30), To("prt_vilg02", 89, 145));

		AddWarp(From("prt_vilg02", 68, 116), To("prt_intr02", 28, 52));
		AddWarp(From("prt_intr02", 28, 48), To("prt_vilg02", 71, 113));

		AddWarp(From("prt_vilg02", 148, 141), To("prt_intr02", 104, 20));
		AddWarp(From("prt_intr02", 100, 20), To("prt_vilg02", 146, 138));

		// Inside North Prontera
		AddWarp(From("prt_intr02", 43, 126), To("prt_intr02", 14, 172));
		AddWarp(From("prt_intr02", 10, 172), To("prt_intr02", 39, 126));

		AddWarp(From("prt_intr02", 38, 137), To("prt_intr02", 72, 176));
		AddWarp(From("prt_intr02", 72, 172), To("prt_intr02", 38, 133));

		AddWarp(From("prt_intr02", 38, 114), To("prt_intr02", 104, 185));
		AddWarp(From("prt_intr02", 104, 189), To("prt_intr02", 38, 118));

		AddWarp(From("prt_intr02", 76, 185), To("prt_intr02", 137, 184));
		AddWarp(From("prt_intr02", 133, 184), To("prt_intr02", 72, 185));

		AddWarp(From("prt_intr02", 108, 176), To("prt_intr02", 175, 174));
		AddWarp(From("prt_intr02", 171, 174), To("prt_intr02", 104, 176));

		AddWarp(From("prt_intr02", 66, 140), To("prt_intr02", 121, 142));
		AddWarp(From("prt_intr02", 125, 142), To("prt_intr02", 70, 140));

		AddWarp(From("prt_intr02", 93, 140), To("prt_intr02", 152, 142));
		AddWarp(From("prt_intr02", 148, 142), To("prt_intr02", 89, 140));

		AddWarp(From("prt_intr02", 80, 126), To("prt_intr02", 22, 95));
		AddWarp(From("prt_intr02", 22, 99), To("prt_intr02", 80, 130));

		AddWarp(From("prt_intr02", 120, 125), To("prt_intr02", 59, 98));
		AddWarp(From("prt_intr02", 63, 98), To("prt_intr02", 120, 129));

		AddWarp(From("prt_intr02", 156, 125), To("prt_intr02", 90, 98));
		AddWarp(From("prt_intr02", 86, 98), To("prt_intr02", 156, 129));

		AddWarp(From("prt_intr02", 64, 79), To("prt_intr02", 100, 72));
		AddWarp(From("prt_intr02", 100, 68), To("prt_intr02", 64, 75));

		AddWarp(From("prt_intr02", 64, 64), To("prt_intr02", 140, 73));
		AddWarp(From("prt_intr02", 140, 77), To("prt_intr02", 64, 68));

		// Prontera Castle
		AddWarp(From("prt_vilg00", 100, 25), To("prt_vilg01", 100, 172));
		AddWarp(From("prt_vilg01", 100, 175), To("prt_vilg00", 100, 28));

		AddWarp(From("prt_vilg00", 23, 99), To("prt_fild05", 181, 54));
		AddWarp(From("prt_fild05", 184, 54), To("prt_vilg00", 26, 99));

		AddWarp(From("prt_vilg00", 100, 122), To("prt_cstl01", 44, 64));
		AddWarp(From("prt_cstl01", 44, 61), To("prt_vilg00", 100, 119));

		AddWarp(From("prt_vilg00", 100, 130), To("prt_cstl01", 44, 127));
		AddWarp(From("prt_cstl01", 44, 130), To("prt_vilg00", 100, 135));

		AddWarp(From("prt_vilg00", 126, 126), To("prt_cstl01", 62, 159));
		AddWarp(From("prt_cstl01", 58, 159), To("prt_vilg00", 123, 124));

		AddWarp(From("prt_vilg00", 125, 73), To("prt_cstl01", 122, 171));
		AddWarp(From("prt_cstl01", 122, 175), To("prt_vilg00", 123, 76));

		AddWarp(From("prt_vilg00", 73, 73), To("prt_cstl01", 183, 160));
		AddWarp(From("prt_cstl01", 187, 160), To("prt_vilg00", 75, 76));

		AddWarp(From("prt_vilg00", 73, 126), To("prt_cstl01", 26, 150));
		AddWarp(From("prt_cstl01", 26, 146), To("prt_vilg00", 76, 124));

		// Inside Prontera Castle
		AddWarp(From("prt_cstl01", 176, 52), To("prt_cstl01", 166, 106));
		AddWarp(From("prt_cstl01", 166, 110), To("prt_cstl01", 176, 56));

		AddWarp(From("prt_cstl01", 150, 114), To("prt_cstl01", 96, 118));
		AddWarp(From("prt_cstl01", 96, 122), To("prt_cstl01", 150, 118));

		AddWarp(From("prt_cstl01", 178, 114), To("prt_cstl01", 124, 118));
		AddWarp(From("prt_cstl01", 124, 122), To("prt_cstl01", 178, 118));

		AddWarp(From("prt_cstl01", 110, 89), To("prt_cstl01", 44, 89));
		AddWarp(From("prt_cstl01", 44, 93), To("prt_cstl01", 110, 93));

		AddWarp(From("prt_cstl01", 100, 55), To("prt_cstl01", 16, 74));
		AddWarp(From("prt_cstl01", 16, 70), To("prt_cstl01", 100, 51));

		AddWarp(From("prt_cstl01", 132, 55), To("prt_cstl01", 72, 74));
		AddWarp(From("prt_cstl01", 72, 70), To("prt_cstl01", 132, 51));

		AddWarp(From("prt_cstl01", 100, 40), To("prt_cstl01", 20, 36));
		AddWarp(From("prt_cstl01", 20, 40), To("prt_cstl01", 100, 44));

		AddWarp(From("prt_cstl01", 132, 40), To("prt_cstl01", 60, 36));
		AddWarp(From("prt_cstl01", 60, 40), To("prt_cstl01", 132, 44));

		// Prontera West Field
		AddWarp(From("prt_fild05", 30, 161), To("prt_dugn01", 189, 10));
		AddWarp(From("prt_dugn01", 191, 8), To("prt_fild05", 33, 158));

		// Prontera Dungeon
		AddWarp(From("prt_dugn01", 166, 183), To("prt_dugn02", 178, 185));
		AddWarp(From("prt_dugn02", 178, 188), To("prt_dugn01", 166, 180));

		AddWarp(From("prt_dugn02", 98, 117), To("prt_dugn03", 102, 87));
		AddWarp(From("prt_dugn03", 102, 90), To("prt_dugn02", 98, 120));

		// Prontera East Field
		AddWarp(From("prt_dugn00", 182, 152), To("prt_fild04", 29, 112));

		// Prontera South
		AddWarp(From("prt_vilg01", 100, 22), To("prt_fild00", 100, 177));
		AddWarp(From("prt_fild00", 100, 180), To("prt_vilg01", 100, 25));

		AddWarp(From("prt_vilg01", 133, 115), To("prt_intr01", 116, 101));
		AddWarp(From("prt_intr01", 112, 101), To("prt_vilg01", 130, 112));

		AddWarp(From("prt_vilg01", 107, 159), To("prt_intr01", 16, 184));
		AddWarp(From("prt_intr01", 12, 184), To("prt_vilg01", 103, 159));

		AddWarp(From("prt_vilg01", 122, 124), To("prt_intr01", 60, 92));
		AddWarp(From("prt_intr01", 56, 92), To("prt_vilg01", 119, 121));

		AddWarp(From("prt_vilg01", 30, 99), To("prt_intr01", 33, 68));
		AddWarp(From("prt_intr01", 37, 68), To("prt_vilg01", 34, 99));

		AddWarp(From("prt_vilg01", 39, 111), To("prt_intr01", 139, 22));
		AddWarp(From("prt_intr01", 143, 22), To("prt_vilg01", 43, 111));

		AddWarp(From("prt_vilg01", 39, 87), To("prt_intr01", 89, 46));
		AddWarp(From("prt_intr01", 93, 46), To("prt_vilg01", 43, 87));

		AddWarp(From("prt_vilg01", 71, 77), To("prt_intr01", 185, 22));
		AddWarp(From("prt_intr01", 189, 22), To("prt_vilg01", 74, 80));

		AddWarp(From("prt_vilg01", 152, 91), To("prt_intr01", 39, 14));
		AddWarp(From("prt_intr01", 39, 10), To("prt_vilg01", 152, 87));

		AddWarp(From("prt_vilg01", 37, 65), To("prt_intr01", 57, 180));
		AddWarp(From("prt_intr01", 61, 180), To("prt_vilg01", 41, 65));

		AddWarp(From("prt_vilg01", 165, 100), To("prt_intr01", 114, 63));
		AddWarp(From("prt_intr01", 110, 63), To("prt_vilg01", 161, 100));

		AddWarp(From("prt_vilg01", 108, 138), To("prt_intr01", 180, 162));
		AddWarp(From("prt_intr01", 180, 158), To("prt_vilg01", 108, 134));

		AddWarp(From("prt_vilg01", 57, 55), To("prt_intr01", 80, 175));
		AddWarp(From("prt_intr01", 76, 175), To("prt_vilg01", 54, 52));

		AddWarp(From("prt_vilg01", 54, 153), To("prt_intr01", 14, 111));
		AddWarp(From("prt_intr01", 10, 111), To("prt_vilg01", 50, 150));

		AddWarp(From("prt_vilg01", 90, 160), To("prt_intr01", 145, 182));
		AddWarp(From("prt_intr01", 149, 182), To("prt_vilg01", 94, 160));

		AddWarp(From("prt_vilg01", 86, 64), To("prt_intr01", 123, 140));
		AddWarp(From("prt_intr01", 127, 140), To("prt_vilg01", 89, 66));

		AddWarp(From("prt_vilg01", 128, 80), To("prt_intr01", 66, 149));
		AddWarp(From("prt_intr01", 66, 153), To("prt_vilg01", 125, 83));

		AddWarp(From("prt_vilg01", 153, 67), To("prt_intr01", 158, 149));
		AddWarp(From("prt_intr01", 158, 153), To("prt_vilg01", 150, 71));

		AddWarp(From("prt_vilg01", 172, 122), To("prt_intr01", 14, 152));
		AddWarp(From("prt_intr01", 10, 152), To("prt_vilg01", 172, 118));

		// Inside Prontera South
		AddWarp(From("prt_intr01", 114, 10), To("prt_intr01", 22, 79));
		AddWarp(From("prt_intr01", 22, 83), To("prt_intr01", 114, 14));

		AddWarp(From("prt_intr01", 64, 57), To("prt_intr01", 22, 56));
		AddWarp(From("prt_intr01", 22, 52), To("prt_intr01", 64, 53));

		AddWarp(From("prt_intr01", 20, 27), To("prt_intr01", 164, 92));
		AddWarp(From("prt_intr01", 164, 88), To("prt_intr01", 20, 23));

		// Prontera South Field
		AddWarp(From("prt_fild00", 79, 18), To("moc_fild04", 162, 191));
	}
}
