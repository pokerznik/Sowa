using System;
using System.Collections.Generic;
using ZanP.OrderBooks.Models.Data;

namespace ZanP.OrderBooks.Models
{
    public class BestPrice
    {
        public BestPrice(List<OrderItemBalance> p_orders, decimal p_price)
        {
            orders = p_orders;
            price = p_price;
        }

        public List<OrderItemBalance> orders { get; private set; }
        public decimal price { get; private set; }

        public override string ToString()
        {
            string toRet = $"Best price would be {Math.Round(price, 2)} EUR, with following orders:\n\n";
            toRet += "#\t\tAmount\t\t\tPrice\t\tType\t\tOrignal amount:\t\t\tOriginal price";
            int i = 1;

            foreach(var order in orders)
            {
                string amount = Math.Round(order.order.amount, 6).ToString();
                string price = Math.Round(order.order.price, 2).ToString();

                toRet += "\n";
                toRet += $"{i}\t\t{amount}\t\t{price}\t\t{order.order.type}\t\t{order.originalAmount}\t\t\t{order.originalPrice}";
                i++;
            }

            return toRet;
        }
    }
}