using System;
using Yggdrasil.Network.Framing;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Utility class for parsing and framing messages according to the RO
	/// protocol.
	/// </summary>
	public class RoFramer
	{
		private readonly byte[] _headerBuffer;
		private byte[] _messageBuffer;
		private int _bytesReceived, _headerLength;

		/// <summary>
		/// Maximum size of messages.
		/// </summary>
		public int MaxMessageSize { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="maxMessageSize">Maximum size of messages</param>
		public RoFramer(int maxMessageSize)
		{
			this.MaxMessageSize = maxMessageSize;
			_headerBuffer = new byte[4];
			_headerLength = 2;
		}

		/// <summary>
		/// Calculates the size of the packet when framed.
		/// </summary>
		/// <param name="packet">The packet to calculate the size of.</param>
		/// <param name="tableSize">The size of the packet according to the packet table.</param>
		/// <param name="packetSize">The total size of the packet when framed.</param>
		public void GetPacketSize(Packet packet, out int tableSize, out int packetSize)
		{
			var opNetwork = PacketTable.ToNetwork(packet.Op);

			tableSize = PacketTable.GetSize(opNetwork);

			if (tableSize == PacketTable.Dynamic)
				packetSize = sizeof(short) * 2 + packet.Length;
			else
				packetSize = sizeof(short) + packet.Length;
		}

		/// <summary>
		/// Writes the full, framed packet into buffer.
		/// </summary>
		/// <param name="packet">The packet to write into buffer.</param>
		/// <param name="tableSize">The size of the packet according to the packet table.</param>
		/// <param name="packetSize">The total size of the packet when framed.</param>
		/// <param name="buffer">The buffer to write the framed packet into.</param>
		public void Frame(Packet packet, int tableSize, int packetSize, byte[] buffer)
		{
			var opNetwork = PacketTable.ToNetwork(packet.Op);

			BitConverter.TryWriteBytes(buffer.AsSpan(0), (short)opNetwork);

			var offset = sizeof(short);
			if (tableSize == PacketTable.Dynamic)
			{
				BitConverter.TryWriteBytes(buffer.AsSpan(offset), (short)packetSize);
				offset += sizeof(short);
			}

			packet.Build(ref buffer, offset);
		}

		/// <summary>
		/// Receives data and calls MessageReceived every time a full message
		/// has arrived.
		/// </summary>
		/// <param name="data">Buffer to read from.</param>
		/// <param name="length">Length of actual information in data.</param>
		/// <param name="messageReceived">Callback invoked when a full message has been received.</param>
		/// <exception cref="InvalidMessageSizeException">
		/// Thrown if a message has an invalid size. Should this occur,
		/// the connection should be terminated, because it's not save to
		/// keep receiving anymore.
		/// </exception>
		public void ReceiveData(byte[] data, int length, Action<byte[]> messageReceived)
		{
			var bytesAvailable = length;
			if (bytesAvailable == 0)
				return;

			for (var i = 0; i < bytesAvailable;)
			{
				if (_messageBuffer == null)
				{
					// Fill header buffer until we know how long the message
					// is going to be exactly.

					_headerBuffer[_bytesReceived] = data[i];
					_bytesReceived += 1;
					i += 1;

					// Once we have enough bytes in the header buffer,
					// get the op code and check the size for this packet.
					if (_bytesReceived >= _headerLength)
					{
						var op = BitConverter.ToUInt16(_headerBuffer, 0);
						var messageSize = PacketTable.GetSize(op);

						// If the packet size is 0, the size is dynamic and
						// we have to read it from the packet. Increase the
						// header length and go back to filling the buffer.
						// On the second round we can read the size from
						// the buffer.
						if (messageSize == PacketTable.Dynamic)
						{
							if (_headerLength == 2)
							{
								_headerLength = 4;
								continue;
							}

							messageSize = BitConverter.ToUInt16(_headerBuffer, 2);
						}

						// The size can't be smaller than the header or
						// larger than the defined max size.
						if (messageSize < _headerLength || messageSize > this.MaxMessageSize)
							throw new InvalidMessageSizeException("Invalid size (" + messageSize + ").");

						// Create buffer for the packet and copy the contents
						// of the header buffer to it.
						_messageBuffer = new byte[messageSize];
						Buffer.BlockCopy(_headerBuffer, 0, _messageBuffer, 0, _headerLength);

						_headerLength = 2;
					}
				}

				if (_messageBuffer != null)
				{
					// Copy the rest of the packet to the message buffer,
					// or at least as many bytes as we have.
					var read = Math.Min(_messageBuffer.Length - _bytesReceived, bytesAvailable - i);
					Buffer.BlockCopy(data, i, _messageBuffer, _bytesReceived, read);

					_bytesReceived += read;
					i += read;

					// Once we have received the full packet we can send
					// it out
					if (_bytesReceived == _messageBuffer.Length)
					{
						messageReceived?.Invoke(_messageBuffer);

						_messageBuffer = null;
						_bytesReceived = 0;
					}
				}
			}
		}
	}
}
