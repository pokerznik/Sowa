using Xunit;
using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;
using System;

namespace ZanP.OrderBooks.Tests.Handlers
{
    public class OrderHandler_Test_Expected
    {
        private OrderHandler m_orderHandler;

        public OrderHandler_Test_Expected()
        {
            m_orderHandler = new OrderHandler();
        }

        [Fact]
        public void GetBestPrice_Selling()
        {
            OrderType type = OrderType.Sell;
            decimal sellAmount = 1;
            Order sellOrder = new Order(type, sellAmount);
            BestPrice bestBuy = m_orderHandler.Process(sellOrder);
            AssemblyLoadEventArgs.Equals(bestBuy.Price, 6900M);
        }
    }
}