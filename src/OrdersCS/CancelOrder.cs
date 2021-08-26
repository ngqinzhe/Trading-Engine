using TradingEngineServer.Common;

namespace TradingEngineServer.Orders
{
    public class CancelOrder : IOrderCore, IPrototype<CancelOrder>
    {
        public CancelOrder(IOrderCore orderBase)
        {
            _orderBase = orderBase;
        }

        // PROPERTIES //
        public long OrderId => _orderBase.OrderId;
        public string Username => _orderBase.Username;
        public int SecurityId => _orderBase.SecurityId;

        // IPROTOTYPE //
        public CancelOrder Clone()
        {
            return Cloner<CancelOrder>.CreateDeepCopy(this);
        }

        // TOSTRING //
        public override string ToString()
        {
            return $"[{_orderBase}]";
        }

        private readonly IOrderCore _orderBase;
    }
}