using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class PronteraNorthMapScript : MapScript
{
	public override void Load()
	{
		AddNpc("Guide", 54, "prt_vilg02", 99, 89, async dialog =>
		{
			dialog.Msg("[Guide]");
			dialog.Msg("Hello, world!");
			await dialog.Next();

			dialog.Msg("[Guide]");
			dialog.Msg("How are you?");
			await dialog.Next();

			var response = await dialog.Select(Option("Good!", "good"), Option("Meh.", "bad"));

			dialog.Msg("[Guide]");
			switch (response)
			{
				case "good": dialog.Msg("That's nice to hear!"); break;
				case "bad": dialog.Msg("Toughen up!"); break;
			}

			// Move along, just having fun here.

			//dialog.AthenaFTW(out var mes, out var next, out var select);

			//mes("[Guide]");
			//mes("Hello, world!");
			//await next();

			//mes("[Guide]");
			//mes("How are you?");
			//await next();

			//var response = await select("Good!", "Meh");

			//mes("[Guide]");
			//switch (response)
			//{
			//	case 1: mes("That's nice to hear!"); break;
			//	case 2: mes("Toughen up!"); break;
			//}

			// Style test for how NPCs could look like. Msg would always
			// send a wait at the end and codes like p and br could be
			// used to split up the message. The NPC's name would become
			// the default title and always be displayed with each
			// message, unless unset.

			//await dialog.MsgAdv("Hello, world!<p/>How are you?");
			//var response = await dialog.Select(Option("Good!", "good"), Option("Meh.", "bad"));

			//switch (response)
			//{
			//	case "good": await dialog.MsgAdv("That's nice to hear!"); break;
			//	case "bad": await dialog.MsgAdv("Toughen up!"); break;
			//}
		});

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
}
