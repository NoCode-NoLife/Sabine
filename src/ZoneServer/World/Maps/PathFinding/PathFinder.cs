﻿using Sabine.Shared.World;

namespace Sabine.Zone.World.Maps.PathFinding
{
	/// <summary>
	/// Siri, nagivate to where I click.
	/// </summary>
	public interface IPathFinder
	{
		/// <summary>
		/// Searches for a path between the given positions and returns it.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		Position[] FindPath(Position from, Position to);

		/// <summary>
		/// Returns true if there's a valid path to get from one position
		/// to the other.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		bool PathExists(Position from, Position to);
	}
}
