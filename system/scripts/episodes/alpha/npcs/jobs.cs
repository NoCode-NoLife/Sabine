//--- Sabine Script ---------------------------------------------------------
// Job Changers
//--- Description -----------------------------------------------------------
// Allow Novices to change their job.
//---------------------------------------------------------------------------

using Sabine.Shared.Const;
using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class JobChangerNpcScripts : GeneralScript
{
	public override void Load()
	{
		if (!MapsExist("prt_intr01", "prt_intr02", "moc_intr02", "moc_intr04"))
			return;

		// Swordman job changer in a combat instruction building in
		// South Prontera
		AddNpc("Job Tester", 53, "prt_intr01", 129, 102, 3, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Swordman)
			{
				await dialog.Talk("Hello, how is life as a Swordman?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.Talk("You have made your choice.");
				return;
			}

			await dialog.Talk("Hello, would you like to become a Swordman?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Come back if you change your mind.");
				return;
			}

			await dialog.Talk("To become a Swordman, you need to reach level 10 and aquire a Resident Certificate and 10 Claw of Wolves.<p/>You also need to pay a fee of 200 Zeny. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 10 || player.Parameters.Zeny < 200 || !player.Inventory.Contains(ItemId.ResidentCert) || !player.Inventory.Contains(ItemId.ClawofWolves, 10))
			{
				await dialog.Talk("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Parameters.Modify(ParameterType.Zeny, -200);
			player.Inventory.Remove(ItemId.ClawofWolves, 10);
			player.ChangeJob(JobId.Swordman);

			await dialog.Talk("Very well. You are now a Swordman. Good luck.");
		});

		// Merchant job changer in the town hall in South Prontera
		AddNpc("Job Tester", 53, "prt_intr01", 30, 58, 5, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Merchant)
			{
				await dialog.Talk("Hello, how is life as a Merchant?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.Talk("You have made your choice.");
				return;
			}

			await dialog.Talk("Hello, would you like to become a Merchant?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Come back if you change your mind.");
				return;
			}

			await dialog.Talk("To become a Merchant, you need to reach level 5 and aquire a Resident Certificate and a Business Certificate.<p/>You also need to pay a fee of 500 Zeny. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 5 || player.Parameters.Zeny < 500 || !player.Inventory.Contains(ItemId.ResidentCert) || !player.Inventory.Contains(ItemId.BusinessCert))
			{
				await dialog.Talk("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Parameters.Modify(ParameterType.Zeny, -500);
			player.ChangeJob(JobId.Merchant);

			await dialog.Talk("Very well. You are now a Merchant. Good luck.");
		});

		// Thief job changer in the pyramid in Morocc Village
		AddNpc("Job Tester", 53, "moc_intr04", 173, 42, 5, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Thief)
			{
				await dialog.Talk("Hello, how is life as a Thief?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.Talk("You have made your choice.");
				return;
			}

			await dialog.Talk("Hello, would you like to become a Thief?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Come back if you change your mind.");
				return;
			}

			await dialog.Talk("To become a Thief, you need to reach level 6.<p/>You also need to pay a fee of 400 Zeny. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 6 || player.Parameters.Zeny < 400)
			{
				await dialog.Talk("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Parameters.Modify(ParameterType.Zeny, -400);
			player.ChangeJob(JobId.Thief);

			await dialog.Talk("Very well. You are now a Thief. Good luck.");
		});

		// Archer job changer in the large building in Archers' Village
		AddNpc("Job Tester", 53, "moc_intr02", 26, 176, 5, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Archer)
			{
				await dialog.Talk("Hello, how is life as an Archer?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.Talk("You have made your choice.");
				return;
			}

			await dialog.Talk("Hello, would you like to become an Archer?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Come back if you change your mind.");
				return;
			}

			await dialog.Talk("To become an Archer, you need to reach level 8 and aquire 20 Feather of Birds. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 8 || !player.Inventory.Contains(ItemId.FeatherofBird, 20))
			{
				await dialog.Talk("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Inventory.Remove(ItemId.FeatherofBird, 20);
			player.ChangeJob(JobId.Archer);

			await dialog.Talk("Very well. You are now an Archer. Good luck.");
		});

		// Acolyte job changer in the cathedral in North Prontera
		AddNpc("Job Tester", 53, "prt_intr02", 35, 177, 3, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Acolyte)
			{
				await dialog.Talk("Hello, how is life as an Acolyte?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.Talk("You have made your choice.");
				return;
			}

			await dialog.Talk("Hello, would you like to become an Acolyte?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.Talk("Come back if you change your mind.");
				return;
			}

			if (player.Parameters.BaseLevel < 5)
			{
				await dialog.Talk("Unfortunately you don't meet the requirements. You need to be at least level 5.<p/>Come back once you've become stronger.");
				return;
			}

			await dialog.Talk("Very well.");
			player.ChangeJob(JobId.Acolyte);

			await dialog.Talk("You are now an Acolyte. Good luck...");
		});
	}
}
