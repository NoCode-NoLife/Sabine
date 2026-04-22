using System;
using System.Buffers;
using System.Net.Sockets;
using System.Threading.Tasks;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;

namespace Sabine.Shared.Network
{
	/// <summary>
	/// Connection base class for the servers.
	/// </summary>
	public abstract class Connection : TcpConnection
	{
		protected readonly RoFramer _framer = new(1024);

		/// <summary>
		/// Called when new data was sent from the client.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="length"></param>
		protected override void ReceiveData(byte[] buffer, int length)
		{
			//Log.Debug("< {0}", Hex.ToString(buffer, 0, length));

			_framer.ReceiveData(buffer, length, this.OnMessageReceived);
		}

		/// <summary>
		/// Called when a full message was received from the client.
		/// </summary>
		/// <param name="buffer"></param>
		protected virtual void OnMessageReceived(byte[] buffer)
		{
			using var packet = Packet.Rent(buffer);
			packet.Op = PacketTable.ToHost((int)packet.Op);

			//Log.Debug("< Op: 0x{0:X4} ({1})\r\n{2}", PacketTable.ToNetwork(packet.Op), packet.Op, Hex.ToString(buffer, HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine));
			//Log.Debug("".PadRight(40, '-'));

			this.OnPacketReceived(packet);
		}

		/// <summary>
		/// Called when a packet was received from the client.
		/// </summary>
		/// <param name="packet"></param>
		protected abstract void OnPacketReceived(Packet packet);

		/// <summary>
		/// Sends packet to client.
		/// </summary>
		/// <param name="packet"></param>
		public void Send(Packet packet)
		{
			_framer.GetPacketSize(packet, out var tableSize, out var packetSize);

			var buffer = ArrayPool<byte>.Shared.Rent(packetSize);
			_framer.Frame(packet, tableSize, packetSize, buffer);

			if (tableSize != PacketTable.Dynamic && packetSize != tableSize)
			{
				var name = packet.Op.ToString();

				var opNetwork = PacketTable.ToNetwork(packet.Op);
				var nameNetwork = PacketTable.GetName(opNetwork);

				Log.Warning("Connection.Send: Invalid packet size for '{0:X4}' ({1}) (got {2}, expected {3}).", opNetwork, nameNetwork, packetSize, tableSize);

				if (name != nameNetwork)
					Log.Warning("Connection: Potential packet table error. Packet's op is '{0}', while the table's network op is '{1}'.", name, nameNetwork);

				// We can't send a packet that's not the correct size, as
				// that will mess up the data stream, at which point we might
				// as well close the connection. Instead, let's fix the size
				// and hope for the best. The incorrect packet needs to be
				// fixed of course, but at least you're not kicked every
				// time you haven't updated some packet for a new version
				// yet.
				var newBuffer = ArrayPool<byte>.Shared.Rent(tableSize);
				var copySize = Math.Min(packetSize, tableSize);
				Buffer.BlockCopy(buffer, 0, newBuffer, 0, copySize);

				ArrayPool<byte>.Shared.Return(buffer);

				buffer = newBuffer;
				packetSize = tableSize;
			}

			//Log.Debug("> {0}", Hex.ToString(buffer, 0, packetSize));

			try
			{
				this.Send(buffer, packetSize);
			}
			catch (SocketException)
			{
				this.Close();
			}
		}

		/// <summary>
		/// Called after data sent is no longer needed by the connection.
		/// </summary>
		/// <param name="data"></param>
		/// <param name="length"></param>
		/// <param name="type"></param>
		protected override void PostSend(byte[] data, int length, PostSendType type)
		{
			try
			{
				ArrayPool<byte>.Shared.Return(data);
			}
			catch (ArgumentException ex)
			{
				Log.Warning("A send buffer could not be returned to the pool. Error: " + ex.Message);
				Log.Debug("Data sent: " + Hex.ToString(data, 0, length));
			}
		}

		/// <summary>
		/// Called when an exception occurred while reading data from
		/// the TCP stream.
		/// </summary>
		/// <param name="ex"></param>
		protected override void OnReceiveException(Exception ex)
		{
			Log.Error("Error while receiving data: {0}", ex);
		}

		/// <summary>
		/// Called when the connection was closed.
		/// </summary>
		/// <param name="type"></param>
		protected override void OnClosed(ConnectionCloseType type)
		{
			switch (type)
			{
				case ConnectionCloseType.Disconnected: Log.Info("Connection from '{0}' was closed by the client.", this.Address); break;
				case ConnectionCloseType.Closed: Log.Info("Connection to '{0}' was closed by the server.", this.Address); break;
				case ConnectionCloseType.Lost: Log.Info("Lost connection from '{0}'.", this.Address); break;
			}
		}

		/// <summary>
		/// Closes the connection after the given amount of seconds,
		/// to give the client time to, for example, display login
		/// error messages before the connection is closed and the
		/// messages get lost.
		/// </summary>
		/// <param name="seconds"></param>
		public void Close(int seconds)
		{
			Task.Delay(seconds * 1000).ContinueWith(_ => this.Close());
		}
	}
}
