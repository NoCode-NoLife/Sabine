using Sabine.Shared.Const;
using Sabine.Shared.World;
using Shared.Const;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// An entity that can appear in the world and be spawned via the
	/// *ENTRY packets.
	/// </summary>
	public interface IEntryCharacter : IEntity
	{
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

		/// <summary>
		/// Returns the character's name.
		/// </summary>
		string Name { get; }

		/// <summary>
		/// Returns the character's username.
		/// </summary>
		string Username { get; }

		/// <summary>
		/// Returns a character's current state.
		/// </summary>
		CharacterState State { get; }
	}
}
