using System;
using System.Collections.Generic;
using System.Text;

namespace TradingEngineServer.Orders
{
    public class Limit
    {
        public long Price { get; set; }
        public OrderbookEntry Head { get; set; }
        public OrderbookEntry Tail { get; set; }
        public uint GetLevelOrderCount()
        {
            uint orderCount = 0;
            OrderbookEntry headPointer = Head;
            while (headPointer != null)
            {
                if (headPointer.Current.CurrentQuantity != 0)
                    orderCount++;
                headPointer = headPointer.Next;
            }
            return orderCount;
        }
        public uint GetLevelOrderQuantity()
        {
            uint orderQuantity = 0;
            OrderbookEntry headPointer = Head;
            while (headPointer != null)
            {
                orderQuantity += headPointer.Current.CurrentQuantity;
                headPointer = headPointer.Next;
            }
            return orderQuantity;
        }
        public List<OrderbookEntryData> GetLevelOrderMetaData()
        {
            List<OrderbookEntryData> orderMetaData = new List<OrderbookEntryData>();
            OrderbookEntry headPointer = Head;
            while (headPointer != null)
            {
                if (headPointer.Current.CurrentQuantity != 0)
                    orderMetaData.Add(new OrderbookEntryData()
                    {
                        OrderId = headPointer.Current.OrderId,
                        Quantity = headPointer.Current.CurrentQuantity,
                    });
                headPointer = headPointer.Next;
            }
            return orderMetaData;
        }
        public bool IsEmpty
        {
            get
            {
                return Head == null && Tail == null;
            }
        }
        public Side Side
        {
            get
            {
                if (IsEmpty)
                    return Side.Unknown;
                else
                {
                    return Head.Current.IsBuySide ? Side.Bid : Side.Ask;
                }
            }
        }
    }
}
