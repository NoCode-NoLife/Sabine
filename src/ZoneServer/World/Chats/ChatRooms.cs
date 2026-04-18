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

	/// <summary>
	/// Provides a stack-only snapshot of a collection using a pooled
	/// array to minimize allocations.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	public ref struct PooledSnapshot<T>
	{
		private T[] _rentedArray;
		private readonly int _count;

		/// <summary>
		/// Creates a snapshot of the provided collection.
		/// </summary>
		/// <remarks>
		/// The snapshot doesn't perform any locking or synchronization,
		/// the caller must ensure thread-safety if the collection is
		/// being modified concurrently.
		/// </remarks>
		/// <param name="source"></param>
		public PooledSnapshot(ICollection<T> source)
		{
			_count = source.Count;
			if (_count == 0)
			{
				_rentedArray = null;
				return;
			}

			_rentedArray = ArrayPool<T>.Shared.Rent(_count);
			source.CopyTo(_rentedArray, 0);
		}

		/// <summary>
		/// Returns a read-only span over the snapshot data. The span is
		/// only valid while the snapshot is in scope and undisposed.
		/// </summary>
		public readonly ReadOnlySpan<T> Span
		{
			[MethodImpl(MethodImplOptions.AggressiveInlining)]
			get => new(_rentedArray, 0, _count);
		}

		/// <summary>
		/// Disposes the snapshot, returning the rented array to the pool.
		/// </summary>
		public void Dispose()
		{
			if (_rentedArray != null)
			{
				var clearArray = true; // RuntimeHelpers.IsReferenceOrContainsReferences<T>();
				ArrayPool<T>.Shared.Return(_rentedArray, clearArray);
				_rentedArray = null;
			}
		}

		/// <summary>
		/// Returns an enumerator for the snapshot's span.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public readonly ReadOnlySpan<T>.Enumerator GetEnumerator()
		{
			return this.Span.GetEnumerator();
		}
	}
}
