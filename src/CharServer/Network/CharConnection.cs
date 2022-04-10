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
		public Account Account = new Account() { Username = "admin" };

		public List<Character> Characters = new List<Character>
		{
			new Character() { Slot = 0, Id = 10001, Name = "exec", JobId = JobId.Acolyte },
			new Character() { Slot = 1, Id = 10002, Name = "exec", JobId = JobId.Archer },
			new Character() { Slot = 2, Id = 10003, Name = "exec", JobId = JobId.Thief },
		};

		protected override void OnPacketReceived(Packet packet)
		{
			CharServer.Instance.PacketHandler.Handle(this, packet);
		}
	}
}
