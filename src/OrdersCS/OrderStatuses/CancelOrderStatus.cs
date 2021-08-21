using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders.OrderStatuses
{
    public class CancelOrderStatus : IOrderStatus
    {
        public CancelOrderStatus(IOrderCore orderCore)
        {
            // PROPERTIES //
            CreationTime = DateTime.UtcNow;

            // FIELDS //
            _orderCore = orderCore;
        }

        // PROPERTIES //

        public DateTime CreationTime { get; private set; }
        public long OrderId => _orderCore.OrderId;
        public string Username => _orderCore.Username;
        public int SecurityId => _orderCore.SecurityId;

        // FIELDS //

        private readonly IOrderCore _orderCore;
    }
}
