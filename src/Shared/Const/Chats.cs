using System.Collections.Generic;
using Sabine.Shared.Data.Databases;

namespace Sabine.Shared.Const
{
	/// <summary>
	/// Defines who can join a chat.
	/// </summary>
	public enum ChatPrivacy : byte
	{
		/// <summary>
		/// Chat requires a password to join.
		/// </summary>
		Private = 0,

		/// <summary>
		/// Chat is open to everyone.
		/// </summary>
		Public = 1,
	}

	/// <summary>
	/// Defines the result of a chat room  operation.
	/// </summary>
	public enum ChatRoomSuccess : byte
	{
		/// <summary>
		/// Operation was executed successfully.
		/// </summary>
		Success = 0,

		/// <summary>
		/// Operation failed.
		/// </summary>
		Fail = 1,
	}

	/// <summary>
	/// Defines the reason for a negative response to a chat room entry
	/// request.
	/// </summary>
	public enum ChatRoomRefuseReason : byte
	{
		/// <summary>
		/// The room has alrady reached its limit.
		/// </summary>
		Full = 0,

		/// <summary>
		/// The given password was incorrect.
		/// </summary>
		WrongPassword = 1,

		/// <summary>
		/// The player was kicked and banned from the chat.
		/// </summary>
		Banned = 2,
	}

	/// <summary>
	/// Defines the reason a chat member exited the chat.
	/// </summary>
	public enum MemberExitReason : byte
	{
		/// <summary>
		/// Member left the chat.
		/// </summary>
		Left = 0,

		/// <summary>
		/// Member was kicked.
		/// </summary>
		Kicked = 1,
	}

	/// <summary>
	/// Defines the role of a chat member.
	/// </summary>
	public enum ChatRoomRole : int
	{
		/// <summary>
		/// A normal member, indicated by a black name color.
		/// </summary>
		Member = 1,

		/// <summary>
		/// The owner of the chat, indicated by a turquoise name color.
		/// </summary>
		Owner = 0,
	}
}
