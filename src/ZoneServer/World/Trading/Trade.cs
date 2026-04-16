using System;
using System.Collections.Generic;
using Sabine.Shared.Const;
using Sabine.Shared.Util;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;
using Sabine.Zone.World.Maps;
using Yggdrasil.Logging;

namespace Sabine.Zone.World.Trading
{
	/// <summary>
	/// Represents a trade between two players.
	/// </summary>
	public class Trade
	{
		private readonly object _syncLock = new();

		/// <summary>
		/// Gets or sets the trade's current state.
		/// </summary>
		public TradeState State { get; private set; }

		/// <summary>
		/// Returns the map the trade takes place on.
		/// </summary>
		public Map Map { get; }

		/// <summary>
		/// Returns the first of the two traders, typically the one who
		/// initiated the trade.
		/// </summary>
		public Trader Trader1 { get; }

		/// <summary>
		/// Returns the second of the two traders, typically the one who
		/// received the trade request.
		/// </summary>
		public Trader Trader2 { get; }

		/// <summary>
		/// Creates a new trade between two players.
		/// </summary>
		/// <param name="trader1"></param>
		/// <param name="trader2"></param>
		public Trade(PlayerCharacter trader1, PlayerCharacter trader2)
		{
			if (trader1.Map != trader2.Map)
				throw new ArgumentException("The traders must be on the same map.");

			this.Map = trader1.Map;
			this.Trader1 = new Trader(trader1);
			this.Trader2 = new Trader(trader2);

			this.State = TradeState.Requested;
			this.Trader1.State = TraderState.Requesting;
			this.Trader2.State = TraderState.Requested;
		}

		/// <summary>
		/// Returns true if the given character is one of the two traders
		/// involved in this trade.
		/// </summary>
		/// <param name="character"></param>
		/// <returns></returns>
		public bool Involves(PlayerCharacter character)
		{
			return this.Trader1.CharacterId == character.Id || this.Trader2.CharacterId == character.Id;
		}

		/// <summary>
		/// Returns the trader corresponding to the given character and
		/// the respective other trader via out. Returns false if neither
		/// trader matched the character.
		/// </summary>
		/// <param name="character"></param>
		/// <param name="trader"></param>
		/// <param name="other"></param>
		/// <returns></returns>
		private bool TryGetTrader(PlayerCharacter character, out Trader trader, out Trader other)
		{
			if (this.Trader1?.CharacterId == character.Id)
			{
				trader = this.Trader1;
				other = this.Trader2;
				return true;
			}

			if (this.Trader2?.CharacterId == character.Id)
			{
				trader = this.Trader2;
				other = this.Trader1;
				return true;
			}

			trader = null;
			other = null;
			return false;
		}

		/// <summary>
		/// Updates the trade based on response given by the player,
		/// either accepting or rejecting the trade request.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="response"></param>
		/// <exception cref="ArgumentException"></exception>
		/// <exception cref="InvalidOperationException"></exception>
		public void Acknowledge(PlayerCharacter player, TradingResponse response)
		{
			lock (_syncLock)
			{
				if (response != TradingResponse.Accept && response != TradingResponse.Cancel)
					throw new ArgumentException($"User {player.Connection.Account.Username} sent invalid trading response {response}.");

				if (this.State != TradeState.Requested)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot acknowledge trade because trade is not in requested state.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (!this.Map.TryGetPlayerById(otherTrader.CharacterId, out var otherPlayer))
				{
					Log.Debug($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because partner {otherTrader.CharacterId} is not in the same map.");
					this.Cancel();
					return;
				}

				if (response == TradingResponse.Accept)
				{
					this.State = TradeState.Active;
					trader.State = TraderState.Trading;
					otherTrader.State = TraderState.Trading;
				}
				else if (response == TradingResponse.Cancel)
				{
					ZoneServer.Instance.World.Trades.RemoveTrade(this);
				}

				Send.ZC_ACK_EXCHANGE_ITEM(player, response);
				Send.ZC_ACK_EXCHANGE_ITEM(otherPlayer, response);
			}
		}

		/// <summary>
		/// Adds the item with the given inventory id and amount to the
		/// trade on the side of the player.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="inventoryId"></param>
		/// <param name="amount"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void AddItem(PlayerCharacter player, int inventoryId, int amount)
		{
			lock (_syncLock)
			{
				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because trade is not active.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (trader.State != TraderState.Trading)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot add Zeny because they are not trading.");

				if (!this.Map.TryGetPlayerById(otherTrader.CharacterId, out var otherPlayer))
				{
					Log.Debug($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because partner {otherTrader.CharacterId} is not in the same map.");
					this.Cancel();
					return;
				}

				// The client prevents adding the same item multiple times
				if (trader.IsTradingItem(inventoryId))
					throw new InvalidOperationException($"User {player.Connection.Account.Username} is already trading item with inventory ID {inventoryId}.");

				if (!player.Inventory.TryGetItem(inventoryId, out var item))
				{
					player.ServerMessage(Localization.Get("Item not found."));
					Send.ZC_ACK_ADD_EXCHANGE_ITEM(player, inventoryId, TradingSuccess.Fail);
					return;
				}

				if (amount < 1 || amount > item.Amount)
				{
					player.ServerMessage(Localization.Get("Not enough items."));
					Send.ZC_ACK_ADD_EXCHANGE_ITEM(player, inventoryId, TradingSuccess.Fail);
					return;
				}

				trader.Items.Add(new TradeItem(inventoryId, amount));
				Send.ZC_ADD_EXCHANGE_ITEM.Item(otherPlayer, item, amount);

				Send.ZC_ACK_ADD_EXCHANGE_ITEM(player, inventoryId, TradingSuccess.Success);
			}
		}

		/// <summary>
		/// Sets the amount of Zeny the player is offering in the trade.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void SetZeny(PlayerCharacter player, int amount)
		{
			lock (_syncLock)
			{
				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because trade is not active.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (trader.State != TraderState.Trading)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot add Zeny because they are not trading.");

				if (!this.Map.TryGetPlayerById(otherTrader.CharacterId, out var otherPlayer))
				{
					Log.Debug($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because partner {otherTrader.CharacterId} is not in the same map.");
					this.Cancel();
					return;
				}

				// The client silently clamps the Zeny input to a valid amount
				amount = Math.Clamp(amount, 0, player.Parameters.Zeny);

				if (amount == 0)
					return;

				trader.Zeny = amount;
				Send.ZC_ADD_EXCHANGE_ITEM.Zeny(otherPlayer, amount);

				Send.ZC_ACK_ADD_EXCHANGE_ITEM(player, 0, TradingSuccess.Success);
			}
		}

		/// <summary>
		/// Concludes the trade for the player, indicating that they are
		/// ready to complete it.
		/// </summary>
		/// <param name="player"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void Conclude(PlayerCharacter player)
		{
			lock (_syncLock)
			{
				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because trade is not active.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (!this.Map.TryGetPlayerById(otherTrader.CharacterId, out var otherPlayer))
				{
					Log.Debug($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because partner {otherTrader.CharacterId} is not in the same map.");
					this.Cancel();
					return;
				}

				if (trader.State != TraderState.Trading)
				{
					Log.Debug($"Cannot conclude trade between {trader.CharacterId} and {otherTrader.CharacterId} because trader {trader.CharacterId} is not trading.");
					return;
				}

				trader.State = TraderState.Concluding;

				Send.ZC_CONCLUDE_EXCHANGE_ITEM(player, TradingSide.Self);
				Send.ZC_CONCLUDE_EXCHANGE_ITEM(otherPlayer, TradingSide.Partner);
			}
		}

		/// <summary>
		/// Marks the trade as complete for the player. If both traders
		/// have indicated that they're done, the trade is executed.
		/// </summary>
		/// <param name="player"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void Complete(PlayerCharacter player)
		{
			lock (_syncLock)
			{
				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because trade is not active.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot complete trade because trade is not active.");

				if (trader.State != TraderState.Concluding && trader.State != TraderState.Complete)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot complete trade because they have not concluded.");

				if (otherTrader.State != TraderState.Concluding && otherTrader.State != TraderState.Complete)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot complete trade because partner has not concluded.");

				if (!this.Map.TryGetPlayerById(otherTrader.CharacterId, out var otherPlayer))
				{
					Log.Debug($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because partner {otherTrader.CharacterId} is not in the same map.");
					this.Cancel();
					return;
				}

				trader.State = TraderState.Complete;

				var bothCompleted = trader.State == TraderState.Complete && otherTrader.State == TraderState.Complete;
				if (!bothCompleted)
					return;

				this.State = TradeState.Complete;

				var traderZeny = trader.Zeny;
				var otherTraderZeny = otherTrader.Zeny;

				if (traderZeny > player.Parameters.Zeny)
				{
					Log.Warning($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because trader {trader.CharacterId} doesn't have enough Zeny.");
					this.Cancel();
					return;
				}

				if (otherTraderZeny > otherPlayer.Parameters.Zeny)
				{
					Log.Warning($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because trader {otherTrader.CharacterId} doesn't have enough Zeny.");
					this.Cancel();
					return;
				}

				var traderItems = new List<Item>();
				var otherTraderItems = new List<Item>();

				foreach (var tradeItem in trader.Items)
				{
					if (!player.Inventory.TryGetItem(tradeItem.InventoryId, out var item))
					{
						Log.Warning($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because trader {trader.CharacterId} does not have item with inventory id {tradeItem.InventoryId}.");
						this.Cancel();
						return;
					}

					traderItems.Add(item);
				}

				foreach (var tradeItem in otherTrader.Items)
				{
					if (!otherPlayer.Inventory.TryGetItem(tradeItem.InventoryId, out var item))
					{
						Log.Warning($"Cancelling trade between {trader.CharacterId} and {otherTrader.CharacterId} because trader {otherTrader.CharacterId} does not have item with inventory id {tradeItem.InventoryId}.");
						this.Cancel();
						return;
					}

					otherTraderItems.Add(item);
				}

				foreach (var item in traderItems)
				{
					player.Inventory.RemoveItem(item);
					otherPlayer.Inventory.AddItem(item);
				}

				foreach (var item in otherTraderItems)
				{
					otherPlayer.Inventory.RemoveItem(item);
					player.Inventory.AddItem(item);
				}

				player.Parameters.Zeny -= traderZeny;
				otherPlayer.Parameters.Zeny -= otherTraderZeny;

				player.Parameters.Zeny += otherTraderZeny;
				otherPlayer.Parameters.Zeny += traderZeny;

				Send.ZC_EXEC_EXCHANGE_ITEM(player, TradingSuccess.Success);
				Send.ZC_EXEC_EXCHANGE_ITEM(otherPlayer, TradingSuccess.Success);

				ZoneServer.Instance.World.Trades.RemoveTrade(this);
			}
		}

		/// <summary>
		/// Cancels the trade at the request of the player.
		/// </summary>
		/// <param name="player"></param>
		/// <exception cref="InvalidOperationException"></exception>
		/// <exception cref="ArgumentException"></exception>
		public void RequestCancellation(PlayerCharacter player)
		{
			lock (_syncLock)
			{
				if (this.State != TradeState.Active)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because trade is not active.");

				if (!this.TryGetTrader(player, out var trader, out var otherTrader))
					throw new ArgumentException($"User {player.Connection.Account.Username} is not part of this trade.");

				if (trader.State != TraderState.Trading && trader.State != TraderState.Concluding)
					throw new InvalidOperationException($"User {player.Connection.Account.Username} cannot request trade cancellation because they are not trading.");

				this.Cancel();
			}
		}

		/// <summary>
		/// Cancels the trade, marking it as cancelled and notifying both
		/// traders.
		/// </summary>
		public void Cancel()
		{
			lock (_syncLock)
			{
				this.State = TradeState.Cancelled;

				this.Trader1.State = TraderState.Cancelled;
				this.Trader1.Items.Clear();
				this.Trader1.Zeny = 0;

				this.Trader2.State = TraderState.Cancelled;
				this.Trader2.Items.Clear();
				this.Trader2.Zeny = 0;

				if (this.Map.TryGetPlayerById(this.Trader1.CharacterId, out var player1))
					Send.ZC_CANCEL_EXCHANGE_ITEM(player1);

				if (this.Map.TryGetPlayerById(this.Trader2.CharacterId, out var player2))
					Send.ZC_CANCEL_EXCHANGE_ITEM(player2);

				ZoneServer.Instance.World.Trades.RemoveTrade(this);
			}
		}
	}

	/// <summary>
	/// Represents a character in a trade.
	/// </summary>
	/// <param name="character"></param>
	public class Trader(PlayerCharacter character)
	{
		/// <summary>
		/// Returns the character's id.
		/// </summary>
		public int CharacterId { get; } = character.Id;

		/// <summary>
		/// Gets or sets the character's current state.
		/// </summary>
		public TraderState State { get; set; } = TraderState.None;

		/// <summary>
		/// Returns a list of the items the character is offering in the
		/// trade.
		/// </summary>
		public List<TradeItem> Items { get; } = new();

		/// <summary>
		/// Gets or sets the amount of Zeny the character is offering in
		/// the trade.
		/// </summary>
		public int Zeny { get; set; }

		/// <summary>
		/// Returns true if the item with the given inventory id is
		/// already part of the trade.
		/// </summary>
		/// <param name="inventoryId"></param>
		/// <returns></returns>
		public bool IsTradingItem(int inventoryId)
		{
			foreach (var item in this.Items)
			{
				if (item.InventoryId == inventoryId)
					return true;
			}

			return false;
		}
	}

	/// <summary>
	/// Represents an item being traded.
	/// </summary>
	/// <param name="InventoryId"></param>
	/// <param name="Amount"></param>
	public record TradeItem(int InventoryId, int Amount);

	/// <summary>
	/// Describes the current state of a trade.
	/// </summary>
	public enum TradeState
	{
		/// <summary>
		/// The trade was initiated by a player, but the other player has
		/// not responded yet.
		/// </summary>
		Requested,

		/// <summary>
		/// The other player rejected the trade request.
		/// </summary>
		Rejected,

		/// <summary>
		/// The trade is active and both players can add items and Zeny to
		/// it.
		/// </summary>
		Active,

		/// <summary>
		/// The trade was cancelled.
		/// </summary>
		Cancelled,

		/// <summary>
		/// The trade was completed by both players.
		/// </summary>
		Complete,
	}

	/// <summary>
	/// Describes the current state of a trader in a trade.
	/// </summary>
	public enum TraderState
	{
		/// <summary>
		/// The state was not yet set.
		/// </summary>
		None,

		/// <summary>
		/// The player initiated the trade and is waiting for the other
		/// player to respond.
		/// </summary>
		Requesting,

		/// <summary>
		/// The player received a trade request and has not responded yet.
		/// </summary>
		Requested,

		/// <summary>
		/// The trade was cancelled.
		/// </summary>
		Cancelled,

		/// <summary>
		/// The trade is active and the player can add items and Zeny to
		/// it.
		/// </summary>
		Trading,

		/// <summary>
		/// The player has indicated that they are done adding items and
		/// Zeny and is ready to complete the trade.
		/// </summary>
		Concluding,

		/// <summary>
		/// The player has completed the trade.
		/// </summary>
		Complete,
	}
}
