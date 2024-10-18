//--- Melia Script ----------------------------------------------------------
// Character Initialization
//--- Description -----------------------------------------------------------
// Grants default items to newly created characters.
//---------------------------------------------------------------------------

using Sabine.Zone.Events.Args;
using Sabine.Zone.Scripting;
using Yggdrasil.Events;

public class CharacterInitializationScript : GeneralScript
{
	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		var character = args.Character;

		if (!character.Vars.Perm.ActivateOnce("Sabine.Initialized"))
			return;

		character.Inventory.Add(ItemId.Knife, 1);
		character.Inventory.Add(ItemId.CottonShirt, 1);
	}
}
