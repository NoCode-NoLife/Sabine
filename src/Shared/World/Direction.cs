namespace Sabine.Shared.World
{
	/// <summary>
	/// Specifies the direction a character is looking in.
	/// </summary>
	public enum Direction : byte
	{
		North = 0,
		NorthWest = 1,
		West = 2,
		SouthWest = 3,
		South = 4,
		SouthEast = 5,
		East = 6,
		NorthEast = 7,
	}

	/// <summary>
	/// Specifies the head turning direction of a character.
	/// </summary>
	/// <remarks>
	/// The head turn is relative to the current body direction. For
	/// example, if the character is facíng south-west and the head is
	/// turned west, it's turned right relative to the body, for a Right
	/// HeadTurn.
	/// </remarks>
	public enum HeadTurn : byte
	{
		Straight = 0,
		Right = 1,
		Left = 2,
	}
}
