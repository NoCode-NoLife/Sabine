using Sabine.Shared.Data;
using Sabine.Shared.World;
using Yggdrasil.Configuration;
using Yggdrasil.Logging;

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
		public string StartMapStringId { get; set; }
		public Position StartPosition { get; set; }

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

			this.ReadStartLocation("player_start_location", "prt_vilg02,99,81");
		}

		/// <summary>
		/// Reads the given option in the format "map,x,y" and sets the
		/// respective options.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="defaultValue"></param>
		/// <returns></returns>
		private void ReadStartLocation(string name, string defaultValue)
		{
			var startLocationStr = this.GetString(name, defaultValue);
			var split = startLocationStr.Split(',');
			if (split.Length != 3)
			{
				Log.Error("CharConf.Load: Invalid start location '{0}'.", startLocationStr);
			}
			else if (!int.TryParse(split[1], out var x) || !int.TryParse(split[2], out var y))
			{
				Log.Error("CharConf.Load: Invalid coordinates in start location '{0}'.", startLocationStr);
			}
			else
			{
				this.StartMapStringId = split[0];
				this.StartPosition = new Position(x, y);
				return;
			}

			// Fallback
			this.StartMapStringId = "prt_vilg02";
			this.StartPosition = new Position(99, 81);
		}
	}
}
