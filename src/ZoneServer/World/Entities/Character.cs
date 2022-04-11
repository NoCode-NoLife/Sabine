using Sabine.Shared.Const;
using Sabine.Shared.World;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Describes a character that can interact with the world and can be
	/// interacted with, such as a player or an NPC.
	/// </summary>
	public interface ICharacter : IEntity
	{
		/// <summary>
		/// Returns the character's globally unique id, which is used
		/// to identify it.
		/// </summary>
		int Handle { get; }

		/// <summary>
		/// Returns the character's class id, specifying (part of) its
		/// appearance.
		/// </summary>
		int ClassId { get; }

		/// <summary>
		/// Returns the character's speed.
		/// </summary>
		int Speed { get; }

		/// <summary>
		/// Returns the character's sex.
		/// </summary>
		Sex Sex { get; }

		/// <summary>
		/// Returns a character's hair id, specifying the look of their
		/// head.
		/// </summary>
		int HairId { get; }

		/// <summary>
		/// Returns a character's weapon id, specifying what weapon they
		/// can be seen holding during combat.
		/// </summary>
		int WeaponId { get; }

		/// <summary>
		/// Returns the direction the character is turned towards.
		/// </summary>
		Direction Direction { get; }
	}
}
