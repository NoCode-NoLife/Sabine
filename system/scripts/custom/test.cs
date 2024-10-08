﻿//--- Sabine Script ---------------------------------------------------------
// Test NPC
//--- Description -----------------------------------------------------------
// Playground.
//---------------------------------------------------------------------------

using System.Threading.Tasks;
using Sabine.Zone.Scripting;
using Sabine.Zone.Scripting.Dialogues;
using Sabine.Zone.World.Shops;
using static Sabine.Zone.Scripting.Shortcuts;

public class TestNpcScript : GeneralScript
{
	public override void Load()
	{
		AddShop("TestShop1", shop =>
		{
			shop.AddItem(ItemId.RedPotion, 100);
			shop.AddItem(ItemId.Sword, 200);
		});

		if (MapsExist("prt_vilg02"))
			AddNpc("Test", 54, "prt_vilg02", 105, 89, TestDialog);
		else if (MapsExist("prontera"))
			AddNpc("Test", 54, "prontera", 157, 193, TestDialog);
	}

	private static async Task TestDialog(Dialog dialog)
	{
		dialog.PlayerLocalization(out var L, out var LN);

		dialog.Msg("[Test]");
		dialog.Msg("Hello, world!");
		await dialog.Next();

		dialog.Msg("[Test]");
		dialog.Msg(L("How are you?"));
		await dialog.Next();

		var response = await dialog.Select(Option("Good!", "good"), Option("Meh.", "bad"));

		dialog.Msg("[Test]");
		switch (response)
		{
			case "good":
			{
				dialog.Msg("That's nice to hear! Sounds like you're in the mood for a good deal!");
				await dialog.Next();
				dialog.OpenShop(NpcShop.Build(501, -1, 502, -1, 503, 250), ShopOpenType.BuyOnly);
				break;
			}
			case "bad":
			{
				dialog.Msg("Aw, you should buy yourself something nice then! That will cheer you right up!");
				await dialog.Next();
				dialog.OpenShop("TestShop1");
				break;
			}
		}

		// Move along, just having fun here.

		//dialog.AthenaFTW(out var mes, out var next, out var select);

		//mes("[Test]");
		//mes("Hello, world!");
		//await next();

		//mes("[Test]");
		//mes("How are you?");
		//await next();

		//var response = await select("Good!", "Meh");

		//mes("[Test]");
		//switch (response)
		//{
		//	case 1: mes("That's nice to hear!"); break;
		//	case 2: mes("Toughen up!"); break;
		//}

		// Style test for what NPCs could look like. Msg would always
		// send a wait at the end and codes like p and br could be
		// used to split up the message. The NPC's name would become
		// the default title and would always be displayed with each
		// message, unless unset.

		//await dialog.MsgAdv("Hello, world!<p/>How are you?");
		//var response = await dialog.Select(Option("Good!", "good"), Option("Meh.", "bad"));

		//switch (response)
		//{
		//	case "good": await dialog.MsgAdv("That's nice to hear!"); break;
		//	case "bad": await dialog.MsgAdv("Toughen up!"); break;
		//}
	}
}
