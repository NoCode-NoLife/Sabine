using System.IO;
using System.IO.Compression;
using Yggdrasil.Data.Binary;

namespace Sabine.Shared.Data.Databases
{
	/// <summary>
	/// Represents the cache data of a map.
	/// </summary>
	public class MapCacheData
	{
		public string StringId { get; set; }
		public int Width { get; set; }
		public int Height { get; set; }
		public MapCacheTile[,] Tiles { get; set; }

		/// <summary>
		/// Returns true if the given tile can be walked on.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsPassable(int x, int y)
		{
			return this.Tiles[x, y].IsWalkable;
		}

		/// <summary>
		/// Returns true if the given tile can be walked on.
		/// </summary>
		/// <param name="x"></param>
		/// <param name="y"></param>
		/// <returns></returns>
		public bool IsUnpassable(int x, int y)
		{
			return !this.Tiles[x, y].IsWalkable;
		}
	}

	/// <summary>
	/// Represents one tile on a map.
	/// </summary>
	public class MapCacheTile
	{
		public TileType Type { get; set; }
		public bool IsWalkable => this.Type != TileType.Unpassable;

		public MapCacheTile(TileType type)
		{
			this.Type = type;
		}
	}

	/// <summary>
	/// Defines a tile's type.
	/// </summary>
	/// <remarks>
	/// The tile types in the alpha client appear to be slightly
	/// different from later clients. For example, type 5 was
	/// apparently not passable later on.
	/// </remarks>
	public enum TileType
	{
		/// <summary>
		/// Passable, but somehow different from type 5.
		/// </summary>
		/// <remarks>
		/// Potentially natural surfaces vs man-made. For example,
		/// towns are usually entirely type 5, while fields and
		/// dungeons are mostly type 0.
		/// </remarks>
		Unk0 = 0,

		/// <summary>
		/// Characters can't walk on this tile.
		/// </summary>
		Unpassable = 1,

		/// <summary>
		/// Possibly passable water.
		/// </summary>
		Unk2 = 2,

		/// <summary>
		/// Characters can walk on this tile.
		/// </summary>
		Passable = 5,
	}

	/// <summary>
	/// A map cache database, holding detailed information about a map's
	/// terrain.
	/// </summary>
	public class MapCacheDb : DatabaseBinaryIndexed<string, MapCacheData>
	{
		/// <summary>
		/// Called to read the binary database from a file.
		/// </summary>
		/// <param name="brfs"></param>
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
