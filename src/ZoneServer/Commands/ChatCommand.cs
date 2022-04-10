using Sabine.Zone.World.Entities;
using Yggdrasil.Util.Commands;

namespace Sabine.Zone.Commands
{
	/// <summary>
	/// Represents a chat command.
	/// </summary>
	public class ChatCommand : Command<CommandFunc>
	{
		/// <summary>
		/// Creates new instance.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="usage"></param>
		/// <param name="description"></param>
		/// <param name="func"></param>
		public ChatCommand(string name, string usage, string description, CommandFunc func)
			: base(name, usage, description, func)
		{
		}
	}

	/// <summary>
	/// The handler function for a chat command.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="target"></param>
	/// <param name="message"></param>
	/// <param name="commandName"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	public delegate CommandResult CommandFunc(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args);
}
