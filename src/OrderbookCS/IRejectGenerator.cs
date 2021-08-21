using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Orderbook
{
    public interface IRejectGenerator
    {
        bool TryRejectNewOrder(Order order, IReadOnlyOrderbook orderbook, out Rejection rejection);
        bool TryRejectModifyOrder(ModifyOrder modifyOrder, IReadOnlyOrderbook orderbook, out Rejection rejection);
        bool TryRejectCancelOrder(CancelOrder cancelOrder, IReadOnlyOrderbook orderbook, out Rejection rejection);
    }
}
