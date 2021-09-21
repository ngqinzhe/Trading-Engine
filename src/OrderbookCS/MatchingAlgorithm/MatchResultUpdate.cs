﻿using System;
using TradingEngineServer.OrderbookData;
using TradingEngineServer.Orders;
using TradingEngineServer.Trades;

namespace TradingEngineServer.Orderbook.MatchingAlgorithm
{
    internal class MatchResultUpdate
    {
        public static MatchResult Update(MatchResult matchResult, TradeResult tradeResult, OrderbookEntry orderToMatchBid, OrderbookEntry orderToMatchAsk, DateTime eventTime)
        {
            matchResult.AddTradeResult(tradeResult);
            bool buySideIsAggressor = orderToMatchBid.CreationTime > orderToMatchAsk.CreationTime;
            Limit relevantOrderbookLimit = buySideIsAggressor ? orderToMatchAsk.ParentLimit : orderToMatchBid.ParentLimit;
            var orderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(relevantOrderbookLimit, eventTime);
            matchResult.AddIncrementalOrderbookUpdate(orderbookUpdate);
            return matchResult;
        }
    }
}
