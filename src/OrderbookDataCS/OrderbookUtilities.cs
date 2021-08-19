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
            bool containsSingleOrder = orderCount == 1;
            long price = limit.IsEmpty ? PriceConstants.InvalidPrice : limit.Price;
            int securityId = limit.IsEmpty ? SecurityConstants.InvalidSecurityId : headEntry.Current.SecurityId;
            IncrementalOrderbookUpdateType updateType = limit.IsEmpty ? IncrementalOrderbookUpdateType.Delete :
                containsSingleOrder ? IncrementalOrderbookUpdateType.New :
                IncrementalOrderbookUpdateType.Change;
            OrderbookEntryType entryType = limit.Side == Side.Unknown ? OrderbookEntryType.Null :
                limit.Side == Side.Bid ? OrderbookEntryType.Bid :
                OrderbookEntryType.Ask;

            return new IncrementalOrderbookUpdate()
            {
                EventTime = eventTime,
                SecurityId = securityId,
                EntryType = entryType,
                UpdateType = updateType,
                OrderCount = orderCount,
                Price = price,
                Quantity = orderQuantity,
                IncrementalOrderbookUpdateEntries = GenerateIncrementalOrderbookUpdateEntries(limit),
            };
        }

        private static List<IncrementalOrderbookUpdateEntry> GenerateIncrementalOrderbookUpdateEntries(Limit limit)
        {
            var orderMetaDatas = limit.GetLevelOrderRecords();
            List<IncrementalOrderbookUpdateEntry> orderbookUpdateEntries = new List<IncrementalOrderbookUpdateEntry>(orderMetaDatas.Count);
            foreach (var orderMeta in orderMetaDatas)
                orderbookUpdateEntries.Add(new IncrementalOrderbookUpdateEntry(AnonymizeOrderId(orderMeta.OrderId), orderMeta.Quantity));
            return orderbookUpdateEntries;
        }

        private static long AnonymizeOrderId(long orderId)
        {
            return orderId.GetHashCode();
        }
    }
}
