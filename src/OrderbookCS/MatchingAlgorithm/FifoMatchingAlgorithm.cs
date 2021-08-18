using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingEngineServer.Fills;
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

            if (!bids.Any() || !asks.Any())
                return matchResult; // Can't match without both sides.

            OrderbookEntry orderToMatchBid = bids.First();
            OrderbookEntry orderToMatchAsk = asks.First();
            
            do
            {
                if (orderToMatchAsk.Current.Price > orderToMatchBid.Current.Price)
                    break;  // No book match candidates.
                var remainingQuantityBid = orderToMatchBid.Current.CurrentQuantity;
                if (remainingQuantityBid == 0)
                {
                    orderToMatchBid = orderToMatchBid.Next;
                    continue;
                }
                var remainingQuantityAsk = orderToMatchAsk.Current.CurrentQuantity;
                if (remainingQuantityAsk == 0)
                {
                    orderToMatchAsk = orderToMatchAsk.Next;
                    continue;
                }
                var fillQuantity = Math.Min(remainingQuantityAsk, remainingQuantityBid);

                orderToMatchBid.Current.DecreaseQuantity(fillQuantity);
                orderToMatchAsk.Current.DecreaseQuantity(fillQuantity);

                var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, FillAllocationAlgorithm.Fifo, eventTime);
                matchResult.AddTradeResult(tradeResult);
                var orderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(orderToMatchBid.ParentLimit, eventTime);
                matchResult.AddIncrementalOrderbookUpdate(orderbookUpdate);

                // Lets move on!
                if (tradeResult.BuyFill.IsCompleteFill)
                    orderToMatchBid = orderToMatchBid.Next;
                if (tradeResult.SellFill.IsCompleteFill)
                    orderToMatchAsk = orderToMatchAsk.Next;
            }
            while (orderToMatchBid != null && orderToMatchAsk != null);

            return matchResult;
        }
    }
}
