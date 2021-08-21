using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Core.Configuration
{
    public class TradingEngineServerConfiguration
    {
        public TradingEngineServerSettings TradingEngineServerSettings { get; set; }
    }

    public class TradingEngineServerSettings
    {
        public int Port { get; set; }
    }
}
