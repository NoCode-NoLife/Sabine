using System;
using Sabine.Shared.Database;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;

namespace Sabine.Zone.Network
{
	public class ZoneConnection : TcpConnection
	{
		private readonly RoFramer _framer;

		public Account Account { get; set; }
		public PlayerCharacter Character { get; set; }

		public ZoneConnection()
		{
			_framer = new RoFramer(1024);
			_framer.MessageReceived += this.OnMessageReceived;
		}

		protected override void ReveiveData(byte[] buffer, int length)
		{
			//Log.Debug("< {0}", Hex.ToString(buffer, 0, length));

			_framer.ReceiveData(buffer, length);
		}

		private void OnMessageReceived(byte[] buffer)
		{
			var packet = new Packet(buffer);

			Log.Debug("< Op: 0x{0:X4} ({1})\r\n{2}", packet.Op, PacketTable.GetName(packet.Op), Hex.ToString(buffer, HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine));
			Log.Debug("".PadRight(40, '-'));

			ZoneServer.Instance.PacketHandler.Handle(this, packet);
		}

		public virtual void Send(Packet packet)
		{
			var buffer = _framer.Frame(packet);

			Log.Debug("> Op: 0x{0:X4} ({1})\r\n{2}", packet.Op, PacketTable.GetName(packet.Op), Hex.ToString(buffer, HexStringOptions.SpaceSeparated | HexStringOptions.SixteenNewLine));
			Log.Debug("".PadRight(40, '-'));

			var tableSize = PacketTable.GetSize(packet.Op);
			if (tableSize != PacketTable.Dynamic && buffer.Length != tableSize)
				Log.Warning("ZoneConnection: Invalid packet size for '{0:X4}' ({1}) ({2} != {3}).", packet.Op, PacketTable.GetName(packet.Op), buffer.Length, tableSize);

			this.Send(buffer);
		}

		protected override void OnReceiveException(Exception ex)
		{
			Log.Error("Error while receiving data: {0}", ex.Message);
		}

		protected override void OnClosed(ConnectionCloseType type)
		{
			switch (type)
			{
				case ConnectionCloseType.Disconnected: Log.Info("Connection from '{0}' was closed by the client.", this.Address); break;
				case ConnectionCloseType.Closed: Log.Info("Connection to '{0}' was closed by the server.", this.Address); break;
				case ConnectionCloseType.Lost: Log.Info("Lost connection from '{0}'.", this.Address); break;
			}

			this.CleanUp();
		}

		private void CleanUp()
		{
			var character = this.Character;

			if (character != null)
			{
				character?.Map.RemoveCharacter(character);
			}
		}

		public PlayerCharacter GetCurrentCharacter()
		{
			return this.Character;
		}
	}
}
