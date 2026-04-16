using System;
using System.Collections.Generic;
using Sabine.Zone.Network;
using Sabine.Zone.World.Entities;

namespace Sabine.Zone.World.Trading
{
	/// <summary>
	/// Trade manager that handles trades between players in the world.
	/// </summary>
	public class Trades
	{
		private readonly List<Trade> _trades = [];

		/// <summary>
		/// Starts a trade between two players, initializing it as a
		/// requested trade from player1.
		/// </summary>
		/// <param name="player1">The character requesting the trade.</param>
		/// <param name="player2">The character receiving the trade request.</param>
		/// <exception cref="ArgumentException"></exception>
		public void InitiateTrade(PlayerCharacter player1, PlayerCharacter player2)
		{
			if (player1.Map != player2.Map)
				throw new ArgumentException("Both players must be on the same map to initiate a trade.");

			var trade = new Trade(player1, player2);
			lock (_trades)
				_trades.Add(trade);

			Send.ZC_REQ_EXCHANGE_ITEM(player2, player1.Name);
		}

		/// <summary>
		/// Removes the given trade from the list active trades.
		/// </summary>
		/// <param name="trade"></param>
		public void RemoveTrade(Trade trade)
		{
			lock (_trades)
				_trades.Remove(trade);
		}

		/// <summary>
		/// Returns the active trade for the given player via out, if any.
		/// Returns false if no trades involving the player were found.
		/// </summary>
		/// <param name="player">The player to check for an active trade.</param>
		/// <param name="result">The active trade involving the player, if found.</param>
		/// <returns>True if an active trade involving the player was found.</returns>
		public bool TryGetTrade(PlayerCharacter player, out Trade result)
		{
			lock (_trades)
			{
				foreach (var trade in _trades)
				{
					if (trade.Involves(player))
					{
						result = trade;
						return true;
					}
				}
			}

			result = null;
			return false;
		}
	}
}
