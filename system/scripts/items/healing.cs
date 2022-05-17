//--- Sabine Script ---------------------------------------------------------
// Healing Item Scripts
//--- Description -----------------------------------------------------------
// Defines usage scripts for healing items.
//--- Credits ---------------------------------------------------------------
// eAthena
//---------------------------------------------------------------------------

using Sabine.Zone.Scripting;
using Sabine.Zone.World.Entities;

// Red Potion
[ItemScript(501)]
public class Item501 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		// TODO: Update healing amounts for alpha, use a feature check.

		player.HealHp(Random(45, 65));
		return ItemUseResult.Okay;
	}
}

// Orange Potion
[ItemScript(502)]
public class Item502 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(105, 145));
		return ItemUseResult.Okay;
	}
}

// Yellow Potion
[ItemScript(503)]
public class Item503 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(175, 235));
		return ItemUseResult.Okay;
	}
}

// White Potion
[ItemScript(504)]
public class Item504 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(325, 405));
		return ItemUseResult.Okay;
	}
}

// Blue Potion
[ItemScript(505)]
public class Item505 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(40, 60));
		return ItemUseResult.Okay;
	}
}

// Green Potion
[ItemScript(506)]
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

// Red Herb
[ItemScript(507)]
public class Item507 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(18, 28));
		return ItemUseResult.Okay;
	}
}

// Yellow Herb
[ItemScript(508)]
public class Item508 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(38, 58));
		return ItemUseResult.Okay;
	}
}

// White Herb
[ItemScript(509)]
public class Item509 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(75, 115));
		return ItemUseResult.Okay;
	}
}

// Blue Herb
[ItemScript(510)]
public class Item510 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(15, 30));
		return ItemUseResult.Okay;
	}
}

// Green Herb
[ItemScript(511)]
public class Item511 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		//player.RemoveStatusEffect(StatusEffectId.Poison);
		return ItemUseResult.Okay;
	}
}

// Apple
[ItemScript(512)]
public class Item512 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(16, 22));
		return ItemUseResult.Okay;
	}
}

// Banana
[ItemScript(513)]
public class Item513 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(17, 21));
		return ItemUseResult.Okay;
	}
}

// Grape
[ItemScript(514)]
public class Item514 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealSp(Random(10, 15));
		return ItemUseResult.Okay;
	}
}

// Meat
[ItemScript(517)]
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
// certain online item database from 2001. However, we don't have any
// concrete healing amounts, so have to wing it a little.
//---------------------------------------------------------------------------

// Tree Root
[ItemScript(902)]
public class Item902 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}

// Scorpion Tail
[ItemScript(904)]
public class Item904 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(15, 30));
		//player.RemoveStatusEffect(StatusEffectId.Poison);

		return ItemUseResult.Okay;
	}
}

// Scale
[ItemScript(906)]
public class Item906 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}

// Resin
[ItemScript(907)]
public class Item907 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(15, 30));
		return ItemUseResult.Okay;
	}
}

// Spawn
[ItemScript(908)]
public class Item908 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(30, 60));
		return ItemUseResult.Okay;
	}
}

// Jellopy
[ItemScript(909)]
public class Item909 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(1, 3));
		return ItemUseResult.Okay;
	}
}

// Sel
[ItemScript(911)]
public class Item911 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(10, 20));
		return ItemUseResult.Okay;
	}
}
