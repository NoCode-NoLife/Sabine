using System;
using System.Collections.Generic;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// Represents a map in the world.
	/// </summary>
	public class Map
	{
		private readonly Dictionary<int, PlayerCharacter> _characters = new Dictionary<int, PlayerCharacter>();

		/// <summary>
		/// Returns a reference to the Limbo map. See Limbo class for
		/// more information.
		/// </summary>
		public static readonly Limbo Limbo = new Limbo();

		/// <summary>
		/// Returns the map's name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Returns the number of players on this number.
		/// </summary>
		public int PlayerCount
		{
			get
			{
				lock (_characters)
					return _characters.Count;
			}
		}

		/// <summary>
		/// Creates new map.
		/// </summary>
		/// <param name="name"></param>
		public Map(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Adds character to this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void AddCharacter(PlayerCharacter character)
		{
			lock (_characters)
			{
				if (_characters.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' already exists on the map.");

				_characters[character.Id] = character;
				character.Map = this;
				Log.Debug("+ Characters on {0}: {1}", this.Name, _characters.Count);
			}
		}

		/// <summary>
		/// Removes character from this map.
		/// </summary>
		/// <param name="character"></param>
		/// <exception cref="ArgumentException"></exception>
		public virtual void RemoveCharacter(PlayerCharacter character)
		{
			lock (_characters)
			{
				if (!_characters.ContainsKey(character.Id))
					throw new ArgumentException($"A character with the id '{character.Id}' doesn't exists on the map.");

				_characters.Remove(character.Id);
				character.Map = this;

				Log.Debug("- Characters on {0}: {1}", this.Name, _characters.Count);
			}
		}

		/// <summary>
		/// Broadcasts packet to players on this map.
		/// </summary>
		/// <param name="packet">Packet to send.</param>
		/// <param name="source">Source of the packet if it's only sent in a range around the source. Use null for map-wide broadcast.</param>
		/// <param name="includeSource">If true, the packet is sent to the source as well.</param>
		public void Broadcast(Packet packet, PlayerCharacter source = null, bool includeSource = false)
		{
			lock (_characters)
			{
				foreach (var character in _characters.Values)
				{
					if (source != null && !includeSource && source == character)
						continue;

					character.Connection.Send(packet);
				}
			}
		}
	}
}
