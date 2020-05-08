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

            return GetBestBuyingPrice(p_order, orderItemBalances);
        }

        private BestPrice GetBestBuyingPrice(Order p_order, List<OrderItemBalance> p_orderItemBalances)
        {
            double totalAmount = .0;
            double totalPrice = .0;

            int i = 0;
            bool finished = false;
            
            List<OrderItem> orders = new List<OrderItem>();
            
            while(!finished)
            {
                OrderItemBalance item = p_orderItemBalances[i];
                double amount = item.Order.Amount;
                double diff = p_order.Amount - (totalAmount + amount);
                double itemPrice = (item.Order.Price * amount);

                // we don't wanna buy too much, if diff is negative, we have to split whole amount
                if(diff <= 0)
                {
                    finished = true;
                    amount = item.Order.Amount + diff;
                }

                // if balance is not sufficient, we need to to buy as much as we can
                if((item.ExchangeBalance.EUR - itemPrice) < 0)
                {
                    amount = item.ExchangeBalance.EUR / item.Order.Price;
                    itemPrice = (item.Order.Price * amount);
                    finished = false; // no, we are not finished yet ... we'd be if we could afford whole thing
                }

                // decreasing EUR and increasing amount of BTC
                item.ExchangeBalance.DecreaseEUR(itemPrice);
                item.ExchangeBalance.IncreaseBTC(amount);

                totalAmount += amount;
                totalPrice += itemPrice;
                orders.Add(item);
                
                i++;

                if(i > p_orderItemBalances.Count() -1)
                {
                    finished = true;
                }
            }

            return new BestPrice(orders, totalPrice);
        }

        private BestPrice Selling(Order p_order)
        {

            List<OrderItemBalance> orderItemBalances = new List<OrderItemBalance>();

            foreach(var exchange in m_exchanges)
            {
                foreach(var bid in exchange.Bids)
                {
                    OrderItemBalance orderItemBalance = new OrderItemBalance(exchange.Balance, bid.Order);
                    orderItemBalances.Add(orderItemBalance);
                }
            }

            // we'll use integrated quick sort to sort in ascending order by order price
            orderItemBalances.Sort((x,y) => x.Order.Price.CompareTo(y.Order.Price));
            // but when we're selling, we wanna get max possible price, so let's reverse list to descending order
            orderItemBalances.Reverse();

            return GetBestSellingPrice(p_order, orderItemBalances);
        }

        private BestPrice GetBestSellingPrice(Order p_order, List<OrderItemBalance> p_orderItemBalances)
        {
            double totalAmount = .0;
            double totalPrice = .0;

            int i = 0;
            bool finished = false;
            
            List<OrderItem> orders = new List<OrderItem>();

            while(!finished)
            {
                OrderItemBalance item = p_orderItemBalances[i];
                double amount = item.Order.Amount;
                double diff = p_order.Amount - (totalAmount + amount);
                double itemPrice = (item.Order.Price * amount);

                // we don't wanna buy too much, if diff is negative, we have to split whole amount
                if(diff <= 0)
                {
                    finished = true;
                    amount = item.Order.Amount + diff;
                }

                // we can sell only that much as we have on our balance
                if((item.ExchangeBalance.BTC - amount) < 0)
                {
                    amount = item.ExchangeBalance.BTC / item.Order.Amount;
                    itemPrice = (item.Order.Price * amount);
                    finished = false; // no, we are not finished yet ... we'd be if we could sell whole thing
                }

                // increasing EUR and decreasing amount of BTC
                item.ExchangeBalance.IncreaseEUR(itemPrice);
                item.ExchangeBalance.DecreaseBTC(amount);

                totalAmount += amount;
                totalPrice += itemPrice;
                orders.Add(item);
                
                i++;

                if(i > p_orderItemBalances.Count() - 1)
                {
                    finished = true;
                }
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