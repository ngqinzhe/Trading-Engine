using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingEngineServer.Fills;
using TradingEngineServer.Trades;

namespace TradingEngineServer.Orderbook.MatchingAlgorithm
{
    public class LifoMatchingAlgorithm : IMatchingAlgorithm
    {
        public static IMatchingAlgorithm MatchingAlgorithm { get; } = new LifoMatchingAlgorithm();

        public MatchResult Match(IEnumerable<OrderbookEntry> bids, IEnumerable<OrderbookEntry> asks)
        {
            var eventTime = DateTime.UtcNow;
            var matchResult = new MatchResult();

            if (!bids.Any() || !asks.Any())
                return matchResult; // Can't match without both sides.

            // Cannot guarantee that bids are ordered in last-in-first-out order. Let's go ahead and do that.
            var reorderedBids = bids.GroupBy(x => x.ParentLimit.Price).Select(x => x.OrderByDescending(oe => oe.CreationTime))
                .SelectMany(x => x);
            var reorderedAsks = asks.GroupBy(x => x.ParentLimit.Price).Select(x => x.OrderByDescending(oe => oe.CreationTime))
                .SelectMany(x => x);

            OrderbookEntry orderToMatchBid = reorderedBids.First();
            OrderbookEntry orderToMatchAsk = reorderedAsks.First();

            do
            {
                if (orderToMatchAsk.Current.Price > orderToMatchBid.Current.Price)
                    break;  // No book match candidates.
                var remainingQuantityBid = orderToMatchBid.Current.CurrentQuantity;
                if (remainingQuantityBid == 0)
                {
                    orderToMatchBid = orderToMatchBid.Previous;
                    continue;
                }
                var remainingQuantityAsk = orderToMatchAsk.Current.CurrentQuantity;
                if (remainingQuantityAsk == 0)
                {
                    orderToMatchAsk = orderToMatchAsk.Previous;
                    continue;
                }
                var fillQuantity = Math.Min(remainingQuantityAsk, remainingQuantityBid);

                orderToMatchBid.Current.DecreaseQuantity(fillQuantity);
                orderToMatchAsk.Current.DecreaseQuantity(fillQuantity);

                var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, FillAllocationAlgorithm.Lifo, eventTime);
                matchResult.AddTradeResult(tradeResult);

                // Lets move on. Or better said, let's move back!
                if (tradeResult.BuyFill.IsCompleteFill)
                    orderToMatchBid = orderToMatchBid.Previous;
                if (tradeResult.SellFill.IsCompleteFill)
                    orderToMatchAsk = orderToMatchAsk.Previous;
            }
            while (orderToMatchBid != null && orderToMatchAsk != null);

            return matchResult;
        }
    }
}
