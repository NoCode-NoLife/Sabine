using System;
using MySql.Data.MySqlClient;
using Sabine.Shared.Const;
using Sabine.Shared.Database.MySQL;

namespace Sabine.Shared.Database
{
	/// <summary>
	/// Base class for MySQL database interfaces.
	/// </summary>
	public abstract class Db
	{
		private string _connectionString;

		/// <summary>
		/// Returns a valid connection.
		/// </summary>
		public MySqlConnection GetConnection()
		{
			if (_connectionString == null)
				throw new Exception("Database has not been initialized.");

			var result = new MySqlConnection(_connectionString);
			result.Open();
			return result;
		}

		/// <summary>
		/// Sets connection string and calls TestConnection.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="port"></param>
		/// <param name="user"></param>
		/// <param name="pass"></param>
		/// <param name="db"></param>
		public void Init(string host, int port, string user, string pass, string db)
		{
			_connectionString = string.Format("server={0}; port={1}; database={2}; uid={3}; password={4}; pooling=true; min pool size=0; max pool size=100; ConvertZeroDateTime=true", host, port, db, user, pass);
			this.TestConnection();
		}

		/// <summary>
		/// Tests connection, throws on error.
		/// </summary>
		public void TestConnection()
		{
			MySqlConnection conn = null;
			try
			{
				conn = this.GetConnection();
			}
			finally
			{
				if (conn != null)
					conn.Close();
			}
		}

		/// <summary>
		/// Returns true if an account with the given username exists.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public bool UsernameExists(string username)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `username` = @username", conn))
			{
				cmd.AddParameter("@username", username);

				using (var reader = cmd.ExecuteReader())
					return reader.HasRows;
			}
		}

		/// <summary>
		/// Returns an account by its username, or null if no account was
		/// found.
		/// </summary>
		/// <param name="username"></param>
		/// <returns></returns>
		public Account GetAccountByUsername(string username)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `username` = @username", conn))
			{
				cmd.AddParameter("@username", username);

				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					return this.ReadAccount(reader);
				}
			}
		}

		/// <summary>
		/// Returns an account by its account id, or null if no account was
		/// found.
		/// </summary>
		/// <param name="accountId"></param>
		/// <returns></returns>
		public Account GetAccountById(int accountId)
		{
			using (var conn = this.GetConnection())
			using (var cmd = new MySqlCommand("SELECT * FROM `accounts` WHERE `accountId` = @accountId", conn))
			{
				cmd.AddParameter("@accountId", accountId);

				using (var reader = cmd.ExecuteReader())
				{
					if (!reader.Read())
						return null;

					return this.ReadAccount(reader);
				}
			}
		}

		/// <summary>
		/// Reads account from reader and returns it.
		/// </summary>
		/// <param name="reader"></param>
		/// <returns></returns>
		private Account ReadAccount(MySqlDataReader reader)
		{
			var account = new Account();

			account.Id = reader.GetInt32("accountId");
			account.Username = reader.GetStringSafe("username");
			account.Password = reader.GetStringSafe("password");
			account.Sex = (Sex)reader.GetByte("sex");
			account.Authority = reader.GetByte("authority");
			account.SessionId = reader.GetInt32("sessionId");

			return account;
		}
	}
}
