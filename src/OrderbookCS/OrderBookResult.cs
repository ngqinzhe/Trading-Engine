using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TradingEngineServer.Orders.OrderStatuses;
using TradingEngineServer.Rejects;

namespace TradingEngineServer.Orderbook
{
    public class OrderbookResult
    {
        // METHODS //
        public void Merge(OrderbookResult obr)
        {
            AddRejections(obr.Rejections);
        }

        public void AddRejections(IReadOnlyList<Rejection> ros)
        {
            _rejections.AddRange(ros);
        }

        public void AddRejection(Rejection r)
        {
            _rejections.Add(r);
        }

        // GETTERS //

        public IReadOnlyList<Rejection> Rejections
        {
            get
            {
                return new ReadOnlyCollection<Rejection>(_rejections);
            }
        }


        // PROPERTIES //

        public bool HasRejection
        {
            get
            {
                return _rejections.Any();
            }
        }

        // PRIVATE // 
        private List<Rejection> _rejections = new List<Rejection>();
    }
}
