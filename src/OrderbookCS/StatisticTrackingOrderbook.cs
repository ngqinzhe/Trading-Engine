using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Extensions.List;
using TradingEngineServer.Fills;
using TradingEngineServer.Instrument;
using TradingEngineServer.Orders;
using TradingEngineServer.Trades;

namespace TradingEngineServer.Orderbook
{
    public interface IStatisticsTrackingOrderbook
    {
        SecurityStatistics OrderbookStatistics { get; }
    }

    /// <summary>
    /// Wrapper around a matching orderbook to track statistics.
    /// </summary>
    public class StatisticTrackingOrderbook : IMatchingOrderbook, IStatisticsTrackingOrderbook
    {
        public StatisticTrackingOrderbook(Security security)
        {
            _underlyingOrderbook = OrderbookFactory.CreateOrderbook(security);
            _securityStatistics = new SecurityStatistics(security);
        }

        public void AddOrder(Order order)
        {
            _underlyingOrderbook.AddOrder(order);
        }

        public void ChangeOrder(ModifyOrder modifyOrder)
        {
            _underlyingOrderbook.ChangeOrder(modifyOrder);
        }

        public void RemoveOrder(CancelOrder cancelOrder)
        {
            _underlyingOrderbook.RemoveOrder(cancelOrder);
        }

        public OrderbookSpread GetSpread()
        {
            return _underlyingOrderbook.GetSpread();
        }

        public bool TryGetOrder(long orderId, out Order order)
        {
            return _underlyingOrderbook.TryGetOrder(orderId, out order);
        }

        public bool ContainsOrder(long orderId)
        {
            return _underlyingOrderbook.ContainsOrder(orderId);
        }

        public int Count => _underlyingOrderbook.Count;

        public MatchResult Match()
        {
            var results = _underlyingOrderbook.Match();
            LogMatchStatistics(_securityStatistics, results);
            return results;
        }

        public SecurityStatistics OrderbookStatistics
        {
            get
            {
                return _securityStatistics;
            }
        }

        private static void LogMatchStatistics(SecurityStatistics securityStatistics, MatchResult matchResult)
        {
            LogTradeStatistics(securityStatistics, matchResult.Trades);
            LogFillStatistics(securityStatistics, matchResult.Fills);
        }

        private static void LogFillStatistics(SecurityStatistics securityStatistics, IReadOnlyList<Fill> fills)
        {
            // TODO: Not much to do here.
        }

        private static void LogTradeStatistics(SecurityStatistics securityStatistics, IReadOnlyList<Trade> trades)
        {
            trades.ForEach(t =>
            {
                securityStatistics.AddVolume(t.Quantity);
                securityStatistics.AcceptPrice(t.Price);
            });
        }

        // FIELDS //
        private readonly IMatchingOrderbook _underlyingOrderbook;
        private readonly SecurityStatistics _securityStatistics;
    }
}
