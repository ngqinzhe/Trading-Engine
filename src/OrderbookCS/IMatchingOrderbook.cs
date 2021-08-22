using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orderbook
{
    public interface IMatchingOrderbook : IOrderEntryOrderbook
    {
        MatchResult Match();
    }
}
