using Yggdrasil.Configuration;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents char.conf.
	/// </summary>
	public class CharConf : ConfFile
	{
		public string BindIp { get; set; }
		public int BindPort { get; set; }
		public string ServerIp { get; set; }
		public string Name { get; set; }

		/// <summary>
		/// Loads the conf file and its options from the given path.
		/// </summary>
		public void Load(string filePath)
		{
			this.Require(filePath);

			this.BindIp = this.GetString("char_bind_ip", "0.0.0.0");
			this.BindPort = this.GetInt("char_bind_port", 7000);
			this.ServerIp = this.GetString("char_server_ip", "127.0.0.1");
			this.Name = this.GetString("char_server_name", "Sabine");
		}
	}
}
