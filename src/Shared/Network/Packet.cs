using System;
using System.Net;
using System.Text;
using Yggdrasil.Extensions;
using Yggdrasil.Util;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Packet reader and writer.
	/// </summary>
	public class Packet
	{
		private readonly BufferReaderWriter _buffer;
		private readonly int _bodyStart;

		private static readonly Encoding EncodingKR = Encoding.GetEncoding("EUC-KR");

		/// <summary>
		/// Gets or sets the packet's opcode.
		/// </summary>
		public Op Op { get; set; }

		/// <summary>
		/// Creates new packet to write to.
		/// </summary>
		/// <param name="op"></param>
		public Packet(Op op)
		{
			this.Op = op;

			_buffer = new BufferReaderWriter();
			_buffer.Endianness = Endianness.LittleEndian;
		}

		/// <summary>
		/// Creates packet from buffer to read it.
		/// </summary>
		/// <param name="buffer"></param>
		public Packet(byte[] buffer)
		{
			_buffer = new BufferReaderWriter(buffer);
			_buffer.Endianness = Endianness.LittleEndian;

			this.Op = (Op)_buffer.ReadInt16();
			_bodyStart = _buffer.Index;
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutByte(byte value)
			=> _buffer.WriteByte(value);

		/// <summary>
		/// Writes value to packet as 1 or 0.
		/// </summary>
		/// <param name="value"></param>
		public void PutByte(bool value)
			=> _buffer.WriteByte(value ? (byte)1 : (byte)0);

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutBytes(byte[] value)
			=> _buffer.Write(value);

		/// <summary>
		/// Writes given number of bytes with the value 0 to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutEmpty(int amount)
		{
			for (var i = 0; i < amount; ++i)
				_buffer.WriteByte(0);
		}

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutShort(short value)
			=> _buffer.WriteInt16(value);

		/// <summary>
		/// Writes value to packet.
		/// </summary>
		/// <param name="value"></param>
		public void PutInt(int value)
			=> _buffer.WriteInt32(value);

		/// <summary>
		/// Writes DateTime to packet as a UNIX timestamp.
		/// </summary>
		/// <param name="value"></param>
		public void PutInt(DateTime value)
			=> _buffer.WriteInt32(value.GetUnixTimestamp());

		/// <summary>
		/// Writes IP to packet as an int.
		/// </summary>
		/// <param name="value"></param>
		public void PutInt(IPAddress value)
			=> _buffer.WriteInt32(BitConverter.ToInt32(value.GetAddressBytes(), 0));

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
			var bytes = EncodingKR.GetBytes(value ?? "");
			var writeLength = Math.Min(bytes.Length, length - 1);
			var remain = length - writeLength;

			_buffer.Write(bytes, 0, writeLength);

			for (var i = 0; i < remain; ++i)
				_buffer.WriteByte(0);
		}

		/// <summary>
		/// Writes string and a null terminator to packet.
		/// </summary>
		/// <param name="value"></param>
		/// <param name="terminate">Whether to put a null-terminator at the end of the string.</param>
		public void PutString(string value, bool terminate = true)
		{
			var bytes = EncodingKR.GetBytes(value ?? "");

			_buffer.Write(bytes);
			if (terminate)
				_buffer.WriteByte(0);
		}

		/// <summary>
		/// Reads a byte from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public byte GetByte()
			=> _buffer.ReadByte();

		/// <summary>
		/// Reads the given number of bytes from the packet and returns them.
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public byte[] GetBytes(int length)
			=> _buffer.Read(length);

		/// <summary>
		/// Reads a short from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public short GetShort()
			=> _buffer.ReadInt16();

		/// <summary>
		/// Reads an int from packet and returns it.
		/// </summary>
		/// <returns></returns>
		public int GetInt()
			=> _buffer.ReadInt32();

		/// <summary>
		/// Reads given number of bytes from packet and returns them
		/// as a UTF8 string.
		/// </summary>
		/// <returns></returns>
		public string GetString(int length)
		{
			var bytes = _buffer.Read(length);
			var len = Array.IndexOf(bytes, (byte)0);

			if (len == -1)
				len = bytes.Length;

			var result = EncodingKR.GetString(bytes, 0, len);

			return result;
		}

		/// <summary>
		/// Returns a buffer containing the packet's body, without opcode
		/// or length.
		/// </summary>
		/// <returns></returns>
		public byte[] Build()
		{
			var length = _buffer.Length - _bodyStart;
			var result = new byte[length];

			_buffer.CopyTo(result, 0, _bodyStart);

			return result;
		}

		/// <summary>
		/// Returns a string representation of the packet.
		/// </summary>
		/// <returns></returns>
		public override string ToString()
			=> Hex.ToString(_buffer.Copy(), HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine);
	}
}
