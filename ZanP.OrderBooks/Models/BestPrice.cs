using System;
using System.Collections.Generic;
using ZanP.OrderBooks.Models.Data;

namespace ZanP.OrderBooks.Models
{
    public class BestPrice
    {
        public BestPrice(List<OrderItemBalance> p_orders, decimal p_price)
        {
            Orders = p_orders;
            Price = p_price;
        }

        public List<OrderItemBalance> Orders { get; private set; }
        public decimal Price { get; private set; }

        public override string ToString()
        {
            string toRet = $"Best price would be {Math.Round(Price, 2)} EUR, with following orders:\n\n";
            toRet += "#\t\tAmount\t\t\tPrice\t\tType\t\tOrignal amount:\t\t\tOriginal price";
            int i = 1;

            foreach(var order in Orders)
            {
                string amount = Math.Round(order.Order.Amount, 6).ToString();
                string price = Math.Round(order.Order.Price, 2).ToString();

                toRet += "\n";
                toRet += $"{i}\t\t{amount}\t\t{price}\t\t{order.Order.Type}\t\t{order.OriginalAmount}\t\t\t{order.OriginalPrice}";
                i++;
            }

            return toRet;
        }
    }
}