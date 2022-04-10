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
	/// <summary>
	/// Represents the auth server.
	/// </summary>
	public class AuthServer : Server
	{
		/// <summary>
		/// Global singleton for the auth server.
		/// </summary>
		public static readonly AuthServer Instance = new AuthServer();

		private TcpConnectionAcceptor<AuthConnection> _acceptor;

		/// <summary>
		/// Returns a reference to the server's packet handlers.
		/// </summary>
		public PacketHandler PacketHandler { get; } = new PacketHandler();

		/// <summary>
		/// Returns reference to the server's database interface.
		/// </summary>
		public AuthDb Database { get; } = new AuthDb();

		/// <summary>
		/// Starts the server.
		/// </summary>
		/// <param name="args"></param>
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

		/// <summary>
		/// Called when a new connection to the server was established
		/// by a client.
		/// </summary>
		/// <param name="conn"></param>
		private void OnConnectionAccepted(AuthConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
