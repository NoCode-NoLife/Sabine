//--- Sabine Script ---------------------------------------------------------
// Go Chat Command
//--- Description -----------------------------------------------------------
// Demonstration of writing a chat command in a script. The go command
// warps players to one of several pre-defined locations, and since
// it's a script, anyone can easily modify the destinations.
//---------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Data;
using Sabine.Shared.Util;
using Sabine.Shared.World;
using Sabine.Zone.Scripting;
using Sabine.Zone.World.Entities;
using Yggdrasil.Logging;
using Yggdrasil.Util.Commands;
using static Sabine.Zone.Scripting.Shortcuts;

public class GoCommandScript : GeneralScript
{
	public override void Load()
	{
		this.RegisterDestination("prontera", "prt_vilg02", 99, 78);
		this.RegisterDestination("morocc", "moc_vilg01", 98, 95);

		AddChatCommand("go", "<destination>", Localization.Get("Warps to a specific destination."), this.Go, 50, 50);
	}

	//-----------------------------------------------------------------------

	/// <summary>
	/// Executed when the go command is used by a player. Warps them or the
	/// target to the selected destination.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="target"></param>
	/// <param name="message"></param>
	/// <param name="commandName"></param>
	/// <param name="args"></param>
	/// <returns></returns>
	private CommandResult Go(PlayerCharacter sender, PlayerCharacter target, string message, string commandName, Arguments args)
	{
		if (args.Count < 1)
		{
			sender.ServerMessage(Localization.Get("Destinations: {0}"), _destinationsStr ?? Localization.Get("None"));
			return CommandResult.Okay;
		}

		var destinationName = args.Get(0).ToLower();
		var potentialDestinations = _destinations.Values.Where(a => a.Name.ToLower().StartsWith(destinationName));
		var count = potentialDestinations.Count();

		if (count == 0)
		{
			sender.ServerMessage(Localization.Get("Destination not found."));
			return CommandResult.Okay;
		}
		else if (count >= 2)
		{
			var potentialDestinationsStr = string.Join(", ", potentialDestinations.Select(a => a.Name));
			sender.ServerMessage(Localization.Get("Multiple potential destinations found: {0}"), potentialDestinationsStr);

			return CommandResult.Okay;
		}
		else
		{
			var destination = potentialDestinations.First();
			var location = destination.Location;

			target.Warp(destination.Location);

			sender.ServerMessage(Localization.Get("Warped to {0} ({1}, {2}, {3})."), destination.Name, destination.MapStringId, location.X, location.Y);
			if (target != sender)
				target.ServerMessage(Localization.Get("You were warped to {0} by {1}."), destination.Name, sender.Name);
		}

		return CommandResult.Okay;
	}

	/// <summary>
	/// Registers a destination for the go command.
	/// </summary>
	/// <param name="name"></param>
	/// <param name="mapStringId"></param>
	/// <param name="x"></param>
	/// <param name="y"></param>
	private void RegisterDestination(string name, string mapStringId, int x, int y)
	{
		if (!SabineData.Maps.TryFind(mapStringId, out var mapData))
		{
			Log.Error("GoCommandScript: Destination '{0}' not found.", mapStringId);
			return;
		}

		_destinations[name] = new GoDestination(name, mapStringId, new Location(mapData.Id, x, y));
		_destinationsStr = string.Join(", ", _destinations.Keys);
	}

	/// <summary>
	/// A destination for the go command.
	/// </summary>
	private class GoDestination
	{
		/// <summary>
		/// Returns the name of the destination, which is used as its
		/// identification.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Returns the string id of the map this destination is at.
		/// </summary>
		public string MapStringId { get; }

		/// <summary>
		/// Returns the exact location players are warped to when this
		/// destination is used.
		/// </summary>
		public Location Location { get; }

		/// <summary>
		/// Creates new destination.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="mapStringId"></param>
		/// <param name="location"></param>
		public GoDestination(string name, string mapStringId, Location location)
		{
			this.Name = name;
			this.MapStringId = mapStringId;
			this.Location = location;
		}
	}

	private readonly Dictionary<string, GoDestination> _destinations = new Dictionary<string, GoDestination>();
	private string _destinationsStr;
}
