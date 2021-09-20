namespace TradingEngineServer.Orderbook.MatchingAlgorithm 
{
    public class OrderBookUpdate
    {
        public static void Update(OrderbookEntry orderToMatchBid, OrderbookEntry orderToMatchAsk, 
            decimal fillQuantity, DateTime eventTime, MatchResult matchResult) {
            var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, AllocationAlgorithm.ProRata, eventTime);
            matchResult.AddTradeResult(tradeResult);
            bool buySideIsAggressor = orderToMatchBid.CreationTime > orderToMatchAsk.CreationTime;
            Limit relevantOrderbookLimit = buySideIsAggressor ? orderToMatchAsk.ParentLimit : orderToMatchBid.ParentLimit;
            var orderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(relevantOrderbookLimit, eventTime);
            matchResult.AddIncrementalOrderbookUpdate(orderbookUpdate);
        }
    }
}