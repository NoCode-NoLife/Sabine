using System;
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
		protected readonly RoFramer _framer;

		/// <summary>
		/// Creates new connection.
		/// </summary>
		public Connection()
		{
			_framer = new RoFramer(1024);
			_framer.MessageReceived += this.OnMessageReceived;
		}

		/// <summary>
		/// Called when new data was sent from the client.
		/// </summary>
		/// <param name="buffer"></param>
		/// <param name="length"></param>
		protected override void ReveiveData(byte[] buffer, int length)
		{
			Log.Debug("< {0}", Hex.ToString(buffer, 0, length));

			_framer.ReceiveData(buffer, length);
		}

		/// <summary>
		/// Called when a full message was received from the client.
		/// </summary>
		/// <param name="buffer"></param>
		protected virtual void OnMessageReceived(byte[] buffer)
		{
			var packet = new Packet(buffer);

			Log.Debug("< Op: 0x{0:X4} ({1})\r\n{2}", packet.Op, PacketTable.GetName(packet.Op), Hex.ToString(buffer, HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine));
			Log.Debug("".PadRight(40, '-'));

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
			var buffer = _framer.Frame(packet);

			Log.Debug("> Op: 0x{0:X4} ({1})\r\n{2}", packet.Op, PacketTable.GetName(packet.Op), Hex.ToString(buffer, HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine));
			Log.Debug("".PadRight(40, '-'));

			var tableSize = PacketTable.GetSize(packet.Op);
			if (tableSize != PacketTable.Dynamic && buffer.Length != tableSize)
				Log.Warning("Connection: Invalid packet size for '{0:X4}' ({1}) ({2} != {3}).", packet.Op, PacketTable.GetName(packet.Op), buffer.Length, tableSize);

			this.Send(buffer);
		}

		/// <summary>
		/// Called when an exception occurred while reading data from
		/// the TCP stream.
		/// </summary>
		/// <param name="ex"></param>
		protected override void OnReceiveException(Exception ex)
		{
			Log.Error("Error while receiving data: {0}", ex.Message);
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
