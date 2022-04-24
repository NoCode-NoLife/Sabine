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
		// Swordman job changer in a combat instruction building in
		// South Prontera
		AddNpc("Job Tester", 53, "prt_intr01", 129, 102, 3, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Swordman)
			{
				await dialog.MsgAdv("Hello, how is life as an Swordman?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.MsgAdv("You have made your choice.");
				return;
			}

			await dialog.MsgAdv("Hello, would you like to become an Swordman?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.MsgAdv("Come back if you change your mind.");
				return;
			}

			await dialog.MsgAdv("To become a Swordman, you need to reach level 10 and aquire a Resident Certificate and 10 Claw of Wolves.<p/>You also need to pay a fee of 200 Zeny. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.MsgAdv("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 10 || player.Parameters.Zeny < 200 || !player.Inventory.Contains(ItemId.ResidentCert) || !player.Inventory.Contains(ItemId.ClawofWolves, 10))
			{
				await dialog.MsgAdv("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Parameters.Modify(ParameterType.Zeny, -200);
			player.Inventory.Remove(ItemId.ClawofWolves, 10);
			player.ChangeJob(JobId.Swordman);

			await dialog.MsgAdv("Very well. You are now a Swordman. Good luck.");
		});

		// Merchant job changer in the town hall in South Prontera
		AddNpc("Job Tester", 53, "prt_intr01", 30, 58, 5, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Merchant)
			{
				await dialog.MsgAdv("Hello, how is life as an Merchant?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.MsgAdv("You have made your choice.");
				return;
			}

			await dialog.MsgAdv("Hello, would you like to become an Merchant?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.MsgAdv("Come back if you change your mind.");
				return;
			}

			await dialog.MsgAdv("To become a Merchant, you need to reach level 5 and aquire a Resident Certificate and a Business Certificate.<p/>You also need to pay a fee of 500 Zeny. Would you like to continue?");
			response = await dialog.Select(Option("Yes.", "yes"), Option("I changed my mind.", "no"));

			if (response == "no")
			{
				await dialog.MsgAdv("Alright, come back any time.");
				return;
			}

			if (player.Parameters.BaseLevel < 5 || player.Parameters.Zeny < 500 || !player.Inventory.Contains(ItemId.ResidentCert) || !player.Inventory.Contains(ItemId.BusinessCert))
			{
				await dialog.MsgAdv("Unfortunately you don't meet the requirements.");
				return;
			}

			player.Parameters.Modify(ParameterType.Zeny, -500);
			player.ChangeJob(JobId.Merchant);

			await dialog.MsgAdv("Very well. You are now a Merchant. Good luck.");
		});

		// Acolyte job changer in the cathedral in North Prontera
		AddNpc("Job Tester", 53, "prt_intr02", 35, 177, 3, async dialog =>
		{
			var player = dialog.Player;

			if (player.JobId == JobId.Acolyte)
			{
				await dialog.MsgAdv("Hello, how is life as an Acolyte?");
				return;
			}
			else if (player.JobId != JobId.Novice)
			{
				await dialog.MsgAdv("You have made your choice.");
				return;
			}

			await dialog.MsgAdv("Hello, would you like to become an Acolyte?");
			var response = await dialog.Select(Option("Yes, please.", "yes"), Option("No, thanks.", "no"));

			if (response == "no")
			{
				await dialog.MsgAdv("Come back if you change your mind.");
				return;
			}

			if (player.Parameters.BaseLevel < 5)
			{
				await dialog.MsgAdv("Unfortunately you don't meet the requirements. You need to be at least level 5.<p/>Come back once you've become stronger.");
				return;
			}

			await dialog.MsgAdv("Very well.");
			player.ChangeJob(JobId.Acolyte);

			await dialog.MsgAdv("You are now an Acolyte. Good luck...");
		});
	}
}
