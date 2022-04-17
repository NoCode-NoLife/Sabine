namespace Sabine.Zone.World.Shops
{
	/// <summary>
	/// Defines what choices the player has when a shop is opened.
	/// </summary>
	public enum ShopOpenType
	{
		/// <summary>
		/// Player gets a prompt to click either buy or sell.
		/// </summary>
		BuyAndSell,

		/// <summary>
		/// Player gets right to the list of items to buy.
		/// </summary>
		BuyOnly,

		/// <summary>
		/// Player gets right to the selling menu.
		/// </summary>
		SellOnly,
	}
}
