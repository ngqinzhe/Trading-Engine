using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;
using TradingEngineServer.Price;

namespace TradingEngineServer.OrderbookData
{
    public sealed class OrderbookUtilities
    {
        public static IncrementalOrderbookUpdate CreateIncrementalOrderbookUpdate(Limit limit, DateTime eventTime)
        {
            OrderbookEntry headEntry = limit.Head;

            uint orderCount = limit.GetLevelOrderCount();
            uint orderQuantity = limit.GetLevelOrderQuantity();
            bool containsSingleOrder = orderQuantity == 1;
            long price = limit.IsEmpty ? PriceConstants.InvalidPrice : limit.Price;
            int securityId = limit.IsEmpty ? SecurityConstants.InvalidSecurityId : headEntry.Current.SecurityId;
            IncrementalOrderbookUpdateType updateType = limit.IsEmpty ? IncrementalOrderbookUpdateType.Delete :
                containsSingleOrder ? IncrementalOrderbookUpdateType.New : 
                IncrementalOrderbookUpdateType.Change;

            return new IncrementalOrderbookUpdate()
            {
                EventTime = eventTime,
                SecurityId = securityId,
                UpdateType = updateType,
                OrderCount = orderCount,
                Price = price,
                Quantity = orderQuantity,
            };
        }
    }
}
