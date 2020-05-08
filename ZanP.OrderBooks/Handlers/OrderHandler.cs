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

        // refresh all existing balances
        public void ResetData()
        {
            GetExchanges();
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
            decimal totalAmount = .0M;
            decimal totalPrice = .0M;

            int i = 0;
            bool finished = false;
            bool notPossible = false;
            
            List<OrderItemBalance> orders = new List<OrderItemBalance>();
            
            while(!finished)
            {
                if(notPossible)
                    throw new Exception("Cannot buy within your account balances.");

                OrderItemBalance item = p_orderItemBalances[i];
                decimal amount = item.Order.Amount;
                decimal diff = p_order.Amount - (totalAmount + amount);
                decimal itemPrice = (item.Order.Price * amount);

                // we don't wanna buy too much, if diff is negative, we have to split whole amount
                if(diff <= 0)
                {
                    finished = true;
                    amount = item.Order.Amount + diff;
                    itemPrice = (item.Order.Price * amount);
                }

                // there are very tiny differences sometimes
                if(item.ExchangeBalance.EUR <= 0.00000001M || amount <= 0)
                {
                    i++;
                    finished = false; // we're not finished cause we are poor men :/
                    notPossible = (i >= (p_orderItemBalances.Count() - 1));
                    continue;
                }
                
                // if balance is not sufficient, we need to to buy as much as we can
                decimal balanceDiff = (item.ExchangeBalance.EUR - itemPrice);
                if(balanceDiff < 0.000001M)
                {
                    amount = item.ExchangeBalance.EUR / item.Order.Price;
                    itemPrice = (item.Order.Price * amount);
                    finished = false; // we're not finished cause we couldn't use our balance fully
                }


                // decreasing EUR and increasing amount of BTC
                item.ExchangeBalance.DecreaseEUR(itemPrice);
                item.ExchangeBalance.IncreaseBTC(amount);

                totalAmount += amount;
                totalPrice += itemPrice;

                // change to actual values, which depends on balance constraints
                item.OriginalAmount = item.Order.Amount;
                item.OriginalPrice = item.Order.Price;
                item.Order.Amount = amount;
                item.Order.Price = itemPrice;

                orders.Add(item);
                
                i++;

                // zero based index and funny stuff like this
                notPossible = (i >= (p_orderItemBalances.Count() - 1));
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
            decimal totalAmount = .0M;
            decimal totalPrice = .0M;

            int i = 0;
            bool finished = false;
            bool notPossible = false;
            
            List<OrderItemBalance> orders = new List<OrderItemBalance>();

            while(!finished)
            {
                if(notPossible)
                    throw new Exception("Cannot buy within your account balances.");
                
                OrderItemBalance item = p_orderItemBalances[i];
                decimal amount = item.Order.Amount;
                decimal diff = p_order.Amount - (totalAmount + amount);
                decimal itemPrice = (item.Order.Price * amount);

                // we don't wanna sell too much, if diff is negative, we have to split whole amount
                if(diff <= 0)
                {
                    finished = true;
                    amount = item.Order.Amount + diff;
                    itemPrice = (item.Order.Price * amount);
                }

                // there are very tiny differences sometimes
                if(item.ExchangeBalance.BTC <= 0.00000001M || amount <= 0)
                {
                    i++;
                    finished = false; // we're not finished cause we are poor men :/
                    notPossible = (i >= (p_orderItemBalances.Count() - 1));
                    continue;
                }

                // if balance is not sufficient, we need to to buy as much as we can
                decimal balanceDiff = (item.ExchangeBalance.BTC - amount);
                if(balanceDiff < 0.000001M)
                {
                    amount = item.ExchangeBalance.BTC;
                    itemPrice = (item.Order.Price * amount);
                    finished = false; // we're not finished cause we couldn't use our balance fully
                }


                // increasing EUR and decreasing amount of BTC
                item.ExchangeBalance.IncreaseEUR(itemPrice);
                item.ExchangeBalance.DecreaseBTC(amount);

                totalAmount += amount;
                totalPrice += itemPrice;

                // change to actual values, which depends on balance constraints
                item.OriginalAmount = item.Order.Amount;
                item.OriginalPrice = item.Order.Price;
                item.Order.Amount = amount;
                item.Order.Price = itemPrice;

                orders.Add(item);
                
                i++;

                // zero based index and funny stuff like this
                notPossible = (i >= (p_orderItemBalances.Count() - 1));
            }

            return new BestPrice(orders, totalPrice);
        }

        public BestPrice Process(Order p_order)
        {
            if(p_order == null)
                throw new Exception("Order cannot be processed, because is not valid.");

            if(p_order.Amount <= 0)
                throw new Exception("Order amount should be greater than zero.");

            WriteToConsole(p_order);

            if(p_order.Type == OrderType.Buy)
            {
                return Buying(p_order);
            }
            else
            {
                return Selling(p_order);
            }
        }

        private void WriteToConsole(Order p_order)
        {
            string actionToString = "Buying";

            if(p_order.Type == OrderType.Sell)
                actionToString = "Selling";

            Console.WriteLine($"Processing order of {actionToString} {p_order.Amount} BTC ...");
        }
    }
}