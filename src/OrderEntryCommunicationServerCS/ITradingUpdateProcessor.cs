﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;
using TradingEngineServer.Orders.OrderStatuses;

namespace TradingEngineServer.OrderEntryCommunicationServer
{
    public interface ITradingUpdateProcessor
    {
        Task<ExchangeResult> ProcessOrderAsync(Order order);
        Task<ExchangeResult> ProcessOrderAsync(ModifyOrder modifyOrder);
        Task<ExchangeResult> ProcessOrderAsync(CancelOrder cancelOrder);
        Task CancelAllAsync(List<IOrderStatus> orderIds);
    }
}
