using System;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Events.Args
{
	/// <summary>
	/// Arguments for events related to a player character.
	/// </summary>
	public class PlayerEventArgs : EventArgs
	{
		/// <summary>
		/// Returns the character associated with the event.
		/// </summary>
		public PlayerCharacter Character { get; }

		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="character"></param>
		public PlayerEventArgs(PlayerCharacter character)
		{
			this.Character = character;
		}
	}
}
