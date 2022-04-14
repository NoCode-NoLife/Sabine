using System;
using System.Collections.Generic;
using System.Linq;

namespace Sabine.Zone.World.Maps
{
	/// <summary>
	/// Collection of maps.
	/// </summary>
	public class MapManager : IUpdateable
	{
		private readonly object _syncLock = new object();

		private readonly Dictionary<int, Map> _maps = new Dictionary<int, Map>();
		private readonly List<Map> _mapsList = new List<Map>();

		/// <summary>
		/// Returns the numer of maps in this collection.
		/// </summary>
		public int Count
		{
			get
			{
				lock (_syncLock)
					return _maps.Count;
			}
		}

		/// <summary>
		/// Adds map to the collection.
		/// </summary>
		/// <param name="map"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Add(Map map)
		{
			lock (_syncLock)
			{
				if (_maps.ContainsKey(map.Id))
					throw new ArgumentException($"A map with the id '{map.Id}' already exists.");

				_maps[map.Id] = map;
				_mapsList.Add(map);
			}
		}

		/// <summary>
		/// Removes map from the collection.
		/// </summary>
		/// <param name="map"></param>
		/// <exception cref="ArgumentException"></exception>
		public void Remove(Map map)
		{
			lock (_syncLock)
			{
				if (!_maps.ContainsKey(map.Id))
					throw new ArgumentException($"A map with the id '{map.Id}' doesn't exists.");

				_maps.Remove(map.Id);
				_mapsList.Remove(map);
			}
		}

		/// <summary>
		/// Returns the map with the given id, or null if it wasn't found.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Map Get(int id)
		{
			lock (_syncLock)
			{
				_maps.TryGetValue(id, out var map);
				return map;
			}
		}

		/// <summary>
		/// Returns the map with the given id via out. Returns false
		/// if the map wasn't found.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public bool TryGet(int id, out Map map)
		{
			map = this.Get(id);
			return map != null;
		}

		/// <summary>
		/// Returns the map with the given name, or null if it wasn't found.
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public Map GetByStringId(string stringId)
		{
			lock (_syncLock)
				return _maps.Values.FirstOrDefault(a => a.StringId == stringId);
		}

		/// <summary>
		/// Returns the map with the given name via out. Returns false
		/// if the map wasn't found.
		/// </summary>
		/// <param name="name"></param>
		/// <param name="map"></param>
		/// <returns></returns>
		public bool TryGetByStringId(string name, out Map map)
		{
			map = this.GetByStringId(name);
			return map != null;
		}

		/// <summary>
		/// Returns a list of all maps.
		/// </summary>
		/// <returns></returns>
		public Map[] GetAll()
		{
			lock (_syncLock)
				return _maps.Values.ToArray();
		}

		/// <summary>
		/// Executes given action for each map in the collection.
		/// </summary>
		/// <param name="action"></param>
		public void Do(Action<Map> action)
		{
			lock (_syncLock)
			{
				foreach (var map in _mapsList)
					action(map);
			}
		}

		/// <summary>
		/// Runs update on all maps.
		/// </summary>
		/// <param name="elapsed"></param>
		public void Update(TimeSpan elapsed)
		{
			lock (_syncLock)
			{
				foreach (var map in _mapsList)
					map.Update(elapsed);
			}
		}
	}
}
