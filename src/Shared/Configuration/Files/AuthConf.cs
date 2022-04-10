using Yggdrasil.Configuration;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents auth.conf.
	/// </summary>
	public class AuthConf : ConfFile
	{
		public string BindIp { get; set; }
		public int BindPort { get; set; }

		/// <summary>
		/// Loads the conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			this.Require(filePath);

			this.BindIp = this.GetString("auth_bind_ip", "0.0.0.0");
			this.BindPort = this.GetInt("auth_bind_port", 7000);
		}
	}
}
