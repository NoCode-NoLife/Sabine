using System;
using System.Threading;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;
using Sabine.Shared.World;
using Sabine.Zone.World.Maps;
using Yggdrasil.Util;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents an item, either inside a player's inventory or on a map.
	/// </summary>
	public class Item : IEntity
	{
		private static int HandlePool = 0x6000_0000;

		/// <summary>
		/// Returns the id the item is identified by in the data.
		/// </summary>
		public int ClassId => this.Data.ClassId;

		/// <summary>
		/// Returns the item's handle, which identifies it on a map.
		/// </summary>
		public int Handle { get; set; }

		/// <summary>
		/// Gets or sets the item's inventory id, which identifies it
		/// inside an inventory.
		/// </summary>
		public int InventoryId { get; set; }

		/// <summary>
		/// Returns the item's unique string id.
		/// </summary>
		public string StringId => this.Data.StringId;

		/// <summary>
		/// Gets or sets the item's type, which affects where it appears
		/// inside an inventory and what can be done with it.
		/// </summary>
		public ItemType Type => this.Data.Type;

		/// <summary>
		/// Gets or sets on which equip slots the item can be equipped on.
		/// </summary>
		public EquipSlots WearSlots => this.Data.WearSlots;

		/// <summary>
		/// Gets or sets on which slots the item is currently equipped.
		/// </summary>
		public EquipSlots EquippedOn { get; set; }

		/// <summary>
		/// Gets or sets whether the item has been identified.
		/// </summary>
		public bool IsIdentified { get; set; } = true;

		/// <summary>
		/// Returns true if the item is currently equipped.
		/// </summary>
		public bool IsEquipped => this.EquippedOn != EquipSlots.None;

		/// <summary>
		/// Gets or sets the item's amount.
		/// </summary>
		public int Amount
		{
			get => _amount;
			set => _amount = Math2.Clamp(0, short.MaxValue, value);
		}
		private int _amount = 1;

		/// <summary>
		/// Gets or sets the id of the map the item is on, if any.
		/// </summary>
		public int MapId { get; set; }

		/// <summary>
		/// Gets or sets the item's position on the map it's on.
		/// </summary>
		public Position Position { get; set; }

		/// <summary>
		/// Gets or sets a reference to the map the item is on.
		/// </summary>
		public Map Map { get; set; }

		/// <summary>
		/// Returns true if this item is stackable and can have an
		/// amount greater than 1.
		/// </summary>
		public bool IsStackable => !this.Type.IsEquip();

		/// <summary>
		/// Returns a reference to the item's data entry.
		/// </summary>
		public ItemData Data { get; private set; }

		/// <summary>
		/// Returns the time at which this item should be removed from
		/// the map it was dropped on.
		/// </summary>
		public DateTime DropDisappearTime { get; private set; } = DateTime.MaxValue;

		/// <summary>
		/// Creates new item from class id.
		/// </summary>
		/// <param name="classId"></param>
		public Item(int classId, int amount = 1)
		{
			this.Handle = GetNewHandle();
			this.Amount = Math.Max(1, amount);

			this.LoadData(classId);
		}

		/// <summary>
		/// Creates new item from string id.
		/// </summary>
		/// <param name="stringId"></param>
		public Item(string stringId)
		{
			this.Handle = GetNewHandle();
			this.LoadData(stringId);
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);

		/// <summary>
		/// Loads data by class id.
		/// </summary>
		/// <param name="classId"></param>
		/// <exception cref="ArgumentException"></exception>
		private void LoadData(int classId)
		{
			if (!SabineData.Items.TryFind(classId, out var data))
				throw new ArgumentException($"Class id '{classId}' not found in database.");

			this.LoadData(data);
		}

		/// <summary>
		/// Loads data by string id.
		/// </summary>
		/// <param name="stringId"></param>
		/// <exception cref="ArgumentException"></exception>
		private void LoadData(string stringId)
		{
			if (!SabineData.Items.TryFind(stringId, out var data))
				throw new ArgumentException($"String id '{stringId}' not found in database.");

			this.LoadData(data);
		}

		/// <summary>
		/// Loads the given data.
		/// </summary>
		/// <param name="data"></param>
		private void LoadData(ItemData data)
		{
			this.Data = data;
		}

		/// <summary>
		/// Adds item to the map, dropping it at the given position.
		/// </summary>
		/// <param name="map"></param>
		/// <param name="pos"></param>
		public void Drop(Map map, Position pos)
		{
			var disappearSeconds = ZoneServer.Instance.Conf.World.ItemDisappearTime;

			this.MapId = map.Id;
			this.Position = pos;
			this.EquippedOn = EquipSlots.None;
			this.DropDisappearTime = DateTime.Now.AddSeconds(disappearSeconds);

			map.AddItem(this);
		}
	}
}
