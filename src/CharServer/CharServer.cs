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
	/// <summary>
	/// Represents a char server.
	/// </summary>
	public class CharServer : Server
	{
		/// <summary>
		/// Global singleton for the server.
		/// </summary>
		public static readonly CharServer Instance = new CharServer();

		private TcpConnectionAcceptor<CharConnection> _acceptor;

		/// <summary>
		/// Returns a reference to the server's packet handlers.
		/// </summary>
		public PacketHandler PacketHandler { get; } = new PacketHandler();

		/// <summary>
		/// Returns a reference to the server's database interface.
		/// </summary>
		public CharDb Database { get; } = new CharDb();

		/// <summary>
		/// Starts server.
		/// </summary>
		/// <param name="args"></param>
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

		/// <summary>
		/// Called when a new client connected to the server.
		/// </summary>
		/// <param name="conn"></param>
		private void OnConnectionAccepted(CharConnection conn)
		{
			Log.Info("New connection accepted from '{0}'.", conn.Address);
		}
	}
}
