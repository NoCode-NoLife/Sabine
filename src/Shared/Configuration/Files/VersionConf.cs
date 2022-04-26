using Yggdrasil.Configuration;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents version.conf.
	/// </summary>
	public class VersionConf : ConfFile
	{
		public int PacketVersion { get; set; }

		/// <summary>
		/// Loads the conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			this.Require(filePath);

			this.PacketVersion = this.GetInt("packet_version", 0);
		}
	}
}
