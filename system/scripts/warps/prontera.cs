//--- Sabine Script ---------------------------------------------------------
// Prontera Warps
//--- Description -----------------------------------------------------------
// Sets up warps in and around Prontera and its surrounding maps.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;

public class PronteraWarpsScript : MapScript
{
	public override void Load()
	{
		Overworld();
		Inside();
	}

	private void Overworld()
	{
		// Prontera North Field <--> Prontera North
		AddWarp(From("prt_fild01", 113, 22), To("prt_vilg02", 100, 174));
		AddWarp(From("prt_vilg02", 100, 176), To("prt_fild01", 113, 24));

		// Prontera North <--> Prontera Castle
		AddWarp(From("prt_vilg02", 100, 22), To("prt_vilg00", 100, 120));
		AddWarp(From("prt_vilg00", 100, 122), To("prt_vilg02", 100, 24));

		// Prontera Castle <--> Prontera West Field
		AddWarp(From("prt_vilg00", 26, 100), To("prt_fild05", 182, 54));
		AddWarp(From("prt_fild05", 184, 54), To("prt_vilg00", 28, 100));

		// Prontera West Field <--> Prontera Dungeon 1B
		AddWarp(From("prt_fild05", 30, 161), To("prt_dugn01", 187, 12));
		AddWarp(From("prt_dugn01", 189, 10), To("prt_fild05", 32, 160));

		// Prontera Castle <--> Prontera East Field
		AddWarp(From("prt_vilg00", 173, 100), To("prt_fild04", 17, 156));
		AddWarp(From("prt_fild04", 15, 156), To("prt_vilg00", 171, 100));

		// Prontera South <--> Underground Waterway
		AddWarp(From("prt_vilg01", 31, 100), To("prt_dugn00", 182, 150));
		AddWarp(From("prt_dugn00", 182, 152), To("prt_vilg01", 33, 100));

		// Prontera Castle <--> Prontera South
		AddWarp(From("prt_vilg00", 100, 24), To("prt_vilg01", 100, 174));
		AddWarp(From("prt_vilg01", 100, 176), To("prt_vilg00", 100, 26));

		// Prontera South <--> Prontera South Field
		AddWarp(From("prt_vilg01", 100, 22), To("prt_fild00", 100, 178));
		AddWarp(From("prt_fild00", 100, 180), To("prt_vilg01", 100, 24));
	}

	private void Inside()
	{
		// Prontera North <--> Building Center Northwest
		AddWarp(From("prt_vilg02", 127, 119), To("prt_intr02", 50, 72));
		AddWarp(From("prt_intr02", 46, 72), To("prt_vilg02", 124, 116));
	}
}
