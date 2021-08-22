using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace TradingEngineServer.Server
{
    public record TradingServerContext(ITradingServer TradingServer, CancellationToken CancellationToken);
}
