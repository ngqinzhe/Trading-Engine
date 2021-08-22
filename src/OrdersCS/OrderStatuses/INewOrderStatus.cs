using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public interface INewOrderStatus : IOrderStatus
    {
        long Price { get; }
        bool IsBuySide { get; }
        uint Quantity { get; }
    }
}
