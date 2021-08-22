using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public interface IOrderStatus
    {
        DateTime CreationTime { get; }
        long OrderId { get; }
        string Username { get; }
        int SecurityId { get; }
    }
}
