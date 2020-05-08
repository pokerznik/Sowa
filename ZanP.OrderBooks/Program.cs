using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;

namespace ZanP.OrderBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderType type = OrderType.Buy;
            double amount = 4;

            Order buyOrder = new Order(type, amount);

            OrderHandler order = new OrderHandler();
            BestPrice price = order.Process(buyOrder);
        }
    }
}
