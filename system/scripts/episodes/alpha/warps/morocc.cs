//--- Sabine Script ---------------------------------------------------------
// Morocc Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Morocc and its surrounding maps.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("moc_vilg00", "moc_intr00", "moc_fild01", "dungeon000")]
public class MoroccWarpsScript : GeneralScript
{
	public override void Load()
	{
		// Morocc South-East Field
		AddWarp(From("moc_fild04", 162, 194), To("prt_fild00", 79, 21));

		AddWarp(From("moc_fild04", 12, 63), To("moc_fild01", 185, 114));
		AddWarp(From("moc_fild01", 188, 114), To("moc_fild04", 15, 63));

		// Morocc South Field
		AddWarp(From("moc_fild01", 103, 182), To("moc_vilg00", 100, 24));
		AddWarp(From("moc_vilg00", 99, 21), To("moc_fild01", 103, 179));

		// Morocc Castle
		AddWarp(From("moc_vilg00", 99, 179), To("moc_vilg01", 99, 24));
		AddWarp(From("moc_vilg01", 99, 21), To("moc_vilg00", 99, 176));

		AddWarp(From("moc_vilg00", 59, 114), To("moc_intr00", 24, 154));
		AddWarp(From("moc_intr00", 24, 150), To("moc_vilg00", 62, 111));

		AddWarp(From("moc_vilg00", 152, 124), To("moc_intr00", 58, 154));
		AddWarp(From("moc_intr00", 58, 150), To("moc_vilg00", 152, 120));

		AddWarp(From("moc_vilg00", 152, 55), To("moc_intr00", 92, 177));
		AddWarp(From("moc_intr00", 92, 181), To("moc_vilg00", 149, 58));

		AddWarp(From("moc_vilg00", 100, 91), To("moc_intr00", 44, 133));
		AddWarp(From("moc_intr00", 44, 137), To("moc_vilg00", 100, 95));

		// Inside Morocc Castle
		AddWarp(From("moc_intr00", 17, 102), To("moc_intr00", 103, 94));
		AddWarp(From("moc_intr00", 107, 94), To("moc_intr00", 21, 102));

		AddWarp(From("moc_intr00", 71, 102), To("moc_intr00", 88, 126));
		AddWarp(From("moc_intr00", 84, 126), To("moc_intr00", 67, 102));

		AddWarp(From("moc_intr00", 93, 96), To("moc_intr00", 172, 134));
		AddWarp(From("moc_intr00", 176, 134), To("moc_intr00", 97, 96));

		AddWarp(From("moc_intr00", 98, 128), To("moc_intr00", 133, 122));
		AddWarp(From("moc_intr00", 129, 122), To("moc_intr00", 94, 128));

		AddWarp(From("moc_intr00", 98, 124), To("moc_intr00", 104, 20));
		AddWarp(From("moc_intr00", 100, 20), To("moc_intr00", 94, 124));

		AddWarp(From("moc_intr00", 93, 92), To("moc_intr00", 159, 26));
		AddWarp(From("moc_intr00", 163, 26), To("moc_intr00", 97, 92));

		AddWarp(From("moc_intr00", 44, 109), To("moc_intr00", 58, 57));
		AddWarp(From("moc_intr00", 58, 61), To("moc_intr00", 44, 113));

		AddWarp(From("moc_intr00", 58, 44), To("moc_intr00", 18, 63));
		AddWarp(From("moc_intr00", 18, 67), To("moc_intr00", 58, 48));

		AddWarp(From("moc_intr00", 22, 62), To("moc_intr00", 182, 20));
		AddWarp(From("moc_intr00", 182, 16), To("moc_intr00", 22, 58));

		AddWarp(From("moc_intr00", 182, 47), To("moc_intr00", 168, 79));
		AddWarp(From("moc_intr00", 168, 75), To("moc_intr00", 182, 43));

		AddWarp(From("moc_intr00", 18, 51), To("moc_intr00", 126, 90));
		AddWarp(From("moc_intr00", 126, 94), To("moc_intr00", 18, 55));

		AddWarp(From("moc_intr00", 48, 48), To("moc_intr00", 22, 29));
		AddWarp(From("moc_intr00", 22, 33), To("moc_intr00", 48, 52));

		AddWarp(From("moc_intr00", 68, 48), To("moc_intr00", 50, 29));
		AddWarp(From("moc_intr00", 50, 33), To("moc_intr00", 68, 52));

		AddWarp(From("moc_intr00", 86, 37), To("moc_intr00", 102, 52));
		AddWarp(From("moc_intr00", 102, 48), To("moc_intr00", 86, 33));

		AddWarp(From("moc_intr00", 142, 25), To("moc_intr00", 148, 156));
		AddWarp(From("moc_intr00", 152, 156), To("moc_intr00", 142, 21));

		AddWarp(From("moc_intr00", 125, 180), To("moc_intr00", 160, 180));
		AddWarp(From("moc_intr00", 156, 180), To("moc_intr00", 121, 180));

		AddWarp(From("moc_intr00", 139, 67), To("moc_intr00", 125, 44));

		// Morocc Village
		AddWarp(From("moc_vilg01", 120, 177), To("moc_fild02", 104, 20));
		AddWarp(From("moc_fild02", 104, 17), To("moc_vilg01", 120, 174));

		AddWarp(From("moc_vilg01", 48, 163), To("moc_intr04", 151, 101));
		AddWarp(From("moc_intr04", 151, 98), To("moc_vilg01", 51, 163));

		AddWarp(From("moc_vilg01", 60, 97), To("moc_intr01", 114, 164));
		AddWarp(From("moc_intr01", 114, 160), To("moc_vilg01", 63, 94));

		AddWarp(From("moc_vilg01", 81, 128), To("moc_intr01", 72, 132));
		AddWarp(From("moc_intr01", 72, 128), To("moc_vilg01", 84, 125));

		AddWarp(From("moc_vilg01", 64, 47), To("moc_intr01", 18, 174));
		AddWarp(From("moc_intr01", 14, 174), To("moc_vilg01", 60, 47));

		AddWarp(From("moc_vilg01", 78, 47), To("moc_intr01", 75, 176));
		AddWarp(From("moc_intr01", 79, 176), To("moc_vilg01", 82, 47));

		AddWarp(From("moc_vilg01", 128, 37), To("moc_intr01", 181, 26));
		AddWarp(From("moc_intr01", 185, 26), To("moc_vilg01", 132, 37));

		AddWarp(From("moc_vilg01", 63, 58), To("moc_intr01", 106, 72));
		AddWarp(From("moc_intr01", 106, 68), To("moc_vilg01", 63, 54));

		AddWarp(From("moc_vilg01", 129, 160), To("moc_intr01", 134, 46));
		AddWarp(From("moc_intr01", 130, 46), To("moc_vilg01", 125, 160));

		AddWarp(From("moc_vilg01", 157, 80), To("moc_intr01", 146, 144));
		AddWarp(From("moc_intr01", 142, 144), To("moc_vilg01", 153, 80));

		AddWarp(From("moc_vilg01", 68, 72), To("moc_intr01", 72, 85));
		AddWarp(From("moc_intr01", 72, 89), To("moc_vilg01", 68, 76));

		AddWarp(From("moc_vilg01", 54, 38), To("moc_intr01", 76, 27));
		AddWarp(From("moc_intr01", 76, 31), To("moc_vilg01", 54, 42));

		AddWarp(From("moc_vilg01", 99, 143), To("moc_intr01", 60, 42));
		AddWarp(From("moc_intr01", 60, 38), To("moc_vilg01", 99, 139));

		AddWarp(From("moc_vilg01", 161, 130), To("moc_intr01", 106, 24));
		AddWarp(From("moc_intr01", 102, 24), To("moc_vilg01", 157, 130));

		AddWarp(From("moc_vilg01", 146, 155), To("moc_intr01", 147, 80));
		AddWarp(From("moc_intr01", 151, 80), To("moc_vilg01", 150, 155));

		AddWarp(From("moc_vilg01", 118, 84), To("moc_intr01", 30, 75));
		AddWarp(From("moc_intr01", 30, 79), To("moc_vilg01", 118, 88));

		AddWarp(From("moc_vilg01", 130, 88), To("moc_intr01", 92, 57));
		AddWarp(From("moc_intr01", 92, 61), To("moc_vilg01", 130, 92));

		// Inside Morocc Village
		AddWarp(From("moc_intr01", 96, 174), To("moc_intr01", 165, 176));
		AddWarp(From("moc_intr01", 169, 176), To("moc_intr01", 100, 174));

		AddWarp(From("moc_intr01", 54, 140), To("moc_intr01", 29, 136));
		AddWarp(From("moc_intr01", 33, 136), To("moc_intr01", 58, 140));

		AddWarp(From("moc_intr01", 89, 140), To("moc_intr01", 110, 136));
		AddWarp(From("moc_intr01", 106, 136), To("moc_intr01", 85, 140));

		AddWarp(From("moc_intr01", 22, 140), To("moc_intr01", 18, 102));
		AddWarp(From("moc_intr01", 14, 102), To("moc_intr01", 18, 140));

		AddWarp(From("moc_intr01", 117, 140), To("moc_intr01", 59, 102));
		AddWarp(From("moc_intr01", 63, 102), To("moc_intr01", 121, 140));

		AddWarp(From("moc_intr01", 26, 107), To("moc_intr01", 86, 104));
		AddWarp(From("moc_intr01", 86, 100), To("moc_intr01", 26, 103));

		AddWarp(From("moc_intr01", 26, 96), To("moc_intr01", 150, 115));
		AddWarp(From("moc_intr01", 150, 119), To("moc_intr01", 26, 100));

		AddWarp(From("moc_intr01", 38, 92), To("moc_intr01", 24, 43));
		AddWarp(From("moc_intr01", 24, 47), To("moc_intr01", 38, 96));

		AddWarp(From("moc_intr01", 50, 107), To("moc_intr01", 118, 100));
		AddWarp(From("moc_intr01", 118, 96), To("moc_intr01", 50, 103));

		AddWarp(From("moc_intr01", 50, 96), To("moc_intr01", 176, 99));
		AddWarp(From("moc_intr01", 176, 103), To("moc_intr01", 50, 100));

		AddWarp(From("moc_intr01", 39, 174), To("moc_intr01", 61, 176));
		AddWarp(From("moc_intr01", 57, 176), To("moc_intr01", 35, 174));

		// Pyramid Dungeon 1F
		AddWarp(From("moc_dugn01", 188, 10), To("moc_intr04", 32, 26));
		AddWarp(From("moc_intr04", 32, 30), To("moc_dugn01", 188, 13));

		AddWarp(From("moc_dugn01", 32, 24), To("moc_intr04", 43, 124));
		AddWarp(From("moc_intr04", 43, 120), To("moc_dugn01", 28, 24));

		AddWarp(From("moc_dugn01", 43, 186), To("moc_intr04", 43, 179));
		AddWarp(From("moc_intr04", 43, 183), To("moc_dugn01", 47, 186));

		AddWarp(From("moc_dugn01", 182, 128), To("moc_intr04", 118, 77));
		AddWarp(From("moc_intr04", 118, 81), To("moc_dugn01", 182, 124));

		// Pyramid Dungeon 2F
		AddWarp(From("moc_intr04", 93, 80), To("moc_dugn02", 73, 182));
		AddWarp(From("moc_dugn02", 76, 182), To("moc_intr04", 93, 77));

		AddWarp(From("moc_dugn02", 181, 115), To("moc_intr04", 67, 123));
		AddWarp(From("moc_intr04", 67, 120), To("moc_dugn02", 181, 119));

		AddWarp(From("moc_dugn02", 29, 28), To("moc_intr04", 118, 22));
		AddWarp(From("moc_intr04", 118, 19), To("moc_dugn02", 32, 28));

		// Inside Pyramid
		AddWarp(From("moc_intr04", 29, 17), To("moc_intr04", 142, 75));
		AddWarp(From("moc_intr04", 142, 79), To("moc_intr04", 29, 21));

		AddWarp(From("moc_intr04", 142, 19), To("moc_intr04", 168, 137));
		AddWarp(From("moc_intr04", 168, 133), To("moc_intr04", 142, 23));

		AddWarp(From("moc_intr04", 168, 153), To("moc_intr04", 178, 172));
		AddWarp(From("moc_intr04", 178, 168), To("moc_intr04", 168, 149));

		AddWarp(From("moc_intr04", 149, 113), To("moc_intr04", 34, 21));
		AddWarp(From("moc_intr04", 34, 17), To("moc_intr04", 149, 109));

		AddWarp(From("moc_intr04", 154, 113), To("moc_intr04", 177, 106));
		AddWarp(From("moc_intr04", 177, 109), To("moc_intr04", 154, 110));

		AddWarp(From("moc_intr04", 182, 109), To("moc_intr04", 177, 74));
		AddWarp(From("moc_intr04", 177, 77), To("moc_intr04", 182, 106));

		AddWarp(From("moc_intr04", 182, 77), To("moc_intr04", 123, 117));
		AddWarp(From("moc_intr04", 123, 114), To("moc_intr04", 182, 74));

		AddWarp(From("moc_intr04", 179, 94), To("moc_intr04", 19, 179));
		AddWarp(From("moc_intr04", 19, 182), To("moc_intr04", 179, 97));

		AddWarp(From("moc_intr04", 179, 62), To("moc_intr04", 91, 181));
		AddWarp(From("moc_intr04", 91, 184), To("moc_intr04", 179, 65));

		AddWarp(From("moc_intr04", 113, 145), To("moc_intr04", 173, 19));
		AddWarp(From("moc_intr04", 173, 16), To("moc_intr04", 113, 142));

		AddWarp(From("moc_intr04", 93, 18), To("moc_intr04", 123, 184));
		AddWarp(From("moc_intr04", 123, 187), To("moc_intr04", 93, 21));

		AddWarp(From("moc_intr04", 123, 164), To("moc_intr04", 67, 179));
		AddWarp(From("moc_intr04", 67, 182), To("moc_intr04", 123, 167));

		// Morocc East Field
		AddWarp(From("moc_vilg01", 178, 117), To("moc_fild03", 19, 39));
		AddWarp(From("moc_fild03", 16, 39), To("moc_vilg01", 175, 117));

		AddWarp(From("moc_fild03", 178, 179), To("moc_vilg02", 44, 40));
		AddWarp(From("moc_vilg02", 41, 37), To("moc_fild03", 175, 176));

		// Archers' Village
		AddWarp(From("moc_vilg02", 157, 71), To("dungeon000", 25, 23));
		AddWarp(From("dungeon000", 22, 20), To("moc_vilg02", 153, 67));

		AddWarp(From("moc_vilg02", 118, 116), To("moc_intr02", 26, 159));
		AddWarp(From("moc_intr02", 26, 155), To("moc_vilg02", 118, 112));

		AddWarp(From("moc_vilg02", 44, 62), To("moc_intr02", 28, 122));
		AddWarp(From("moc_intr02", 28, 118), To("moc_vilg02", 47, 61));

		AddWarp(From("moc_vilg02", 96, 51), To("moc_intr02", 100, 126));
		AddWarp(From("moc_intr02", 96, 126), To("moc_vilg02", 93, 48));

		AddWarp(From("moc_vilg02", 60, 81), To("moc_intr02", 39, 90));
		AddWarp(From("moc_intr02", 43, 90), To("moc_vilg02", 60, 77));

		// Inside Archers' village
		AddWarp(From("moc_intr02", 17, 174), To("moc_intr02", 63, 174));
		AddWarp(From("moc_intr02", 67, 174), To("moc_intr02", 21, 174));

		AddWarp(From("moc_intr02", 34, 174), To("moc_intr02", 88, 174));
		AddWarp(From("moc_intr02", 84, 174), To("moc_intr02", 30, 174));

		AddWarp(From("moc_intr02", 30, 183), To("moc_intr02", 128, 175));
		AddWarp(From("moc_intr02", 128, 171), To("moc_intr02", 30, 179));

		AddWarp(From("moc_intr02", 124, 183), To("moc_intr02", 156, 168));
		AddWarp(From("moc_intr02", 156, 164), To("moc_intr02", 124, 179));

		AddWarp(From("moc_intr02", 16, 130), To("moc_intr02", 75, 130));
		AddWarp(From("moc_intr02", 79, 130), To("moc_intr02", 20, 130));

		AddWarp(From("moc_intr02", 108, 137), To("moc_intr02", 140, 134));
		AddWarp(From("moc_intr02", 140, 130), To("moc_intr02", 108, 133));

		AddWarp(From("moc_intr02", 28, 78), To("moc_intr02", 70, 101));
		AddWarp(From("moc_intr02", 70, 105), To("moc_intr02", 28, 82));

		AddWarp(From("moc_intr02", 29, 98), To("moc_intr02", 105, 87));
		AddWarp(From("moc_intr02", 101, 87), To("moc_intr02", 25, 98));

		// Morocc Jungle
		AddWarp(From("dungeon000", 134, 125), To("dungeon001", 99, 113));
		AddWarp(From("dungeon001", 99, 109), To("dungeon000", 129, 123));
	}
}
