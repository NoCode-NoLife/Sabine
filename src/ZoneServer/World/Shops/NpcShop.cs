using System;
using System.Collections.Generic;
using System.Linq;
using Sabine.Shared.Data;

namespace Sabine.Zone.World.Shops
{
	/// <summary>
	/// Represents an NPC's shop.
	/// </summary>
	public class NpcShop
	{
		private readonly List<ShopItem> _items = new List<ShopItem>();

		/// <summary>
		/// Returns the shop's unique name.
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Creates a new shop.
		/// </summary>
		/// <param name="name"></param>
		public NpcShop(string name)
		{
			this.Name = name;
		}

		/// <summary>
		/// Adds item with the given string id to the shop with its
		/// default price.
		/// </summary>
		/// <param name="stringId">String id of the item to add.</param>
		public void AddItem(string stringId)
			=> this.AddItem(stringId, -1);

		/// <summary>
		/// Adds item with the given string id and price to the shop.
		/// </summary>
		/// <param name="stringId">String id of the item to add.</param>
		/// <param name="price">Price to sell the item at. use -1 for the default price.</param>
		/// <exception cref="ArgumentException"></exception>
		public void AddItem(string stringId, int price)
		{
			if (!SabineData.Items.TryFind(stringId, out var itemData))
				throw new ArgumentException($"Item '{stringId}' not found.");

			this.AddItem(itemData.ClassId, price);
		}

		/// <summary>
		/// Adds item with the given class id to the shop with its
		/// default price.
		/// </summary>
		/// <param name="classId">Class id of the item to add.</param>
		public void AddItem(int classId)
			=> this.AddItem(classId, -1);

		/// <summary>
		/// Adds item with the given class id and price to the shop.
		/// </summary>
		/// <param name="classId">Class id of the item to add.</param>
		/// <param name="price">Price to sell the item at. use -1 for the default price.</param>
		/// <exception cref="ArgumentException"></exception>
		public void AddItem(int classId, int price)
		{
			if (!SabineData.Items.TryFind(classId, out var itemData))
				throw new ArgumentException($"Item '{classId}' not found.");

			var item = new ShopItem(itemData.ClassId);
			item.Price = price;

			lock (_items)
				_items.Add(item);
		}

		/// <summary>
		/// Returns a list of all items in the shop.
		/// </summary>
		/// <returns></returns>
		public ShopItem[] GetItems()
		{
			lock (_items)
				return _items.ToArray();
		}

		/// <summary>
		/// Returns the first item in the shop with the given string id,
		/// or null if no matching items were found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <returns></returns>
		public ShopItem GetItem(string stringId)
		{
			lock (_items)
				return _items.FirstOrDefault(a => a.StringId == stringId);
		}

		/// <summary>
		/// Returns the first item in the shop with the given string id
		/// via out. Returns false if no matching items were found.
		/// </summary>
		/// <param name="stringId"></param>
		/// <param name="item"></param>
		/// <returns></returns>
		public bool TryGetItem(string stringId, out ShopItem item)
		{
			item = this.GetItem(stringId);
			return item != null;
		}
	}
}
