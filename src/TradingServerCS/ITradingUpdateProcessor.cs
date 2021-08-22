using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TradingEngineServer.Orders;

namespace TradingEngineServer.Server
{
    public interface ITradingUpdateProcessor
    {
        Task ProcessOrderAsync(Order order, TradingServerContext context);
        Task ProcessOrderAsync(ModifyOrder modifyOrder, TradingServerContext context);
        Task ProcessOrderAsync(CancelOrder cancelOrder, TradingServerContext context);
    }
}
