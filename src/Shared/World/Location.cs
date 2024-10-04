namespace Sabine.Shared.World
{
	/// <summary>
	/// Represents a location in the world.
	/// </summary>
	public readonly struct Location
	{
		/// <summary>
		/// The location's region id.
		/// </summary>
		public readonly int MapId;

		/// <summary>
		/// The location's X coordinate.
		/// </summary>
		public readonly int X;

		/// <summary>
		/// The location's Y coordinate.
		/// </summary>
		public readonly int Y;

		/// <summary>
		/// Returns this location's position based on X and Y.
		/// </summary>
		public Position Position => new(this.X, this.Y);

		/// <summary>
		/// Returns true if all of the location's properties are 0,
		/// indicating that it hasn't been initialized.
		/// </summary>
		public bool IsZero => this.MapId == 0 && this.X == 0 && this.Y == 0;

		/// <summary>
		/// Creates new location.
		/// </summary>
		/// <param name="mapId"></param>
		/// <param name="x"></param>
		/// <param name="y"></param>
		public Location(int mapId, int x, int y)
		{
			this.MapId = mapId;
			this.X = x;
			this.Y = y;
		}

		/// <summary>
		/// Creates new location.
		/// </summary>
		/// <param name="mapId"></param>
		/// <param name="pos"></param>
		public Location(int mapId, Position pos)
			: this(mapId, pos.X, pos.Y)
		{
		}

		/// <summary>
		/// Creates new location from other location.
		/// </summary>
		/// <param name="location"></param>
		public Location(Location location)
			: this(location.MapId, location.X, location.Y)
		{
		}

		/// <summary>
		/// Returns true if the compared locations represent the same
		/// location in the world.
		/// </summary>
		/// <param name="loc1"></param>
		/// <param name="loc2"></param>
		/// <returns></returns>
		public static bool operator ==(Location loc1, Location loc2)
		{
			return (loc1.MapId == loc2.MapId && loc1.X == loc2.X && loc1.Y == loc2.Y);
		}

		/// <summary>
		/// Returns true if the compared locations don't represent the same
		/// location in the world.
		/// </summary>
		/// <param name="loc1"></param>
		/// <param name="loc2"></param>
		/// <returns></returns>
		public static bool operator !=(Location loc1, Location loc2)
		{
			return !(loc1 == loc2);
		}

		/// <summary>
		/// Returns a hash code that is the same when all of the location's
		/// properties are the same.
		/// </summary>
		/// <returns></returns>
		public override int GetHashCode()
		{
			return this.MapId.GetHashCode() ^ this.X.GetHashCode() ^ this.Y.GetHashCode();
		}

		/// <summary>
		/// Returns true if the given object is this exact instance.
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public override bool Equals(object obj)
		{
			return obj is Location location && this == location;
		}

		/// <summary>
		/// Returns a string representation of this location.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return string.Format("(Location - MapId: {0}, X: {1}, Y: {2})", this.MapId, this.X, this.Y);
		}
	}
}
