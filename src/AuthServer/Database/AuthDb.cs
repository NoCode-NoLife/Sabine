using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Sabine.Shared.Database.MySQL;
using Yggdrasil.Util;

namespace Sabine.Auth.Database
{
	public class AuthDb : Db
	{
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

		public void UpdateSessionId(Account account)
		{
			var sessionId = RandomProvider.Get().Next();

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
