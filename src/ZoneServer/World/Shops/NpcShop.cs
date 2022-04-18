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

		/// <summary>
		/// Creates an NPC shop from a series of item class ids and prices
		/// and returns it.
		/// </summary>
		/// <example>
		/// Shop with Red and Scarlet Potions for default prices and White
		/// Potions for 50% off.
		/// NpcShop.Build(501, -1, 502, -1, 503, 250);
		/// </example>
		/// <param name="args"></param>
		/// <returns></returns>
		public static NpcShop Build(params int[] args)
		{
			if (args.Length % 2 != 0)
				throw new ArgumentException($"The number of arguments must be even, with an id and a price for every item.");

			var shop = new NpcShop("__generated__");

			for (var i = 0; i < args.Length; i += 2)
			{
				var classId = args[i + 0];
				var price = args[i + 1];

				shop.AddItem(classId, price);
			}

			return shop;
		}
	}
}
