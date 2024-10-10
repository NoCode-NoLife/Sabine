//--- Sabine Script ---------------------------------------------------------
// Prontera NPCs
//--- Description -----------------------------------------------------------
// Sets up NPCs stationed in the general Prontera area.
//--- Reference -------------------------------------------------------------
// https://gamefaqs.gamespot.com/pc/561051-ragnarok-online/faqs/14086
//---------------------------------------------------------------------------

using Sabine.Shared.Const;
using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class PronteraNpcsScript : GeneralScript
{
	public override void Load()
	{
		if (!MapsExist("prt_vilg01", "prt_intr01", "prt_intr02"))
			return;

		LoadNpcs();
		LoadShops();
	}

	private static void LoadNpcs()
	{
		// This NPC is mentioned in the guide, but its purpose is unknown.
		AddNpc("Prize Exchanger", 83, "prt_intr02", 127, 102, 3, async dialog =>
		{
			await dialog.Talk("Who... am I? And what am I doing here?");
		});

		// Seen in screen shots, names and purposes unknown.
		AddNpc("Office Lady", 102, "prt_intr01", 120, 27, 4);
		AddNpc("Office Lady", 102, "prt_intr01", 124, 27, 4);

		// Seen in screen shots
		AddNpc("Guard", 105, "prt_cstl01", 39, 66, 6);
		AddNpc("Guard", 105, "prt_cstl01", 48, 66, 3);
		AddNpc("Guard", 105, "prt_vilg01", 99, 89, 4);

		// Inn, described in guide.
		AddNpc("Maid", 94, "prt_intr01", 33, 21, 4);
		AddNpc("Receptionist", 102, "prt_intr01", 38, 24, 4);

		// There's a cathedral, there's a pastor sprite, there was probably
		// a pastor in the cathedral.
		AddNpc("Pastor", 60, "prt_intr02", 43, 172, 3, async dialog =>
		{
			await dialog.Talk("Welcome, my child.");
		});

		// Lets players enter the sewers
		AddNpc("Soldier", 105, "prt_intr01", 18, 78, 4, async dialog =>
		{
			await dialog.Talk("The sewer system is infested with monsters. Will you help us clean them out?");
			var response = await dialog.Select(Option("Yes", "yes"), Option("No", "no"));

			switch (response)
			{
				case "yes":
				{
					await dialog.Talk("Great, let's go!");
					dialog.Player.Warp("prt_dugn00", 41, 31);
					break;
				}
				case "no":
				{
					await dialog.Talk("Oh... alright.");
					break;
				}
			}
		});
	}

	private static void LoadShops()
	{
		AddShopNpc("Weapon Dealer", 50, "prt_intr02", 182, 100, 7, shop =>
		{
			shop.AddItems(ItemId.Sword);
			shop.AddItems(ItemId.Falchion);
			shop.AddItems(ItemId.Blade);
			shop.AddItems(ItemId.Lapier);
			shop.AddItems(ItemId.Scimiter);
			shop.AddItems(ItemId.Slayer);
			shop.AddItems(ItemId.Katana);
			shop.AddItems(ItemId.BastardSword);

			shop.AddItems(ItemId.Knife);
			shop.AddItems(ItemId.Dirk);
			shop.AddItems(ItemId.Dagger);
			shop.AddItems(ItemId.Stiletto);

			shop.AddItems(ItemId.Club);

			shop.AddItems(ItemId.Rod);
			shop.AddItems(ItemId.Wand);
		});

		AddShopNpc("Armor Dealer", 69, "prt_intr02", 182, 96, 7, shop =>
		{
			shop.AddItems(ItemId.Guard);

			shop.AddItems(ItemId.Ribbon);
			shop.AddItems(ItemId.HairBand);
			shop.AddItems(ItemId.Goggle);
			shop.AddItems(ItemId.Biretta);
			shop.AddItems(ItemId.Hat);

			shop.AddItems(ItemId.CottonShirt);
			shop.AddItems(ItemId.LeatherJacket);
			shop.AddItems(ItemId.AdventureSuit);
			shop.AddItems(ItemId.WoodenMail);
			shop.AddItems(ItemId.Scapulare);
			shop.AddItems(ItemId.SilkRobe);
			shop.AddItems(ItemId.SilverRobe);
			shop.AddItems(ItemId.Mantle);
			shop.AddItems(ItemId.Coat);

			shop.AddItems(ItemId.Sandals);
			shop.AddItems(ItemId.Hood);
		});

		AddShopNpc("Item Dealer", 83, "prt_intr02", 125, 102, 3, shop =>
		{
			shop.AddItem(ItemId.RedPotion);
			shop.AddItem(ItemId.ScarletPotion);
			shop.AddItem(ItemId.YellowPotion);
			shop.AddItem(ItemId.WhitePotion);
			shop.AddItem(ItemId.BluePotion);
			shop.AddItem(ItemId.GreenPotion);
		});

		AddShopNpc("Food Merchant", 83, "prt_vilg01", 82, 128, 5, shop =>
		{
			shop.AddItem(ItemId.Apple);
			shop.AddItem(ItemId.Banana);
			shop.AddItem(ItemId.Grape);
			shop.AddItem(ItemId.Meat);
		});

		// Seller for certificates
		AddNpc("Resident Office", 69, "prt_intr01", 30, 76, 6, async dialog =>
		{
			await dialog.Talk("What can I do for you?");
			var response = await dialog.Select(Option("Buy Resident Certificate", "buy_resident"), Option("Buy Business Certificate", "buy_business"), Option("Nevermind.", "nvm"));

			var itemClassId = 0;
			var price = 0;

			switch (response)
			{
				default:
				case "nvm":
				{
					await dialog.Talk("Please don't distract me if you don't need anything.");
					return;
				}
				case "buy_resident":
				{
					await dialog.Talk("A Resident Certificate? Certainly. That's 500 Zeny.");
					itemClassId = 20002;
					price = 500;
					break;
				}
				case "buy_business":
				{
					await dialog.Talk("A Business Certificate? Certainly. That's 1000 Zeny.");
					itemClassId = 20003;
					price = 1000;
					break;
				}
			}

			response = await dialog.Select(Option("Here you go.", "yes"), Option("I changed my mind.", "no"));
			if (response == "no")
			{
				await dialog.Talk("Hm, okay then.");
				return;
			}

			if (dialog.Player.Parameters.Zeny < price)
			{
				await dialog.Talk("What's this, you don't have the money? Please don't distract me then.");
				return;
			}

			dialog.Player.Parameters.Modify(ParameterType.Zeny, -price);
			dialog.Player.Inventory.Add(itemClassId, 1);

			await dialog.Talk("Thank you, have a good day.");
		});
	}
}
