using System;
using Sabine.Char.Database;
using Sabine.Char.Network;
using Sabine.Shared;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;

namespace Sabine.Char
{
	public class CharServer : Server
	{
		public static readonly CharServer Instance = new CharServer();

		private TcpConnectionAcceptor<CharConnection> _acceptor;

		public PacketHandler PacketHandler { get; } = new PacketHandler();
		public CharDb Database { get; } = new CharDb();

		public override void Run(string[] args)
		{
			ConsoleUtil.WriteHeader(nameof(Sabine), "Char", ConsoleColor.DarkCyan, ConsoleHeader.Title, ConsoleHeader.Subtitle);
			ConsoleUtil.LoadingTitle();

			this.NavigateToRoot();
			this.LoadConf();
			this.LoadLocalization(this.Conf);
			this.InitDatabase(this.Database, this.Conf);

			_acceptor = new TcpConnectionAcceptor<CharConnection>(this.Conf.Char.BindIp, this.Conf.Char.BindPort);
			_acceptor.ConnectionAccepted += this.OnConnectionAccepted;
			_acceptor.Listen();

			ConsoleUtil.RunningTitle();
			Log.Status("Server ready, listening on {0}.", _acceptor.Address);

			new ConsoleCommands().Wait();
		}

		private void OnConnectionAccepted(CharConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
