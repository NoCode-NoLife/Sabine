using System;
using System.Buffers;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Sabine.Zone.Network;

namespace Sabine.Zone.World.Chats
{
	/// <summary>
	/// Manages the chat rooms in the world.
	/// </summary>
	public class ChatRooms
	{
		private readonly List<ChatRoom> _rooms = new();

		/// <summary>
		/// Adds chat room to the world, to be looked up. Broadcasts the
		/// room's existence to nearby players.
		/// </summary>
		/// <param name="room"></param>
		public void Add(ChatRoom room)
		{
			lock (_rooms)
				_rooms.Add(room);

			Send.ZC_ROOM_NEWENTRY(room);
		}

		/// <summary>
		/// Removes a chat room from the world and forcefully removes all
		/// members from it. Broadcasts the room's removal to nearby
		/// player.
		/// </summary>
		/// <param name="room"></param>
		public void Remove(ChatRoom room)
		{
			lock (_rooms)
				_rooms.Remove(room);

			room.RemoveMembers();

			Send.ZC_DESTROY_ROOM(room);
		}

		/// <summary>
		/// Returns the room with the given id via out. Returns false if
		/// the room doesn't exist.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		public bool TryGet(int id, out ChatRoom result)
		{
			lock (_rooms)
			{
				foreach (var room in _rooms)
				{
					if (room.Id == id)
					{
						result = room;
						return true;
					}
				}
			}

			result = null;
			return false;
		}
	}
}
