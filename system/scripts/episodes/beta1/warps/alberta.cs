//--- Sabine Script ---------------------------------------------------------
// Alberta Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Alberta and its surrounding maps.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1)
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("alberta", "alberta_in")]
public class AlbertaBeta1WarpScripts : GeneralScript
{
	public override void Load()
	{
		// Alberta
		AddWarp(From("alberta", 170, 170), To("alb_ship", 26, 168));
		AddWarp(From("alb_ship", 26, 166), To("alberta", 170, 168));

		AddWarp(From("alberta", 178, 165), To("alb_ship", 39, 163));
		AddWarp(From("alb_ship", 41, 163), To("alberta", 180, 165));

		AddWarp(From("alberta", 209, 173), To("alb_ship", 162, 171));
		AddWarp(From("alb_ship", 160, 171), To("alberta", 207, 173));

		AddWarp(From("alb_ship", 37, 174), To("alb_ship", 70, 99));
		AddWarp(From("alb_ship", 68, 99), To("alb_ship", 35, 173));

		AddWarp(From("alberta", 117, 38), To("alberta_in", 180, 30));
		AddWarp(From("alberta_in", 180, 34), To("alberta", 117, 42));

		AddWarp(From("alberta", 134, 38), To("alberta_in", 70, 141));
		AddWarp(From("alberta_in", 73, 141), To("alberta", 137, 37));

		AddWarp(From("alberta", 33, 42), To("alberta_in", 74, 44));
		AddWarp(From("alberta_in", 78, 44), To("alberta", 37, 41));

		AddWarp(From("alberta", 65, 233), To("alberta_in", 18, 141));
		AddWarp(From("alberta_in", 14, 141), To("alberta", 61, 233));

		AddWarp(From("alberta", 93, 205), To("alberta_in", 114, 134));
		AddWarp(From("alberta_in", 114, 130), To("alberta", 93, 201));

		AddWarp(From("alberta", 98, 153), To("alberta_in", 185, 89));
		AddWarp(From("alberta_in", 189, 89), To("alberta", 102, 153));

		AddWarp(From("alberta", 99, 221), To("alberta_in", 122, 161));
		AddWarp(From("alberta_in", 125, 161), To("alberta", 102, 222));

		AddWarp(From("alberta_in", 114, 183), To("alberta_in", 148, 186));
		AddWarp(From("alberta_in", 152, 186), To("alberta_in", 118, 183));

		AddWarp(From("alberta_in", 114, 49), To("alberta_in", 155, 153));
		AddWarp(From("alberta_in", 159, 153), To("alberta_in", 117, 49));

		AddWarp(From("alberta_in", 114, 97), To("alberta_in", 155, 175));
		AddWarp(From("alberta_in", 159, 175), To("alberta_in", 117, 97));

		AddWarp(From("alberta_in", 22, 113), To("alberta_in", 22, 134));
		AddWarp(From("alberta_in", 22, 130), To("alberta_in", 22, 109));

		AddWarp(From("alberta_in", 22, 153), To("alberta_in", 22, 174));
		AddWarp(From("alberta_in", 22, 170), To("alberta_in", 22, 149));

		AddWarp(From("alberta_in", 24, 33), To("alberta_in", 64, 35));
		AddWarp(From("alberta_in", 64, 31), To("alberta_in", 24, 29));

		AddWarp(From("alberta_in", 24, 54), To("alberta_in", 64, 53));
		AddWarp(From("alberta_in", 64, 57), To("alberta_in", 24, 58));

		AddWarp(From("alberta_in", 66, 113), To("alberta_in", 66, 132));
		AddWarp(From("alberta_in", 66, 130), To("alberta_in", 66, 111));

		AddWarp(From("alberta_in", 66, 153), To("alberta_in", 66, 173));
		AddWarp(From("alberta_in", 66, 170), To("alberta_in", 66, 149));

		//AddWarp(From("alb2trea", 88, 111), To("treasure01", 69, 24));
		AddWarp(From("alberta", 15, 234), To("pay_fild03", 388, 63));
	}
}
