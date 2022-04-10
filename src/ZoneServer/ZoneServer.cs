using System;
using Sabine.Shared;
using Sabine.Zone.Commands;
using Sabine.Zone.Database;
using Sabine.Zone.Network;
using Sabine.Zone.World;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;

namespace Sabine.Zone
{
	public class ZoneServer : Server
	{
		public static readonly ZoneServer Instance = new ZoneServer();

		private TcpConnectionAcceptor<ZoneConnection> _acceptor;

		public PacketHandler PacketHandler { get; } = new PacketHandler();

		public WorldManager World { get; } = new WorldManager();

		public ChatCommands ChatCommands { get; } = new ChatCommands();

		public ZoneDb Database { get; } = new ZoneDb();

		public override void Run(string[] args)
		{
			ConsoleUtil.WriteHeader(nameof(Sabine), "Zone", ConsoleColor.DarkGreen, ConsoleHeader.Title, ConsoleHeader.Subtitle);
			ConsoleUtil.LoadingTitle();

			this.NavigateToRoot();
			this.LoadConf();
			this.LoadLocalization(this.Conf);
			this.InitDatabase(this.Database, this.Conf);
			this.LoadCommands();

			_acceptor = new TcpConnectionAcceptor<ZoneConnection>(this.Conf.Zone.BindIp, this.Conf.Zone.BindPort);
			_acceptor.ConnectionAccepted += this.OnConnectionAccepted;
			_acceptor.Listen();

			ConsoleUtil.RunningTitle();
			Log.Status("Server ready, listening on {0}.", _acceptor.Address);

			new ConsoleCommands().Wait();
		}

		private void LoadCommands()
		{
			Log.Info("Loading commands...");

			this.ChatCommands.Load();
		}

		private void OnConnectionAccepted(ZoneConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
