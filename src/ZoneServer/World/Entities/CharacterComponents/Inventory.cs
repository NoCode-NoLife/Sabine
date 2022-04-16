using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Const;
using Sabine.Zone.Network;

namespace Sabine.Zone.World.Entities.CharacterComponents
{
	/// <summary>
	/// Represents a player's inventory.
	/// </summary>
	public class Inventory
	{
		private readonly object _syncLock = new object();

		private readonly List<Item> _items = new List<Item>();
		private EquipSlots _occupiedSlots;

		/// <summary>
		/// Returns the character this inventory belongs to.
		/// </summary>
		public PlayerCharacter Character { get; }

		/// <summary>
		/// Creates new inventory for character.
		/// </summary>
		/// <param name="character"></param>
		public Inventory(PlayerCharacter character)
		{
			this.Character = character;
		}

		/// <summary>
		/// Adds item to inventory during loading of a character.
		/// </summary>
		/// <param name="item"></param>
		internal void AddItemInit(Item item)
		{
			lock (_syncLock)
			{
				item.InventoryId = this.GetNewInventoryId();

				_items.Add(item);
				_occupiedSlots |= item.EquippedOn;

				if ((item.EquippedOn & EquipSlots.RightHand) != 0)
					this.Character.WeaponId = item.Data.LookId;
			}
		}

		/// <summary>
		/// Adds item to inventory and updates client.
		/// </summary>
		/// <param name="item"></param>
		public void AddItem(Item item)
		{
			lock (_syncLock)
			{
				if (_items.Contains(item))
					throw new ArgumentException("The item is already part of this inventory.");

				if (item.IsStackable)
				{
					var existingStack = _items.FirstOrDefault(a => a.ClassId == item.ClassId);
					if (existingStack != null)
					{
						var addAmount = item.Amount;
						Send.ZC_ITEM_THROW_ACK(this.Character, existingStack.InventoryId, -addAmount);
						return;
					}
				}

				item.InventoryId = this.GetNewInventoryId();
				_items.Add(item);
			}

			Send.ZC_ITEM_PICKUP_ACK(this.Character, item, PickUpResult.Okay);
		}

		/// <summary>
		/// Returns an unused inventory id.
		/// </summary>
		private int GetNewInventoryId()
		{
			// TODO: Optimize. Maybe just create a pool for freed ids
			//   that we can take new ones from?

			lock (_syncLock)
			{
				for (var i = 1; i < short.MaxValue; ++i)
				{
					if (!_items.Any(a => a.InventoryId == i))
						return i;
				}
			}

			return -1;
		}

		/// <summary>
		/// Removes item from inventory and updates client.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool RemoveItem(Item item)
		{
			var amount = item.Amount;
			return this.DecrementItem(item, item.Amount) == amount;
		}

		/// <summary>
		/// Decrements the item by the given amount. The item is removed
		/// if the amount was greater or equal to the item's amount.
		/// Returns the amount that was actually removed.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="amount"></param>
		/// <returns></returns>
		public int DecrementItem(Item item, int amount)
		{
			if (!this.ContainsItem(item))
				throw new ArgumentException("Item not found in inventory.");

			var amountBefore = item.Amount;
			item.Amount -= amount;

			var removedAmount = amountBefore - item.Amount;

			if (item.Amount == 0)
			{
				lock (_syncLock)
					_items.Remove(item);
			}

			Send.ZC_ITEM_THROW_ACK(this.Character, item.InventoryId, amount);
			return removedAmount;
		}

		/// <summary>
		/// Returns the item with the given inventory id, or null if it
		/// wasn't found.
		/// </summary>
		/// <param name="invId"></param>
		/// <returns></returns>
		public Item GetItem(int invId)
		{
			lock (_syncLock)
				return _items.FirstOrDefault(a => a.InventoryId == invId);
		}

		/// <summary>
		/// Returns the item with the given handle, or null if it wasn't
		/// found.
		/// </summary>
		/// <param name="handle"></param>
		public Item GetItemByHandle(int handle)
		{
			lock (_syncLock)
				return _items.FirstOrDefault(a => a.Handle == handle);
		}

		/// <summary>
		/// Returns a list with all items in the inventory.
		/// </summary>
		/// <returns></returns>
		public Item[] GetItems()
		{
			lock (_syncLock)
				return _items.ToArray();
		}

		/// <summary>
		/// Returns a list of all items in the inventory that match
		/// the given predicate.
		/// </summary>
		/// <param name="predicate"></param>
		/// <returns></returns>
		public Item[] GetItems(Func<Item, bool> predicate)
		{
			lock (_syncLock)
				return _items.Where(predicate).ToArray();
		}

		/// <summary>
		/// Returns true if the given item is part of this inventory.
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool ContainsItem(Item item)
		{
			lock (_syncLock)
				return _items.Contains(item);
		}

		/// <summary>
		/// Equips the item on the given slot.
		/// </summary>
		/// <param name="item"></param>
		/// <param name="slots"></param>
		public void EquipItem(Item item, EquipSlots slots)
		{
			if (!this.ContainsItem(item))
				throw new ArgumentException("The item must be added to the inventory before it can be equipped.");

			lock (_syncLock)
			{
				if ((_occupiedSlots & slots) != 0)
					throw new ArgumentException($"Other items are already occupying the given slots (Slots: {slots}, Occupied: {_occupiedSlots}).");

				_occupiedSlots |= slots;
			}

			item.EquippedOn = slots;

			Send.ZC_REQ_WEAR_EQUIP_ACK(this.Character, item.InventoryId, slots);

			this.OnEquippedItem(item, slots);
		}

		/// <summary>
		/// Unequips the item from its current slot.
		/// </summary>
		/// <param name="item"></param>
		public void UnequipItem(Item item)
		{
			if (!this.ContainsItem(item))
				throw new ArgumentException("The item can't be unequipped if it's not part of the inventory.");

			if (item.EquippedOn == EquipSlots.None)
				throw new ArgumentException("The item is not equipped.");

			var slots = item.EquippedOn;
			item.EquippedOn = EquipSlots.None;

			lock (_syncLock)
				_occupiedSlots &= ~slots;

			Send.ZC_REQ_TAKEOFF_EQUIP_ACK(this.Character, item.InventoryId, slots);

			this.OnUnequippedItem(item, slots);
		}

		/// <summary>
		/// Sends a list of all items to the client again to refresh it,
		/// particularly the equip items.
		/// </summary>
		/// <remarks>
		/// Since we can't send negative responses to item equip requests
		/// (see CZ_USE_ITEM handler), we need to be able to tell the
		/// client to not try to equip items they shouldn't be able to
		/// equip in the first place. For this purpose we're setting the
		/// wear slots to 0 for the character when sending the items.
		/// After a job or level change, however, we need to update that
		/// information, so the player is able to now equip those items.
		/// This method simply removes all items from the inventory and
		/// adds them again. The player doesn't notice and we get what
		/// we want.
		/// </remarks>
		public void RefreshClient()
		{
			var character = this.Character;
			var items = character.Inventory.GetItems();

			foreach (var item in items)
				Send.ZC_ITEM_THROW_ACK(character, item.InventoryId, item.Amount);

			Send.ZC_EQUIPMENT_ITEMLIST(character, items);
			Send.ZC_NORMAL_ITEMLIST(character, items);
		}

		/// <summary>
		/// Called when the player equipped an item.
		/// </summary>
		/// <param name="item">The item that was equipped.</param>
		/// <param name="slots">The slot(s) the item was equipped on.</param>
		private void OnEquippedItem(Item item, EquipSlots slots)
		{
			if ((slots & EquipSlots.RightHand) != 0)
				this.Character.ChangeLook(SpriteType.Weapon, item.Data.LookId);
		}

		/// <summary>
		/// Called when the player unequipped an item.
		/// </summary>
		/// <param name="item">The item that was unequipped.</param>
		/// <param name="slots">The slot(s) the item was unequipped from.</param>
		private void OnUnequippedItem(Item item, EquipSlots slots)
		{
			if ((slots & EquipSlots.RightHand) != 0)
				this.Character.ChangeLook(SpriteType.Weapon, 0);
		}
	}
}
