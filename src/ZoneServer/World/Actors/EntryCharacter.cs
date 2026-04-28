using Sabine.Shared.Const;
using Sabine.Shared.World;
using Shared.Const;

namespace Sabine.Zone.World.Actors
{
	/// <summary>
	/// An actor that can appear in the world and be spawned via the
	/// *ENTRY packets.
	/// </summary>
	public interface IStandEntry : IActor
	{
		/// <summary>
		/// Returns the character's identity id, specifying (part of) its
		/// appearance.
		/// </summary>
		IdentityId IdentityId { get; }

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
		int WeaponLook { get; }

		/// <summary>
		/// Returns a character's top headgear look, specifying what
		/// headgear they're wearing.
		/// </summary>
		int HeadTopLook { get; }

		/// <summary>
		/// Returns a character's middle headgear look, specifying what
		/// headgear they're wearing.
		/// </summary>
		int HeadMiddleLook { get; }

		/// <summary>
		/// Returns a character's bottom headgear look, specifying what
		/// headgear they're wearing.
		/// </summary>
		int HeadBottomLook { get; }

		/// <summary>
		/// Returns the direction the character is turned towards.
		/// </summary>
		Direction Direction { get; }

		/// <summary>
		/// Returns the head turn direction relative to the character's
		/// direction.
		/// </summary>
		HeadTurn HeadTurn { get; }

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
