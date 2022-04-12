using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Database.MySQL;
using Yggdrasil.Util;

namespace Sabine.Auth.Database
{
	/// <summary>
	/// Auth server database interface.
	/// </summary>
	public class AuthDb : Db
	{
		/// <summary>
		/// Creates new account.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="sex"></param>
		/// <param name="authority"></param>
		/// <returns></returns>
		public Account CreateAccount(string username, string password, Sex sex, int authority)
		{
			var account = new Account();

			account.Username = username;
			account.Password = password;
			account.Sex = sex;
			account.Authority = authority;

			using (var conn = this.GetConnection())
			using (var cmd = new InsertCommand("INSERT INTO `accounts` {0}", conn))
			{
				cmd.Set("username", account.Username);
				cmd.Set("password", account.Password);
				cmd.Set("sex", account.Sex);
				cmd.Set("authority", account.Authority);

				cmd.Execute();
				account.Id = (int)cmd.LastId;
			}

			return account;
		}

		/// <summary>
		/// Generates a random session id, assigns it to the account,
		/// and updates the database.
		/// </summary>
		/// <param name="account"></param>
		public void UpdateSessionId(ref Account account)
		{
			// If I'm understanding this right, the session id becomes
			// the "handle" for the player's character. It's regularly
			// checked on the client for various things, but it's only
			// set once, upon login on the auth server. That leads me
			// to be believe that we need to be careful with it, because
			// it needs to be unique across all users *and* NPCs, who use
			// handles from the same pool so to speak.
			// There is also a security concern, because a limited range
			// on the session id means less attempts necessary for brute-
			// forcing it. For now I'll just randomize it, but we need
			// a better solution.
			var sessionId = RandomProvider.Get().Next(0x1000_0000, 0x2000_0000);

			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `accounts` SET {0} WHERE `accountId` = @accountId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.Set("sessionId", sessionId);

				cmd.Execute();
			}

			account.SessionId = sessionId;
		}
	}
}
