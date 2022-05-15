using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Sabine.Zone.Ais
{
	/// <summary>
	/// Loads and creates AIs.
	/// </summary>
	public class AiManager
	{
		private readonly Dictionary<string, Type> _ais = new Dictionary<string, Type>();

		/// <summary>
		/// Returns the number of AIs that are available.
		/// </summary>
		public int Count { get { lock (_ais) return _ais.Count; } }

		/// <summary>
		/// Loads all AIs defined in the assembly.
		/// </summary>
		public void LoadAiTypes()
		{
			foreach (var type in Assembly.GetCallingAssembly().GetTypes().Where(a => a.IsSubclassOf(typeof(MonsterAi))))
			{
				var attr = type.GetCustomAttribute<AiAttribute>();
				if (attr == null)
					continue;

				lock (_ais)
				{
					foreach (var name in attr.Names)
						_ais[name] = type;
				}
			}
		}

		/// <summary>
		/// Returns true if an AI with the given name exists.
		/// </summary>
		/// <param name="aiName"></param>
		/// <returns></returns>
		public bool Exists(string aiName)
		{
			lock (_ais)
				return _ais.ContainsKey(aiName);
		}

		/// <summary>
		/// Adds the given AI to the available ones.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		public void AddAi(string name, Type type)
		{
			lock (_ais)
				_ais[name] = type;
		}

		/// <summary>
		/// Creates the AI with the given name and returns it. Returns null
		/// if it doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public MonsterAi CreateAi(string name)
		{
			Type type;

			lock (_ais)
			{
				if (!_ais.TryGetValue(name, out type))
					return null;
			}

			return (MonsterAi)Activator.CreateInstance(type);
		}

		/// <summary>
		/// Creates the AI with the given name and returns it via out,
		/// returns false if the AI doesn't exist.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="ai"></param>
		/// <returns></returns>
		public bool TryCreateAi(string name, out MonsterAi ai)
		{
			ai = this.CreateAi(name);
			return ai != null;
		}
	}
}
