using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;

namespace ZanP.OrderBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderHandler orderHandler = new OrderHandler();

            OrderType type = OrderType.Buy;
            double amount = 9;
            Order buyOrder = new Order(type, amount);
            BestPrice price = orderHandler.Process(buyOrder);

            OrderType sellType = OrderType.Sell;
            double sellAmount = 9;
            Order sellOrder = new Order(sellType, sellAmount);
            BestPrice bestSell = orderHandler.Process(sellOrder);
        }
    }
}
