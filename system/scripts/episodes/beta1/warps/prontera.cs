//--- Sabine Script ---------------------------------------------------------
// Prontera Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Prontera and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (1.0)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("prontera", "prontera_in", "prt_castle")]
public class PronteraBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Capital City, Prontera
		AddWarp(From("prontera", 238, 318), To("prontera_in", 128, 105));
		AddWarp(From("prontera_in", 128, 102), To("prontera", 235, 315));

		AddWarp(From("prontera_in", 127, 45), To("prontera", 107, 188));
		AddWarp(From("prontera", 107, 191), To("prontera_in", 127, 42));

		AddWarp(From("prontera", 79, 67), To("prontera_in", 283, 125));
		AddWarp(From("prontera_in", 283, 122), To("prontera", 79, 64));

		AddWarp(From("prontera", 107, 215), To("prontera_in", 240, 139));
		AddWarp(From("prontera_in", 240, 141), To("prontera", 107, 218));

		AddWarp(From("prontera", 120, 267), To("prontera_in", 180, 97));
		AddWarp(From("prontera_in", 183, 97), To("prontera", 120, 264));

		AddWarp(From("prontera", 133, 183), To("prontera_in", 50, 105));
		AddWarp(From("prontera_in", 53, 105), To("prontera", 136, 186));

		AddWarp(From("prontera", 134, 221), To("prontera_in", 131, 71));
		AddWarp(From("prontera_in", 135, 71), To("prontera", 136, 219));

		AddWarp(From("prontera", 177, 221), To("prontera_in", 168, 128));
		AddWarp(From("prontera_in", 168, 124), To("prontera", 174, 218));

		AddWarp(From("prontera", 179, 184), To("prontera_in", 60, 73));
		AddWarp(From("prontera_in", 60, 77), To("prontera", 175, 188));

		AddWarp(From("prontera", 192, 267), To("prontera_in", 178, 55));
		AddWarp(From("prontera_in", 181, 55), To("prontera", 192, 264));

		AddWarp(From("prontera", 204, 192), To("prontera_in", 68, 134));
		AddWarp(From("prontera_in", 68, 130), To("prontera", 204, 188));

		AddWarp(From("prontera", 208, 154), To("prontera_in", 172, 29));
		AddWarp(From("prontera_in", 172, 33), To("prontera", 205, 157));

		AddWarp(From("prontera", 42, 67), To("prontera_in", 44, 29));
		AddWarp(From("prontera_in", 47, 29), To("prontera", 46, 67));

		AddWarp(From("prontera", 45, 346), To("prontera_in", 80, 110));
		AddWarp(From("prontera_in", 80, 113), To("prontera", 48, 343));

		AddWarp(From("prontera", 73, 100), To("prontera_in", 208, 176));
		AddWarp(From("prontera_in", 208, 179), To("prontera", 76, 102));

		AddWarp(From("prontera", 74, 90), To("prontera_in", 248, 170));
		AddWarp(From("prontera_in", 248, 173), To("prontera", 77, 93));

		AddWarp(From("prontera", 84, 89), To("prontera_in", 282, 176));
		AddWarp(From("prontera_in", 282, 179), To("prontera", 87, 91));

		AddWarp(From("prontera", 156, 360), To("prt_castle", 102, 20));
		AddWarp(From("prt_castle", 102, 16), To("prontera", 156, 356));

		AddWarp(From("prontera", 156, 22), To("prt_fild08", 170, 375));
		AddWarp(From("prt_fild08", 170, 378), To("prontera", 156, 26));

		AddWarp(From("prontera", 22, 203), To("prt_fild05", 367, 205));
		AddWarp(From("prt_fild06", 23, 193), To("prontera", 285, 203));

		AddWarp(From("prontera", 289, 203), To("prt_fild06", 27, 193));
		AddWarp(From("prt_fild05", 373, 205), To("prontera", 26, 203));

		// Prontera Inside
		AddWarp(From("prontera_in", 217, 163), To("prontera_in", 236, 163));
		AddWarp(From("prontera_in", 234, 163), To("prontera_in", 215, 163));

		AddWarp(From("prontera_in", 254, 113), To("prontera_in", 256, 134));
		AddWarp(From("prontera_in", 256, 131), To("prontera_in", 254, 110));

		AddWarp(From("prontera_in", 263, 163), To("prontera_in", 276, 163));
		AddWarp(From("prontera_in", 274, 163), To("prontera_in", 261, 163));

		AddWarp(From("prontera_in", 37, 65), To("prontera_in", 51, 65));
		AddWarp(From("prontera_in", 48, 65), To("prontera_in", 34, 65));

		AddWarp(From("prontera_in", 69, 65), To("prontera_in", 84, 65));
		AddWarp(From("prontera_in", 82, 65), To("prontera_in", 66, 65));

		AddWarp(From("prontera_in", 70, 143), To("prontera_in", 70, 165));
		AddWarp(From("prontera_in", 70, 162), To("prontera_in", 70, 140));

		AddWarp(From("prontera_in", 126, 142), To("prontera_in", 125, 120));
		AddWarp(From("prontera_in", 125, 123), To("prontera_in", 128, 145));

		AddWarp(From("prontera_in", 132, 123), To("prontera_in", 128, 145));
		AddWarp(From("prontera_in", 130, 142), To("prontera_in", 132, 120));

		AddWarp(From("prontera_in", 118, 166), To("prontera_in", 98, 174));
		AddWarp(From("prontera_in", 101, 174), To("prontera_in", 121, 166));

		AddWarp(From("prontera_in", 139, 166), To("prontera_in", 161, 171));
		AddWarp(From("prontera_in", 158, 171), To("prontera_in", 136, 166));

		// Prontera Castle
		AddWarp(From("prt_castle", 102, 129), To("prt_castle", 102, 143));
		AddWarp(From("prt_castle", 102, 140), To("prt_castle", 102, 126));

		AddWarp(From("prt_castle", 102, 73), To("prt_castle", 102, 91));
		AddWarp(From("prt_castle", 102, 88), To("prt_castle", 102, 70));

		AddWarp(From("prt_castle", 113, 107), To("prt_castle", 134, 107));
		AddWarp(From("prt_castle", 130, 107), To("prt_castle", 110, 107));

		AddWarp(From("prt_castle", 121, 29), To("prt_castle", 148, 29));
		AddWarp(From("prt_castle", 144, 29), To("prt_castle", 117, 29));

		AddWarp(From("prt_castle", 135, 153), To("prt_castle", 167, 145));
		AddWarp(From("prt_castle", 164, 145), To("prt_castle", 132, 153));

		AddWarp(From("prt_castle", 149, 113), To("prt_castle", 175, 113));
		AddWarp(From("prt_castle", 172, 113), To("prt_castle", 146, 113));

		AddWarp(From("prt_castle", 170, 138), To("prt_castle", 176, 118));
		AddWarp(From("prt_castle", 176, 121), To("prt_castle", 170, 141));

		AddWarp(From("prt_castle", 28, 121), To("prt_castle", 40, 141));
		AddWarp(From("prt_castle", 40, 138), To("prt_castle", 28, 118));

		AddWarp(From("prt_castle", 31, 113), To("prt_castle", 58, 113));
		AddWarp(From("prt_castle", 54, 113), To("prt_castle", 27, 113));

		AddWarp(From("prt_castle", 45, 145), To("prt_castle", 72, 153));
		AddWarp(From("prt_castle", 68, 153), To("prt_castle", 42, 145));

		AddWarp(From("prt_castle", 59, 29), To("prt_castle", 85, 29));
		AddWarp(From("prt_castle", 82, 29), To("prt_castle", 56, 29));

		AddWarp(From("prt_castle", 75, 107), To("prt_castle", 95, 107));
		AddWarp(From("prt_castle", 92, 107), To("prt_castle", 72, 107));

		// Prontera Fields
		AddWarp(From("prt_fild00", 165, 18), To("prt_fild04", 158, 384));
		AddWarp(From("prt_fild04", 160, 387), To("prt_fild00", 164, 24));

		AddWarp(From("prt_fild00", 317, 18), To("prt_fild04", 323, 384));
		AddWarp(From("prt_fild04", 323, 387), To("prt_fild00", 315, 21));

		AddWarp(From("prt_fild01", 199, 19), To("prt_castle", 102, 178));
		AddWarp(From("prt_castle", 102, 181), To("prt_fild01", 199, 23));

		AddWarp(From("prt_fild01", 380, 243), To("prt_fild02", 20, 242));
		AddWarp(From("prt_fild02", 17, 242), To("prt_fild01", 377, 243));

		AddWarp(From("prt_fild01", 382, 304), To("prt_fild02", 20, 305));
		AddWarp(From("prt_fild02", 17, 305), To("prt_fild01", 379, 302));

		AddWarp(From("prt_fild01", 382, 351), To("prt_fild02", 20, 350));
		AddWarp(From("prt_fild02", 17, 350), To("prt_fild01", 379, 351));

		AddWarp(From("prt_fild02", 380, 347), To("prt_fild03", 23, 249));
		AddWarp(From("prt_fild03", 16, 249), To("prt_fild02", 373, 353));

		AddWarp(From("prt_fild02", 305, 17), To("prt_fild06", 277, 315));
		AddWarp(From("prt_fild06", 277, 320), To("prt_fild02", 305, 22));

		AddWarp(From("prt_fild04", 378, 72), To("prt_fild05", 17, 59));
		AddWarp(From("prt_fild05", 13, 63), To("prt_fild04", 374, 73));

		AddWarp(From("prt_fild04", 384, 155), To("prt_fild05", 20, 134));
		AddWarp(From("prt_fild05", 14, 141), To("prt_fild04", 380, 158));

		AddWarp(From("prt_fild04", 384, 334), To("prt_fild05", 20, 333));
		AddWarp(From("prt_fild05", 15, 333), To("prt_fild04", 380, 332));

		AddWarp(From("prt_fild05", 134, 14), To("prt_fild07", 129, 374));
		AddWarp(From("prt_fild07", 132, 381), To("prt_fild05", 142, 18));

		AddWarp(From("prt_fild05", 255, 14), To("prt_fild07", 248, 369));
		AddWarp(From("prt_fild07", 248, 376), To("prt_fild05", 257, 18));

		AddWarp(From("prt_fild07", 383, 239), To("prt_fild08", 20, 239));
		AddWarp(From("prt_fild08", 16, 239), To("prt_fild07", 379, 239));

		AddWarp(From("prt_fild07", 385, 186), To("prt_fild08", 20, 186));
		AddWarp(From("prt_fild08", 16, 187), To("prt_fild07", 380, 186));

		// Maze
		AddWarp(From("prt_fild01", 136, 373), To("prt_maze01", 176, 7));
		AddWarp(From("prt_maze01", 176, 4), To("prt_fild01", 136, 368));

		AddWarp(From("prt_maze01", 100, 35), To("prt_maze01", 139, 47));
		AddWarp(From("prt_maze01", 139, 44), To("prt_maze01", 100, 32));

		AddWarp(From("prt_maze01", 102, 165), To("prt_maze01", 98, 151));
		AddWarp(From("prt_maze01", 98, 155), To("prt_maze01", 102, 169));

		AddWarp(From("prt_maze01", 105, 115), To("prt_maze01", 175, 168));
		AddWarp(From("prt_maze01", 175, 164), To("prt_maze01", 105, 111));

		AddWarp(From("prt_maze01", 105, 75), To("prt_maze01", 54, 8));
		AddWarp(From("prt_maze01", 54, 4), To("prt_maze01", 105, 71));

		AddWarp(From("prt_maze01", 115, 145), To("prt_maze01", 8, 186));
		AddWarp(From("prt_maze01", 4, 186), To("prt_maze01", 111, 145));

		AddWarp(From("prt_maze01", 115, 21), To("prt_maze01", 167, 22));
		AddWarp(From("prt_maze01", 164, 22), To("prt_maze01", 112, 21));

		AddWarp(From("prt_maze01", 115, 56), To("prt_maze01", 7, 57));
		AddWarp(From("prt_maze01", 4, 57), To("prt_maze01", 112, 56));

		AddWarp(From("prt_maze01", 115, 96), To("prt_maze01", 128, 105));
		AddWarp(From("prt_maze01", 124, 105), To("prt_maze01", 111, 96));

		AddWarp(From("prt_maze01", 124, 169), To("prt_maze01", 191, 139));
		AddWarp(From("prt_maze01", 195, 139), To("prt_maze01", 129, 174));

		AddWarp(From("prt_maze01", 138, 124), To("prt_maze01", 142, 111));
		AddWarp(From("prt_maze01", 142, 115), To("prt_maze01", 138, 128));

		AddWarp(From("prt_maze01", 14, 75), To("prt_maze01", 63, 128));
		AddWarp(From("prt_maze01", 63, 124), To("prt_maze01", 14, 71));

		AddWarp(From("prt_maze01", 140, 75), To("prt_maze01", 96, 47));
		AddWarp(From("prt_maze01", 96, 44), To("prt_maze01", 140, 72));

		AddWarp(From("prt_maze01", 155, 133), To("prt_maze01", 8, 140));
		AddWarp(From("prt_maze01", 4, 140), To("prt_maze01", 151, 133));

		AddWarp(From("prt_maze01", 155, 181), To("prt_maze01", 88, 145));
		AddWarp(From("prt_maze01", 84, 145), To("prt_maze01", 151, 181));

		AddWarp(From("prt_maze01", 155, 21), To("prt_maze01", 87, 13));
		AddWarp(From("prt_maze01", 84, 10), To("prt_maze01", 152, 25));

		AddWarp(From("prt_maze01", 164, 140), To("prt_maze01", 70, 68));
		AddWarp(From("prt_maze01", 75, 66), To("prt_maze01", 169, 140));

		AddWarp(From("prt_maze01", 164, 93), To("prt_maze01", 72, 11));
		AddWarp(From("prt_maze01", 75, 11), To("prt_maze01", 167, 93));

		AddWarp(From("prt_maze01", 17, 115), To("prt_maze01", 50, 48));
		AddWarp(From("prt_maze01", 50, 44), To("prt_maze01", 17, 111));

		AddWarp(From("prt_maze01", 17, 34), To("prt_maze01", 23, 128));
		AddWarp(From("prt_maze01", 23, 124), To("prt_maze01", 17, 30));

		AddWarp(From("prt_maze01", 176, 35), To("prt_maze01", 182, 88));
		AddWarp(From("prt_maze01", 182, 84), To("prt_maze01", 177, 31));

		AddWarp(From("prt_maze01", 176, 44), To("prt_maze01", 18, 152));
		AddWarp(From("prt_maze01", 18, 155), To("prt_maze01", 176, 47));

		AddWarp(From("prt_maze01", 195, 15), To("prt_maze01", 47, 105));
		AddWarp(From("prt_maze01", 44, 105), To("prt_maze01", 192, 15));

		AddWarp(From("prt_maze01", 195, 174), To("prt_maze01", 47, 23));
		AddWarp(From("prt_maze01", 44, 23), To("prt_maze01", 192, 174));

		AddWarp(From("prt_maze01", 195, 55), To("prt_maze01", 87, 97));
		AddWarp(From("prt_maze01", 84, 97), To("prt_maze01", 192, 55));

		AddWarp(From("prt_maze01", 195, 93), To("prt_maze01", 88, 55));
		AddWarp(From("prt_maze01", 84, 55), To("prt_maze01", 191, 93));

		AddWarp(From("prt_maze01", 22, 84), To("prt_maze01", 55, 151));
		AddWarp(From("prt_maze01", 55, 155), To("prt_maze01", 22, 88));

		AddWarp(From("prt_maze01", 25, 5), To("prt_maze01", 65, 113));
		AddWarp(From("prt_maze01", 65, 116), To("prt_maze01", 23, 9));

		AddWarp(From("prt_maze01", 51, 195), To("prt_maze01", 63, 88));
		AddWarp(From("prt_maze01", 63, 84), To("prt_maze01", 58, 192));

		AddWarp(From("prt_maze01", 63, 195), To("prt_maze01", 63, 88));
		AddWarp(From("prt_maze01", 63, 84), To("prt_maze01", 58, 192));

		AddWarp(From("prt_maze01", 75, 95), To("prt_maze01", 88, 173));
		AddWarp(From("prt_maze01", 84, 173), To("prt_maze01", 71, 95));

		AddWarp(From("prt_maze01", 70, 75), To("prt_maze01", 169, 140));
		AddWarp(From("prt_maze01", 164, 140), To("prt_maze01", 70, 68));

		AddWarp(From("prt_maze01", 19, 195), To("prt_maze02", 94, 19));
		AddWarp(From("prt_maze02", 103, 15), To("prt_maze01", 22, 191));

		AddWarp(From("prt_maze02", 84, 15), To("prt_maze01", 22, 191));
		AddWarp(From("prt_maze01", 19, 195), To("prt_maze02", 94, 19));

		AddWarp(From("prt_maze02", 108, 182), To("prt_maze03", 23, 8));
		AddWarp(From("prt_maze03", 25, 4), To("prt_maze02", 95, 177));

		AddWarp(From("prt_maze02", 80, 182), To("prt_maze03", 23, 8));
		AddWarp(From("prt_maze03", 25, 4), To("prt_maze02", 95, 177));
	}
}
