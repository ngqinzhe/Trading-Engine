﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingEngineServer.Instrument;
using TradingEngineServer.Orderbook.MatchingAlgorithm.OrderbookIterator;
using TradingEngineServer.OrderbookData;
using TradingEngineServer.Orders;
using TradingEngineServer.Trades;

namespace TradingEngineServer.Orderbook.MatchingAlgorithm
{
    public class FifoMatchingAlgorithm : IMatchingAlgorithm
    {
        public static IMatchingAlgorithm MatchingAlgorithm { get; } = new FifoMatchingAlgorithm();

        public MatchResult Match(IEnumerable<OrderbookEntry> bids, IEnumerable<OrderbookEntry> asks)
        {
            var eventTime = DateTime.UtcNow;
            var matchResult = new MatchResult();

            if (!bids.Any() && !asks.Any())
                throw new MatchException("There are no bids and no asks."); // Can't match without both sides.
            if (!bids.Any() || !asks.Any())
                return matchResult;

            var bidOrderIterator = new OrderbookEntryIterator(bids);
            var askOrderIterator = new OrderbookEntryIterator(asks);

            OrderbookEntry orderToMatchBid = bidOrderIterator.CurrentItemOrDefault();
            OrderbookEntry orderToMatchAsk = askOrderIterator.CurrentItemOrDefault();
            
            do
            {
                if (orderToMatchAsk.Current.Price > orderToMatchBid.Current.Price)
                    break;  // No book match candidates.
                var remainingQuantityBid = orderToMatchBid.Current.CurrentQuantity;
                if (remainingQuantityBid == 0)
                {
                    bidOrderIterator.Next();
                    orderToMatchBid = bidOrderIterator.CurrentItemOrDefault();
                    continue;
                }
                var remainingQuantityAsk = orderToMatchAsk.Current.CurrentQuantity;
                if (remainingQuantityAsk == 0)
                {
                    askOrderIterator.Next();
                    orderToMatchAsk = askOrderIterator.CurrentItemOrDefault();
                    continue;
                }
                var fillQuantity = Math.Min(remainingQuantityAsk, remainingQuantityBid);

                orderToMatchBid.Current.DecreaseQuantity(fillQuantity);
                orderToMatchAsk.Current.DecreaseQuantity(fillQuantity);

                // TODO: This is duplicate code in all matching algorithms.
                // Think of refactoring this by including it elsewhere
                var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, AllocationAlgorithm.Fifo, eventTime);
                matchResult = MatchResultCreator.CreateMatchResult(tradeResult, orderToMatchBid, orderToMatchAsk, eventTime);

                // Lets move on!
                if (tradeResult.BuyFill.IsCompleteFill)
                {
                    bidOrderIterator.Next();
                    orderToMatchBid = bidOrderIterator.CurrentItemOrDefault();
                }
                if (tradeResult.SellFill.IsCompleteFill)
                {
                    askOrderIterator.Next();
                    orderToMatchAsk = askOrderIterator.CurrentItemOrDefault();
                }
            }
            while (orderToMatchBid != null && orderToMatchAsk != null);

            return matchResult;
        }
    }
}
