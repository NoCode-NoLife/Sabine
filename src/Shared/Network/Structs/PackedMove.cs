using System;
using Sabine.Shared.World;

namespace Sabine.Shared.Network.Structs
{
	/// <summary>
	/// Represents a packed movement between two positions, crammed into
	/// six bytes, as typically used inside packets.
	/// </summary>
	public struct PackedMove
	{
		public readonly byte B1;
		public readonly byte B2;
		public readonly byte B3;
		public readonly byte B4;
		public readonly byte B5;
		public readonly byte B6;

		public readonly Position From;
		public readonly Position To;
		public readonly int SX;
		public readonly int SY;

		/// <summary>
		/// Creates new packed position.
		/// </summary>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		/// <param name="b3"></param>
		/// <param name="b4"></param>
		/// <param name="b5"></param>
		/// <param name="b6"></param>
		public PackedMove(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6)
		{
			this.B1 = b1;
			this.B2 = b2;
			this.B3 = b3;
			this.B4 = b4;
			this.B5 = b5;
			this.B6 = b6;

			FromBytes(b1, b2, b3, b4, b5, b6, out this.From, out this.To, out this.SX, out this.SY);
		}

		/// <summary>
		/// Creates new packed position from position and direction.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public PackedMove(Position from, Position to, int sx, int sy)
		{
			this.From = from;
			this.To = to;
			this.SX = sx;
			this.SY = sy;

			ToBytes(from, to, sx, sy, out this.B1, out this.B2, out this.B3, out this.B4, out this.B5, out this.B6);
		}

		/// <summary>
		/// Converts positions to packed move and returns it.
		/// </summary>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public static void ToBytes(Position from, Position to, int sx, int sy, out byte b1, out byte b2, out byte b3, out byte b4, out byte b5, out byte b6)
		{
			var x1 = from.X;
			var y1 = from.Y;
			var x2 = to.X;
			var y2 = to.Y;

			b1 = (byte)((x1) >> 2);
			b2 = (byte)(((x1) << 6) | (((y1) >> 4) & 0x3f));
			b3 = (byte)(((y1) << 4) | (((x2) >> 6) & 0x0f));
			b4 = (byte)(((x2) << 2) | (((y2) >> 8) & 0x03));
			b5 = (byte)(y2);
			b6 = (byte)(((sx) << 4) | ((sy) & 0x0f));
		}

		/// <summary>
		/// Converts bytes to positions and returns them.
		/// </summary>
		/// <param name="b1"></param>
		/// <param name="b2"></param>
		///	<param name="b3"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public static void FromBytes(byte b1, byte b2, byte b3, byte b4, byte b5, byte b6, out Position from, out Position to, out int sx, out int sy)
		{
			var x1 = ((b1 << 2) | (b2 >> 6));
			var y1 = ((b2 & 0x3F) << 4) | (b3 >> 4);
			var x2 = ((b3 & 0x0F) << 6) | (b4 >> 2);
			var y2 = (b5 | ((b4 & 3) << 8));
			sx = (b6 >> 4);
			sy = (b6 & 0xF);

			from = new Position(x1, y1);
			to = new Position(x2, y2);
		}
	}
}
