//--- Sabine Script ---------------------------------------------------------
// Prontera Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Prontera and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (1.0)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class PronteraBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		if (!MapsExist("prontera", "prontera_in"))
			return;

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
	}
}
