using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public interface IModifyOrderStatus : IOrderStatus
    {
        long Price { get; }
        uint Quantity { get; }
        bool IsBuySide { get; }
    }
}
