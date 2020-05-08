﻿using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;
using System;

namespace ZanP.OrderBooks
{
    class Program
    {
        static void Main(string[] args)
        {
            OrderHandler orderHandler = new OrderHandler();

            OrderType type = OrderType.Buy;
            decimal amount = 14.42M;
            Order buyOrder = new Order(type, amount);
            BestPrice bestBuy = orderHandler.Process(buyOrder);
            Console.WriteLine(bestBuy);

            orderHandler.ResetData(); // if we wanna re-gather data and reset existing balances

            OrderType sellType = OrderType.Sell;
            decimal sellAmount = 9;
            Order sellOrder = new Order(sellType, sellAmount);
            /*BestPrice bestSell = orderHandler.Process(sellOrder);
            Console.WriteLine(bestSell);*/
        }
    }
}
