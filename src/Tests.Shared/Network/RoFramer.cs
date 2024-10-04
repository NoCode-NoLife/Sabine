using System.Collections.Generic;
using Sabine.Shared;
using Sabine.Shared.Network;
using Xunit;

namespace Tests.Sabine.Shared.Network
{
	public class RoFramerTests
	{
		static RoFramerTests()
		{
			Game.Version = Versions.Alpha;
			PacketTable.Load();
		}

		[Fact]
		internal void ReceiveSingle()
		{
			var framer = new RoFramer(1024);
			var testBuffer = default(byte[]);
			var messagesReceived = 0;

			framer.MessageReceived += (buffer) =>
			{
				testBuffer = buffer;
				messagesReceived++;
			};

			// HC_ACCEPT_DELETECHAR
			var test1 = new byte[] { 0x0B, 0x00 };

			framer.ReceiveData(test1, 2);

			Assert.Equal(test1, testBuffer);
			Assert.Equal(1, messagesReceived);
		}

		[Fact]
		internal void ReceiveSplit()
		{
			var framer = new RoFramer(1024);
			var testBuffer = default(byte[]);
			var messagesReceived = 0;

			framer.MessageReceived += (buffer) =>
			{
				testBuffer = buffer;
				messagesReceived++;
			};

			// HC_ACCEPT_DELETECHAR
			var test2 = new byte[] { 0x0B, 0x00 };

			framer.ReceiveData([0x0B], 1);
			framer.ReceiveData([0x00], 1);

			Assert.Equal(test2, testBuffer);
			Assert.Equal(1, messagesReceived);
		}

		[Fact]
		internal void ReceiveMultiple()
		{
			var framer = new RoFramer(1024);
			var testBuffers = new List<byte[]>();
			var messagesReceived = 0;

			framer.MessageReceived += (buffer) =>
			{
				testBuffers.Add(buffer);
				messagesReceived++;
			};

			// HC_ACCEPT_DELETECHAR
			var test1 = new byte[] { 0x0B, 0x00 };
			var test2 = new byte[] { 0x0B, 0x00, 0x0B, 0x00 };

			framer.ReceiveData(test2, 4);

			Assert.Equal(test1, testBuffers[0]);
			Assert.Equal(test1, testBuffers[1]);
			Assert.Equal(2, messagesReceived);
		}

		[Fact]
		internal void ReceiveSingleDynamic()
		{
			var framer = new RoFramer(1024);
			var testBuffer = default(byte[]);
			var messagesReceived = 0;

			framer.MessageReceived += (buffer) =>
			{
				testBuffer = buffer;
				messagesReceived++;
			};

			// HC_ACCEPT_ENTER
			var test1 = new byte[] { 0x07, 0x00, 0x08, 0x00, 0x01, 0x02, 0x03, 0x04 };

			framer.ReceiveData(test1, 8);

			Assert.Equal(test1, testBuffer);
			Assert.Equal(1, messagesReceived);
		}
	}
}
