using System;
using Sabine.Auth.Database;
using Sabine.Auth.Network;
using Sabine.Shared;
using Yggdrasil.Logging;
using Yggdrasil.Network.TCP;
using Yggdrasil.Util;
using Yggdrasil.Util.Commands;

namespace Sabine.Auth
{
	public class AuthServer : Server
	{
		public static readonly AuthServer Instance = new AuthServer();

		private TcpConnectionAcceptor<AuthConnection> _acceptor;

		public PacketHandler PacketHandler { get; } = new PacketHandler();
		public AuthDb Database { get; } = new AuthDb();

		public override void Run(string[] args)
		{
			ConsoleUtil.WriteHeader(nameof(Sabine), "Auth", ConsoleColor.DarkYellow, ConsoleHeader.Title, ConsoleHeader.Subtitle);
			ConsoleUtil.LoadingTitle();

			this.NavigateToRoot();
			this.LoadConf();
			this.LoadLocalization(this.Conf);
			this.InitDatabase(this.Database, this.Conf);

			_acceptor = new TcpConnectionAcceptor<AuthConnection>(this.Conf.Auth.BindIp, this.Conf.Auth.BindPort);
			_acceptor.ConnectionAccepted += this.OnConnectionAccepted;
			_acceptor.Listen();

			ConsoleUtil.RunningTitle();
			Log.Status("Server ready, listening on {0}.", _acceptor.Address);

			new ConsoleCommands().Wait();
		}

		private void OnConnectionAccepted(AuthConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
