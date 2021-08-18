using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TradingEngineServer.Instrument;
using TradingEngineServer.OrderbookData;
using TradingEngineServer.Orders;
using TradingEngineServer.Price;

namespace OrderbookDataCSTests
{
    [TestClass]
    public class IncrementalOrderbookUpdateTests
    {
        [TestMethod]
        public void OrderbookLevelDeleted_UpdateTypeDelete()
        {
            // Empty limit (empty price level with no orderbook entries)
            var limit = new Limit();
            var incrementalOrderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(limit, DateTime.UtcNow);

            Assert.AreEqual(IncrementalOrderbookUpdateType.Delete, incrementalOrderbookUpdate.UpdateType);
            Assert.AreEqual(SecurityConstants.InvalidSecurityId, incrementalOrderbookUpdate.SecurityId);
            Assert.AreEqual(0u, incrementalOrderbookUpdate.OrderCount);
            Assert.AreEqual(PriceConstants.InvalidPrice, incrementalOrderbookUpdate.Price);
            Assert.AreEqual(0u, incrementalOrderbookUpdate.Quantity);
        }

        [TestMethod]
        public void OrderbookLevelDeleted_UpdateTypeNew()
        {
            const long price = 10;
            const int securityId = 1;
            Limit limit = CreateLimitWithOneEntry(price, securityId);

            var incrementalOrderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(limit, DateTime.UtcNow);

            Assert.AreEqual(IncrementalOrderbookUpdateType.New, incrementalOrderbookUpdate.UpdateType);
            Assert.AreEqual(securityId, incrementalOrderbookUpdate.SecurityId);
            Assert.AreEqual(1u, incrementalOrderbookUpdate.OrderCount);
            Assert.AreEqual(price, incrementalOrderbookUpdate.Price);
            Assert.AreEqual(10u, incrementalOrderbookUpdate.Quantity);
        }

        private static Limit CreateLimitWithOneEntry(long price, int securityId)
        {
            var limit = new Limit()
            {
                Price = price,
            };
            var orderbookEntry = new OrderbookEntry(new Order(new OrderCore(0, string.Empty, securityId), price, 10, true), limit);
            limit.Head = orderbookEntry;
            limit.Tail = orderbookEntry;
            return limit;
        }

        [TestMethod]
        public void OrderbookLevelDeleted_UpdateTypeChange()
        {
            const long price = 10;
            const int securityId = 1;
            Limit limit = CreateLimitWithTwoEntries(price, securityId);

            var incrementalOrderbookUpdate = OrderbookUtilities.CreateIncrementalOrderbookUpdate(limit, DateTime.UtcNow);

            Assert.AreEqual(IncrementalOrderbookUpdateType.Change, incrementalOrderbookUpdate.UpdateType);
            Assert.AreEqual(securityId, incrementalOrderbookUpdate.SecurityId);
            Assert.AreEqual(2u, incrementalOrderbookUpdate.OrderCount);
            Assert.AreEqual(price, incrementalOrderbookUpdate.Price);
            Assert.AreEqual(15u, incrementalOrderbookUpdate.Quantity);
        }

        private static Limit CreateLimitWithTwoEntries(long price, int securityId)
        {
            var limit = new Limit()
            {
                Price = price,
            };
            var orderbookEntry = new OrderbookEntry(new Order(new OrderCore(0, string.Empty, securityId), price, 10, true), limit);
            var orderbookEntryTail = new OrderbookEntry(new Order(new OrderCore(0, string.Empty, securityId), price, 5, true), limit);
            orderbookEntry.Next = orderbookEntryTail;
            limit.Head = orderbookEntry;
            orderbookEntryTail.Previous = limit.Head;
            limit.Tail = orderbookEntryTail;
            return limit;
        }
    }
}
