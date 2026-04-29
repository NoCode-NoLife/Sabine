using System;
using System.IO;
using System.Net;
using System.Text;
using Microsoft.Extensions.ObjectPool;
using Sabine.Shared.Database;
using Yggdrasil.Util;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Packet reader and writer.
	/// </summary>
	/// <remarks>
	/// Packets are pooled internally and need to be disposed after use.
	/// They are not thread-safe and only usable within the context of
	/// their handling or sending.
	/// </remarks>
	public class Packet : IDisposable
	{
		private static readonly ObjectPool<Packet> Pool = new DefaultObjectPool<Packet>(new PacketPoolPolicy(), 5000);

		private class PacketPoolPolicy : IPooledObjectPolicy<Packet>
		{
			public Packet Create()
			{
				return new Packet();
			}

			public bool Return(Packet packet)
			{
				// The vast majority of packets are small and we shouldn't
				// need larger ones on constant standby.
				if (packet._buffer.Length >= 1024)
				{
					packet._buffer.Dispose();
					return false;
				}

				packet.Reset();
				return true;
			}
		}

		private BufferReaderWriter _buffer;
		private int _bodyStart;
		private bool _disposed;

		private static readonly Encoding EncodingKR = Encoding.GetEncoding("EUC-KR");

		/// <summary>
		/// Gets or sets the packet's opcode.
		/// </summary>
		public Op Op { get; set; }

		/// <summary>
		/// Returns the length of the packet's buffer.
		/// </summary>
		public int Length => _buffer.Length;

		/// <summary>
		/// Creates a new instance. Used only internally for the object
		/// pool, use <see cref="Packet.Rent"/> to get a packet instance.
		/// </summary>
		private Packet()
		{
		}

		/// <summary>
		/// Creates new packet to write to.
		/// </summary>
		/// <param name="op"></param>
		public static Packet Rent(Op op)
		{
			var packet = Pool.Get();
			packet._disposed = false;

			if (packet._buffer == null)
			{
				packet._buffer = new BufferReaderWriter(BufferReaderWriter.DefaultSize);
				packet._buffer.Endianness = Endianness.LittleEndian;
			}
			else
			{
				packet._buffer.Reuse(BufferReaderWriter.DefaultSize);
			}

			packet.Op = op;
			packet._bodyStart = 0;

			return packet;
		}

		/// <summary>
		/// Creates packet from buffer to read it.
		/// </summary>
		/// <param name="buffer"></param>
		public static Packet Rent(byte[] buffer)
		{
			var packet = Pool.Get();
			packet._disposed = false;

			if (packet._buffer == null)
			{
				packet._buffer = new BufferReaderWriter(buffer);
				packet._buffer.Endianness = Endianness.LittleEndian;
			}
			else
			{
				packet._buffer.Reuse(buffer);
			}

			packet.Op = (Op)packet.GetShort();
			packet._bodyStart = packet._buffer.Index;

			return packet;
		}

		/// <summary>
		/// Disposes the packet, returning it to the internal pool.
		/// </summary>
		public void Dispose()
		{
			if (_disposed)
				return;

			_disposed = true;
			Pool.Return(this);
		}

		/// <summary>
		/// Throws an exception if the packet has been disposed.
		/// </summary>
		/// <exception cref="ObjectDisposedException"></exception>
		private void AssertNotDisposed()
		{
			if (_disposed)
				throw new PacketDisposedException(this);
		}

		/// <summary>
		/// Resets the packet's state, preparing it for reuse.
		/// </summary>
		private void Reset()
		{
			this.Op = 0;
		}

		/// <summary>
		/// Returns the length of the remaining unread data in the packet.
		/// </summary>
		/// <returns></returns>
		public int GetRemainingLength()
		{
			this.AssertNotDisposed();
			return _buffer.Length - _buffer.Index;
		}

		/// <summary>
		/// Skips the given number of bytes ahead in the packet, ignoring
		/// the data.
		/// </summary>
		/// <param name="amount"></param>
		public void Skip(int amount)
		{
			this.AssertNotDisposed();

			if (amount == 0)
				return;

			_buffer.Seek(amount, SeekOrigin.Current);
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutByte(byte value)
		{
			this.AssertNotDisposed();
			_buffer.WriteByte(value);
		}

		/// <summary>
		/// Writes value to packet as 1 or 0.
		/// </summary>
		/// <param name="value"></param>
		public void PutByte(bool value)
		{
			this.AssertNotDisposed();
			_buffer.WriteByte(value ? (byte)1 : (byte)0);
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutBytes(byte[] value)
		{
			this.AssertNotDisposed();
			_buffer.Write(value);
		}

		/// <summary>
		/// Writes given number of bytes with the value 0 to packet.
		/// </summary>
		/// <param name="amount"></param>
		public void PutEmpty(int amount)
		{
			this.AssertNotDisposed();

			for (var i = 0; i < amount; ++i)
				_buffer.WriteByte(0);
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutShort(short value)
		{
			this.AssertNotDisposed();
			_buffer.WriteInt16(value);
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutInt(int value)
		{
			this.AssertNotDisposed();
			_buffer.WriteInt32(value);
		}

		/// <summary>
		/// Writes IP to packet as an int.
		/// </summary>
		/// <param name="value"></param>
		public void PutInt(IPAddress value)
		{
			this.AssertNotDisposed();

			Span<byte> bytes = stackalloc byte[4];
			value.TryWriteBytes(bytes, out _);

			_buffer.Write(bytes);
		}

		/// <summary>
		/// Writes string to packet, padding or capping it at the
		/// given length, while also adding a null terminator.
		/// </summary>
		/// <example>
		/// packet.PutString("foobar", 8);
		/// => 66 6F 6F 62 61 72 00 00
		/// 
		/// packet.PutString("foobar", 4);
		/// => 66 6F 6F 00
		/// </example>
		/// <param name="value"></param>
		public void PutString(string value, int length)
		{
			this.AssertNotDisposed();
			_buffer.WriteString(EncodingKR, value, length, StringWriteOptions.NullTerminated);
		}

		/// <summary>
		/// Writes string and a null terminator to packet.
		/// </summary>
		/// <example>
		/// packet.PutString("foobar", false);
		/// => 66 6F 6F 62 61 72
		/// 
		/// packet.PutString("foobar", true);
		/// => 66 6F 6F 62 61 72 00
		/// </example>
		/// <param name="value"></param>
		/// <param name="terminate">Whether to put a null-terminator at the end of the string.</param>
		public void PutString(string value, bool terminate = true)
		{
			this.AssertNotDisposed();
			_buffer.WriteString(EncodingKR, value, terminate ? StringWriteOptions.NullTerminated : StringWriteOptions.None);
		}

		/// <summary>
		/// Reads a byte from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public byte GetByte()
		{
			this.AssertNotDisposed();
			return _buffer.ReadByte();
		}

		/// <summary>
		/// Reads a signed byte from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public sbyte GetSByte()
		{
			this.AssertNotDisposed();
			return _buffer.ReadSByte();
		}

		/// <summary>
		/// Reads the given number of bytes from the packet and returns them.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public byte[] GetBytes(int length)
		{
			this.AssertNotDisposed();
			return _buffer.Read(length);
		}

		/// <summary>
		/// Reads a short from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public int GetShort()
		{
			this.AssertNotDisposed();
			return _buffer.ReadInt16();
		}

		/// <summary>
		/// Reads an int from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public int GetInt()
		{
			this.AssertNotDisposed();
			return _buffer.ReadInt32();
		}

		/// <summary>
		/// Reads given number of bytes from packet and returns them
		/// as a UTF8 string.
		/// </summary>
		/// <returns></returns>
		public string GetString(int length)
		{
			this.AssertNotDisposed();

			var bytes = _buffer.ReadAsSpan(length);
			var val = EncodingKR.GetString(bytes);

			var nullIndex = val.IndexOf((char)0);
			if (nullIndex != -1)
				val = val.Substring(0, nullIndex);

			return val;
		}

		/// <summary>
		/// Returns a buffer containing the packet's body.
		/// </summary>
		/// <returns></returns>
		public byte[] Build()
		{
			this.AssertNotDisposed();

			var length = _buffer.Length - _bodyStart;
			var buffer = new byte[length];

			this.Build(ref buffer, 0);

			return buffer;
		}

		/// <summary>
		/// Copies packet's body to given buffer at offset.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <returns></returns>
		public void Build(ref byte[] buffer, int offset)
		{
			this.AssertNotDisposed();
			_buffer.CopyTo(buffer, offset, _bodyStart);
		}

		/// <summary>
		/// Returns a string representation of the packet.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			this.AssertNotDisposed();
			return Hex.ToString(_buffer.Copy(), HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine);
		}
	}

	/// <summary>
	/// Custom exception for disposed packets.
	/// </summary>
	/// <remarks>
	/// This exception is thrown when an operation is attempted on a
	/// packet that has already been disposed. It's important for us to
	/// distinguish this from a regular ObjectDisposedException, because
	/// ObjectDisposedException will be thrown and swallowed by the
	/// network handling in certain circumstances.
	/// </remarks>
	public class PacketDisposedException : Exception
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="obj"></param>
		public PacketDisposedException(object obj)
			: base($"Packet '{obj}' has been disposed and cannot be used.")
		{
		}

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="message"></param>
		public PacketDisposedException(string message) : base(message)
		{
		}
	}
}
