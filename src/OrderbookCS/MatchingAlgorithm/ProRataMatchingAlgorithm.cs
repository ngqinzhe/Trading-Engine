﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using TradingEngineServer.Trades;
using TradingEngineServer.Orderbook.MatchingAlgorithm.OrderbookIterator;
using TradingEngineServer.Orders;
using TradingEngineServer.OrderbookData;
using TradingEngineServer.Instrument;

namespace TradingEngineServer.Orderbook.MatchingAlgorithm
{
    public class ProRataMatchingAlgorithm : IMatchingAlgorithm
    {
        public static IMatchingAlgorithm MatchingAlgorithm { get; } = new ProRataMatchingAlgorithm();

        public MatchResult Match(IEnumerable<OrderbookEntry> bids, IEnumerable<OrderbookEntry> asks)
        {
            var eventTime = DateTime.UtcNow;
            var matchResult = new MatchResult();

            if (!bids.Any() || !asks.Any())
                return matchResult; // Can't match without both sides.

            var reorderedBids = bids.GroupBy(x => x.ParentLimit.Price)
                .Select(x => x.OrderByDescending(x => x.Current.InitialQuantity).ThenBy(x => x.CreationTime))
                .SelectMany(x => x);

            var reorderedAsks = asks.GroupBy(x => x.ParentLimit.Price)
                .Select(x => x.OrderByDescending(x => x.Current.InitialQuantity).ThenBy(x => x.CreationTime))
                .SelectMany(x => x);

            var bidIterator = new OrderbookEntryIterator(reorderedBids);
            var askIterator = new OrderbookEntryIterator(reorderedAsks);

            OrderbookEntry orderToMatchBid = bidIterator.CurrentItemOrDefault();
            OrderbookEntry orderToMatchAsk = askIterator.CurrentItemOrDefault();
            
            do
            {
                if (orderToMatchAsk.Current.Price > orderToMatchBid.Current.Price)
                    break;  // No book match candidates.
                var remainingQuantityBid = orderToMatchBid.Current.CurrentQuantity;
                if (remainingQuantityBid == 0)
                {
                    bidIterator.Next();
                    orderToMatchBid = bidIterator.CurrentItemOrDefault();
                    continue;
                }
                var remainingQuantityAsk = orderToMatchAsk.Current.CurrentQuantity;
                if (remainingQuantityAsk == 0)
                {
                    askIterator.Next();
                    orderToMatchAsk = askIterator.CurrentItemOrDefault();
                    continue;
                }
                var fillQuantity = Math.Min(remainingQuantityAsk, remainingQuantityBid);

                orderToMatchBid.Current.DecreaseQuantity(fillQuantity);
                orderToMatchAsk.Current.DecreaseQuantity(fillQuantity);
<<<<<<< HEAD

                // TODO: This is duplicate code in all matching algorithms.
                // Think of refactoring this by including it elsewhere
                var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, AllocationAlgorithm.ProRata, eventTime);
                OrderbookUpdate.Update(matchResult, tradeResult, orderToMatchBid, orderToMatchAsk, eventTime);
=======
                // Replaced duplicate code
                OrderbookUpdate.Update(orderToMatchBid, orderToMatchAsk, fillQuantity, eventTime, matchResult);
>>>>>>> 4a4d7985e67493fdb53cf976f4822cf6bd706177

                if (tradeResult.BuyFill.IsCompleteFill)
                {
                    bidIterator.Next();
                    orderToMatchBid = bidIterator.CurrentItemOrDefault();
                }
                if (tradeResult.SellFill.IsCompleteFill)
                {
                    askIterator.Next();
                    orderToMatchAsk = askIterator.CurrentItemOrDefault();
                }
            }
            while (orderToMatchBid != null && orderToMatchAsk != null);

            return matchResult;
        }
    }
}
