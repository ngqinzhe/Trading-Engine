using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Fills;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Exchange
{
    public interface IOrderbookRetriever
    {
        bool TryGetOrderbook(Security security, out IMatchingOrderbook orderbook);
    }

    public interface ITradingExchange : IOrderbookRetriever
    {
        int GetExchangeId();
        string GetExchangeName();
    }
}
