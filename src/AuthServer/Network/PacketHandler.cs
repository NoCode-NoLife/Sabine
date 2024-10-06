using System.Globalization;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Network;
using Yggdrasil.Logging;
using Yggdrasil.Security.Hashing;
using Yggdrasil.Util;

namespace Sabine.Auth.Network
{
	/// <summary>
	/// Packet handler methods.
	/// </summary>
	public class PacketHandler : PacketHandler<AuthConnection>
	{
		/// <summary>
		/// Login request, first packet sent after connecting.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CA_LOGIN, Op.CA_LOGIN2)]
		public void CA_LOGIN(AuthConnection conn, Packet packet)
		{
			int version = 0, langType = 1;
			string password, hashedPassword;

			if (Game.Version >= Versions.Beta1)
				version = packet.GetInt();

			var username = packet.GetString(Sizes.Usernames);

			// For the standard login we read the password and then hash
			// it with MD5, to arrive at the same input we get from clients
			// that have passwordencrypt(2) enabled. This way, it doesn't
			// matter what the client sends, and we can safely upgrade it
			// to BCrypt for storing it. For more information about the
			// hashing, see the CA_REQ_HASH handler.
			if (packet.Op == Op.CA_LOGIN)
			{
				password = packet.GetString(Sizes.Usernames);
				hashedPassword = MD5.Encode(password);
			}
			else
			{
				var hashBytes = packet.GetBytes(16);
				password = hashedPassword = Hex.ToString(hashBytes, HexStringOptions.None);
			}

			if (Game.Version >= Versions.Beta2)
				langType = packet.GetByte();

			var db = AuthServer.Instance.Database;

			// If account doesn't exist, try to create it
			var account = db.GetAccountByUsername(username);
			if (account == null)
			{
				var creationAllowed = AuthServer.Instance.Conf.Auth.AllowAccountCreation;
				var hasSuffix = (username.EndsWith("_M", true, CultureInfo.InvariantCulture) || username.EndsWith("_F", true, CultureInfo.InvariantCulture));

				if (!creationAllowed || !hasSuffix)
				{
					Send.AC_REFUSE_LOGIN(conn, LoginConnectError.UserNotFound);
					conn.Close(2);
					return;
				}

				var sex = username.EndsWith("_M", true, CultureInfo.InvariantCulture) ? Sex.Male : Sex.Female;
				username = username.Substring(0, username.Length - 2);

				if (db.UsernameExists(username))
				{
					Send.AC_REFUSE_LOGIN(conn, LoginConnectError.UserNotFound);
					conn.Close(2);
					return;
				}

				var bcryptedPassword = BCrypt.HashPassword(hashedPassword, BCrypt.GenerateSalt());
				account = db.CreateAccount(username, bcryptedPassword, sex, 0);

				Log.Info("New account {0} was created.", username);
			}

			var passwordCorrect = BCrypt.CheckPassword(hashedPassword, account.Password);
			if (!passwordCorrect)
			{

				Send.AC_REFUSE_LOGIN(conn, LoginConnectError.PasswordIncorrect);
				conn.Close(2);
				return;
			}

			db.UpdateSessionId(ref account);

			Send.AC_ACCEPT_LOGIN(conn, account);

			Log.Info("User '{0}' logged in.", username);
		}

		/// <summary>
		/// Request from the client for a salt key to hash passwords with.
		/// </summary>
		/// <param name="conn"></param>
		/// <param name="packet"></param>
		[PacketHandler(Op.CA_REQ_HASH)]
		public void CA_REQ_HASH(AuthConnection conn, Packet packet)
		{
			// The client supports two ways to hash passwords, passwordencrypt
			// (md5(salt+pw)) and passwordencrypt2 (md5(pw+salt)). Both of
			// these secure the password on its way to the server, but they
			// both have a major flaw. To check the password on the server
			// side, we'd have to store the passwords in plain text, which
			// we'll most definitely not do. Supporting this login type
			// would be nice though, since it wouldn't matter how you
			// configured your client then, removing a potential point
			// of failure.
			// 
			// The solution is pretty simple:
			// Send an empty salt for the client to use, so it hashes the
			// password alone, without salt. Technically, this makes it
			// a little less safe, but it makes what we get from the client
			// predictable. No matter whether you put the empty salt before
			// the password or after it, we'll always get an MD5 hash of the
			// password.
			//
			// Sidenote about safety: MD5 has been considered broken for
			// many years anyway, and whether you use a salt or not, at
			// the end of the day, it's just an MD5 hash. We should be
			// more concerned about storing the passwords safely, than
			// getting them to the server in salted form.

			Send.AC_ACK_HASH(conn, "");
		}
	}
}
