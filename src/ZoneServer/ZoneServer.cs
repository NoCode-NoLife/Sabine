using System;
using System.IO;
using System.Linq;
using Sabine.Shared;
using Sabine.Shared.Network;
using Sabine.Zone.Commands;
using Sabine.Zone.Database;
using Sabine.Zone.Network;
using Sabine.Zone.World;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;

namespace Sabine.Zone
{
	/// <summary>
	/// Represents a zone server.
	/// </summary>
	public class ZoneServer : Server
	{
		/// <summary>
		/// Global singleton for the server.
		/// </summary>
		public static readonly ZoneServer Instance = new ZoneServer();

		private TcpConnectionAcceptor<ZoneConnection> _acceptor;

		/// <summary>
		/// Returns a reference to the server's packet handlers.
		/// </summary>
		public PacketHandler PacketHandler { get; } = new PacketHandler();

		/// <summary>
		/// Returns a reference to the world on this server.
		/// </summary>
		public WorldManager World { get; } = new WorldManager();

		/// <summary>
		/// Returns a reference to the server's chat command handlers.
		/// </summary>
		public ChatCommands ChatCommands { get; } = new ChatCommands();

		/// <summary>
		/// Returns a reference to the server's database interface.
		/// </summary>
		public ZoneDb Database { get; } = new ZoneDb();

		/// <summary>
		/// Runs the server.
		/// </summary>
		/// <param name="args"></param>
		public override void Run(string[] args)
		{
			ConsoleUtil.WriteHeader(nameof(Sabine), "Zone", ConsoleColor.DarkGreen, ConsoleHeader.Title, ConsoleHeader.Subtitle);
			ConsoleUtil.LoadingTitle();

			this.NavigateToRoot();
			this.LoadConf();
			this.LoadLocalization(this.Conf);
			this.LoadData();
			this.InitDatabase(this.Database, this.Conf);
			this.LoadCommands();
			this.LoadWorld();
			this.LoadScripts("system/scripts/scripts_zone.txt", this.Conf);
			this.InitialSpawn();
			this.CreateDebugInfo();

			this.World.Heartbeat.Start();
			this.World.Scheduler.Start();

			_acceptor = new TcpConnectionAcceptor<ZoneConnection>(this.Conf.Zone.BindIp, this.Conf.Zone.BindPort);
			_acceptor.ConnectionAccepted += this.OnConnectionAccepted;
			_acceptor.Listen();

			ConsoleUtil.RunningTitle();
			Log.Status("Server ready, listening on {0}.", _acceptor.Address);

			new ConsoleCommands().Wait();
		}

		/// <summary>
		/// Executes initial spawn.
		/// </summary>
		private void InitialSpawn()
		{
			Log.Info("Populating world with monsters...");
			this.World.Spawners.InitialSpawn();
		}

		/// <summary>
		/// Loads world and its maps.
		/// </summary>
		private void LoadWorld()
		{
			Log.Info("Loading world...");

			this.World.Load();

			Log.Info("  loaded {0} maps.", this.World.Maps.Count);
		}

		/// <summary>
		/// Loads commands.
		/// </summary>
		private void LoadCommands()
		{
			Log.Info("Loading commands...");

			this.ChatCommands.Load();
		}

		/// <summary>
		/// Creates potential debug information in the user folder.
		/// </summary>
		private void CreateDebugInfo()
		{
			var packetTable = PacketTable.GetTable();
			var filePath = "user/debug/packet_table_" + Game.Version + ".txt";
			var folderPath = Path.GetDirectoryName(filePath);

			if (!Directory.Exists(folderPath))
				Directory.CreateDirectory(folderPath);

			using (var fs = new FileStream(filePath, FileMode.Create))
			using (var sw = new StreamWriter(fs))
			{
				var longestName = packetTable.Max(a => a.Op.ToString().Length);
				var col1 = Math.Max(42, longestName + 2);
				var col2 = 8;
				var col3 = 5;

				sw.WriteLine("{0,-" + col1 + "} | {1,-" + col2 + "} | {2}", "Name", "Op", "Size");
				sw.WriteLine("".PadRight(col1 + col2 + col3 + 3 * 2 + 2, '-'));

				foreach (var entry in packetTable)
					sw.WriteLine("{0,-" + col1 + "} | 0x{1,-" + (col2 - 2) + ":X4} | {2}", entry.Op, entry.OpNetwork, entry.Size);
			}

			using (var fs = new FileStream(Path.ChangeExtension(filePath, ".cs"), FileMode.Create))
			using (var sw = new StreamWriter(fs))
			{
				foreach (var entry in packetTable)
					sw.WriteLine("Register(Op.{0}, 0x{1:X4}, {2});", entry.Op, entry.OpNetwork, entry.Size);
			}
		}

		/// <summary>
		/// Called when a new client connected to the server.
		/// </summary>
		/// <param name="conn"></param>
		private void OnConnectionAccepted(ZoneConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
