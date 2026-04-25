//--- Sabine Script ---------------------------------------------------------
// Morocc Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Morocc.
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;
using static Sabine.Shared.Const.IdentityId;

[RequiresMaps("moc_fild01", "moc_dugn01", "dungeon000")]
public class MoroccMonstersScript : GeneralScript
{
	public override void Load()
	{
		// Morocc North-East Field
		AddSpawner("moc_fild04", "Condor", JT_CONDOR, 30);
		AddSpawner("moc_fild04", "Steel Chocho", JT_STEEL_CHONCHON, 20);
		AddSpawner("moc_fild04", "PecoPeco", JT_PECOPECO, 10);
		AddSpawner("moc_fild04", "Wolf", JT_WOLF, 10);

		// Morocc South Field
		AddSpawner("moc_fild01", "Condor", JT_CONDOR, 30);
		AddSpawner("moc_fild01", "Steel Chocho", JT_STEEL_CHONCHON, 20);
		AddSpawner("moc_fild01", "PecoPeco", JT_PECOPECO, 10);
		AddSpawner("moc_fild01", "Wolf", JT_WOLF, 10);

		// Morocc East Field
		AddSpawner("moc_fild03", "Spore", JT_SPORE, 30);
		AddSpawner("moc_fild03", "PecoPeco", JT_PECOPECO, 20);
		AddSpawner("moc_fild03", "Mandragora", JT_MANDRAGORA, 10);
		AddSpawner("moc_fild03", "Warmtail", JT_WORM_TAIL, 5);
		AddSpawner("moc_fild03", "Boa", JT_SNAKE, 10);

		// Morocc North Field
		AddSpawner("moc_fild02", "Condor", JT_CONDOR, 30);
		AddSpawner("moc_fild02", "Steel Chocho", JT_STEEL_CHONCHON, 20);
		AddSpawner("moc_fild02", "Wolf", JT_WOLF, 15);
		AddSpawner("moc_fild02", "PecoPeco", JT_PECOPECO, 15);
		AddSpawner("moc_fild02", "Scorpion", JT_SCORPION, 10);

		// Morocc Jungle
		AddSpawner("dungeon000", "Fabre", JT_FABRE, 20);
		AddSpawner("dungeon000", "Scorpion", JT_SCORPION, 15);
		AddSpawner("dungeon000", "Poporing", JT_POPORING, 15);
		AddSpawner("dungeon000", "Creamy", JT_CREAMY, 10);
		AddSpawner("dungeon000", "Anacondaq", JT_ANACONDAQ, 10);

		// Inside Morocc Jungle
		AddSpawner("dungeon001", "Elder Willow", JT_ELDER_WILOW, 30);
		AddSpawner("dungeon001", "Poporing", JT_POPORING, 20);
		AddSpawner("dungeon001", "Anacondaq", JT_ANACONDAQ, 20);
		AddSpawner("dungeon001", "Baphomet", JT_BAPHOMET, 1, initialDelay: Hours(3), respawnDelay: Hours(6));

		// Pyramid 1F
		AddSpawner("moc_dugn01", "Farmiliar", JT_FARMILIAR, 20);
		AddSpawner("moc_dugn01", "Skel Archer", JT_ARCHER_SKELETON, 15);
		AddSpawner("moc_dugn01", "Skel Soldier", JT_SOLDIER_SKELETON, 15);
		AddSpawner("moc_dugn01", "Munak", JT_MUNAK, 15);
		AddSpawner("moc_dugn01", "Isis", JT_ISIS, 10);

		// Pyramid 2F
		AddSpawner("moc_dugn02", "Mummy", JT_MUMMY, 20);
		AddSpawner("moc_dugn02", "Vorit", JT_VERIT, 20);
		AddSpawner("moc_dugn02", "Osiris", JT_OSIRIS, 1, initialDelay: Hours(3), respawnDelay: Hours(6));
	}
}
