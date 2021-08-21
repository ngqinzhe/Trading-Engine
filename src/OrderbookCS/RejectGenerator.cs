using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Orders;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Orderbook
{
    public class RejectGenerator : IRejectGenerator
    {
        public bool TryRejectCancelOrder(CancelOrder cancelOrder, IOrderEntryOrderbook orderbook, out Rejection rejection)
        {
            if (!orderbook.ContainsOrder(cancelOrder.OrderId))
            {
                rejection = RejectionCreator.GenerateOrderCoreReject(cancelOrder, RejectionReason.OrderNotFound);
                return true;
            }
            rejection = null;
            return false;
        }

        public bool TryRejectModifyOrder(ModifyOrder modifyOrder, IOrderEntryOrderbook orderbook, out Rejection rejection)
        {
            if (!orderbook.ContainsOrder(modifyOrder.OrderId))
            {
                rejection = RejectionCreator.GenerateOrderCoreReject(modifyOrder, RejectionReason.OrderNotFound);
                return true;
            }
            else if (orderbook.TryGetOrder(modifyOrder.OrderId, out var order))
            {
                if (order.IsBuySide != modifyOrder.IsBuySide)
                {
                    rejection = RejectionCreator.GenerateOrderCoreReject(modifyOrder, RejectionReason.AttemptingToModifyWrongSide);
                    return true;
                }
            }
            rejection = null;
            return false;
        }

        public bool TryRejectNewOrder(Order order, IOrderEntryOrderbook orderbook, out Rejection rejection)
        {
            if (orderbook.ContainsOrder(order.OrderId))
            {
                rejection = RejectionCreator.GenerateOrderCoreReject(order, RejectionReason.OrderIdAlreadyPresent);
                return true;
            }
            rejection = null;
            return false;
        }
    }
}
