using System;

namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines the result of a pickup request.
	/// </summary>
	public enum PickUpResult : byte
	{
		/// <summary>
		/// Item was picked up and will be added to the inventory.
		/// </summary>
		Okay = 0,

		/// <summary>
		/// Shows an error saying that the item is not obtainable.
		/// </summary>
		CantGet = 1,

		/// <summary>
		/// Shows an error that the item can't be picked up due to
		/// the character being above their weight limit.
		/// </summary>
		Overweight = 2,
	}

	/// <summary>
	/// Specifies an item's type, which affects under which tab it's
	/// displayed by the client.
	/// </summary>
	public enum ItemType : byte
	{
		/// <summary>
		/// Item tab.
		/// </summary>
		Healing = 0,

		/// <summary>
		/// Item tab.
		/// </summary>
		Item2 = 1,

		/// <summary>
		/// Item tab.
		/// </summary>
		Usable = 2,

		/// <summary>
		/// Etc tab.
		/// </summary>
		Etc = 3,

		/// <summary>
		/// Equip tab.
		/// </summary>
		Weapon = 4,

		/// <summary>
		/// Equip tab.
		/// </summary>
		Armor = 5,

		/// <summary>
		/// Etc tab.
		/// </summary>
		Etc2 = 6,

		/// <summary>
		/// Etc tab.
		/// </summary>
		Etc3 = 7,

		/// <summary>
		/// Equip tab.
		/// </summary>
		/// <remarks>
		/// The alpha client uses this type to modify the attack range.
		/// By default, it's "17" (not in tiles), and it's increased
		/// to "80" if a bow-type weapon is equipped. (Maybe it's in "sub-
		/// tiles"?)
		/// </remarks>
		RangedWeapon = 8,

		/// <summary>
		/// Equip tab.
		/// </summary>
		Weapon3 = 9,

		// Items assigned types greater than 9 don't appear in the
		// inventory on the alpha client.
	}

	/// <summary>
	/// Specifies the equip slot(s) an item can be equipped on.
	/// </summary>
	[Flags]
	public enum EquipSlots : byte
	{
		None = 0x00,
		Head = 0x01, // Lower in > Alpha
		RightHand = 0x02,
		Robe = 0x04,
		Accessory1 = 0x08,
		Body = 0x10,
		LeftHand = 0x20,
		Shoes = 0x40,
		Accessory2 = 0x80,
		//HeadUpper = 0x100,
		//HeadMiddle = 0x200,

		Accessories = Accessory1 | Accessory2,
	}

	/// <summary>
	/// Extension for item enums.
	/// </summary>
	public static class ItemConstExtensions
	{
		/// <summary>
		/// Returns true if the type is an equip type.
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public static bool IsEquip(this ItemType type)
		{
			return type >= ItemType.Weapon;
		}
	}
}
