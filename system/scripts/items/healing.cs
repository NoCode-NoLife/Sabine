using Sabine.Zone.Scripting;
using Sabine.Zone.World.Entities;

// Red Potion
[ItemScript(501)]
public class Item501 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
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

// Yellow White
[ItemScript(504)]
public class Item504 : ItemScript
{
	public override ItemUseResult OnUse(PlayerCharacter player, Item item)
	{
		player.HealHp(Random(325, 405));
		return ItemUseResult.Okay;
	}
}
