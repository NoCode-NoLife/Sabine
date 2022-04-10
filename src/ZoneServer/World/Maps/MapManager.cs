using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// Collection of maps.
	/// </summary>
	public class MapManager
	{
		private readonly Dictionary<string, Map> _maps = new Dictionary<string, Map>();

		/// <summary>
		/// Adds map to the collection.
		/// </summary>
		/// <param name="map"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Add(Map map)
		{
			lock (_maps)
			{
				if (_maps.ContainsKey(map.Name))
					throw new ArgumentException($"A map with the name '{map.Name}' already exists.");

				_maps[map.Name] = map;
			}
		}

		/// <summary>
		/// Removes map from the collection.
		/// </summary>
		/// <param name="map"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Remove(Map map)
		{
			lock (_maps)
			{
				if (!_maps.ContainsKey(map.Name))
					throw new ArgumentException($"A character with the id '{map.Name}' doesn't exists.");

				_maps.Remove(map.Name);
			}
		}

		/// <summary>
		/// Returns the map with the given name, or null if it wasn't found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Map Get(string name)
		{
			lock (_maps)
			{
				_maps.TryGetValue(name, out var map);
				return map;
			}
		}

		/// <summary>
		/// Returns a list of all maps.
		/// </summary>
		/// <returns></returns>
		public Map[] GetAll()
		{
			lock (_maps)
				return _maps.Values.ToArray();
		}
	}
}
