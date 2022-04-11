using Sabine.Shared.World;
using Sabine.Zone.World.Maps;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents something that can exist on a map.
	/// </summary>
	public interface IEntity
	{
		/// <summary>
		/// Returns the id of the map the entity is currently on.
		/// </summary>
		int MapId { get; }

		/// <summary>
		/// Returns the entity's current position.
		/// </summary>
		Position Position { get; }

		/// <summary>
		/// Returns a reference to the map the entity is currently on.
		/// </summary>
		Map Map { get; }
	}
}
