<<<<<<< HEAD
﻿using System;
using TradingEngineServer.OrderbookData;
using TradingEngineServer.Orders;
using TradingEngineServer.Trades;

namespace TradingEngineServer.Orderbook.MatchingAlgorithm
{
    internal class OrderbookUpdate
    {
        public static void Update(MatchResult matchResult, TradeResult tradeResult, OrderbookEntry orderToMatchBid, OrderbookEntry orderToMatchAsk, DateTime eventTime)
        {
=======
namespace TradingEngineServer.Orderbook.MatchingAlgorithm 
{
    public class OrderBookUpdate
    {
        public static void Update(OrderbookEntry orderToMatchBid, OrderbookEntry orderToMatchAsk, 
            decimal fillQuantity, DateTime eventTime, MatchResult matchResult) {
            var tradeResult = TradeUtilities.CreateTradeAndFills(orderToMatchBid.Current, orderToMatchAsk.Current,
                    fillQuantity, AllocationAlgorithm.ProRata, eventTime);
>>>>>>> 4a4d7985e67493fdb53cf976f4822cf6bd706177
            matchResult.AddTradeResult(tradeResult);
            bool buySideIsAggressor = orderToMatchBid.CreationTime > orderToMatchAsk.CreationTime;
            Limit relevantOrderbookLimit = buySideIsAggressor ? orderToMatchAsk.ParentLimit : orderToMatchBid.ParentLimit;
            var orderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(relevantOrderbookLimit, eventTime);
            matchResult.AddIncrementalOrderbookUpdate(orderbookUpdate);
        }
    }
<<<<<<< HEAD
}
=======
}
>>>>>>> 4a4d7985e67493fdb53cf976f4822cf6bd706177
