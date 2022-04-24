//--- Sabine Script ---------------------------------------------------------
// Morocc Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Morocc and its surrounding maps.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class MoroccWarpsScript : GeneralScript
{
	public override void Load()
	{
		// Morocc South-East Field
		AddWarp(To("moc_fild04", 162, 194), From("prt_fild00", 79, 21));

		AddWarp(To("moc_fild04", 12, 63), From("moc_fild01", 185, 114));
		AddWarp(To("moc_fild01", 188, 114), From("moc_fild04", 15, 63));

		// Morocc South Field
		AddWarp(To("moc_fild01", 103, 182), From("moc_vilg00", 100, 24));
		AddWarp(To("moc_vilg00", 99, 21), From("moc_fild01", 103, 179));

		// Morocc Castle
		AddWarp(To("moc_vilg00", 99, 179), From("moc_vilg01", 99, 24));
		AddWarp(To("moc_vilg01", 99, 21), From("moc_vilg00", 99, 176));

		AddWarp(To("moc_vilg00", 59, 114), From("moc_intr00", 24, 154));
		AddWarp(To("moc_intr00", 24, 150), From("moc_vilg00", 62, 111));

		AddWarp(To("moc_vilg00", 152, 124), From("moc_intr00", 58, 154));
		AddWarp(To("moc_intr00", 58, 150), From("moc_vilg00", 152, 120));

		AddWarp(To("moc_vilg00", 152, 55), From("moc_intr00", 92, 177));
		AddWarp(To("moc_intr00", 92, 181), From("moc_vilg00", 149, 58));

		AddWarp(To("moc_vilg00", 100, 91), From("moc_intr00", 44, 133));
		AddWarp(To("moc_intr00", 44, 137), From("moc_vilg00", 100, 95));

		// Inside Morocc Castle
		AddWarp(To("moc_intr00", 17, 102), From("moc_intr00", 103, 94));
		AddWarp(To("moc_intr00", 107, 94), From("moc_intr00", 21, 102));

		AddWarp(To("moc_intr00", 71, 102), From("moc_intr00", 88, 126));
		AddWarp(To("moc_intr00", 84, 126), From("moc_intr00", 67, 102));

		AddWarp(To("moc_intr00", 93, 96), From("moc_intr00", 172, 134));
		AddWarp(To("moc_intr00", 176, 134), From("moc_intr00", 97, 96));

		AddWarp(To("moc_intr00", 98, 128), From("moc_intr00", 133, 122));
		AddWarp(To("moc_intr00", 129, 122), From("moc_intr00", 94, 128));

		AddWarp(To("moc_intr00", 98, 124), From("moc_intr00", 104, 20));
		AddWarp(To("moc_intr00", 100, 20), From("moc_intr00", 94, 124));

		AddWarp(To("moc_intr00", 93, 92), From("moc_intr00", 159, 26));
		AddWarp(To("moc_intr00", 163, 26), From("moc_intr00", 97, 92));

		AddWarp(To("moc_intr00", 44, 109), From("moc_intr00", 58, 57));
		AddWarp(To("moc_intr00", 58, 61), From("moc_intr00", 44, 113));

		AddWarp(To("moc_intr00", 58, 44), From("moc_intr00", 18, 63));
		AddWarp(To("moc_intr00", 18, 67), From("moc_intr00", 58, 48));

		AddWarp(To("moc_intr00", 22, 62), From("moc_intr00", 182, 20));
		AddWarp(To("moc_intr00", 182, 16), From("moc_intr00", 22, 58));

		AddWarp(To("moc_intr00", 182, 47), From("moc_intr00", 168, 79));
		AddWarp(To("moc_intr00", 168, 75), From("moc_intr00", 182, 43));

		AddWarp(To("moc_intr00", 18, 51), From("moc_intr00", 126, 90));
		AddWarp(To("moc_intr00", 126, 94), From("moc_intr00", 18, 55));

		AddWarp(To("moc_intr00", 48, 48), From("moc_intr00", 22, 29));
		AddWarp(To("moc_intr00", 22, 33), From("moc_intr00", 48, 52));

		AddWarp(To("moc_intr00", 68, 48), From("moc_intr00", 50, 29));
		AddWarp(To("moc_intr00", 50, 33), From("moc_intr00", 68, 52));

		AddWarp(To("moc_intr00", 86, 37), From("moc_intr00", 102, 52));
		AddWarp(To("moc_intr00", 102, 48), From("moc_intr00", 86, 33));

		AddWarp(To("moc_intr00", 142, 25), From("moc_intr00", 148, 156));
		AddWarp(To("moc_intr00", 152, 156), From("moc_intr00", 142, 21));

		AddWarp(To("moc_intr00", 125, 180), From("moc_intr00", 160, 180));
		AddWarp(To("moc_intr00", 156, 180), From("moc_intr00", 121, 180));

		AddWarp(To("moc_intr00", 139, 67), From("moc_intr00", 125, 44));

		// Morocc Village
		AddWarp(To("moc_vilg01", 120, 177), From("moc_fild02", 104, 20));
		AddWarp(To("moc_fild02", 104, 17), From("moc_vilg01", 120, 174));

		AddWarp(To("moc_vilg01", 48, 163), From("moc_intr04", 151, 101));
		AddWarp(To("moc_intr04", 151, 98), From("moc_vilg01", 51, 163));

		AddWarp(To("moc_vilg01", 60, 97), From("moc_intr01", 114, 164));
		AddWarp(To("moc_intr01", 114, 160), From("moc_vilg01", 63, 94));

		AddWarp(To("moc_vilg01", 81, 128), From("moc_intr01", 72, 132));
		AddWarp(To("moc_intr01", 72, 128), From("moc_vilg01", 84, 125));

		AddWarp(To("moc_vilg01", 64, 47), From("moc_intr01", 18, 174));
		AddWarp(To("moc_intr01", 14, 174), From("moc_vilg01", 60, 47));

		AddWarp(To("moc_vilg01", 78, 47), From("moc_intr01", 75, 176));
		AddWarp(To("moc_intr01", 79, 176), From("moc_vilg01", 82, 47));

		AddWarp(To("moc_vilg01", 128, 37), From("moc_intr01", 181, 26));
		AddWarp(To("moc_intr01", 185, 26), From("moc_vilg01", 132, 37));

		AddWarp(To("moc_vilg01", 63, 58), From("moc_intr01", 106, 72));
		AddWarp(To("moc_intr01", 106, 68), From("moc_vilg01", 63, 54));

		AddWarp(To("moc_vilg01", 129, 160), From("moc_intr01", 134, 46));
		AddWarp(To("moc_intr01", 130, 46), From("moc_vilg01", 125, 160));

		AddWarp(To("moc_vilg01", 157, 80), From("moc_intr01", 146, 144));
		AddWarp(To("moc_intr01", 142, 144), From("moc_vilg01", 153, 80));

		AddWarp(To("moc_vilg01", 68, 72), From("moc_intr01", 72, 85));
		AddWarp(To("moc_intr01", 72, 89), From("moc_vilg01", 68, 76));

		AddWarp(To("moc_vilg01", 54, 38), From("moc_intr01", 76, 27));
		AddWarp(To("moc_intr01", 76, 31), From("moc_vilg01", 54, 42));

		AddWarp(To("moc_vilg01", 99, 143), From("moc_intr01", 60, 42));
		AddWarp(To("moc_intr01", 60, 38), From("moc_vilg01", 99, 139));

		AddWarp(To("moc_vilg01", 161, 130), From("moc_intr01", 106, 24));
		AddWarp(To("moc_intr01", 102, 24), From("moc_vilg01", 157, 130));

		AddWarp(To("moc_vilg01", 146, 155), From("moc_intr01", 147, 80));
		AddWarp(To("moc_intr01", 151, 80), From("moc_vilg01", 150, 155));

		AddWarp(To("moc_vilg01", 118, 84), From("moc_intr01", 30, 75));
		AddWarp(To("moc_intr01", 30, 79), From("moc_vilg01", 118, 88));

		AddWarp(To("moc_vilg01", 130, 88), From("moc_intr01", 92, 57));
		AddWarp(To("moc_intr01", 92, 61), From("moc_vilg01", 130, 92));

		// Inside Morocc Village
		AddWarp(To("moc_intr01", 96, 174), From("moc_intr01", 165, 176));
		AddWarp(To("moc_intr01", 169, 176), From("moc_intr01", 100, 174));

		AddWarp(To("moc_intr01", 54, 140), From("moc_intr01", 29, 136));
		AddWarp(To("moc_intr01", 33, 136), From("moc_intr01", 58, 140));

		AddWarp(To("moc_intr01", 89, 140), From("moc_intr01", 110, 136));
		AddWarp(To("moc_intr01", 106, 136), From("moc_intr01", 85, 140));

		AddWarp(To("moc_intr01", 22, 140), From("moc_intr01", 18, 102));
		AddWarp(To("moc_intr01", 14, 102), From("moc_intr01", 18, 140));

		AddWarp(To("moc_intr01", 117, 140), From("moc_intr01", 59, 102));
		AddWarp(To("moc_intr01", 63, 102), From("moc_intr01", 121, 140));

		AddWarp(To("moc_intr01", 26, 107), From("moc_intr01", 86, 104));
		AddWarp(To("moc_intr01", 86, 100), From("moc_intr01", 26, 103));

		AddWarp(To("moc_intr01", 26, 96), From("moc_intr01", 150, 115));
		AddWarp(To("moc_intr01", 150, 119), From("moc_intr01", 26, 100));

		AddWarp(To("moc_intr01", 38, 92), From("moc_intr01", 24, 43));
		AddWarp(To("moc_intr01", 24, 47), From("moc_intr01", 38, 96));

		AddWarp(To("moc_intr01", 50, 107), From("moc_intr01", 118, 100));
		AddWarp(To("moc_intr01", 118, 96), From("moc_intr01", 50, 103));

		AddWarp(To("moc_intr01", 50, 96), From("moc_intr01", 176, 99));
		AddWarp(To("moc_intr01", 176, 103), From("moc_intr01", 50, 100));

		AddWarp(To("moc_intr01", 39, 174), From("moc_intr01", 61, 176));
		AddWarp(To("moc_intr01", 57, 176), From("moc_intr01", 35, 174));

		// Pyramid Dungeon 1F
		AddWarp(To("moc_dugn01", 188, 10), From("moc_intr04", 32, 26));
		AddWarp(To("moc_intr04", 32, 30), From("moc_dugn01", 188, 13));

		AddWarp(To("moc_dugn01", 32, 24), From("moc_intr04", 43, 124));
		AddWarp(To("moc_intr04", 43, 120), From("moc_dugn01", 28, 24));

		AddWarp(To("moc_dugn01", 43, 186), From("moc_intr04", 43, 179));
		AddWarp(To("moc_intr04", 43, 183), From("moc_dugn01", 47, 186));

		AddWarp(To("moc_dugn01", 182, 128), From("moc_intr04", 118, 77));
		AddWarp(To("moc_intr04", 118, 81), From("moc_dugn01", 182, 124));

		// Pyramid Dungeon 2F
		AddWarp(To("moc_intr04", 93, 80), From("moc_dugn02", 73, 182));
		AddWarp(To("moc_dugn02", 76, 182), From("moc_intr04", 93, 77));

		AddWarp(To("moc_dugn02", 181, 115), From("moc_intr04", 67, 123));
		AddWarp(To("moc_intr04", 67, 120), From("moc_dugn02", 181, 119));

		AddWarp(To("moc_dugn02", 29, 28), From("moc_intr04", 118, 22));
		AddWarp(To("moc_intr04", 118, 19), From("moc_dugn02", 32, 28));

		// Inside Pyramid
		AddWarp(To("moc_intr04", 29, 17), From("moc_intr04", 142, 75));
		AddWarp(To("moc_intr04", 142, 79), From("moc_intr04", 29, 21));

		AddWarp(To("moc_intr04", 142, 19), From("moc_intr04", 168, 137));
		AddWarp(To("moc_intr04", 168, 133), From("moc_intr04", 142, 23));

		AddWarp(To("moc_intr04", 168, 153), From("moc_intr04", 178, 172));
		AddWarp(To("moc_intr04", 178, 168), From("moc_intr04", 168, 149));

		AddWarp(To("moc_intr04", 149, 113), From("moc_intr04", 34, 21));
		AddWarp(To("moc_intr04", 34, 17), From("moc_intr04", 149, 109));

		AddWarp(To("moc_intr04", 154, 113), From("moc_intr04", 177, 106));
		AddWarp(To("moc_intr04", 177, 109), From("moc_intr04", 154, 110));

		AddWarp(To("moc_intr04", 182, 109), From("moc_intr04", 177, 74));
		AddWarp(To("moc_intr04", 177, 77), From("moc_intr04", 182, 106));

		AddWarp(To("moc_intr04", 182, 77), From("moc_intr04", 123, 117));
		AddWarp(To("moc_intr04", 123, 114), From("moc_intr04", 182, 74));

		AddWarp(To("moc_intr04", 179, 94), From("moc_intr04", 19, 179));
		AddWarp(To("moc_intr04", 19, 182), From("moc_intr04", 179, 97));

		AddWarp(To("moc_intr04", 179, 62), From("moc_intr04", 91, 181));
		AddWarp(To("moc_intr04", 91, 184), From("moc_intr04", 179, 65));

		AddWarp(To("moc_intr04", 113, 145), From("moc_intr04", 173, 19));
		AddWarp(To("moc_intr04", 173, 16), From("moc_intr04", 113, 142));

		AddWarp(To("moc_intr04", 93, 18), From("moc_intr04", 123, 184));
		AddWarp(To("moc_intr04", 123, 187), From("moc_intr04", 93, 21));

		AddWarp(To("moc_intr04", 123, 164), From("moc_intr04", 67, 179));
		AddWarp(To("moc_intr04", 67, 182), From("moc_intr04", 123, 167));

		// Morocc East Field
		AddWarp(To("moc_vilg01", 178, 117), From("moc_fild03", 19, 39));
		AddWarp(To("moc_fild03", 16, 39), From("moc_vilg01", 175, 117));

		AddWarp(To("moc_fild03", 178, 179), From("moc_vilg02", 44, 40));
		AddWarp(To("moc_vilg02", 41, 37), From("moc_fild03", 175, 176));

		// Archers' Village
		AddWarp(To("moc_vilg02", 157, 71), From("dungeon000", 25, 23));
		AddWarp(To("dungeon000", 22, 20), From("moc_vilg02", 153, 67));

		AddWarp(To("moc_vilg02", 118, 116), From("moc_intr02", 26, 159));
		AddWarp(To("moc_intr02", 26, 155), From("moc_vilg02", 118, 112));

		AddWarp(To("moc_vilg02", 44, 62), From("moc_intr02", 28, 122));
		AddWarp(To("moc_intr02", 28, 118), From("moc_vilg02", 47, 61));

		AddWarp(To("moc_vilg02", 96, 51), From("moc_intr02", 100, 126));
		AddWarp(To("moc_intr02", 96, 126), From("moc_vilg02", 93, 48));

		AddWarp(To("moc_vilg02", 60, 81), From("moc_intr02", 39, 90));
		AddWarp(To("moc_intr02", 43, 90), From("moc_vilg02", 60, 77));

		// Inside Archers' village
		AddWarp(To("moc_intr02", 17, 174), From("moc_intr02", 63, 174));
		AddWarp(To("moc_intr02", 67, 174), From("moc_intr02", 21, 174));

		AddWarp(To("moc_intr02", 34, 174), From("moc_intr02", 88, 174));
		AddWarp(To("moc_intr02", 84, 174), From("moc_intr02", 30, 174));

		AddWarp(To("moc_intr02", 30, 183), From("moc_intr02", 128, 175));
		AddWarp(To("moc_intr02", 128, 171), From("moc_intr02", 30, 179));

		AddWarp(To("moc_intr02", 124, 183), From("moc_intr02", 156, 168));
		AddWarp(To("moc_intr02", 156, 164), From("moc_intr02", 124, 179));

		AddWarp(To("moc_intr02", 16, 130), From("moc_intr02", 75, 130));
		AddWarp(To("moc_intr02", 79, 130), From("moc_intr02", 20, 130));

		AddWarp(To("moc_intr02", 108, 137), From("moc_intr02", 140, 134));
		AddWarp(To("moc_intr02", 140, 130), From("moc_intr02", 108, 133));

		AddWarp(To("moc_intr02", 28, 78), From("moc_intr02", 70, 101));
		AddWarp(To("moc_intr02", 70, 105), From("moc_intr02", 28, 82));

		AddWarp(To("moc_intr02", 29, 98), From("moc_intr02", 105, 87));
		AddWarp(To("moc_intr02", 101, 87), From("moc_intr02", 25, 98));

		// Morocc Jungle
		AddWarp(To("dungeon000", 134, 125), From("dungeon001", 99, 113));
		AddWarp(To("dungeon001", 99, 109), From("dungeon000", 129, 123));
	}
}
