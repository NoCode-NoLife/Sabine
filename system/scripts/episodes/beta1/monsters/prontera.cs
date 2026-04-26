//--- Sabine Script ---------------------------------------------------------
// Prontera Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Prontera.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1), Prontera.net
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Shared.Const.IdentityId;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("prt_fild00", "prt_fild01", "prt_maze01")]
public class PronteraMonstersScript_200 : GeneralScript
{
	public override void Load()
	{
		AddSpawner("prt_fild00", "Poring", JT_PORING, 10);
		AddSpawner("prt_fild00", "Lunatic", JT_LUNATIC, 5);
		AddSpawner("prt_fild00", "Fabre", JT_FABRE, 10);
		AddSpawner("prt_fild00", "Pupa", JT_PUPA, 10);
		AddSpawner("prt_fild00", "Creamy", JT_CREAMY, 5);
		AddSpawner("prt_fild00", "Roda Frog", JT_RODA_FROG, 15);
		AddSpawner("prt_fild00", "Hornet", JT_HORNET, 5);

		AddSpawner("prt_fild01", "Poring", JT_PORING, 10);
		AddSpawner("prt_fild01", "Lunatic", JT_LUNATIC, 5);
		AddSpawner("prt_fild01", "Fabre", JT_FABRE, 10);
		AddSpawner("prt_fild01", "Pupa", JT_PUPA, 5);
		AddSpawner("prt_fild01", "Thief Bug Egg", JT_THIEF_BUG_EGG, 10);
		AddSpawner("prt_fild01", "Roda Frog", JT_RODA_FROG, 5);

		AddSpawner("prt_fild02", "Thief Bug", JT_THIEF_BUG, 20);
		AddSpawner("prt_fild02", "Thief Bug", JT_THIEF_BUG_, 10);
		AddSpawner("prt_fild00", "Roda Frog", JT_RODA_FROG, 15);
		AddSpawner("prt_fild03", "Yoyo", JT_YOYO, 5);
		AddSpawner("prt_fild01", "Orc Hero", JT_ORK_HERO, 1, initialDelay: Hours(3), respawnDelay: Hours(6));

		AddSpawner("prt_fild03", "Lunatic", JT_LUNATIC, 10);
		AddSpawner("prt_fild03", "Fabre", JT_FABRE, 5);
		AddSpawner("prt_fild03", "Pupa", JT_PUPA, 15);
		AddSpawner("prt_fild03", "Creamy", JT_CREAMY, 10);
		AddSpawner("prt_fild03", "Thief Bug Egg", JT_THIEF_BUG_EGG, 10);
		AddSpawner("prt_fild03", "Poporing", JT_POPORING, 10);
		AddSpawner("prt_fild03", "Yoyo", JT_YOYO, 5);

		AddSpawner("prt_fild04", "Poring", JT_PORING, 10);
		AddSpawner("prt_fild04", "Lunatic", JT_LUNATIC, 10);
		AddSpawner("prt_fild04", "Roda Frog", JT_RODA_FROG, 10);
		AddSpawner("prt_fild04", "Hornet", JT_HORNET, 15);
		AddSpawner("prt_fild04", "Rocker", JT_ROCKER, 10);

		AddSpawner("prt_fild05", "Poring", JT_PORING, 10);
		AddSpawner("prt_fild05", "Lunatic", JT_LUNATIC, 10);
		AddSpawner("prt_fild05", "Fabre", JT_FABRE, 5);
		AddSpawner("prt_fild05", "Pupa", JT_PUPA, 5);
		AddSpawner("prt_fild05", "Elder Willow", JT_ELDER_WILOW, 5);

		AddSpawner("prt_fild06", "Poring", JT_PORING, 5);
		AddSpawner("prt_fild06", "Lunatic", JT_LUNATIC, 5);
		AddSpawner("prt_fild06", "Fabre", JT_FABRE, 5);
		AddSpawner("prt_fild06", "Pupa", JT_PUPA, 10);
		AddSpawner("prt_fild06", "Roda Frog", JT_RODA_FROG, 5);
		AddSpawner("prt_fild06", "Thief Bug Egg", JT_THIEF_BUG_EGG, 10);
		AddSpawner("prt_fild06", "Thief Bug", JT_THIEF_BUG, 5);

		AddSpawner("prt_fild07", "Roda Frog", JT_RODA_FROG, 10);
		AddSpawner("prt_fild07", "Rocker", JT_ROCKER, 10);
		AddSpawner("prt_fild07", "Smokie", JT_SMOKIE, 10);
		AddSpawner("prt_fild07", "Elder Willow", JT_ELDER_WILOW, 10);

		AddSpawner("prt_fild08", "Poring", JT_PORING, 10);
		AddSpawner("prt_fild08", "Lunatic", JT_LUNATIC, 15);
		AddSpawner("prt_fild08", "Fabre", JT_FABRE, 5);
		AddSpawner("prt_fild08", "Pupa", JT_PUPA, 10);
		AddSpawner("prt_fild08", "Chonchon", JT_CHONCHON, 5);
		AddSpawner("prt_fild08", "Roda Frog", JT_RODA_FROG, 15);
	}
}
