using Sabine.Shared.Database;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Network
{
	public class ZoneConnection : Connection
	{
		public Account Account { get; set; }
		public PlayerCharacter Character { get; set; }

		protected override void OnPacketReceived(Packet packet)
		{
			ZoneServer.Instance.PacketHandler.Handle(this, packet);
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
