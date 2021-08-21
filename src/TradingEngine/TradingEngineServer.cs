using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using TradingEngineServer.Core.Configuration;
using TradingEngineServer.Logging;
using TradingEngineServer.Exchange;

namespace TradingEngineServer.Core
{
    class TradingEngineServer : BackgroundService, ITradingEngine
    {
        private readonly TradingEngineServerConfiguration _engineConfiguration;
        private readonly ITextLogger _textLogger;
        private readonly IExchange _exchange;

        public TradingEngineServer(IOptions<TradingEngineServerConfiguration> engineConfiguration,
            IExchange exchange,
            ITextLogger textLogger)
        {
            _engineConfiguration = engineConfiguration?.Value ?? throw new ArgumentNullException(nameof(engineConfiguration));
            _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            _textLogger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));

        }

        public Task RunAsync(CancellationToken token) => ExecuteAsync(token);

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _textLogger.Information(nameof(TradingEngineServer), $"Starting Trading Engine");
            while (!stoppingToken.IsCancellationRequested)
            {

            }
            _textLogger.Information(nameof(TradingEngineServer), $"Stopping Trading Engine");
            return Task.CompletedTask;
        }
    }
}
