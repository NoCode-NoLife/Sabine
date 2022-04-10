using System;
using System.Collections.Generic;
using System.Linq;
using Yggdrasil.Configuration;
using Yggdrasil.Logging;

namespace Sabine.Shared.Configuration.Files
{
	/// <summary>
	/// Represents commands.conf.
	/// </summary>
	public class CommandsConf : ConfFile
	{
		public string Prefix { get; protected set; }
		public Dictionary<string, AuthLevels> Levels { get; } = new Dictionary<string, AuthLevels>();

		/// <summary>
		/// Loads conf file and its options from the given file.
		/// </summary>
		/// <param name="filePath"></param>
		public void Load(string filePath)
		{
			this.Include(filePath);

			this.Prefix = this.GetString("prefix", ">").Substring(0, 1);

			foreach (var option in _options.Where(a => a.Key != "prefix"))
			{
				var commandName = option.Key;

				var split = option.Value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (split.Length != 2 || !int.TryParse(split[0], out var auth) || !int.TryParse(split[0], out var characterAuth))
				{
					Log.Error("AuthConf: Invalid auth setting '{0}: {1}'.", option.Key, option.Value);
					continue;
				}

				this.Levels[commandName] = new AuthLevels() { Self = auth, Target = characterAuth };
			}
		}

		/// <summary>
		/// Returns the auth levels for the given command.
		/// </summary>
		/// <param name="command"></param>
		/// <returns></returns>
		public AuthLevels GetLevels(string command)
		{
			if (!this.Levels.TryGetValue(command, out var levels))
				return null;

			return levels;
		}
	}

	public class AuthLevels
	{
		public int Self { get; set; }
		public int Target { get; set; }
	}
}
