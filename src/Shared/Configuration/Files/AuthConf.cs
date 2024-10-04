using System;
using Yggdrasil.Configuration;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents auth.conf.
	/// </summary>
	public class AuthConf : ConfFile
	{
		public string BindIp { get; set; }
		public int[] BindPorts { get; set; }
		public bool AllowAccountCreation { get; set; }

		/// <summary>
		/// Loads the conf file and its options from the given path.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			this.Require(filePath);

			this.BindIp = this.GetString("auth_bind_ip", "0.0.0.0");
			this.BindPorts = this.GetBindPorts("auth_bind_ports", [6900, 7000]);
			this.AllowAccountCreation = this.GetBool("allow_account_creation", true);
		}

		private int[] GetBindPorts(string option, int[] defaultValue)
		{
			var bindPorts = this.GetString(option, null);
			if (bindPorts == null)
				return defaultValue;

			var split = bindPorts.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
			if (split.Length == 0)
				return defaultValue;

			var result = new int[split.Length];

			for (var i = 0; i < split.Length; ++i)
			{
				var portStr = split[i];

				if (!int.TryParse(portStr, out var port))
					throw new FormatException($"Invalid port: {portStr}");

				result[i] = port;
			}

			return result;
		}
	}
}
