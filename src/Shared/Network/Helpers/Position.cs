using Sabine.Shared.Network.Structs;
using Sabine.Shared.World;

namespace Sabine.Shared.Network.Helpers
{
	/// <summary>
	/// Position related helper extensions for the Packet class.
	/// </summary>
	public static class PacketPositionExtensions
	{
		/// <summary>
		/// Writes position and direction into packet as three bytes.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="pos"></param>
		/// <param name="dir"></param>
		public static void AddPackedPosition(this Packet packet, Position pos, Direction dir)
		{
			var packed = new PackedPosition(pos, dir);

			packet.PutByte(packed.B1);
			packet.PutByte(packed.B2);
			packet.PutByte(packed.B3);
		}

		/// <summary>
		/// Reads three bytes from packed and returns them as a packed
		/// position.
		/// </summary>
		/// <param name="packet"></param>
		public static PackedPosition GetPackedPosition(this Packet packet)
		{
			var b1 = packet.GetByte();
			var b2 = packet.GetByte();
			var b3 = packet.GetByte();

			return new PackedPosition(b1, b2, b3);
		}

		/// <summary>
		/// Writes move to the packet as six bytes.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="from"></param>
		/// <param name="to"></param>
		/// <param name="sx"></param>
		/// <param name="sy"></param>
		public static void AddPackedMove(this Packet packet, Position from, Position to, int sx, int sy)
		{
			var packed = new PackedMove(from, to, sx, sy);

			packet.PutByte(packed.B1);
			packet.PutByte(packed.B2);
			packet.PutByte(packed.B3);
			packet.PutByte(packed.B4);
			packet.PutByte(packed.B5);
			packet.PutByte(packed.B6);
		}
	}
}
