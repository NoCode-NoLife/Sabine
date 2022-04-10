using System.Collections.Generic;
using Sabine.Char.Database;
using Sabine.Shared.Database;
using Sabine.Shared.Network;

namespace Sabine.Char.Network
{
	/// <summary>
	/// Represents a connection to a client.
	/// </summary>
	public class CharConnection : Connection
	{
		/// <summary>
		/// Gets or sets the account associated with this connection.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// Returns a list of the characters available.
		/// </summary>
		public List<Character> Characters { get; } = new List<Character>();

		/// <summary>
		/// Called when the client sent a packet.
		/// </summary>
		/// <param name="packet"></param>
		protected override void OnPacketReceived(Packet packet)
		{
			CharServer.Instance.PacketHandler.Handle(this, packet);
		}
	}
}
