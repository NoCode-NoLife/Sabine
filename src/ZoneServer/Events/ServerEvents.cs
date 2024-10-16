﻿using Sabine.Zone.Events.Args;
using Yggdrasil.Events;

namespace Sabine.Zone.Events
{
	/// <summary>
	/// Manager for events occurring on the server, such as players logging
	/// in or killing monsters.
	/// </summary>
	public class ServerEvents
	{
		/// <summary>
		/// Raised when a player logged in.
		/// </summary>
		/// <remarks>
		/// This event is raised right after the login was confirmed and
		/// before any more packets are sent to the client or they are
		/// added to the world. This makes it a good time to make 
		/// modifications to the character, but packets sent to the
		/// client might not get handled as intended yet.
		/// </remarks>
		public readonly Event<PlayerEventArgs> PlayerLoggedIn = new();

		/// <summary>
		/// Raised after the player's client loaded the map and is ready
		/// to start the game.
		/// </summary>
		/// <remarks>
		/// This event is raised once the player is fully logged in and intialized.
		/// </remarks>
		public readonly Event<PlayerEventArgs> PlayerReady = new();

	}
}
