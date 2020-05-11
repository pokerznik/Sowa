using ZanP.OrderBooks.Enums;
using System.Collections.Generic;
using ZanP.OrderBooks.Models.Data;

namespace ZanP.OrderBooks.Models.Orders
{
    public abstract class Order
    {
        public Order(OrderType p_type, decimal p_amount)
        {
            type = p_type;
            amount = p_amount;
        }

        public abstract BestPrice GetBestPrice();
        public abstract void Handle(List<Exchange> p_exchanges);

        public OrderType type { get; private set; }
        public decimal amount { get; private set; }
    }
}