using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public interface IOrderStatus : IOrderCore
    {
        DateTime CreationTime { get; }
    }
}
