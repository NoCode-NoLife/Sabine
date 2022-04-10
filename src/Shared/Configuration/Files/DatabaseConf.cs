using Yggdrasil.Configuration;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents char.conf.
	/// </summary>
	public class DatabaseConf : ConfFile
	{
		public string Host { get; set; }
		public int Port { get; set; }
		public string Username { get; set; }
		public string Password { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Loads the conf file and its options from the given path.
		/// </summary>
		public void Load(string filePath)
		{
			this.Require(filePath);

			this.Host = this.GetString("db_host", "127.0.0.1");
			this.Port = this.GetInt("db_port", 3306);
			this.Username = this.GetString("db_user", "root");
			this.Password = this.GetString("db_pass", "");
			this.Name = this.GetString("db_name", "sabine");
		}
	}
}
