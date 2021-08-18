using System;
using System.Collections.Generic;

namespace TradingEngineServer.OrderbookData
{
    public class IncrementalOrderbookUpdate
    {
        public DateTime EventTime { get; set; }
        public OrderbookEntryType EntryType { get; set; }
        public IncrementalOrderbookUpdateType UpdateType { get; set; }
        public int SecurityId { get; set; }
        public uint Quantity { get; set; }
        public long Price { get; set; }
        public uint OrderCount { get; set; }
        public List<IncrementalOrderbookUpdateEntry> IncrementalOrderbookUpdateEntries { get; set; }
    }
}
