using System;
using System.Collections.Generic;
using ZanP.OrderBooks.Models.Data;

namespace ZanP.OrderBooks.Models
{
    public class BestPrice
    {
        public BestPrice(List<OrderItem> p_orders, double p_price)
        {
            Orders = p_orders;
            Price = p_price;
        }

        public List<OrderItem> Orders { get; private set; }
        public double Price { get; private set; }
    }
}