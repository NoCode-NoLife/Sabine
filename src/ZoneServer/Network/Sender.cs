using System.Collections.Generic;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;

namespace Sabine.Zone.Network
{
	/// <summary>
	/// A class that is able to send packets to an arbitrary destination.
	/// </summary>
	public interface ISender
	{
		void Send(Packet packet);
	}

	/// <summary>
	/// A sender that sends packets to a character's connection.
	/// </summary>
	public readonly struct SingleConnectionSender : ISender
	{
		/// <summary>
		/// Returns the connection to which packets will be sent.
		/// </summary>
		private readonly Connection _connection;

		/// <summary>
		/// Creates new instance for character's connection.
		/// </summary>
		/// <param name="character"></param>
		public SingleConnectionSender(PlayerCharacter character)
			: this(character.Connection)
		{
		}

		/// <summary>
		/// Creates new instance for the given connection.
		/// </summary>
		/// <param name="conn"></param>
		public SingleConnectionSender(Connection conn)
		{
			_connection = conn;
		}

		/// <summary>
		/// Sends the packet to the connection.
		/// </summary>
		/// <param name="packet"></param>
		public readonly void Send(Packet packet)
		{
			_connection.Send(packet);
		}
	}

	/// <summary>
	/// A sender that sends packets to all characters on the map.
	/// </summary>
	public readonly struct MapBroadcastSender : ISender
	{
		private readonly Map _map;

		/// <summary>
		/// Creates new instance for the map the character is on.
		/// </summary>
		/// <param name="source"></param>
		public MapBroadcastSender(Character source)
			: this(source.Map)
		{
		}

		/// <summary>
		/// Creates new instance for the given map.
		/// </summary>
		/// <param name="map"></param>
		public MapBroadcastSender(Map map)
		{
			_map = map;
		}

		/// <summary>
		/// Broadcasts the packet to all characters on the map.
		/// </summary>
		/// <param name="packet"></param>
		public readonly void Send(Packet packet)
		{
			_map.Broadcast(packet);
		}
	}

	/// <summary>
	/// A sender that sends packets to all characters within the sight of
	/// a specific character.
	/// </summary>
	public readonly struct SightBroadcastSender : ISender
	{
		private readonly Character _source;
		private readonly BroadcastTargets _targets;

		/// <summary>
		/// Creates new instance for the given character.
		/// </summary>
		/// <param name="source"></param>
		public SightBroadcastSender(Character source)
			: this(source, BroadcastTargets.All)
		{
		}

		/// <summary>
		/// Creates new instance for the given character.
		/// </summary>
		/// <param name="source"></param>
		/// <param name="targets"></param>
		public SightBroadcastSender(Character source, BroadcastTargets targets)
		{
			_source = source;
			_targets = targets;
		}

		/// <summary>
		/// Sends the packet to all characters within view range of the
		/// source character.
		/// </summary>
		/// <param name="packet"></param>
		public readonly void Send(Packet packet)
		{
			_source.Map.Broadcast(packet, _source, _targets);
		}
	}

	/// <summary>
	/// A sender that sends packets to a list of characters.
	/// </summary>
	public readonly struct ListBroadcastSender : ISender
	{
		/// <summary>
		/// Returns the characters the packet will be sent to.
		/// </summary>
		private readonly List<PlayerCharacter> _characters;

		/// <summary>
		/// Creates new instance for the given list of characters.
		/// </summary>
		/// <param name="characters"></param>
		public ListBroadcastSender(List<PlayerCharacter> characters)
		{
			_characters = characters;
		}

		/// <summary>
		/// Sends the packet to all characters in the list.
		/// </summary>
		/// <param name="packet"></param>
		public readonly void Send(Packet packet)
		{
			foreach (var character in _characters)
				character.Connection.Send(packet);
		}
	}
}
