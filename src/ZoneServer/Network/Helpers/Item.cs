using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sabine.Shared;
using Sabine.Shared.Const;
using Sabine.Shared.Network;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.Network.Helpers
{
	/// <summary>
	/// Item-related extension methods for Packet.
	/// </summary>
	public static class PacketItemExtensions
	{
		/// <summary>
		/// Adds item information to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="item"></param>
		public static void AddNormalItem(this Packet packet, Item item)
		{
			if (Game.Version < Versions.Beta2)
			{
				var size = 6 + Sizes.ItemNames;
				if (Game.Version >= Versions.Beta1)
					size += 4;

				// The first byte contains the size of the item
				// struct plus the size byte, which the client
				// memcpys for handling. It's currently unclear
				// why this size byte is necessary, but it's
				// working this way.
				packet.PutByte((byte)size);

				packet.PutByte((byte)item.Type);
				packet.PutShort((short)item.InventoryId);
				packet.PutShort((short)item.Amount);

				if (Game.Version >= Versions.Beta1)
				{
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
				}

				packet.PutString(item.StringId, Sizes.ItemNames);
			}
			else
			{
				packet.PutShort((short)item.InventoryId);
				packet.PutShort((short)item.ClassId);
				packet.PutByte((byte)item.Type);
				packet.PutByte(item.IsIdentified);
				packet.PutShort((short)item.Amount);
				packet.PutShort(0);
			}
		}

		/// <summary>
		/// Adds equip item information to packet.
		/// </summary>
		/// <param name="packet"></param>
		/// <param name="item"></param>
		/// <param name="wearSlots"></param>
		public static void AddEquipItem(this Packet packet, Item item, EquipSlots wearSlots)
		{
			if (Game.Version < Versions.Beta2)
			{
				var size = 6 + Sizes.ItemNames;
				if (Game.Version >= Versions.Beta1)
					size += 16;

				// The first byte contains the size of the item
				// struct plus the size byte, which the client
				// memcpys for handling. It's currently unclear
				// why this size byte is necessary, but it's
				// working this way.
				packet.PutByte((byte)size);

				packet.PutByte((byte)item.Type);
				packet.PutByte((byte)wearSlots);
				packet.PutShort((short)item.InventoryId);
				packet.PutByte((byte)item.EquippedOn);

				if (Game.Version >= Versions.Beta1)
				{
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
					packet.PutByte(0);
				}

				packet.PutString(item.StringId, Sizes.ItemNames);
			}
			else
			{
				packet.PutShort((short)item.InventoryId);
				packet.PutShort((short)item.ClassId);
				packet.PutByte((byte)item.Type);
				packet.PutByte(item.IsIdentified);
				packet.PutShort((short)wearSlots);
				packet.PutShort((short)item.EquippedOn);
				packet.PutByte(0);   // Attribute
				packet.PutByte(0);   // Refine
				packet.PutShort(0);  // Card1
				packet.PutShort(0);  // Card2
				packet.PutShort(0);  // Card3
				packet.PutShort(0);  // Card4
			}
		}
	}
}
