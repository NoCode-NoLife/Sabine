using System;
using System.Collections.Generic;
using Sabine.Char.Database;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;

namespace Sabine.Char.Network
{
	public class CharConnection : TcpConnection
	{
		public Account Account = new Account() { Username = "admin" };

		public List<Character> Characters = new List<Character>
		{
			new Character() { Slot = 0, Id = 10001, Name = "exec", JobId = JobId.Acolyte },
			new Character() { Slot = 1, Id = 10002, Name = "exec", JobId = JobId.Archer },
			new Character() { Slot = 2, Id = 10003, Name = "exec", JobId = JobId.Thief },
		};

		private readonly RoFramer _framer;

		public CharConnection()
		{
			_framer = new RoFramer(1024);
			_framer.MessageReceived += this.OnMessageReceived;
		}

		protected override void ReveiveData(byte[] buffer, int length)
		{
			Log.Debug("< {0}", Hex.ToString(buffer, 0, length));

			_framer.ReceiveData(buffer, length);
		}

		private void OnMessageReceived(byte[] buffer)
		{
			var packet = new Packet(buffer);

			Log.Debug("Op: {0:X4} ({1})", packet.Op, PacketTable.GetName(packet.Op));

			CharServer.Instance.PacketHandler.Handle(this, packet);
		}

		public void Send(Packet packet)
		{
			var data = _framer.Frame(packet);
			this.Send(data);
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
		}
	}
}
