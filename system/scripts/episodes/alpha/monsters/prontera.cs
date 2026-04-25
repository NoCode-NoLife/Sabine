//--- Sabine Script ---------------------------------------------------------
// Prontera Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Prontera.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;
using static Sabine.Shared.Const.IdentityId;

[RequiresMaps("prt_fild00", "prt_dugn01")]
public class PronteraMonstersScript : GeneralScript
{
	public override void Load()
	{
		// Prontera North Field
		AddSpawner("prt_fild01", "Poring", JT_PORING, 30);
		AddSpawner("prt_fild01", "Fabre", JT_FABRE, 20);
		AddSpawner("prt_fild01", "Chocho", JT_CHONCHON, 10);

		// Prontera West Field
		AddSpawner("prt_fild05", "Poring", JT_PORING, 30);
		AddSpawner("prt_fild05", "Fabre", JT_FABRE, 20);
		AddSpawner("prt_fild05", "Chocho", JT_CHONCHON, 10);
		AddSpawner("prt_fild05", "Wilow", JT_WILOW, 5);
		AddSpawner("prt_fild05", "Spore", JT_WILOW, 5);
		AddSpawner("prt_fild05", "Roda Frog", JT_RODA_FROG, 3);

		// Prontera East Field
		AddSpawner("prt_fild04", "Poring", JT_PORING, 30);
		AddSpawner("prt_fild04", "Fabre", JT_FABRE, 20);
		AddSpawner("prt_fild04", "Chocho", JT_CHONCHON, 10);
		AddSpawner("prt_fild04", "Roda Frog", JT_RODA_FROG, 3);

		// Prontera South Field
		AddSpawner("prt_fild00", "Poring", JT_PORING, 30);
		AddSpawner("prt_fild00", "Fabre", JT_FABRE, 20);
		AddSpawner("prt_fild00", "Chocho", JT_CHONCHON, 10);

		// Underground Waterway
		AddSpawner("prt_dugn00", "Poring", JT_PORING, 30);
		AddSpawner("prt_dugn00", "Farmiliar", JT_FARMILIAR, 20);

		// Prontera Dungeon 1B
		AddSpawner("prt_dugn01", "Poring", JT_PORING, 30);
		AddSpawner("prt_dugn01", "Spore", JT_WILOW, 15);
		AddSpawner("prt_dugn01", "Willow", JT_WILOW, 15);
		AddSpawner("prt_dugn01", "Skel Archer", JT_ARCHER_SKELETON, 10);
		AddSpawner("prt_dugn01", "Skel Soldier", JT_SOLDIER_SKELETON, 10);

		// Prontera Dungeon 2B
		AddSpawner("prt_dugn02", "Spore", JT_WILOW, 30);
		AddSpawner("prt_dugn02", "Elder Willow", JT_ELDER_WILOW, 20);
		AddSpawner("prt_dugn02", "Poporing", JT_POPORING, 20);
		AddSpawner("prt_dugn02", "Skel Soldier", JT_SOLDIER_SKELETON, 10);

		// Prontera Dungeon 3B
		AddSpawner("prt_dugn03", "Willow", JT_WILOW, 30);
		AddSpawner("prt_dugn03", "Elder Willow", JT_ELDER_WILOW, 20);
		AddSpawner("prt_dugn03", "Seahorse", JT_SEAHORES, 15);
		AddSpawner("prt_dugn03", "Doppelganger", JT_DOPPELGANGER, 1, initialDelay: Hours(3), respawnDelay: Hours(6));
	}
}
