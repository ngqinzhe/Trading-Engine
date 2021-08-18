using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Limit
    {
        public long Price { get; set; }
        public OrderbookEntry Head { get; set; }
        public OrderbookEntry Tail { get; set; }
        public bool IsEmpty
        {
            get
            {
                return Head == null && Tail == null;
            }
        }
    }
}
