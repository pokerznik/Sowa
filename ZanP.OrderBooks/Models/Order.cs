using ZanP.OrderBooks.Enums;

namespace ZanP.OrderBooks.Models
{
    public class Order
    {
        public Order(OrderType p_type, double p_amount)
        {
            Type = p_type;
            Amount = p_amount;
        }

        public OrderType Type { get; private set; }
        public double Amount { get; private set; }
    }
}