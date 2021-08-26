using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using TradingEngineServer.Fills;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Server
{
    public interface ITradingServer
    {
        Task PublishRejectAsync(Rejection rejection, CancellationToken token);
        Task PublishFillsAsync(IReadOnlyList<Fill> fills, CancellationToken token);
    }
}
