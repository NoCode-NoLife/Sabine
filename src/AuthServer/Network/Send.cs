using System.Net;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Network;

namespace Sabine.Auth.Network
{
	/// <summary>
	/// Packet senders.
	/// </summary>
	public static class Send
	{
		/// <summary>
		/// Shows error message on the client about why the login request
		/// was refused.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="errorCode"></param>
		/// <param name="errorMessage">Error message to display for certain errors (not support by all versions).</param>
		public static void AC_REFUSE_LOGIN(AuthConnection conn, LoginConnectError errorCode, string errorMessage = null)
		{
			var packet = new Packet(Op.AC_REFUSE_LOGIN);
			packet.PutByte((byte)errorCode);

			if (Game.Version >= Versions.Beta2)
				packet.PutString(errorMessage, 20);

			conn.Send(packet);
		}

		/// <summary>
		/// Accepts login request, making the client display a list of
		/// available char servers.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="account"></param>
		public static void AC_ACCEPT_LOGIN(AuthConnection conn, Account account)
		{
			var packet = new Packet(Op.AC_ACCEPT_LOGIN);

			if (Game.Version < Versions.Beta1)
			{
				packet.PutInt(account.Id);
				packet.PutByte((byte)account.Sex);
				packet.PutInt(account.SessionId);
			}
			else
			{
				packet.PutInt(account.Id);
				packet.PutInt(account.SessionId);
				packet.PutInt(account.SessionId);

				if (Game.Version >= Versions.Beta2)
				{
					packet.PutInt(0); // IP?
					packet.PutString("", 26); // Last Server?
				}

				packet.PutByte((byte)account.Sex);
			}

			// for server in servers
			{
				var charIp = AuthServer.Instance.Conf.Char.ServerIp;
				var charPort = AuthServer.Instance.Conf.Char.BindPort;
				var charName = AuthServer.Instance.Conf.Char.Name;

				packet.PutInt(IPAddress.Parse(charIp));
				packet.PutShort((short)charPort);
				packet.PutString(charName + " ", 20);

				if (Game.Version >= Versions.Beta2)
				{
					packet.PutShort(0); // PlayerCount
					packet.PutShort(0); // IsMaintenance
					packet.PutShort(0); // IsNew
				}
			}

			conn.Send(packet);
		}

		/// <summary>
		/// Sends a salt to hash passwords with to the client.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="salt"></param>
		public static void AC_ACK_HASH(AuthConnection conn, string salt)
		{
			var packet = new Packet(Op.AC_ACK_HASH);

			// Make sure to not null-terminate the string, as the client
			// uses any potential null-terminators as part of the hash,
			// which can lead to unexpected hashing results. Especially
			// if you want to send an empty salt. "\0" != "".
			packet.PutString(salt, false);

			conn.Send(packet);
		}
	}
}
