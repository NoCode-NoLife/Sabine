using System;
using Sabine.Shared.Database;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// Represents a connection to a client.
	/// </summary>
	public class ZoneConnection : Connection
	{
		/// <summary>
		/// Gets or sets the account associated with this connection.
		/// </summary>
		public Account Account { get; set; }

		/// <summary>
		/// Gets or sets the character currently controlled by this
		/// connection.
		/// </summary>
		public PlayerCharacter Character { get; set; }

		/// <summary>
		/// Called when the client sent a packet.
		/// </summary>
		/// <param name="packet"></param>
		protected override void OnPacketReceived(Packet packet)
		{
			ZoneServer.Instance.PacketHandler.Handle(this, packet);
		}

		/// <summary>
		/// Called when the connection was closed.
		/// </summary>
		/// <param name="type"></param>
		protected override void OnClosed(ConnectionCloseType type)
		{
			base.OnClosed(type);
			this.CleanUp();
		}

		/// <summary>
		/// Cleans up any thing left behind by this connection, such as
		/// despawning and saving the characters.
		/// </summary>
		private void CleanUp()
		{
			var account = this.Account;
			var character = this.Character;

			if (character != null)
			{
				character?.Map.RemoveCharacter(character);
			}

			if (account != null && character != null)
			{
				try
				{
					ZoneServer.Instance.Database.SaveCharacter(this.Account, character);
				}
				catch (Exception ex)
				{
					Log.Error("ZoneConnection.CleanUp: Failed to save character '{0}'. Error: {1}", character.Name, ex);
				}
			}
		}

		/// <summary>
		/// Returns the currently controlled character.
		/// </summary>
		/// <returns></returns>
		public PlayerCharacter GetCurrentCharacter()
		{
			if (this.Character == null)
				throw new InvalidOperationException("This connection isn't controlling any character.");

			return this.Character;
		}
	}
}
