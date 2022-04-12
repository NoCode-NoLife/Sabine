using System;
using System.Collections.Generic;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.World;

namespace Sabine.Zone.World.Maps.PathFinding
{
	/// <summary>
	/// Lazy port of the Hercules path finder.
	/// </summary>
	/// <remarks>
	/// Going to replace this eventually, but I just wanted to get the
	/// basics working for now. There's also a "bug" somewhere, or at
	/// least a difference to the alpha client's path finding, because
	/// the client sometimes takes a slightly different path, like going
	/// around an obstacle on the left instead of right.
	/// 
	/// This code is licensed under the GNU GPL v3.
	/// Copyright (C) 2012-2022 Hercules Dev Team
	/// Copyright (C) Athena Dev Teams
	/// </remarks>
	public class HercPathFinder : IPathFinder
	{
		private const int MoveCost = 10;
		private const int MoveDiagonalCost = 14;
		private const int MaxWalkPath = 32;
		private const int SetOpen = 0;
		private const int SetClosed = 1;
		private const int DirNorth = 1;
		private const int DirWest = 2;
		private const int DirSouth = 4;
		private const int DirEast = 8;

		private readonly MapCacheData _mapCacheData;

		/// <summary>
		/// Creates new path finder.
		/// </summary>
		/// <param name="map"></param>
		public HercPathFinder(MapCacheData mapCacheData)
		{
			_mapCacheData = mapCacheData;
		}

		/// <summary>
		/// Finds a path between the given positions and returns it.
		/// </summary>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <returns></returns>
		public Position[] FindPath(Position from, Position to)
		{
			var wpd = new WalkPathData();
			PathSearch(_mapCacheData, wpd, 0, from.X, from.Y, to.X, to.Y);

			var result = new Position[wpd.PathLen];
			var pathPos = from;

			for (var i = 0; i < wpd.PathLen; ++i)
			{
				var dir = wpd.Path[i];

				switch (dir)
				{
					case 0: pathPos.X += +0; pathPos.Y += +1; break;
					case 1: pathPos.X += -1; pathPos.Y += +1; break;
					case 2: pathPos.X += -1; pathPos.Y += +0; break;
					case 3: pathPos.X += -1; pathPos.Y += -1; break;
					case 4: pathPos.X += +0; pathPos.Y += -1; break;
					case 5: pathPos.X += +1; pathPos.Y += -1; break;
					case 6: pathPos.X += +1; pathPos.Y += +0; break;
					case 7: pathPos.X += +1; pathPos.Y += +1; break;
				}

				result[i] = pathPos;
			}

			return result;
		}

		private class WalkPathData
		{
			public byte PathLen, PathPos;
			public byte[] Path = new byte[MaxWalkPath];
		}

		private readonly static byte[,] WalkChoices = new byte[3, 3]
		{
			{ 1,  0,  7 },
			{ 2, 255, 6 },
			{ 3,  4,  5 },
		};

		public class PathNode
		{
			public PathNode Parent; ///< pointer to parent (for path reconstruction)
			public int X; ///< X-coordinate
			public int Y; ///< Y-coordinate
			public int GCost; ///< Actual cost from start to this node
			public int FCost; ///< g_cost + heuristic(this, goal)
			public int Flag; ///< SET_OPEN / SET_CLOSED
		};

		private class node_heap
		{
			public int Length => Data.Count;
			public List<PathNode> Data = new List<PathNode>();

			public void PushNode(PathNode node)
			{
				Data.Add(node);
			}

			public void Clear()
			{
				Data.Clear();
			}

			///// Updates path_node in the binary node_heap.
			public bool UpdateNode(PathNode node)
			{
				var index = this.Data.IndexOf(node);
				if (index == -1)
					return true;

				this.Data.Sort((i, j) => i.FCost.CompareTo(j.FCost));
				return false;
			}
		}

		private static int CalcIndex(int x, int y)
			=> (((x) + (y) * MaxWalkPath) & (MaxWalkPath * MaxWalkPath - 1));

		private static int Heuristic(int x0, int y0, int x1, int y1)
			=> (MoveCost * (Math.Abs((x1) - (x0)) + Math.Abs((y1) - (y0)))); // Manhattan distance

		private static bool ChkDir(int allowed_dirs, int d)
			=> ((allowed_dirs & (d)) == (d));

		private static int AddPath(node_heap heap, PathNode[] tp, int x, int y, int g_cost, PathNode parent, int h_cost)
		{
			var i = CalcIndex(x, y);

			if (tp[i].X == x && tp[i].Y == y)
			{
				// We processed this node before
				if (g_cost < tp[i].GCost)
				{
					// New path to this node is better than old one
					// Update costs and parent
					tp[i].GCost = g_cost;
					tp[i].Parent = parent;
					tp[i].FCost = g_cost + h_cost;
					if (tp[i].Flag == SetClosed)
					{
						heap.PushNode(tp[i]); // Put it in open set again
					}
					else if (heap.UpdateNode(tp[i]))
					{
						return 1;
					}
					tp[i].Flag = SetOpen;
				}
				return 0;
			}

			if (tp[i].X != 0 || tp[i].Y != 0) // Index is already taken; see `tp` array FIXME for details
				return 1;

			// New node
			tp[i].X = x;
			tp[i].Y = y;
			tp[i].GCost = g_cost;
			tp[i].Parent = parent;
			tp[i].FCost = g_cost + h_cost;
			tp[i].Flag = SetOpen;
			heap.PushNode(tp[i]);
			return 0;
		}

		private static bool PathSearch(MapCacheData md, WalkPathData wpd, int m, int x0, int y0, int x1, int y1)
		{
			int i, x, y, dx, dy;

			// Do not check starting cell as that would get you stuck.
			if (x0 < 0 || x0 >= md.Width || y0 < 0 || y0 >= md.Height /*|| md.IsPassable( x0, y0)*/)
				return false;

			// Check destination cell
			if (x1 < 0 || x1 >= md.Width || y1 < 0 || y1 >= md.Height || md.IsPassable(x1, y1))
				return false;

			if (x0 == x1 && y0 == y1)
			{
				wpd.PathLen = 0;
				wpd.PathPos = 0;
				return true;
			}

			// A* (A-star) pathfinding
			// We always use A* for finding walkpaths because it is what game client uses.
			// Easy pathfinding cuts corners of non-walkable cells, but client always walks around it.

			var open_set = new node_heap(); // 'Open' set

			// FIXME: This array is too small to ensure all paths shorter than MAX_WALKPATH
			// can be found without node collision: calc_index(node1) = calc_index(node2).
			// Figure out more proper size or another way to keep track of known nodes.
			var tp = new PathNode[MaxWalkPath * MaxWalkPath];
			for (var k = 0; k < tp.Length; k++)
				tp[k] = new PathNode();

			PathNode current, it;
			var xs = md.Width - 1;
			var ys = md.Height - 1;
			var len = 0;
			int j;
			//memset(tp, 0, sizeof(tp));

			// Start node
			i = CalcIndex(x0, y0);
			tp[i].Parent = null;
			tp[i].X = x0;
			tp[i].Y = y0;
			tp[i].GCost = 0;
			tp[i].FCost = Heuristic(x0, y0, x1, y1);
			tp[i].Flag = SetOpen;

			open_set.PushNode(tp[i]); // Put start node to 'open' set

			for (; ; )
			{
				var e = 0; // error flag

				// Saves allowed directions for the current cell. Diagonal directions
				// are only allowed if both directions around it are allowed. This is
				// to prevent cutting corner of nearby wall.
				// For example, you can only go NW from the current cell, if you can
				// go N *and* you can go W. Otherwise you need to walk around the
				// (corner of the) non-walkable cell.
				var allowed_dirs = 0;

				int g_cost;

				if (open_set.Length == 0)
				{
					open_set.Clear();
					return false;
				}

				current = open_set.Data[0]; // Look for the lowest f_cost node in the 'open' set
				open_set.Data.RemoveAt(0); // Remove it from 'open' set

				x = current.X;
				y = current.Y;
				g_cost = current.GCost;

				current.Flag = SetClosed; // Add current node to 'closed' set

				if (x == x1 && y == y1)
				{
					open_set.Clear();
					break;
				}

				if (y < ys && !md.IsPassable(x, y + 1)) allowed_dirs |= DirNorth;
				if (y > 0 && !md.IsPassable(x, y - 1)) allowed_dirs |= DirSouth;
				if (x < xs && !md.IsPassable(x + 1, y)) allowed_dirs |= DirEast;
				if (x > 0 && !md.IsPassable(x - 1, y)) allowed_dirs |= DirWest;

				// Process neighbors of current node
				if (ChkDir(allowed_dirs, DirSouth | DirEast) && !md.IsPassable(x + 1, y - 1))
					e += AddPath(open_set, tp, x + 1, y - 1, g_cost + MoveDiagonalCost, current, Heuristic(x + 1, y - 1, x1, y1)); // (x+1, y-1) 5
				if (ChkDir(allowed_dirs, DirEast))
					e += AddPath(open_set, tp, x + 1, y, g_cost + MoveCost, current, Heuristic(x + 1, y, x1, y1)); // (x+1, y) 6
				if (ChkDir(allowed_dirs, DirNorth | DirEast) && !md.IsPassable(x + 1, y + 1))
					e += AddPath(open_set, tp, x + 1, y + 1, g_cost + MoveDiagonalCost, current, Heuristic(x + 1, y + 1, x1, y1)); // (x+1, y+1) 7
				if (ChkDir(allowed_dirs, DirNorth))
					e += AddPath(open_set, tp, x, y + 1, g_cost + MoveCost, current, Heuristic(x, y + 1, x1, y1)); // (x, y+1) 0
				if (ChkDir(allowed_dirs, DirNorth | DirWest) && !md.IsPassable(x - 1, y + 1))
					e += AddPath(open_set, tp, x - 1, y + 1, g_cost + MoveDiagonalCost, current, Heuristic(x - 1, y + 1, x1, y1)); // (x-1, y+1) 1
				if (ChkDir(allowed_dirs, DirWest))
					e += AddPath(open_set, tp, x - 1, y, g_cost + MoveCost, current, Heuristic(x - 1, y, x1, y1)); // (x-1, y) 2
				if (ChkDir(allowed_dirs, DirSouth | DirWest) && !md.IsPassable(x - 1, y - 1))
					e += AddPath(open_set, tp, x - 1, y - 1, g_cost + MoveDiagonalCost, current, Heuristic(x - 1, y - 1, x1, y1)); // (x-1, y-1) 3
				if (ChkDir(allowed_dirs, DirSouth))
					e += AddPath(open_set, tp, x, y - 1, g_cost + MoveCost, current, Heuristic(x, y - 1, x1, y1)); // (x, y-1) 4

				if (e != 0)
				{
					open_set.Clear();
					return false;
				}
			}

			for (it = current; it.Parent != null; it = it.Parent, len++) ;
			if (len > (int)wpd.Path.Length)
			{
				return false;
			}

			// Recreate path
			wpd.PathLen = (byte)len;
			wpd.PathPos = 0;
			for (it = current, j = len - 1; j >= 0; it = it.Parent, j--)
			{
				dx = it.X - it.Parent.X;
				dy = it.Y - it.Parent.Y;
				wpd.Path[j] = WalkChoices[-dy + 1, dx + 1];
			}
			return true;
		}

		private static bool PathSearchSimple(MapCacheData md, WalkPathData wpd, int m, int x0, int y0, int x1, int y1)
		{
			int i, x, y, dx, dy;

			// Do not check starting cell as that would get you stuck.
			if (x0 < 0 || x0 >= md.Width || y0 < 0 || y0 >= md.Height /*|| md.IsPassable(x0, y0)*/)
				return false;

			// Check destination cell
			if (x1 < 0 || x1 >= md.Width || y1 < 0 || y1 >= md.Height || md.IsPassable(x1, y1))
				return false;

			if (x0 == x1 && y0 == y1)
			{
				wpd.PathLen = 0;
				wpd.PathPos = 0;
				return true;
			}

			// Try finding direct path to target
			// Direct path goes diagonally first, then in straight line.

			// calculate (sgn(x1-x0), sgn(y1-y0))
			dx = ((dx = x1 - x0) != 0) ? ((dx < 0) ? -1 : 1) : 0;
			dy = ((dy = y1 - y0) != 0) ? ((dy < 0) ? -1 : 1) : 0;

			x = x0; // Current position = starting cell
			y = y0;
			i = 0;

			while (i < wpd.Path.Length)
			{
				wpd.Path[i] = WalkChoices[-dy + 1, dx + 1];
				i++;

				x += dx; // Advance current position
				y += dy;

				if (x == x1) dx = 0; // destination x reached, no longer move along x-axis
				if (y == y1) dy = 0; // destination y reached, no longer move along y-axis

				if (dx == 0 && dy == 0)
					break; // success

				if (md.IsPassable(x, y))
					break; // obstacle = failure
			}

			if (x == x1 && y == y1)
			{
				// easy path successful.
				wpd.PathLen = (byte)i;
				wpd.PathPos = 0;
				return true;
			}

			return false; // easy path unsuccessful
		}
	}
}
