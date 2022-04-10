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
	public class CharConnection : Connection
	{
		public Account Account { get; set; }

		public List<Character> Characters { get; } = new List<Character>();

		protected override void OnPacketReceived(Packet packet)
		{
			CharServer.Instance.PacketHandler.Handle(this, packet);
		}
	}
}
