using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class OrderbookEntryData
    {
        public long OrderId { get; set; }
        public uint Quantity { get; set; }
    }
}
