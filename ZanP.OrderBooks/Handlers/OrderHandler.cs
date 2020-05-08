using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models.Data;
using System.Collections.Generic;
using ZanP.OrderBooks.Models;
using System;
using System.Linq;

namespace ZanP.OrderBooks.Handlers
{
    /// <summary>
    /// Responsible for making orders and setting best order price
    /// </summary>
    public class OrderHandler 
    {
        private OrderType m_type;
        private double m_amount;
        private DataHandler m_dataHandler;
        private List<Exchange> m_exchanges;
        private string m_path;

        public OrderHandler(string p_path = "data")
        {
            m_path = p_path;
            GetExchanges();
        }

        private void GetExchanges()
        {
            m_dataHandler = new DataHandler(m_path);
            m_exchanges = m_dataHandler.GetExchanges();
        }

        private BestPrice Buying(Order p_order)
        {
            List<OrderItemBalance> orderItemBalances = new List<OrderItemBalance>();

            foreach(var exchange in m_exchanges)
            {
                foreach(var ask in exchange.Asks)
                {
                    OrderItemBalance orderItemBalance = new OrderItemBalance(exchange.Balance, ask.Order);
                    orderItemBalances.Add(orderItemBalance);
                }
            }

            // we'll use integrated quick sort to sort in ascending order by order price
            orderItemBalances.Sort((x,y) => x.Order.Price.CompareTo(y.Order.Price));

            return GetBestPrice(p_order, orderItemBalances);
        }

        private BestPrice Selling(Order p_order)
        {

            return null;
        }

        private BestPrice GetBestPrice(Order p_order, List<OrderItemBalance> p_orderItemBalances)
        {
            double boughtAmount = .0;
            double totalPrice = .0;

            int i = 0;
            bool finished = false;
            
            List<OrderItem> orders = new List<OrderItem>();
            
            while(!finished)
            {
                OrderItemBalance item = p_orderItemBalances[i];
                double amount = item.Order.Amount;
                double diff = p_order.Amount - (boughtAmount + amount);

                if(diff <= 0)
                {
                    finished = true;
                    amount = item.Order.Amount + diff;
                }

                boughtAmount += amount;
                totalPrice += (item.Order.Price * amount);
                orders.Add(item);
                
                i++;
            }

            return new BestPrice(orders, totalPrice);
        }

        public BestPrice Process(Order p_order)
        {
            if(p_order == null)
                throw new Exception("Order cannot be processed, because is not valid.");

            if(p_order.Amount <= 0)
                throw new Exception("Order amount should be greater than zero.");

            if(p_order.Type == OrderType.Buy)
            {
                return Buying(p_order);
            }
            else
            {
                return Selling(p_order);
            }
        }
    }
}