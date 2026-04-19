//--- Sabine Script ---------------------------------------------------------
// Healing Item Scripts
//--- Description -----------------------------------------------------------
// Defines usage scripts for healing items.
//--- Credits ---------------------------------------------------------------
// eAthena
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using Sabine.Zone.World.Actors;

[ItemScript(ItemId.RedPotion)]
public class Item501 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		// TODO: Update healing amounts for alpha, use a feature check.

		player.HealHp(Random(45, 65));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.ScarletPotion)]
public class Item502 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(105, 145));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.YellowPotion)]
public class Item503 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(175, 235));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.WhitePotion)]
public class Item504 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(325, 405));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.BluePotion)]
public class Item505 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(40, 60));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.GreenPotion)]
public class Item506 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		//player.RemoveStatusEffect(StatusEffectId.Poison);
		//player.RemoveStatusEffect(StatusEffectId.Silence);
		//player.RemoveStatusEffect(StatusEffectId.Blind);
		//player.RemoveStatusEffect(StatusEffectId.Confusion);

		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.RedHerb)]
public class Item507 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(18, 28));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.YellowHerb)]
public class Item508 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(38, 58));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.WhiteHerb)]
public class Item509 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(75, 115));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.BlueHerb)]
public class Item510 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(15, 30));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.GreenHerb)]
public class Item511 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		//player.RemoveStatusEffect(StatusEffectId.Poison);
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Apple)]
public class Item512 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(16, 22));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Banana)]
public class Item513 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(17, 21));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Grape)]
public class Item514 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(10, 15));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Meat)]
public class Item517 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(70, 100));
		return ItemUseResult.Okay;
	}
}

//---------------------------------------------------------------------------
// The following items were usable healing items in the alpha, based on
// certain online item databases from 2001. However, we don't have any
// concrete healing amounts, so have to wing it a little.
//---------------------------------------------------------------------------

[ItemScript(ItemId.TreeRoot)]
public class Item902 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.ScorpionTail)]
public class Item904 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(15, 30));
		//player.RemoveStatusEffect(StatusEffectId.Poison);

		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Scale)]
public class Item906 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Resin)]
public class Item907 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(15, 30));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Spawn)]
public class Item908 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(30, 60));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Jellopy)]
public class Item909 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(1, 3));
		return ItemUseResult.Okay;
	}
}

[ItemScript(ItemId.Sel)]
public class Item911 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}
