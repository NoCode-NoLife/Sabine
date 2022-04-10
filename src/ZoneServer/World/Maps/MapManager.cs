using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabine.Zone.World.Maps
{
	public class MapManager
	{
		private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();

		public void Add(Map map)
		{
			lock (_maps)
			{
				if (_maps.ContainsKey(map.Name))
					throw new ArgumentException($"A map with the name '{map.Name}' already exists.");

				_maps[map.Name] = map;
			}
		}

		public void Remove(Map map)
		{
			lock (_maps)
			{
				if (!_maps.ContainsKey(map.Name))
					throw new ArgumentException($"A character with the id '{map.Name}' doesn't exists.");

				_maps.Remove(map.Name);
			}
		}

		public Map Get(string name)
		{
			lock (_maps)
			{
				_maps.TryGetValue(name, out var map);
				return map;
			}
		}

		public Map[] GetAll()
		{
			lock (_maps)
				return _maps.Values.ToArray();
		}
	}
}
