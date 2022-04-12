using System;
using System.IO;
using System.IO.Compression;
using Yggdrasil.Data.Binary;

namespace Sabine.Shared.Data.Databases
{
	public class MapCacheData
	{
		public string StringId { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public MapCacheTile[,] Tiles { get; set; }

		public bool IsPassable(int x, int y)
		{
			return !this.Tiles[x, y].IsWalkable;
		}
	}

	public class MapCacheTile
	{
		public TileType Type { get; set; }
		public bool IsWalkable => this.Type != TileType.Unpassable;

		public MapCacheTile(TileType type)
		{
			this.Type = type;
		}
	}

	public enum TileType
	{
		Unk0 = 0,
		Unpassable = 1,
		Unk2 = 2,
		Passable = 5,
	}

	public class MapCacheDb : DatabaseBinaryIndexed<string, MapCacheData>
	{
		protected override void Read(BinaryReader brfs)
		{
			var header = brfs.ReadBytes(4);
			var version = brfs.ReadInt32();
			var compressed = brfs.ReadBoolean();
			var count = brfs.ReadInt32();

			for (var i = 0; i < count; ++i)
			{
				var data = new MapCacheData();

				var length = brfs.ReadInt32();
				var bytes = brfs.ReadBytes(length);

				if (compressed)
				{
					using (var msCompressed = new MemoryStream(bytes))
					using (var msUncompressed = new MemoryStream())
					using (var ds = new DeflateStream(msCompressed, CompressionMode.Decompress, true))
					{
						ds.CopyTo(msUncompressed);
						bytes = msUncompressed.ToArray();
					}
				}

				using (var ms = new MemoryStream(bytes))
				using (var brms = new BinaryReader(ms))
				{
					data.StringId = brms.ReadString();
					data.Width = brms.ReadInt32();
					data.Height = brms.ReadInt32();

					data.Tiles = new MapCacheTile[data.Width, data.Height];
					for (var y = 0; y < data.Height; ++y)
					{
						for (var x = 0; x < data.Width; ++x)
						{
							var type = (TileType)brms.ReadByte();
							data.Tiles[x, y] = new MapCacheTile(type);
						}
					}

					this.AddOrReplace(data.StringId, data);
				}
			}
		}
	}
}
