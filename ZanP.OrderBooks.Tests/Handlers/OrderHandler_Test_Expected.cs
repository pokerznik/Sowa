using Xunit;
using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;
using System;

namespace ZanP.OrderBooks.Tests.Handlers
{
    /*
    * FOR YOUR NOTICE ONLY, BUT IMPORTANT ANYWAY:
    * - actual prices in test data (data file in test project) of BTC are diferent from real ones. It was made
    * for simplified testing. 
    * e.g. 0.5 BTC price -> 100 EUR each, 0.2 BTC -> 200 EUR, 300 EUR, 400 EUR, 500 EUR each.
    * Refer to data file inside bin/Debug/netcoreapp3.0 for more details.
    */
    public class OrderHandler_Test_Expected
    {
        private OrderHandler m_orderHandler;

        public OrderHandler_Test_Expected()
        {
            m_orderHandler = new OrderHandler(9900M);
        }

        [Fact]
        public void GetBestPrice_Selling_EqualTrue()
        {
            OrderType type = OrderType.Sell;
            decimal sellAmount = 1M;
            Order sellOrder = new Order(type, sellAmount);
            BestPrice bestSell = m_orderHandler.Process(sellOrder);
            Assert.Equal(bestSell.Price, 690M);
        }

        [Fact]
        public void GetBestPrice_Buying_EqualTrue()
        {
            OrderType type = OrderType.Buy;
            decimal buyAmount = 2.5M;
            Order buyOrder = new Order(type, buyAmount);
            BestPrice bestBuy = m_orderHandler.Process(buyOrder);
            Assert.Equal(bestBuy.Price, 870M);
        }

        [Fact]
        public void GetBestPrice_Selling_NegativeAmountException()
        {
            OrderType type = OrderType.Sell;
            decimal sellAmount = -1M;
            Order sellOrder = new Order(type, sellAmount);
            Assert.Throws<Exception>(() => m_orderHandler.Process(sellOrder));
        }

        [Fact]
        public void GetBestPrice_Buying_NegativeAmountFalse()
        {
            OrderType type = OrderType.Buy;
            decimal buyAmount = -.74M;
            Order buyOrder = new Order(type, buyAmount);
            Assert.Throws<Exception>(() => m_orderHandler.Process(buyOrder));
        }

        [Fact]
        public void GetBestPrice_Buying_UnsuitableBalanceException()
        {
            OrderType type = OrderType.Buy;
            decimal buyAmount = 4M;
            Order buyOrder = new Order(type, buyAmount);
            Assert.Throws<Exception>(() => m_orderHandler.Process(buyOrder));
        }

        [Fact]
        public void GetBestPrice_Selling_UnsuitableBalanceException()
        {
            OrderType type = OrderType.Sell;
            decimal sellAmount = 4M;
            Order sellOrder = new Order(type, sellAmount);
            Assert.Throws<Exception>(() => m_orderHandler.Process(sellOrder));
        }

    }
}