using Sabine.Shared.Network.Structs;
using Sabine.Shared.World;
using Xunit;

namespace Tests.Sabine.Shared.Network.Structs
{
	public class PackedPositionTests
	{
		[Fact]
		private void ImplicitPos()
		{
			var pos1 = new Position(10, 20);
			Assert.Equal(10, pos1.X); // 0b_0000_1010
			Assert.Equal(20, pos1.Y); // 0b_0001_0100

			var coord1 = (PackedPosition)pos1;
			Assert.Equal(2, coord1.B1);   // 0b_0000_0010
			Assert.Equal(129, coord1.B2); // 0b_1000_0001
			Assert.Equal(64, coord1.B3);  // 0b_0100_0000

			var pos2 = (Position)coord1;
			Assert.Equal(10, pos1.X);
			Assert.Equal(20, pos1.Y);
		}

		[Fact]
		private void PosDir()
		{
			var pos1 = new Position(10, 20);
			Assert.Equal(10, pos1.X); // 0b_0000_1010
			Assert.Equal(20, pos1.Y); // 0b_0001_0100

			var coord1 = new PackedPosition(pos1, 8);
			Assert.Equal(2, coord1.B1);   // 0b_0000_0010
			Assert.Equal(129, coord1.B2); // 0b_1000_0001
			Assert.Equal(72, coord1.B3);  // 0b_0100_1000

			Assert.Equal(10, coord1.Position.X);
			Assert.Equal(20, coord1.Position.Y);
			Assert.Equal(8, coord1.Direction);
		}
	}
}
