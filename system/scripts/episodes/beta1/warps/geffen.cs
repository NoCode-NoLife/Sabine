//--- Sabine Script ---------------------------------------------------------
// Geffen Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Geffen and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("geffen", "geffen_in")]
public class GeffenBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Geffen
		AddWarp(From("geffen", 138, 138), To("geffen_in", 28, 110));
		AddWarp(From("geffen_in", 28, 106), To("geffen", 136, 136));

		AddWarp(From("geffen", 172, 174), To("geffen_in", 70, 52));
		AddWarp(From("geffen_in", 70, 48), To("geffen", 168, 170));

		AddWarp(From("geffen", 182, 59), To("geffen_in", 106, 177));
		AddWarp(From("geffen_in", 106, 181), To("geffen", 180, 61));

		AddWarp(From("geffen", 43, 85), To("geffen_in", 70, 138));
		AddWarp(From("geffen_in", 70, 132), To("geffen", 46, 88));

		AddWarp(From("geffen", 61, 180), To("geffen_in", 162, 97));
		AddWarp(From("geffen_in", 163, 94), To("geffen", 65, 176));

		AddWarp(From("geffen", 98, 141), To("geffen_in", 28, 160));
		AddWarp(From("geffen_in", 28, 156), To("geffen", 101, 138));

		AddWarp(From("geffen_in", 100, 67), To("geffen_in", 84, 65));
		AddWarp(From("geffen_in", 87, 66), To("geffen_in", 103, 67));

		AddWarp(From("geffen_in", 104, 109), To("geffen_in", 76, 107));
		AddWarp(From("geffen_in", 79, 107), To("geffen_in", 107, 109));

		AddWarp(From("geffen_in", 113, 163), To("geffen_in", 139, 169));
		AddWarp(From("geffen_in", 136, 169), To("geffen_in", 110, 163));

		AddWarp(From("geffen_in", 114, 37), To("geffen_in", 114, 63));
		AddWarp(From("geffen_in", 114, 60), To("geffen_in", 114, 34));

		AddWarp(From("geffen_in", 138, 149), To("geffen_in", 138, 165));
		AddWarp(From("geffen_in", 138, 162), To("geffen_in", 138, 146));

		AddWarp(From("geffen_in", 26, 60), To("geffen_in", 26, 34));
		AddWarp(From("geffen_in", 26, 37), To("geffen_in", 26, 63));

		AddWarp(From("geffen_in", 52, 65), To("geffen_in", 38, 67));
		AddWarp(From("geffen_in", 41, 67), To("geffen_in", 55, 65));

		AddWarp(From("geffen_in", 70, 149), To("geffen_in", 70, 161));
		AddWarp(From("geffen_in", 70, 158), To("geffen_in", 70, 146));

		AddWarp(From("geffen_in", 70, 83), To("geffen_in", 72, 101));
		AddWarp(From("geffen_in", 72, 98), To("geffen_in", 70, 80));

		AddWarp(From("geffen", 217, 119), To("gef_fild00", 36, 322));
		AddWarp(From("gef_fild00", 33, 322), To("geffen", 213, 119));

		// Geffen Tower
		AddWarp(From("gef_tower", 106, 115), To("gef_tower", 106, 72));
		AddWarp(From("gef_tower", 106, 69), To("gef_tower", 106, 112));

		AddWarp(From("gef_tower", 44, 36), To("gef_tower", 106, 162));
		AddWarp(From("gef_tower", 106, 158), To("gef_tower", 44, 33));

		AddWarp(From("gef_tower", 118, 68), To("gef_tower", 116, 28));
		AddWarp(From("gef_tower", 116, 31), To("gef_tower", 118, 71));

		AddWarp(From("gef_tower", 120, 158), To("gef_tower", 118, 111));
		AddWarp(From("gef_tower", 118, 114), To("gef_tower", 120, 161));

		AddWarp(From("gef_tower", 158, 104), To("gef_tower", 156, 90));
		AddWarp(From("gef_tower", 156, 93), To("gef_tower", 158, 107));

		AddWarp(From("gef_tower", 158, 150), To("gef_tower", 158, 124));
		AddWarp(From("gef_tower", 158, 128), To("gef_tower", 158, 153));

		AddWarp(From("gef_tower", 158, 174), To("gef_tower", 52, 140));
		AddWarp(From("gef_tower", 52, 136), To("gef_tower", 158, 169));

		AddWarp(From("gef_tower", 38, 160), To("gef_tower", 42, 90));
		AddWarp(From("gef_tower", 42, 86), To("gef_tower", 38, 157));

		AddWarp(From("gef_tower", 66, 156), To("gef_tower", 42, 90));
		AddWarp(From("gef_tower", 42, 86), To("gef_tower", 38, 157));

		AddWarp(From("gef_tower", 52, 181), To("geffen", 120, 110));
		AddWarp(From("geffen", 120, 114), To("gef_tower", 52, 177));

		AddWarp(From("gef_tower", 60, 32), To("gef_tower", 62, 90));
		AddWarp(From("gef_tower", 62, 87), To("gef_tower", 60, 30));

		AddWarp(From("gef_tower", 153, 28), To("gef_dun00", 104, 99));
		AddWarp(From("gef_dun00", 104, 103), To("gef_tower", 153, 31));

		AddWarp(From("gef_dun00", 107, 169), To("gef_dun01", 115, 236));
		AddWarp(From("gef_dun01", 115, 240), To("gef_dun00", 107, 165));

		// Geffen Fields
		AddWarp(From("gef_fild00", 276, 141), To("prt_fild00", 32, 123));
		AddWarp(From("prt_fild00", 29, 124), To("gef_fild00", 273, 142));

		AddWarp(From("gef_fild01", 382, 111), To("prt_fild04", 20, 114));
		AddWarp(From("gef_fild02", 380, 156), To("prt_fild07", 21, 143));
		AddWarp(From("gef_fild02", 380, 289), To("prt_fild07", 18, 289));
		AddWarp(From("gef_fild02", 380, 68), To("prt_fild07", 17, 64));
	}
}
