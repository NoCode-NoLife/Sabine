namespace Sabine.Shared.Const
{
	/// <summary>
	/// Specifies a part of a sprite to do something to.
	/// </summary>
	public enum SpriteType : short
	{
#pragma warning disable CA1069 // Enums values should not be duplicated

		// Alpha/Beta1
		Class = 0,
		Hair = 1,
		Weapon = 2,
		Head = 3,

		// Beta2
		HeadBottom = 3,
		HeadTop = 4,
		HeadMiddle = 5,
		HairColor = 6,
		ClothesColor = 7,
		Shield = 8,

		// Mentioned in eA
		Shoes = 9,

#pragma warning restore CA1069 // Enums values should not be duplicated
	}
}
