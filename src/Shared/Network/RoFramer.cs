using System;
using Yggdrasil.Network.Framing;
using Yggdrasil.Util;

namespace Sabine.Shared.Network
{
	public class RoFramer : IMessageFramer
	{
		private readonly byte[] _headerBuffer;
		private byte[] _messageBuffer;
		private int _bytesReceived, _headerLength;

		/// <summary>
		/// Maximum size of messages.
		/// </summary>
		public int MaxMessageSize { get; }

		/// <summary>
		/// Called every time ReceiveData got a full message.
		/// </summary>
		public event Action<byte[]> MessageReceived;

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
		/// Wraps message in frame.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public byte[] Frame(byte[] message)
		{
			throw new NotSupportedException();
		}

		/// <summary>
		/// Wraps packet body in frame.
		/// </summary>
		/// <param name="message"></param>
		/// <returns></returns>
		public byte[] Frame(Packet packet)
		{
			var opNetwork = PacketTable.ToNetwork(packet.Op);
			var tableSize = PacketTable.GetSize(opNetwork);

			var bodyBuffer = packet.Build();
			var buffer = new BufferReaderWriter();
			buffer.Endianness = Endianness.LittleEndian;

			buffer.WriteInt16((short)opNetwork);

			if (tableSize == PacketTable.Dynamic)
			{
				var messageSize = sizeof(short) * 2 + bodyBuffer.Length;
				buffer.WriteInt16((short)messageSize);
			}

			buffer.Write(bodyBuffer);

			return buffer.Copy();
		}

		/// <summary>
		/// Receives data and calls MessageReceived every time a full message
		/// has arrived.
		/// </summary>
		/// <param name="data">Buffer to read from.</param>
		/// <param name="length">Length of actual information in data.</param>
		/// <exception cref="InvalidMessageSizeException">
		/// Thrown if a message has an invalid size. Should this occur,
		/// the connection should be terminated, because it's not save to
		/// keep receiving anymore.
		/// </exception>
		public void ReceiveData(byte[] data, int length)
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

						if (messageSize < 0 || messageSize > this.MaxMessageSize)
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
						this.MessageReceived?.Invoke(_messageBuffer);

						_messageBuffer = null;
						_bytesReceived = 0;
					}
				}
			}
		}
	}

}
