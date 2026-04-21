//--- Melia Script ----------------------------------------------------------
// Character Initialization
//--- Description -----------------------------------------------------------
// Grants default items to newly created characters.
//---------------------------------------------------------------------------

using Sabine.Zone.Events.Args;
using Sabine.Zone.Scripting;
using Sabine.Zone.World.Actors;
using Yggdrasil.Events;

public class CharacterInitializationScript : GeneralScript
{
	[On("PlayerReady")]
	public void OnPlayerReady(object sender, PlayerEventArgs args)
	{
		InitItems(args.Character);
		InitSkills(args.Character);
	}

	private static void InitItems(PlayerCharacter character)
	{
		if (!character.Vars.Perm.ActivateOnce("Sabine.Initialized"))
			return;

		character.Inventory.Add(ItemId.Knife, 1);
		character.Inventory.Add(ItemId.CottonShirt, 1);
	}

	private void InitSkills(PlayerCharacter character)
	{
		// Always execute on login, in case the skill tree data changed
		// and we need to update the skills
		character.Skills.UpdateClassSkills();
	}
}
