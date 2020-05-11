using ZanP.OrderBooks.Enums;
using System.Collections.Generic;
using ZanP.OrderBooks.Models.Data;
using System;

namespace ZanP.OrderBooks.Models.Orders
{
    public class SellingOrder : Order
    {
        private List<OrderItemBalance> _orderItemBalances;

        public SellingOrder(decimal p_amount) : base(OrderType.Sell, p_amount)
        {
            _orderItemBalances = new List<OrderItemBalance>();
        }

        public override void Handle(List<Exchange> p_exchanges)
        {
            foreach(var exchange in p_exchanges)
            {
                foreach(var bid in exchange.bids)
                {
                    OrderItemBalance orderItemBalance = new OrderItemBalance(exchange.balance, bid.order);
                    _orderItemBalances.Add(orderItemBalance);
                }
            }

            // we'll use integrated quick sort to sort in ascending order by order price
            _orderItemBalances.Sort((x,y) => x.order.price.CompareTo(y.order.price));
            // but when we're selling, we wanna get max possible price, so let's reverse list to descending order
            _orderItemBalances.Reverse();
        }

        public override BestPrice GetBestPrice()
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
                
                OrderItemBalance item = _orderItemBalances[i];
                decimal itemOrderAmount = item.order.amount;
                decimal diff = amount - (totalAmount + itemOrderAmount);
                decimal itemPrice = (item.order.price * itemOrderAmount);

                // we don't wanna sell too much, if diff is negative, we have to split whole amount
                if(diff <= 0)
                {
                    finished = true;
                    itemOrderAmount = item.order.amount + diff;
                    itemPrice = (item.order.price * itemOrderAmount);
                }

                // there are very tiny differences sometimes
                if(item.exchangeBalance.BTC <= 0.00000001M || itemOrderAmount <= 0)
                {
                    i++;
                    finished = false; // we're not finished cause we are poor men :/
                    notPossible = (i >= (_orderItemBalances.Count - 1));
                    continue;
                }

                // if balance is not sufficient, we need to to buy as much as we can
                decimal balanceDiff = (item.exchangeBalance.BTC - itemOrderAmount);
                if(balanceDiff < 0.000001M)
                {
                    itemOrderAmount = item.exchangeBalance.BTC;
                    itemPrice = (item.order.price * itemOrderAmount);
                    finished = false; // we're not finished cause we couldn't use our balance fully
                }


                // increasing EUR and decreasing amount of BTC
                item.exchangeBalance.IncreaseEUR(itemPrice);
                item.exchangeBalance.DecreaseBTC(itemOrderAmount);

                totalAmount += itemOrderAmount;
                totalPrice += itemPrice;

                // change to actual values, which depends on balance constraints
                item.originalAmount = item.order.amount;
                item.originalPrice = item.order.price;
                item.order.amount = itemOrderAmount;
                item.order.price = itemPrice;

                orders.Add(item);
                
                i++;

                // zero based index and funny stuff like this
                notPossible = (i >= (_orderItemBalances.Count - 1));
            }

            return new BestPrice(orders, totalPrice);
        }
    }
}