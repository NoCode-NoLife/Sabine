using System;
using System.Threading;
using Sabine.Shared.Const;
using Sabine.Shared.Data;
using Sabine.Shared.Data.Databases;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents an item, either inside a player's inventory or on a map.
	/// </summary>
	public class Item
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
		/// Gets or sets the item's amount.
		/// </summary>
		public int Amount
		{
			get => _amount;
			set => _amount = Math.Max(0, value);
		}
		private int _amount = 1;

		/// <summary>
		/// Returns a reference to the item's data entry.
		/// </summary>
		public ItemData Data { get; private set; }

		/// <summary>
		/// Creates new item from class id.
		/// </summary>
		/// <param name="classId"></param>
		public Item(int classId)
		{
			this.Handle = GetNewHandle();
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
	}
}
