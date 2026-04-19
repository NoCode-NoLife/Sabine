using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Sabine.Shared.Const;
using Sabine.Zone.Network;
using Sabine.Zone.World.Actors;
using Sabine.Zone.World.Maps;
using Yggdrasil.Collections;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Chats
{
	/// <summary>
	/// Represents a chat room created by a player.
	/// </summary>
	public class ChatRoom
	{
		private static int IdPool = 1;

		private readonly object _syncLock = new();
		private readonly List<ChatMember> _members = new();
		private readonly HashSet<string> _banned = new();

		/// <summary>
		/// Returns the chat's unique id.
		/// </summary>
		public int Id { get; }

		/// <summary>
		/// Returns the handle of the chat's owner.
		/// </summary>
		public int OwnerHandle { get; private set; }

		/// <summary>
		/// Returns the map the chat was created on.
		/// </summary>
		public Map Map { get; }

		/// <summary>
		/// Gets or sets the chat's title, displayed above the owner's head.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// Gets or sets the number of members allowed in the chat.
		/// </summary>
		public int Limit { get; set; }

		/// <summary>
		/// Gets or sets the chat's privacy setting, determining who can
		/// enter the chat and whether a password is required.
		/// </summary>
		public ChatPrivacy Privacy { get; set; }

		/// <summary>
		/// Gets or sets the chat's password, required to enter if the
		/// privacy is set to private.
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Returns the number of members currently in the chat.
		/// </summary>
		public int MemberCount
		{
			get
			{
				lock (_syncLock)
					return _members.Count;
			}
		}

		/// <summary>
		/// Returns true if the chat has reached its member limit.
		/// </summary>
		public bool IsFull
		{
			get
			{
				lock (_syncLock)
					return _members.Count >= this.Limit;
			}
		}

		/// <summary>
		/// Creates a new chat room.
		/// </summary>
		/// <param name="owner">The character who owns the room.</param>
		/// <param name="title">The title of the room.</param>
		/// <param name="limit">The maximum number of members allowed in the room.</param>
		/// <param name="privacy">The privacy setting of the room.</param>
		/// <param name="password">The password required to enter the room if it's private.</param>
		public ChatRoom(PlayerCharacter owner, string title, int limit, ChatPrivacy privacy, string password)
		{
			this.Id = Interlocked.Increment(ref IdPool);

			this.OwnerHandle = owner.Handle;
			this.Map = owner.Map;

			this.Title = title;
			this.Limit = limit;
			this.Privacy = privacy;
			this.Password = password;
		}

		/// <summary>
		/// Updates the room's settings and updates clients in range.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="limit"></param>
		/// <param name="privacy"></param>
		/// <param name="password"></param>
		public void ChangeSettings(string title, int limit, ChatPrivacy privacy, string password)
		{
			this.Title = title;
			this.Limit = limit;
			this.Privacy = privacy;
			this.Password = password;

			Send.ZC_ROOM_NEWENTRY(this);
		}

		/// <summary>
		/// Returns true if the given character is a member of the chat.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool IsMember(PlayerCharacter character)
		{
			lock (_syncLock)
			{
				foreach (var member in _members)
				{
					if (member.Handle == character.Handle)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the given character is a member of the chat.
		/// </summary>
		/// <param name="characterName"></param>
		/// <returns></returns>
		public bool IsMember(string characterName)
		{
			lock (_syncLock)
			{
				foreach (var member in _members)
				{
					if (member.Name == characterName)
						return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Returns true if the given character is the owner of the chat.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool IsOwner(PlayerCharacter character)
		{
			return this.OwnerHandle == character.Handle;
		}

		/// <summary>
		/// Returns true if the given member is the owner of the chat.
		/// </summary>
		/// <param name="member"></param>
		/// <returns></returns>
		public bool IsOwner(ChatMember member)
		{
			return this.OwnerHandle == member.Handle;
		}

		/// <summary>
		/// Returns true if the character is not allowed to enter the
		/// chat.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool IsBanned(PlayerCharacter character)
		{
			lock (_syncLock)
				return _banned.Contains(character.Name);
		}

		/// <summary>
		/// Returns a snapshop of the chat's members.
		/// </summary>
		/// <returns></returns>
		public PooledSnapshot<ChatMember> GetMembers()
		{
			lock (_syncLock)
				return new PooledSnapshot<ChatMember>(_members);
		}

		/// <summary>
		/// Adds the given character to the chat, notifying existing
		/// members as well as the new member of the update.
		/// </summary>
		/// <param name="character"></param>
		public void AddMember(PlayerCharacter character)
		{
			lock (_syncLock)
			{
				// Send chat info to new character first to open the chat,
				// then add them and send the update, which adds the new
				// member on all clients and displays the enter message.
				Send.ZC_ENTER_ROOM(this, character);

				var role = _members.Count == 0 ? ChatRoomRole.Owner : ChatRoomRole.Member;
				var member = new ChatMember(character.Handle, character.Name, role);

				_members.Add(member);

				character.ChatRoomId = this.Id;

				Send.ZC_MEMBER_NEWENTRY(this, character);
			}
		}

		/// <summary>
		/// Removes the given character from the chat, updating it
		/// according to the remaining members.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="reason"></param>
		public void RemoveMember(PlayerCharacter character, MemberExitReason reason)
		{
			lock (_syncLock)
			{
				var removeMember = _members.FirstOrDefault(a => a.Handle == character.Handle);
				if (removeMember == null)
					throw new ArgumentException($"Character '{character.Name} ({character.Handle})' is not a member of chat '{this.Title} ({this.Id})'.");

				_members.Remove(removeMember);
				character.ChatRoomId = 0;

				if (reason == MemberExitReason.Kicked)
					_banned.Add(removeMember.Name);

				Send.ZC_MEMBER_EXIT(this, character, MemberExitReason.Left);

				// Remove room if the last member left
				if (_members.Count == 0)
				{
					ZoneServer.Instance.World.ChatRooms.Remove(this);
					return;
				}

				// Switch owner if the owner left
				if (this.OwnerHandle == character.Handle)
				{
					var newOwner = _members[0];

					this.OwnerHandle = newOwner.Handle;

					_members.Remove(newOwner);
					_members.Insert(0, newOwner);

					newOwner.Role = ChatRoomRole.Owner;
					Send.ZC_ROLE_CHANGE(this, newOwner.Name, ChatRoomRole.Owner);
				}

				// Update the chat for all outside characters, for the
				// latest character count and to display it again above
				// the (new) owner's head for the leaving character.
				Send.ZC_ROOM_NEWENTRY(this);
			}
		}

		/// <summary>
		/// Removes all members from the chat.
		/// </summary>
		public void RemoveMembers()
		{
			lock (_syncLock)
			{
				foreach (var member in _members)
				{
					if (this.Map.TryGetPlayer(member.Handle, out var character))
					{
						character.ChatRoomId = 0;
						Send.ZC_MEMBER_EXIT(this, character, MemberExitReason.Left);
					}
				}

				_members.Clear();
			}
		}

		/// <summary>
		/// Changes the owner of the chat.
		/// </summary>
		/// <param name="newOwnerName"></param>
		public void ChangeOwner(string newOwnerName)
		{
			lock (_syncLock)
			{
				var newOwner = _members.FirstOrDefault(a => a.Name == newOwnerName);
				if (newOwner == null)
					throw new ArgumentException($"Character '{newOwnerName}' is not a member of chat '{this.Title} ({this.Id})'.");

				var formerOwner = _members.FirstOrDefault(a => a.Handle == this.OwnerHandle);

				this.OwnerHandle = newOwner.Handle;

				_members.Remove(newOwner);
				_members.Insert(0, newOwner);

				newOwner.Role = ChatRoomRole.Owner;
				Send.ZC_ROLE_CHANGE(this, newOwnerName, ChatRoomRole.Owner);

				if (formerOwner != null)
				{
					formerOwner.Role = ChatRoomRole.Member;
					Send.ZC_ROLE_CHANGE(this, formerOwner.Name, ChatRoomRole.Member);
				}
			}
		}
	}

	/// <summary>
	/// A member of a chat room.
	/// </summary>
	/// <param name="handle"></param>
	/// <param name="name"></param>
	/// <param name="role"></param>
	public class ChatMember(int handle, string name, ChatRoomRole role)
	{
		/// <summary>
		/// Returns the character's handle.
		/// </summary>
		public int Handle { get; } = handle;

		/// <summary>
		/// Returns the character's name.
		/// </summary>
		public string Name { get; } = name;

		/// <summary>
		/// Gets or sets the character's role in the chat.
		/// </summary>
		public ChatRoomRole Role { get; set; } = role;
	}
}
