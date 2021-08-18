using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using TradingEngineServer.Orderbook;
using TradingEngineServer.Orders;
using TradingEngineServer.Rejects;

namespace OrderbookCSTest
{
    [TestClass]
    public class CoreOrderbookTests
    {
        [TestMethod]
        public void Orderbook_AddSingleOrder_Success()
        {
            // 1
            Orderbook ob = new Orderbook(default);
            ob.AddOrder(new Order(new OrderCore(0, "Test", 1), 1_000, 10, true));

            // 2
            int actual = ob.Count;
            const int expected = 1;

            // 3
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Orderbook_AddTwoOrders_Success()
        {
            // 1
            Orderbook ob = new Orderbook(default);
            ob.AddOrder(new Order(new OrderCore(0, "Test", 1), 1_000, 10, true));
            ob.AddOrder(new Order(new OrderCore(1, "Test", 1), 1_000, 10, false));

            // 2
            int actual = ob.Count;
            const int expected = 2;

            // 3
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Orderbook_AddOrderThenRemoveOrder_Success()
        {
            // 1
            const long orderId = 0;
            Orderbook ob = new Orderbook(default);
            ob.AddOrder(new Order(new OrderCore(orderId, "Test", 1), 1_000, 10, true));
            ob.RemoveOrder(new CancelOrder(new OrderCore(orderId, "Test", 1)));

            // 2
            int actual = ob.Count;
            const int expected = 0;

            // 3
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Orderbook_RemoveNonExistantOrder_RequestRejects()
        {
            // 1
            const long orderId = 0;
            Orderbook ob = new Orderbook(default);
            var cancelOrder = new CancelOrder(new OrderCore(orderId, "Test", 1));
            var res = ob.RemoveOrder(cancelOrder);

            // 2
            var firstReject = res.Rejections.First();
            Rejection expectedRejection = new Rejection(cancelOrder, RejectionReason.OrderNotFound);

            // 3
            Assert.AreEqual(res.HasRejection, true);
            Assert.AreEqual(expectedRejection.RejectionReason, firstReject.RejectionReason);
            Assert.AreEqual(expectedRejection.OrderId, orderId);
        }

        [TestMethod]
        public void Orderbook_AddOrderThenModify_Success()
        {
            // 1
            const long orderId = 0;
            const uint modifyOrderQuantity = 5;
            Orderbook ob = new Orderbook(default);
            _ = ob.AddOrder(new Order(new OrderCore(orderId, "Test", 1), 1_000, 10, true));
            var res = ob.ChangeOrder(new ModifyOrder(new OrderCore(orderId, "Test", 1), 1_000, modifyOrderQuantity, true));

            // 2
            var buyOrders = ob.GetBuyOrders();

            int actual = ob.Count;
            const int expected = 1;

            // 3
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(modifyOrderQuantity, buyOrders[0].Current.InitialQuantity);
            Assert.IsTrue(res.HasModifyOrderStatus);
        }

        [TestMethod]
        public void Orderbook_ModifyNonExistantOrder_RequestRejects()
        {
            // 1
            Orderbook ob = new Orderbook(default);
            _ = ob.AddOrder(new Order(new OrderCore(0, "Test", 1), 1_000, 10, true));
            var res = ob.ChangeOrder(new ModifyOrder(new OrderCore(0, "Test", 1), 1_000, 5, false));

            // 2
            int actual = ob.Count;
            const int expected = 1;

            // 3
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(res.HasRejection);
        }

        [TestMethod]
        public void Orderbook_BidOrders_AreDescending()
        {
            // 1
            Orderbook ob = new Orderbook(default);
            _ = ob.AddOrder(new Order(new OrderCore(0, "Test", 1), 1_000, 10, true));
            _ = ob.AddOrder(new Order(new OrderCore(1, "Test", 1), 999, 10, true));

            // 2
            var bidOrders = ob.GetBuyOrders();
            int actualSize = bidOrders.Count;
            const int expectedSize = 2;

            // 3
            Assert.AreEqual(expectedSize, actualSize);
            Assert.IsTrue(bidOrders[0].Current.Price > bidOrders[^1].Current.Price);
        }

        [TestMethod]
        public void Orderbook_AskOrders_AreAscending()
        {
            // 1
            Orderbook ob = new Orderbook(default);
            _ = ob.AddOrder(new Order(new OrderCore(0, "Test", 1), 1_000, 10, false));
            _ = ob.AddOrder(new Order(new OrderCore(1, "Test", 1), 1_001, 10, false));

            // 2
            var askOrders = ob.GetAskOrders();
            int actual = ob.Count;
            const int expected = 2;

            // 3
            Assert.AreEqual(expected, actual);
            Assert.IsTrue(askOrders[0].Current.Price < askOrders[^1].Current.Price);
        }
    }
}
