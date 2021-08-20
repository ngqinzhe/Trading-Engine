using System;
using System.Collections.Generic;
using System.Text;
using TradingEngineServer.Extensions;
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
        public StatisticTrackingOrderbook(Security security, Fills.FillAllocationAlgorithm faa)
        {
            _underlyingOrderbook = OrderbookFactory.CreateOrderbook(security, faa);
            _securityStatistics = new SecurityStatistics(security);
        }

        public OrderbookResult AddOrder(Order order)
        {
            return _underlyingOrderbook.AddOrder(order);
        }

        public OrderbookResult ChangeOrder(ModifyOrder modifyOrder)
        {
            return _underlyingOrderbook.ChangeOrder(modifyOrder);
        }

        public bool ContainsOrder(long orderId)
        {
            return _underlyingOrderbook.ContainsOrder(orderId);
        }

        public OrderbookSpread GetSpread()
        {
            return _underlyingOrderbook.GetSpread();
        }

        public OrderbookResult RemoveOrder(CancelOrder cancelOrder)
        {
            return _underlyingOrderbook.RemoveOrder(cancelOrder);
        }

        public int Count => _underlyingOrderbook.Count;

        public (MatchResult MatchResult, OrderbookResult OrderbookResult) Match()
        {
            var results = _underlyingOrderbook.Match();
            LogMatchStatistics(_securityStatistics, results.MatchResult);
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
                securityStatistics.TrySetHigh(t.Price);
                securityStatistics.TrySetLow(t.Price);
                securityStatistics.SetLast(t.Price);
                securityStatistics.IncrementTradeCount();
            });
        }

        // FIELDS //
        private readonly IMatchingOrderbook _underlyingOrderbook;
        private readonly SecurityStatistics _securityStatistics;
    }
}
