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
            Order = p_order;
            ExchangeBalance = p_balance;
        }

        public Balance ExchangeBalance { get; private set; }
        public decimal OriginalPrice { get; set; }
        public decimal OriginalAmount { get; set; }
    }
}