//--- Sabine Script ---------------------------------------------------------
// Izlude Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Izlude and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("izlude", "izlude_in")]
public class IzludeBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Izlude Town
		AddWarp(From("izlude", 109, 151), To("izlude_in", 65, 87));
		AddWarp(From("izlude_in", 65, 84), To("izlude", 113, 147));

		AddWarp(From("izlude", 148, 148), To("izlude_in", 116, 49));
		AddWarp(From("izlude_in", 116, 46), To("izlude", 145, 145));

		AddWarp(From("izlude", 216, 129), To("izlude_in", 151, 127));
		AddWarp(From("izlude_in", 148, 127), To("izlude", 212, 129));

		AddWarp(From("izlude", 52, 140), To("izlude_in", 74, 161));
		AddWarp(From("izlude_in", 74, 158), To("izlude", 52, 136));

		AddWarp(From("izlude_in", 108, 169), To("izlude_in", 84, 169));
		AddWarp(From("izlude_in", 87, 169), To("izlude_in", 111, 169));

		AddWarp(From("izlude_in", 171, 97), To("izlude_in", 172, 119));
		AddWarp(From("izlude_in", 172, 116), To("izlude_in", 172, 94));

		AddWarp(From("izlude_in", 172, 139), To("izlude_in", 172, 161));
		AddWarp(From("izlude_in", 172, 158), To("izlude_in", 172, 136));

		AddWarp(From("izlude_in", 43, 169), To("izlude_in", 63, 169));
		AddWarp(From("izlude_in", 63, 165), To("izlude_in", 68, 165));

		AddWarp(From("izlude", 30, 78), To("prt_fild08", 367, 212));

		// Undersea Tunnel
		AddWarp(From("iz_dun00", 168, 173), To("izlu2dun", 108, 88));
		AddWarp(From("izlu2dun", 108, 83), To("iz_dun00", 168, 168));

		AddWarp(From("iz_dun00", 352, 342), To("iz_dun01", 253, 252));
		AddWarp(From("iz_dun01", 253, 258), To("iz_dun00", 352, 337));

		AddWarp(From("iz_dun00", 39, 41), To("iz_dun01", 41, 37));
		AddWarp(From("iz_dun01", 41, 32), To("iz_dun00", 39, 46));

		AddWarp(From("iz_dun01", 118, 170), To("iz_dun02", 236, 204));
		AddWarp(From("iz_dun02", 236, 198), To("iz_dun01", 118, 165));

		//AddWarp(From("iz_dun02", 339, 331), To("iz_dun03", 32, 63));
		//AddWarp(From("iz_dun03", 29, 63), To("iz_dun02", 339, 328));

		//AddWarp(From("iz_dun03", 264, 245), To("iz_dun04", 26, 27));
		//AddWarp(From("iz_dun04", 26, 24), To("iz_dun03", 261, 245));
	}
}
