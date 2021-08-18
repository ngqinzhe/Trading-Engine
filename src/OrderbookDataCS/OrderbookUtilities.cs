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
            uint levelQuantity = 0, orderCount = 0;
            OrderbookEntry headEntry = limit.Head;
            OrderbookEntry headEntryPointer = headEntry;

            while (headEntryPointer != null)
            {
                orderCount++;
                levelQuantity += headEntryPointer.Current.CurrentQuantity;
                headEntryPointer = headEntryPointer.Next;
            }

            long price = limit.IsEmpty ? PriceConstants.InvalidPrice : limit.Price;
            int securityId = limit.IsEmpty ? SecurityConstants.InvalidSecurityId : headEntry.Current.SecurityId;

            bool singleOrderOnLevel = headEntry != null && limit.Head == limit.Tail;
            IncrementalOrderbookUpdateType updateType = limit.IsEmpty ? IncrementalOrderbookUpdateType.Delete :
                singleOrderOnLevel ? IncrementalOrderbookUpdateType.New : 
                IncrementalOrderbookUpdateType.Change;

            return new IncrementalOrderbookUpdate()
            {
                EventTime = eventTime,
                SecurityId = securityId,
                UpdateType = updateType,
                OrderCount = orderCount,
                Price = price,
                Quantity = levelQuantity,
            };
        }
    }
}
