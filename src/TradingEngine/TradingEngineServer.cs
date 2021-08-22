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
using TradingEngineServer.Server;
using TradingEngineServer.Orders;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Core
{
    class TradingEngineServer : BackgroundService, ITradingEngine, ITradingUpdateProcessor
    {
        private readonly TradingEngineServerConfiguration _engineConfiguration;
        private readonly ITextLogger _textLogger;
        private readonly ITradingExchange _exchange;

        public TradingEngineServer(IOptions<TradingEngineServerConfiguration> engineConfiguration,
            ITradingExchange exchange,
            ITextLogger textLogger)
        {
            _engineConfiguration = engineConfiguration.Value ?? throw new ArgumentNullException(nameof(engineConfiguration));
            _exchange = exchange ?? throw new ArgumentNullException(nameof(exchange));
            _textLogger = textLogger ?? throw new ArgumentNullException(nameof(textLogger));

        }

        public async Task ProcessOrderAsync(Order order, TradingServerContext context)
        {
            _textLogger.Information(nameof(TradingEngineServer), $"Handling NewOrder: {order}");
            if (_exchange.TryGetOrderbook(order.SecurityId, out var orderbook))
            {
                if (RejectGenerator.TryRejectNewOrder(order, orderbook, out var rejection))
                {
                    await context.TradingServer.PublishRejectAsync(rejection, context.CancellationToken);
                }
                else
                {
                    orderbook.AddOrder(order);
                    var matchResults = orderbook.Match();
                    await context.TradingServer.PublishFillsAsync(matchResults.Fills, context.CancellationToken);
                }
            }
            else
            {
                await context.TradingServer.PublishRejectAsync(RejectionCreator.GenerateOrderCoreReject(order, RejectionReason.InstrumentNotFound), 
                    context.CancellationToken);
            }
        }

        public async Task ProcessOrderAsync(ModifyOrder modifyOrder, TradingServerContext context)
        {
            _textLogger.Information(nameof(TradingEngineServer), $"Handling ModifyOrder: {modifyOrder}");
            if (_exchange.TryGetOrderbook(modifyOrder.SecurityId, out var orderbook))
            {
                if (RejectGenerator.TryRejectModifyOrder(modifyOrder, orderbook, out var rejection))
                {
                    await context.TradingServer.PublishRejectAsync(rejection, context.CancellationToken);
                }
                else
                {
                    orderbook.ChangeOrder(modifyOrder);
                    var matchResults = orderbook.Match();
                    await context.TradingServer.PublishFillsAsync(matchResults.Fills, context.CancellationToken);
                }
            }
            else
            {
                await context.TradingServer.PublishRejectAsync(RejectionCreator.GenerateOrderCoreReject(modifyOrder, RejectionReason.InstrumentNotFound),
                    context.CancellationToken);
            }
        }

        public async Task ProcessOrderAsync(CancelOrder cancelOrder, TradingServerContext context)
        {
            _textLogger.Information(nameof(TradingEngineServer), $"Handling CancelOrder: {cancelOrder}");
            if (_exchange.TryGetOrderbook(cancelOrder.SecurityId, out var orderbook))
            {
                if (RejectGenerator.TryRejectCancelOrder(cancelOrder, orderbook, out var rejection))
                {
                    await context.TradingServer.PublishRejectAsync(rejection, context.CancellationToken);
                }
                else
                {
                    orderbook.RemoveOrder(cancelOrder);
                }
            }
            else
            {
                await context.TradingServer.PublishRejectAsync(RejectionCreator.GenerateOrderCoreReject(cancelOrder, RejectionReason.InstrumentNotFound),
                    context.CancellationToken);
            }
        }

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
