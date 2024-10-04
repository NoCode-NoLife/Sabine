using Sabine.Shared.Network.Structs;
using Sabine.Shared.World;
using Xunit;

namespace Tests.Sabine.Shared.Network.Structs
{
	public class PackedMoveTests
	{
		[Fact]
		internal void ToFrom()
		{
			var from = new Position(10, 20);
			Assert.Equal(10, from.X);         // 0b_0000_1010
			Assert.Equal(20, from.Y);         // 0b_0001_0100

			var to = new Position(40, 50);
			Assert.Equal(40, to.X);           // 0b_0010_1000
			Assert.Equal(50, to.Y);           // 0b_0011_0010

			var sx = 2;                       // 0b_0000_0010
			var sy = 6;                       // 0b_0000_0110

			var packed1 = new PackedMove(from, to, sx, sy);
			Assert.Equal(2, packed1.B1);      // 0b_0000_0010
			Assert.Equal(129, packed1.B2);    // 0b_1000_0001
			Assert.Equal(64, packed1.B3);     // 0b_0100_0000
			Assert.Equal(160, packed1.B4);    // 0b_1010_0000
			Assert.Equal(50, packed1.B5);     // 0b_0011_0010
			Assert.Equal(38, packed1.B6);     // 0b_0010_0110

			var packed2 = new PackedMove(packed1.B1, packed1.B2, packed1.B3, packed1.B4, packed1.B5, packed1.B6);
			Assert.Equal(2, packed2.B1);      // 0b_0000_0010
			Assert.Equal(129, packed2.B2);    // 0b_1000_0001
			Assert.Equal(64, packed2.B3);     // 0b_0100_0000
			Assert.Equal(160, packed2.B4);    // 0b_1010_0000
			Assert.Equal(50, packed2.B5);     // 0b_0011_0010
			Assert.Equal(38, packed2.B6);     // 0b_0010_0110
			Assert.Equal(10, packed2.From.X); // 0b_0000_1010
			Assert.Equal(20, packed2.From.Y); // 0b_0001_0100
			Assert.Equal(40, packed2.To.X);   // 0b_0010_1000
			Assert.Equal(50, packed2.To.Y);   // 0b_0011_0010
			Assert.Equal(2, packed2.SX);      // 0b_0000_0010
			Assert.Equal(6, packed2.SY);      // 0b_0000_0110
		}
	}
}
