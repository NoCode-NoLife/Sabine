//--- Sabine Script ---------------------------------------------------------
// Morocc Monster Spawns
//--- Description -----------------------------------------------------------
// Sets up monster spawners in and around Morocc.
//---------------------------------------------------------------------------

using System;
using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class MoroccMonstersScript : GeneralScript
{
	public override void Load()
	{
		if (!MapsExist("moc_fild01", "moc_fild02", "moc_fild04", "moc_fild04", "dungeon000", "dungeon001", "moc_dugn01", "moc_dugn02"))
			return;

		// Morocc North-East Field
		AddSpawner("moc_fild04", "Condor", 1009, 30);
		AddSpawner("moc_fild04", "Steel Chocho", 1042, 20);
		AddSpawner("moc_fild04", "PecoPeco", 1019, 10);
		AddSpawner("moc_fild04", "Wolf", 1013, 10);

		// Morocc South Field
		AddSpawner("moc_fild01", "Condor", 1009, 30);
		AddSpawner("moc_fild01", "Steel Chocho", 1042, 20);
		AddSpawner("moc_fild01", "PecoPeco", 1019, 10);
		AddSpawner("moc_fild01", "Wolf", 1013, 10);

		// Morocc East Field
		AddSpawner("moc_fild03", "Spore", 1014, 30);
		AddSpawner("moc_fild03", "PecoPeco", 1019, 20);
		AddSpawner("moc_fild03", "Mandragora", 1020, 10);
		AddSpawner("moc_fild03", "Warmtail", 1024, 5);
		AddSpawner("moc_fild03", "Boa", 1025, 10);

		// Morocc North Field
		AddSpawner("moc_fild02", "Condor", 1009, 30);
		AddSpawner("moc_fild02", "Steel Chocho", 1042, 20);
		AddSpawner("moc_fild02", "Wolf", 1013, 15);
		AddSpawner("moc_fild02", "PecoPeco", 1019, 15);
		AddSpawner("moc_fild02", "Scorpion", 1001, 10);

		// Morocc Jungle
		AddSpawner("dungeon000", "Fabre", 1007, 20);
		AddSpawner("dungeon000", "Scorpion", 1001, 15);
		AddSpawner("dungeon000", "Poporing", 1031, 15);
		AddSpawner("dungeon000", "Creamy", 1018, 10);
		AddSpawner("dungeon000", "Anacondaq", 1030, 10);

		// Inside Morocc Jungle
		AddSpawner("dungeon001", "Elder Willow", 1033, 30);
		AddSpawner("dungeon001", "Poporing", 1031, 20);
		AddSpawner("dungeon001", "Anacondaq", 1030, 20);
		AddSpawner("dungeon001", "Doppelganger", 1046, 1, initialDelay: TimeSpan.FromHours(3), respawnDelay: TimeSpan.FromHours(6));

		// Pyramid 1F
		AddSpawner("moc_dugn01", "Farmiliar", 1005, 20);
		AddSpawner("moc_dugn01", "Skel Archer", 1016, 15);
		AddSpawner("moc_dugn01", "Skel Soldier", 1028, 15);
		AddSpawner("moc_dugn01", "Munak", 1026, 15);
		AddSpawner("moc_dugn01", "Isis", 1029, 10);

		// Pyramid 2F
		AddSpawner("moc_dugn02", "Mummy", 1041, 20);
		AddSpawner("moc_dugn02", "Vorit", 1032, 20);
		AddSpawner("moc_dugn02", "Osiris", 1046, 1, initialDelay: TimeSpan.FromHours(3), respawnDelay: TimeSpan.FromHours(6));
	}
}
