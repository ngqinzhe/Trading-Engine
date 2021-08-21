using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using TradingEngineServer.Fills;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;

namespace TradingEngineServer.Exchange
{
    public class Exchange : IExchange
    {
        public Exchange(IOptions<ExchangeConfiguration> exchangeConfiguration)
        {
            var ec = exchangeConfiguration.Value ?? throw new ArgumentNullException(nameof(exchangeConfiguration));
            _exchangeId = ec.ExchangeId;
            foreach (var security in ec.Securities)
                _orderbooks.Add(security, OrderbookFactory.CreateOrderbook(security, security.AllocationAlgorithm));
        }

        // IExchange // 
        public int GetExchangeId()
        {
            return _exchangeId;
        }

        public bool TryGetOrderbook(Security security, out IMatchingOrderbook orderbook)
        {
            return _orderbooks.TryGetValue(security, out orderbook);
        }

        private readonly int _exchangeId;
        private readonly Dictionary<Security, IMatchingOrderbook> _orderbooks = new Dictionary<Security, IMatchingOrderbook>(SecurityComparer.Comparer);
    }
}
