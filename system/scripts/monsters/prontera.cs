//--- Sabine Script ---------------------------------------------------------
// Prontera Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Prontera.
//---------------------------------------------------------------------------

using System;
using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class PronteraMonstersScript : GeneralScript
{
	public override void Load()
	{
		// Prontera North Field
		AddSpawner("prt_fild01", "Poring", 1002, 30);
		AddSpawner("prt_fild01", "Fabre", 1007, 20);
		AddSpawner("prt_fild01", "Chocho", 1011, 10);

		// Prontera West Field
		AddSpawner("prt_fild05", "Poring", 1002, 30);
		AddSpawner("prt_fild05", "Fabre", 1007, 20);
		AddSpawner("prt_fild05", "Chocho", 1011, 10);
		AddSpawner("prt_fild05", "Wilow", 1010, 5);
		AddSpawner("prt_fild05", "Spore", 1014, 5);
		AddSpawner("prt_fild05", "Roda Frog", 1012, 3);

		// Prontera East Field
		AddSpawner("prt_fild04", "Poring", 1002, 30);
		AddSpawner("prt_fild04", "Fabre", 1007, 20);
		AddSpawner("prt_fild04", "Chocho", 1011, 10);
		AddSpawner("prt_fild04", "Roda Frog", 1012, 3);

		// Prontera South Field
		AddSpawner("prt_fild00", "Poring", 1002, 30);
		AddSpawner("prt_fild00", "Fabre", 1007, 20);
		AddSpawner("prt_fild00", "Chocho", 1011, 10);

		// Underground Waterway
		AddSpawner("prt_dugn00", "Poring", 1002, 30);
		AddSpawner("prt_dugn00", "Farmiliar", 1005, 20);

		// Prontera Dungeon 1B
		AddSpawner("prt_dugn01", "Poring", 1002, 30);
		AddSpawner("prt_dugn01", "Spore", 1014, 15);
		AddSpawner("prt_dugn01", "Willow", 1010, 15);
		AddSpawner("prt_dugn01", "Skel Archer", 1016, 10);
		AddSpawner("prt_dugn01", "Skel Soldier", 1028, 10);

		// Prontera Dungeon 2B
		AddSpawner("prt_dugn02", "Spore", 1014, 30);
		AddSpawner("prt_dugn02", "Elder Willow", 1033, 20);
		AddSpawner("prt_dugn02", "Poporing", 1031, 20);
		AddSpawner("prt_dugn02", "Skel Soldier", 1028, 10);

		// Prontera Dungeon 3B
		AddSpawner("prt_dugn03", "Willow", 1010, 30);
		AddSpawner("prt_dugn03", "Elder Willow", 1033, 20);
		AddSpawner("prt_dugn03", "Seahorse", 1043, 15);
	}
}
