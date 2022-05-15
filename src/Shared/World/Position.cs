using System;
using Yggdrasil.Util;

namespace Sabine.Shared.World
{
	/// <summary>
	/// Represents a position in the world.
	/// </summary>
	public struct Position
	{
		/// <summary>
		/// Returns a position with both coordinates being 0.
		/// </summary>
		public static Position Zero => new Position(0, 0);

		/// <summary>
		/// The X-coordinate of the position.
		/// </summary>
		public int X;

		/// <summary>
		/// The Y-coordinate of the position.
		/// </summary>
		public int Y;

		/// <summary>
		/// Creates new position.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Position(int x, int y)
		{
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Returns true if the position's coordinates are the same.
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <returns></returns>
		public static bool operator ==(Position pos1, Position pos2)
		{
			return (pos1.X == pos2.X && pos1.Y == pos2.Y);
		}

		/// <summary>
		/// Returns true if the position's coordinates are not the same.
		/// </summary>
		/// <param name="pos1"></param>
		/// <param name="pos2"></param>
		/// <returns></returns>
		public static bool operator !=(Position pos1, Position pos2)
		{
			return !(pos1 == pos2);
		}

		/// <summary>
		/// Returns a hash code for this position that will equal positions
		/// with the same coordinates.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		/// <summary>
		/// Returns true if the given object is this exact position instance.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is Position position && this == position;
		}

		/// <summary>
		/// Returns a string representation of this position.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("(Position - X: {0}, Y: {1})", this.X, this.Y);
		}

		/// <summary>
		/// Returns true if the other position is within the given range.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public bool InRange(Position otherPos, float range)
		{
			return (Math.Pow(this.X - otherPos.X, 2) + Math.Pow(this.Y - otherPos.Y, 2) <= Math.Pow(range, 2));
		}

		/// <summary>
		/// Returns true if the other position is within a square with the
		/// given length around this position.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <param name="range"></param>
		/// <returns></returns>
		public bool InSquareRange(Position otherPos, float length)
		{
			return (Math.Abs(this.X - otherPos.X) <= length && Math.Abs(this.Y - otherPos.Y) <= length);
		}

		/// <summary>
		/// Returns true if the given position is in a straight line from
		/// this one.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <returns></returns>
		public bool InStraightLine(Position otherPos)
		{
			return (this.X == otherPos.X || this.Y == otherPos.Y);
		}

		/// <summary>
		/// Returns distance between this and another position.
		/// </summary>
		/// <param name="otherPos"></param>
		/// <returns></returns>
		public int GetDistance(Position otherPos)
		{
			return (int)Math.Sqrt(Math.Pow(this.X - otherPos.X, 2) + Math.Pow(this.Y - otherPos.Y, 2));
		}

		/// <summary>
		/// Returns random position in range around this position.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public Position GetRandomInRange(int range)
			=> this.GetRandomInRange(0, range);

		/// <summary>
		/// Returns random position around this position, that is at
		/// least minRange away.
		/// </summary>
		/// <param name="minRange"></param>
		/// <param name="maxRange"></param>
		/// <returns></returns>
		public Position GetRandomInRange(int minRange, int maxRange)
		{
			var rnd = RandomProvider.Get();

			var distance = rnd.Next(minRange, maxRange + 1);
			var angle = rnd.NextDouble() * Math.PI * 2;

			var x = this.X + distance * Math.Cos(angle);
			var y = this.Y + distance * Math.Sin(angle);

			return new Position((int)x, (int)y);
		}

		/// <summary>
		/// Returns random position in range around this position.
		/// </summary>
		/// <param name="range"></param>
		/// <returns></returns>
		public Position GetRandomInSquareRange(int range)
		{
			var rnd = RandomProvider.Get();

			var x = this.X + rnd.Next(-range, range + 1);
			var y = this.Y + rnd.Next(-range, range + 1);

			return new Position((int)x, (int)y);
		}
	}
}
