//--- Sabine Script ---------------------------------------------------------
// Morocc NPCs
//--- Description -----------------------------------------------------------
// Sets up NPCs stationed in the general Morocc area.
//--- Reference -------------------------------------------------------------
// https://gamefaqs.gamespot.com/pc/561051-ragnarok-online/faqs/14086
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using static Sabine.Zone.Scripting.Shortcuts;

public class MoroccNpcsScript : GeneralScript
{
	public override void Load()
	{
		if (!MapsExist("moc_vilg01", "moc_intr01"))
			return;

		LoadNpcs();
		LoadShops();
	}

	private static void LoadNpcs()
	{
		// Seen in screen shots. Might be a less important merchant.
		AddNpc("Granny", 103, "moc_vilg01", 104, 65, 2);

		// Seen in screen shots. Building appears to an inn.
		AddNpc("Maid", 69, "moc_intr01", 76, 140, 4);
		AddNpc("Receptionist", 102, "moc_intr01", 72, 144, 4);
	}

	private static void LoadShops()
	{
		AddShopNpc("Weapon Dealer", 58, "moc_intr01", 156, 146, 2, shop =>
		{
			shop.AddItems(ItemId.Axe);
			shop.AddItems(ItemId.BattleAxe);
			shop.AddItems(ItemId.WarHammer);
			shop.AddItems(ItemId.Buster);

			shop.AddItems(ItemId.Javelin);
			shop.AddItems(ItemId.Guisarme);
			shop.AddItems(ItemId.Lance);
			shop.AddItems(ItemId.Spear);
			shop.AddItems(ItemId.Pike);
			shop.AddItems(ItemId.Glaive);
			shop.AddItems(ItemId.Partizan);
			shop.AddItems(ItemId.Trident);

			shop.AddItems(ItemId.Bow);
			shop.AddItems(ItemId.CompositeBow);
			shop.AddItems(ItemId.GreatBow);
			shop.AddItems(ItemId.Crossbow);
			shop.AddItems(ItemId.Arbalest);
		});

		AddShopNpc("Armor Dealer", 58, "moc_intr01", 156, 141, 2, shop =>
		{
			shop.AddItems(ItemId.Buckler);
			shop.AddItems(ItemId.Shield);

			shop.AddItems(ItemId.Hat);
			shop.AddItems(ItemId.Cap);
			shop.AddItems(ItemId.Helm);

			shop.AddItems(ItemId.AdventureSuit);
			shop.AddItems(ItemId.WoodenMail);
			shop.AddItems(ItemId.Scapulare);
			shop.AddItems(ItemId.SilkRobe);
			shop.AddItems(ItemId.SilverRobe);
			shop.AddItems(ItemId.Mantle);
			shop.AddItems(ItemId.Coat);
			shop.AddItems(ItemId.PaddedArmor);
			shop.AddItems(ItemId.ChainMail);
			shop.AddItems(ItemId.Tights);
			shop.AddItems(ItemId.ThievesCloth);

			shop.AddItems(ItemId.Boots);

			shop.AddItems(ItemId.Hood);
			shop.AddItems(ItemId.Muffler);
			shop.AddItems(ItemId.Manteau);
		});

		AddShopNpc("Item Dealer", 102, "moc_vilg01", 105, 46, 2, shop =>
		{
			shop.AddItem(ItemId.RedPotion);
			shop.AddItem(ItemId.ScarletPotion);
			shop.AddItem(ItemId.YellowPotion);
			shop.AddItem(ItemId.WhitePotion);
			shop.AddItem(ItemId.BluePotion);
			shop.AddItem(ItemId.GreenPotion);
		});
	}
}
