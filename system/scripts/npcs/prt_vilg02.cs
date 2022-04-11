using Sabine.Zone.Scripting;

public class PronteraNorthMapScript : MapScript
{
	public override void Load()
	{
		AddNpc("Guide", 54, "prt_vilg02", 99, 89);

		AddWarp(From("prt_vilg02", 100, 22), To("prt_vilg01", 100, 174));
		AddWarp(From("prt_vilg01", 100, 176), To("prt_vilg02", 100, 24));
	}
}
