//--- Sabine Script ---------------------------------------------------------
// Morocc Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Morocc.
//--- Credits ---------------------------------------------------------------
// Athena (Dev 2.1.1), Prontera.net
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Shared.Const.IdentityId;
using static Sabine.Zone.Scripting.Shortcuts;

[RequiresMaps("moc_fild01", "moc_fild07")]
public class MoroccMonstersScript_200 : GeneralScript
{
	public override void Load()
	{
		AddSpawner("moc_fild01", "Condor", JT_CONDOR, 5);
		AddSpawner("moc_fild01", "Peco Peco Egg", JT_PECOPECO_EGG, 5);
		AddSpawner("moc_fild01", "Peco Peco", JT_PECOPECO, 10);
		AddSpawner("moc_fild01", "Picky", JT_PICKY, 10);
		AddSpawner("moc_fild01", "Picky", JT_PICKY_, 10);

		AddSpawner("moc_fild02", "Picky", JT_PICKY, 10);
		AddSpawner("moc_fild02", "Picky", JT_PICKY_, 10);
		AddSpawner("moc_fild02", "Peco Peco Egg", JT_PECOPECO_EGG, 15);
		AddSpawner("moc_fild02", "Condor", JT_CONDOR, 10);
		AddSpawner("moc_fild02", "Scorpion", JT_SCORPION, 5);

		AddSpawner("moc_fild03", "Poring", JT_PORING, 5);
		AddSpawner("moc_fild03", "Lunatic", JT_LUNATIC, 15);
		AddSpawner("moc_fild03", "Wilow", JT_WILOW, 15);
		AddSpawner("moc_fild03", "Roda Frog", JT_RODA_FROG, 5);
		AddSpawner("moc_fild03", "Snake", JT_SNAKE, 10);

		AddSpawner("moc_fild04", "Chonchon", JT_CHONCHON, 5);
		AddSpawner("moc_fild04", "Condor", JT_CONDOR, 10);
		AddSpawner("moc_fild04", "Peco Peco Egg", JT_PECOPECO_EGG, 10);
		AddSpawner("moc_fild04", "Peco Peco", JT_PECOPECO, 15);
		AddSpawner("moc_fild04", "Picky", JT_PICKY, 15);
		AddSpawner("moc_fild04", "Picky", JT_PICKY_, 15);
		AddSpawner("moc_fild04", "Wolf", JT_WOLF, 5);

		AddSpawner("moc_fild05", "Chonchon", JT_CHONCHON, 5);
		AddSpawner("moc_fild05", "Condor", JT_CONDOR, 10);
		AddSpawner("moc_fild05", "Peco Peco Egg", JT_PECOPECO_EGG, 10);
		AddSpawner("moc_fild05", "Picky", JT_PICKY, 15);
		AddSpawner("moc_fild05", "Picky", JT_PICKY_, 15);
		AddSpawner("moc_fild05", "Steel Chonchon", JT_STEEL_CHONCHON, 5);
		AddSpawner("moc_fild05", "Wolf", JT_WOLF, 10);

		AddSpawner("moc_fild06", "Chonchon", JT_CHONCHON, 10);
		AddSpawner("moc_fild06", "Condor", JT_CONDOR, 10);
		AddSpawner("moc_fild06", "Peco Peco Egg", JT_PECOPECO_EGG, 10);
		AddSpawner("moc_fild06", "Picky", JT_PICKY, 10);
		AddSpawner("moc_fild06", "Picky", JT_PICKY_, 10);
		AddSpawner("moc_fild06", "Anacondaq", JT_ANACONDAQ, 10);

		AddSpawner("moc_fild07", "Chonchon", JT_CHONCHON, 10);
		AddSpawner("moc_fild07", "Peco Peco Egg", JT_PECOPECO_EGG, 10);
		AddSpawner("moc_fild07", "Picky", JT_PICKY, 5);
		AddSpawner("moc_fild07", "Picky", JT_PICKY_, 5);
		AddSpawner("moc_fild07", "Golem", JT_GOLEM, 5);
		AddSpawner("moc_fild07", "Metaller", JT_METALLER, 5);

		AddSpawner("moc_prydb1", "Farmiliar", JT_FARMILIAR, 10);
		AddSpawner("moc_prydb1", "Poporing", JT_POPORING, 10);
		AddSpawner("moc_prydb1", "Zombie", JT_ZOMBIE, 5);
		AddSpawner("moc_prydb1", "Ghoul", JT_GHOUL, 5);
		AddSpawner("moc_prydb1", "Verit", JT_VERIT, 5);
		AddSpawner("moc_prydb1", "Isis", JT_ISIS, 5);
		AddSpawner("moc_prydb1", "Mummy", JT_MUMMY, 5);
		AddSpawner("moc_prydb1", "Osiris", JT_OSIRIS, 1, initialDelay: Hours(3), respawnDelay: Hours(6));
	}
}
