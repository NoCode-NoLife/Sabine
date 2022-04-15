using System;
using System.Threading;
using Sabine.Shared.Const;

namespace Sabine.Zone.World.Entities
{
	/// <summary>
	/// Represents an item, either inside a player's inventory or on a map.
	/// </summary>
	public class Item
	{
		private static int HandlePool = 0x6000_0000;

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
		public string StringId { get; set; }

		/// <summary>
		/// Gets or sets the item's type, which affects where it appears
		/// inside an inventory and what can be done with it.
		/// </summary>
		public ItemType Type { get; set; }

		/// <summary>
		/// Gets or sets on which equip slots the item can be equipped on.
		/// </summary>
		public EquipSlots WearSlots { get; set; }

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
		/// Creates new item from string id.
		/// </summary>
		/// <param name="stringId"></param>
		public Item(string stringId)
		{
			this.Handle = GetNewHandle();
			this.StringId = stringId;
		}

		/// <summary>
		/// Returns a new handle.
		/// </summary>
		/// <returns></returns>
		private static int GetNewHandle()
			=> Interlocked.Increment(ref HandlePool);
	}
}
