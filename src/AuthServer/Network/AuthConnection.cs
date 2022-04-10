using System;
using Sabine.Shared.Network;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;

namespace Sabine.Auth.Network
{
	public class AuthConnection : TcpConnection
	{
		private readonly RoFramer _framer;

		public AuthConnection()
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

			AuthServer.Instance.PacketHandler.Handle(this, packet);
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
