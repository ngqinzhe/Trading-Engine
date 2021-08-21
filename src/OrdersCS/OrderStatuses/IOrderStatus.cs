using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public interface IOrderStatus
    {
        DateTime CreationTime { get; }
        public long OrderId { get; }
        public string Username { get; }
        public int SecurityId { get; }
    }
}
