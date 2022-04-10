using System;
using System.Collections.Generic;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Maps
{
	public class Map
	{
		private readonly Dictionary<int, PlayerCharacter> _characters = new Dictionary<int, PlayerCharacter>();

		public static readonly Limbo Limbo = new Limbo();

		public string Name { get; set; }

		public int PlayerCount
		{
			get
			{
				lock (_characters)
					return _characters.Count;
			}
		}

		public Map(string name)
		{
			this.Name = name;
		}

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
