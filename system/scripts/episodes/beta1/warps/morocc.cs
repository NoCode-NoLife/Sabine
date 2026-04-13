//--- Sabine Script ---------------------------------------------------------
// Morocc Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Morocc and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("morocc", "morocc_in")]
public class MoroccBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Morocc Town
		AddWarp(From("morocc", 26, 297), To("moc_ruins", 156, 42));
		AddWarp(From("moc_ruins", 161, 40), To("morocc", 26, 293));

		AddWarp(From("morocc", 26, 297), To("moc_ruins", 156, 42));
		AddWarp(From("moc_ruins", 157, 37), To("morocc", 26, 293));

		AddWarp(From("morocc", 197, 66), To("morocc_in", 83, 92));
		AddWarp(From("morocc_in", 83, 90), To("morocc", 199, 64));

		AddWarp(From("morocc", 22, 294), To("moc_ruins", 156, 42));
		AddWarp(From("moc_ruins", 157, 37), To("morocc", 26, 293));

		AddWarp(From("morocc", 253, 56), To("morocc_in", 134, 74));
		AddWarp(From("morocc_in", 134, 77), To("morocc", 251, 59));

		AddWarp(From("morocc", 274, 269), To("morocc_in", 138, 136));
		AddWarp(From("morocc_in", 136, 136), To("morocc", 271, 269));

		AddWarp(From("morocc", 46, 46), To("morocc_in", 68, 72));
		AddWarp(From("morocc_in", 68, 75), To("morocc", 49, 49));

		AddWarp(From("morocc", 52, 259), To("morocc_in", 180, 65));
		AddWarp(From("morocc_in", 183, 65), To("morocc", 55, 259));

		AddWarp(From("morocc", 84, 170), To("morocc_in", 108, 176));
		AddWarp(From("morocc_in", 108, 179), To("morocc", 283, 173));

		AddWarp(From("morocc", 85, 55), To("morocc_in", 44, 149));
		AddWarp(From("morocc_in", 44, 146), To("morocc", 82, 52));

		AddWarp(From("morocc", 98, 68), To("morocc_in", 44, 175));
		AddWarp(From("morocc_in", 44, 178), To("morocc", 100, 70));

		AddWarp(From("morocc", 160, 297), To("moc_fild07", 198, 25));
		AddWarp(From("moc_fild07", 198, 21), To("morocc", 160, 294));

		AddWarp(From("morocc_in", 93, 95), To("morocc_in", 109, 95));
		AddWarp(From("morocc_in", 105, 95), To("morocc_in", 90, 95));

		AddWarp(From("morocc_in", 93, 123), To("morocc_in", 109, 123));
		AddWarp(From("morocc_in", 106, 123), To("morocc_in", 90, 123));

		AddWarp(From("morocc_in", 144, 122), To("morocc_in", 144, 106));
		AddWarp(From("morocc_in", 144, 109), To("morocc_in", 144, 125));

		AddWarp(From("morocc_in", 144, 151), To("morocc_in", 144, 169));
		AddWarp(From("morocc_in", 144, 166), To("morocc_in", 144, 148));

		AddWarp(From("morocc_in", 149, 129), To("morocc_in", 169, 129));
		AddWarp(From("morocc_in", 166, 130), To("morocc_in", 146, 130));

		AddWarp(From("morocc_in", 171, 50), To("morocc_in", 171, 35));
		AddWarp(From("morocc_in", 171, 37), To("morocc_in", 171, 52));

		AddWarp(From("morocc_in", 174, 151), To("morocc_in", 174, 169));
		AddWarp(From("morocc_in", 174, 166), To("morocc_in", 174, 148));

		AddWarp(From("morocc_in", 23, 161), To("morocc_in", 37, 161));
		AddWarp(From("morocc_in", 34, 161), To("morocc_in", 20, 161));

		AddWarp(From("morocc_in", 68, 123), To("morocc_in", 52, 123));
		AddWarp(From("morocc_in", 55, 123), To("morocc_in", 71, 123));

		AddWarp(From("morocc_in", 68, 95), To("morocc_in", 52, 95));
		AddWarp(From("morocc_in", 55, 95), To("morocc_in", 71, 95));

		AddWarp(From("morocc_in", 57, 161), To("morocc_in", 73, 161));
		AddWarp(From("morocc_in", 70, 161), To("morocc_in", 54, 161));

		AddWarp(From("morocc_in", 68, 62), To("morocc_in", 68, 38));
		AddWarp(From("morocc_in", 68, 42), To("morocc_in", 68, 65));

		AddWarp(From("morocc_in", 74, 109), To("morocc_in", 174, 125));
		AddWarp(From("morocc_in", 174, 122), To("morocc_in", 174, 106));

		AddWarp(From("morocc_in", 86, 101), To("morocc_in", 86, 120));
		AddWarp(From("morocc_in", 86, 117), To("morocc_in", 86, 98));

		AddWarp(From("morocc_in", 72, 67), To("morocc_in", 44, 162));
		AddWarp(From("morocc_in", 34, 161), To("morocc_in", 20, 161));

		AddWarp(From("morocc_in", 108, 179), To("morocc", 284, 175));

		// Morocc Castle
		AddWarp(From("morocc", 160, 183), To("moc_castle", 94, 181));
		AddWarp(From("moc_castle", 94, 183), To("morocc", 160, 185));

		AddWarp(From("moc_castle", 107, 163), To("moc_castle", 124, 163));
		AddWarp(From("moc_castle", 120, 163), To("moc_castle", 103, 163));

		AddWarp(From("moc_castle", 120, 75), To("moc_castle", 56, 33));
		AddWarp(From("moc_castle", 59, 34), To("moc_castle", 124, 75));

		AddWarp(From("moc_castle", 134, 101), To("moc_castle", 134, 128));
		AddWarp(From("moc_castle", 134, 124), To("moc_castle", 134, 98));

		AddWarp(From("moc_castle", 134, 139), To("moc_castle", 134, 160));
		AddWarp(From("moc_castle", 134, 156), To("moc_castle", 134, 136));

		AddWarp(From("moc_castle", 149, 163), To("moc_castle", 162, 163));
		AddWarp(From("moc_castle", 158, 163), To("moc_castle", 145, 163));

		AddWarp(From("moc_castle", 16, 131), To("moc_castle", 16, 164));
		AddWarp(From("moc_castle", 16, 160), To("moc_castle", 16, 125));

		AddWarp(From("moc_castle", 170, 131), To("moc_castle", 170, 163));
		AddWarp(From("moc_castle", 170, 160), To("moc_castle", 170, 128));

		AddWarp(From("moc_castle", 29, 163), To("moc_castle", 44, 163));
		AddWarp(From("moc_castle", 40, 163), To("moc_castle", 25, 163));

		AddWarp(From("moc_castle", 51, 114), To("moc_castle", 54, 65));
		AddWarp(From("moc_castle", 54, 69), To("moc_castle", 52, 117));

		AddWarp(From("moc_castle", 54, 139), To("moc_castle", 54, 160));
		AddWarp(From("moc_castle", 54, 156), To("moc_castle", 54, 134));

		AddWarp(From("moc_castle", 63, 89), To("moc_castle", 83, 89));
		AddWarp(From("moc_castle", 80, 89), To("moc_castle", 60, 89));

		AddWarp(From("moc_castle", 69, 163), To("moc_castle", 86, 163));
		AddWarp(From("moc_castle", 82, 163), To("moc_castle", 66, 163));

		AddWarp(From("moc_castle", 88, 93), To("moc_castle", 94, 119));
		AddWarp(From("moc_castle", 94, 116), To("moc_castle", 88, 90));

		AddWarp(From("moc_castle", 96, 90), To("moc_castle", 94, 119));
		AddWarp(From("moc_castle", 94, 116), To("moc_castle", 88, 90));

		AddWarp(From("moc_castle", 92, 67), To("moc_castle", 92, 85));
		AddWarp(From("moc_castle", 92, 82), To("moc_castle", 92, 63));

		AddWarp(From("moc_castle", 94, 143), To("moc_castle", 94, 160));
		AddWarp(From("moc_castle", 94, 156), To("moc_castle", 94, 140));

		// Morocc Fields
		AddWarp(From("moc_fild01", 239, 382), To("prt_fild08", 233, 20));
		AddWarp(From("prt_fild08", 233, 17), To("moc_fild01", 239, 379));

		AddWarp(From("moc_fild01", 56, 384), To("prt_fild08", 54, 24));
		AddWarp(From("prt_fild08", 56, 20), To("moc_fild01", 56, 381));

		AddWarp(From("moc_fild01", 68, 16), To("moc_fild04", 317, 376));
		AddWarp(From("moc_fild04", 314, 381), To("moc_fild01", 76, 25));

		AddWarp(From("moc_fild01", 101, 16), To("moc_fild04", 317, 376));
		AddWarp(From("moc_fild04", 314, 381), To("moc_fild01", 76, 25));

		AddWarp(From("moc_fild01", 301, 16), To("moc_fild02", 77, 338));
		AddWarp(From("moc_fild02", 67, 342), To("moc_fild01", 315, 25));

		AddWarp(From("moc_fild01", 321, 16), To("moc_fild02", 77, 338));
		AddWarp(From("moc_fild02", 67, 342), To("moc_fild01", 315, 25));

		AddWarp(From("moc_fild01", 341, 16), To("moc_fild02", 77, 338));
		AddWarp(From("moc_fild02", 67, 342), To("moc_fild01", 315, 25));

		AddWarp(From("moc_fild02", 92, 342), To("moc_fild01", 315, 25));
		AddWarp(From("moc_fild01", 321, 16), To("moc_fild02", 77, 338));

		AddWarp(From("moc_fild02", 332, 19), To("moc_fild03", 70, 336));
		AddWarp(From("moc_fild03", 70, 341), To("moc_fild02", 332, 23));

		AddWarp(From("moc_fild04", 14, 98), To("moc_fild05", 378, 119));
		AddWarp(From("moc_fild05", 384, 126), To("moc_fild04", 22, 123));

		AddWarp(From("moc_fild04", 14, 122), To("moc_fild05", 378, 119));
		AddWarp(From("moc_fild05", 384, 126), To("moc_fild04", 22, 123));

		AddWarp(From("moc_fild04", 14, 146), To("moc_fild05", 378, 119));
		AddWarp(From("moc_fild05", 384, 126), To("moc_fild04", 22, 123));

		AddWarp(From("moc_fild04", 19, 206), To("moc_fild05", 373, 208));
		AddWarp(From("moc_fild05", 378, 208), To("moc_fild04", 26, 206));

		AddWarp(From("moc_fild05", 384, 108), To("moc_fild04", 22, 123));
		AddWarp(From("moc_fild04", 14, 122), To("moc_fild05", 378, 119));

		AddWarp(From("moc_fild05", 384, 144), To("moc_fild04", 22, 123));
		AddWarp(From("moc_fild04", 14, 122), To("moc_fild05", 378, 119));

		AddWarp(From("moc_fild05", 18, 136), To("moc_fild06", 367, 317));
		AddWarp(From("moc_fild06", 377, 316), To("moc_fild05", 24, 153));

		AddWarp(From("moc_fild05", 18, 154), To("moc_fild06", 367, 317));
		AddWarp(From("moc_fild06", 377, 316), To("moc_fild05", 24, 153));

		AddWarp(From("moc_fild05", 18, 172), To("moc_fild06", 367, 317));
		AddWarp(From("moc_fild06", 377, 316), To("moc_fild05", 24, 153));

		AddWarp(From("moc_fild06", 18, 198), To("moc_fild07", 378, 201));
		AddWarp(From("moc_fild07", 381, 201), To("moc_fild06", 28, 201));

		AddWarp(From("moc_fild03", 303, 170), To("pay_fild01", 17, 152));
		AddWarp(From("moc_fild04", 292, 381), To("moc_fild01", 76, 25));
		AddWarp(From("moc_fild04", 336, 381), To("moc_fild01", 76, 25));

		// Pyramid
		AddWarp(From("moc_pryd01", 195, 9), To("moc_ruins", 60, 161));
		AddWarp(From("moc_ruins", 54, 161), To("moc_pryd01", 192, 9));

		AddWarp(From("moc_pryd01", 10, 195), To("moc_pryd02", 10, 192));
		AddWarp(From("moc_pryd02", 10, 195), To("moc_pryd01", 10, 192));

		AddWarp(From("moc_pryd02", 100, 99), To("moc_pryd03", 100, 92));
		AddWarp(From("moc_pryd03", 100, 97), To("moc_pryd02", 100, 92));

		AddWarp(From("moc_pryd03", 12, 15), To("moc_pryd04", 12, 18));
		AddWarp(From("moc_pryd04", 12, 15), To("moc_pryd03", 12, 18));

		AddWarp(From("moc_pryd03", 15, 187), To("moc_pryd04", 18, 187));
		AddWarp(From("moc_pryd04", 15, 187), To("moc_pryd03", 18, 187));

		AddWarp(From("moc_pryd03", 184, 11), To("moc_pryd04", 181, 11));
		AddWarp(From("moc_pryd04", 184, 11), To("moc_pryd03", 181, 11));

		AddWarp(From("moc_pryd03", 188, 184), To("moc_pryd04", 188, 181));
		AddWarp(From("moc_pryd04", 188, 184), To("moc_pryd03", 188, 181));
	}
}
