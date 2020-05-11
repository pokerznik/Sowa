using ZanP.OrderBooks.Models.Data;

namespace ZanP.OrderBooks.Models
{
    /// <summary>
    /// Helper class with data about our exchange balance; because we don't wanna loose exchange data balance :)
    /// </summary>
    public class OrderItemBalance : OrderItem
    {
        public OrderItemBalance(Balance p_balance, OrderBook p_order)
        {
            order = p_order;
            exchangeBalance = p_balance;
        }

        public Balance exchangeBalance { get; private set; }
        public decimal originalPrice { get; set; }
        public decimal originalAmount { get; set; }
    }
}