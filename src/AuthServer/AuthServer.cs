using System;
using System.Collections.Generic;
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

		private readonly List<TcpConnectionAcceptor<AuthConnection>> _acceptors = new List<TcpConnectionAcceptor<AuthConnection>>();

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

			foreach (var port in this.Conf.Auth.BindPorts)
			{
				var acceptor = new TcpConnectionAcceptor<AuthConnection>(this.Conf.Auth.BindIp, port);
				acceptor.ConnectionAccepted += this.OnConnectionAccepted;
				acceptor.Listen();

				_acceptors.Add(acceptor);

				Log.Status("Server ready, listening on {0}.", acceptor.Address);
			}

			ConsoleUtil.RunningTitle();
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
