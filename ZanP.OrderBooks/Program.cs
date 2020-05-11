using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;
using System;
using ZanP.OrderBooks.Models.Orders;

namespace ZanP.OrderBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            MetaMarketHandler marketHandler = new MetaMarketHandler();

            decimal amount = 4.25M;
            Order buyOrder = new BuyingOrder(amount);
            BestPrice bestBuy = marketHandler.ProcessOrder(buyOrder);
            Console.WriteLine(bestBuy);

            // without data resetting, previous sell/buy will impact on future results
            marketHandler.ResetData(); // if we wanna re-gather data and reset existing balances

            decimal sellAmount = 1.67M;
            Order sellOrder = new SellingOrder(sellAmount);
            BestPrice bestSell = marketHandler.ProcessOrder(sellOrder);
            Console.WriteLine(bestSell);
        }
    }
}
