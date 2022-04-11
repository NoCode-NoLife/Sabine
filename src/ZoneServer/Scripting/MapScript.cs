using System;
using Sabine.Shared.World;
using Sabine.Zone.World.Entities;
using Yggdrasil.Scripting;

namespace Sabine.Zone.Scripting
{
	public class MapScript : IScript
	{
		public bool Init()
		{
			this.Load();
			return true;
		}

		public virtual void Load()
		{
		}

		public void AddNpc(string name, int classId, string mapStringId, int x, int y)
		{
			if (!ZoneServer.Instance.World.Maps.TryGetByStringId(mapStringId, out var map))
				throw new ArgumentException($"Map '{mapStringId}' not found.");

			var npc = new Npc(classId);
			npc.Name = name;

			npc.Warp(map.Id, new Position(x, y));
		}
	}
}
