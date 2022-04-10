using Sabine.Shared.World;

namespace Sabine.Shared.Network.Structs
{
	/// <summary>
	/// Represents a packed position and direction, cramming the data into
	/// three bytes, as typically used inside packets.
	/// </summary>
	public struct PackedPosition
	{
		public readonly byte B1;
		public readonly byte B2;
		public readonly byte B3;

		public readonly Position Position;
		public readonly int Direction;

		/// <summary>
		/// Creates new packed position.
		/// </summary>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <param name="b3"></param>
		public PackedPosition(byte b1, byte b2, byte b3)
		{
			this.B1 = b1;
			this.B2 = b2;
			this.B3 = b3;

			FromBytes(b1, b2, b3, out this.Position, out this.Direction);
		}

		/// <summary>
		/// Creates new packed position from position and direction.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public PackedPosition(Position pos, int dir)
		{
			this.Position = pos;
			this.Direction = dir;

			ToBytes(pos, dir, out this.B1, out this.B2, out this.B3);
		}

		/// <summary>
		/// Extracts and returns position from packed position.
		/// </summary>
		/// <param name="packedPos"></param>
		public static implicit operator Position(PackedPosition packedPos)
		{
			FromBytes(packedPos.B1, packedPos.B2, packedPos.B3, out var pos, out _);
			return pos;
		}

		/// <summary>
		/// Packs position and returns it.
		/// </summary>
		/// <remarks>
		/// The packed position will not have a direction.
		/// </remarks>
		/// <param name="pos"></param>
		public static implicit operator PackedPosition(Position pos)
		{
			var result = new PackedPosition(pos, 0);
			return result;
		}

		/// <summary>
		/// Converts position and direction to packed position and
		/// returns it.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <param name="b3"></param>
		public static void ToBytes(Position pos, int dir, out byte b1, out byte b2, out byte b3)
		{
			var x = pos.X;
			var y = pos.Y;

			b1 = (byte)(x >> 2);
			b2 = (byte)((x << 6) | ((y >> 4) & 0x3F));
			b3 = (byte)((y << 4) | (dir & 0x0F));
		}

		/// <summary>
		/// Converts bytes to position and direction and returns them.
		/// </summary>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		///	<param name="b3"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public static void FromBytes(byte b1, byte b2, byte b3, out Position pos, out int dir)
		{
			var x = (b2 >> 6) | 4 * b1;
			var y = (b3 >> 4) | 16 * (b2 & 0x3F);
			dir = (b3 & 0x0F);

			pos = new Position(x, y);
		}
	}
}
