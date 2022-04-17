using Sabine.Zone.World.Entities;

namespace Sabine.Zone.World.Shops
{
	/// <summary>
	/// Sub-type of Item, specifically for use in NPC shops.
	/// </summary>
	public class ShopItem : Item
	{
		/// <summary>
		/// Returns the price of the item in its shop. If this is set
		/// to -1, the price becomes the default price.
		/// </summary>
		public int Price
		{
			get => _price != -1 ? _price : this.Data.Price;
			set => _price = value;
		}
		private int _price = -1;

		/// <summary>
		/// Creates new shop item.
		/// </summary>
		/// <param name="classId"></param>
		/// <param name="amount"></param>
		public ShopItem(int classId, int amount = 1)
			: base(classId, amount)
		{
		}
	}
}
