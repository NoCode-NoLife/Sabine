using System;
using MySqlConnector;
using Sabine.Shared.Const;
using Sabine.Shared.Database;
using Yggdrasil.Db.MySql.SimpleCommands;
using Yggdrasil.Logging;
using Yggdrasil.Util;

namespace Sabine.Auth.Database
{
	/// <summary>
	/// Auth server database interface.
	/// </summary>
	public class AuthDb : Db
	{
		/// <summary>
		/// Returns true if the update with the given name was already applied.
		/// </summary>
		/// <param name="updateName"></param>
		/// <returns></returns>
		public bool CheckUpdate(string updateName)
		{
			using (var conn = this.GetConnection())
			{
				// Let the check to through if updates doesn't exist yet,
				// since it was added with the first update.
				using (var cmd = new MySqlCommand("SHOW TABLES LIKE 'updates'", conn))
				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.HasRows)
						return false;
				}

				using (var cmd = new MySqlCommand("SELECT * FROM `updates` WHERE `path` = @path", conn))
				{
					cmd.Parameters.AddWithValue("@path", updateName);

					using (var reader = cmd.ExecuteReader())
						return reader.Read();
				}
			}
		}

		/// <summary>
		/// Executes SQL update.
		/// </summary>
		/// <param name="updateName"></param>
		/// <param name="query"></param>
		public void RunUpdate(string updateName, string query)
		{
			try
			{
				using (var conn = this.GetConnection())
				{
					// Run update
					using (var cmd = new MySqlCommand(query, conn))
						cmd.ExecuteNonQuery();

					// Log update
					using (var cmd = new InsertCommand("INSERT INTO `updates` {parameters}", conn))
					{
						cmd.Set("path", updateName);
						cmd.Execute();
					}

					Log.Info("Successfully applied '{0}'.", updateName);
				}
			}
			catch (Exception ex)
			{
				Log.Error("RunUpdate: Failed to run '{0}': {1}", updateName, ex.Message);
				ConsoleUtil.Exit(1);
			}
		}

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
			using (var cmd = new InsertCommand("INSERT INTO `accounts` {parameters}", conn))
			{
				cmd.Set("username", account.Username);
				cmd.Set("password", account.Password);
				cmd.Set("sex", account.Sex);
				cmd.Set("authority", account.Authority);
				cmd.Set("sessionId", 0);

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
			var sessionId = RandomProvider.Get().Next();

			using (var conn = this.GetConnection())
			using (var cmd = new UpdateCommand("UPDATE `accounts` SET {parameters} WHERE `accountId` = @accountId", conn))
			{
				cmd.AddParameter("@accountId", account.Id);
				cmd.Set("sessionId", sessionId);

				cmd.Execute();
			}

			account.SessionId = sessionId;
		}
	}
}
