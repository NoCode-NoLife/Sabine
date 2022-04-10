using System;
using System.Linq;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;

namespace Sabine.Zone.World
{
	public class WorldManager
	{
		public MapManager Maps = new MapManager();

		public WorldManager()
		{
			this.Maps.Add(new Map("prt_vilg01"));
			this.Maps.Add(new Map("moc_vilg00"));
		}

		public PlayerCharacter GetPlayerCharacter(Func<PlayerCharacter, bool> predicate)
		{
			return null;
		}

		public bool TryGetPlayerCharacter(Func<PlayerCharacter, bool> predicate, out PlayerCharacter character)
		{
			character = this.GetPlayerCharacter(predicate);
			return character != null;
		}

		public int GetPlayerCount()
		{
			var maps = this.Maps.GetAll();
			return maps.Sum(a => a.PlayerCount);
		}
	}
}
