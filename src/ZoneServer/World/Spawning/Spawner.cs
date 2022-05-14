using System;
using System.Threading.Tasks;
using Sabine.Zone.Ais;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;
using Yggdrasil.Extensions;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Spawning
{
	/// <summary>
	/// A spawn point that spawns and respawns monsters.
	/// </summary>
	public class Spawner : IDisposable
	{
		private bool _disposed;

		/// <summary>
		/// Returns the id of the monster that is being spawned.
		/// </summary>
		public int MonsterId { get; }

		/// <summary>
		/// Returns the maximum amount of monsters to spawn.
		/// </summary>
		public int Amount { get; }

		/// <summary>
		/// Returns the delay before the monsters are first spawned
		/// after creation of the spawner.
		/// </summary>
		public TimeSpan InitialDelay { get; }

		/// <summary>
		/// Returns the minimum delay before the monsters are respawned
		/// after getting killed.
		/// </summary>
		public TimeSpan RespawnDelayMin { get; }

		/// <summary>
		/// Returns the maximum delay before the monsters are respawned
		/// after getting killed.
		/// </summary>
		public TimeSpan RespawnDelayMax { get; }

		/// <summary>
		/// Returns the map that monsters are spawned on.
		/// </summary>
		public Map Map { get; }

		/// <summary>
		/// Creates new spawner.
		/// </summary>
		/// <param name="monsterClassId"></param>
		/// <param name="amount"></param>
		/// <param name="initialDelay"></param>
		/// <param name="respawnDelayMin"></param>
		/// <param name="respawnDelayMax"></param>
		/// <param name="mapId"></param>
		public Spawner(int monsterClassId, int amount, TimeSpan initialDelay, TimeSpan respawnDelayMin, TimeSpan respawnDelayMax, int mapId)
		{
			if (!ZoneServer.Instance.World.Maps.TryGet(mapId, out var map))
				throw new ArgumentException($"Map {mapId} not found.");

			if (respawnDelayMax < respawnDelayMin)
				respawnDelayMax = respawnDelayMin;

			this.MonsterId = monsterClassId;
			this.Amount = amount;
			this.InitialDelay = initialDelay;
			this.RespawnDelayMin = respawnDelayMin;
			this.RespawnDelayMax = respawnDelayMax;
			this.Map = map;
		}

		/// <summary>
		/// Stops any spawning activity.
		/// </summary>
		public void Dispose()
		{
			_disposed = true;
		}

		/// <summary>
		/// Spawns the initial batch of monsters.
		/// </summary>
		public void InitialSpawn()
		{
			for (var i = 0; i < this.Amount; ++i)
				this.PlanSpawn(this.InitialDelay);
		}

		/// <summary>
		/// Spawns a monster after the given delay.
		/// </summary>
		/// <param name="delay"></param>
		public void PlanSpawn(TimeSpan delay)
		{
			// If the  delay is zero, spawn right away, otherwise, wait
			// for the delay to pass.
			if (delay <= TimeSpan.Zero)
				this.Spawn();
			else
				Task.Delay(delay).ContinueWith(this.TaskedSpawn);
		}

		/// <summary>
		/// Spawns one monster.
		/// </summary>
		/// <param name="_"></param>
		private void TaskedSpawn(Task _)
		{
			this.Spawn();
		}

		/// <summary>
		/// Spawns one monster.
		/// </summary>
		public void Spawn()
		{
			if (_disposed)
				return;

			var pos = this.Map.GetRandomWalkablePosition();

			var monster = new Monster(this.MonsterId);
			monster.Components.Add(new MonsterAi(monster));
			monster.Killed += this.OnMonsterKilled;

			monster.Warp(this.Map.Id, pos);
		}

		/// <summary>
		/// Called when one of the monster spawned by this spawner
		/// was killed.
		/// </summary>
		/// <param name="monster"></param>
		private void OnMonsterKilled(Monster monster)
		{
			var rnd = RandomProvider.Get();
			var randomDelay = rnd.Between(this.RespawnDelayMin, this.RespawnDelayMax);

			this.PlanSpawn(randomDelay);
		}
	}
}
