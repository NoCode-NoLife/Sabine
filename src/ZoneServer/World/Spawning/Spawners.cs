using System.Collections.Generic;

namespace Sabine.Zone.World.Spawning
{
	/// <summary>
	/// Collection of monster spawners.
	/// </summary>
	public class Spawners
	{
		private readonly List<Spawner> _spawners = new List<Spawner>();

		/// <summary>
		/// Adds spawner.
		/// </summary>
		/// <param name="spawner"></param>
		public void Add(Spawner spawner)
		{
			lock (_spawners)
				_spawners.Add(spawner);
		}

		/// <summary>
		/// Removes all spawners.
		/// </summary>
		public void Clear()
		{
			lock (_spawners)
			{
				foreach (var spawner in _spawners)
					spawner.Dispose();

				_spawners.Clear();
			}
		}

		/// <summary>
		/// Executes initial spawn for all spawners.
		/// </summary>
		public void InitialSpawn()
		{
			lock (_spawners)
			{
				foreach (var spawner in _spawners)
					spawner.InitialSpawn();
			}
		}
	}
}
