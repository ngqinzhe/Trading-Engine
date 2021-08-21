using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orderbook;

namespace TradingEngineServer.Exchange
{
    public class ExchangeConfiguration
    {
        public int ExchangeId { get; set; }
        public List<Security> Securities { get; set; }
    }
}
