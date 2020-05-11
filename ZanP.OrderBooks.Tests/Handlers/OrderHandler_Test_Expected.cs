using Xunit;
using ZanP.OrderBooks.Handlers;
using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models;
using ZanP.OrderBooks.Models.Orders;
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
        private MetaMarketHandler _marketHandler;

        public OrderHandler_Test_Expected()
        {
            _marketHandler = new MetaMarketHandler(9900M);
        }

        [Fact]
        public void GetBestPrice_Selling_EqualTrue()
        {
            decimal sellAmount = 1M;
            Order sellOrder = new SellingOrder(sellAmount);
            BestPrice bestSell = _marketHandler.ProcessOrder(sellOrder);
            Assert.Equal(bestSell.price, 690M);
        }

        [Fact]
        public void GetBestPrice_Buying_EqualTrue()
        {
            decimal buyAmount = 2.5M;
            Order buyOrder = new BuyingOrder(buyAmount);
            BestPrice bestBuy = _marketHandler.ProcessOrder(buyOrder);
            Assert.Equal(bestBuy.price, 870M);
        }

        [Fact]
        public void GetBestPrice_Selling_NegativeAmountException()
        {
            decimal sellAmount = -1M;
            Order sellOrder = new SellingOrder(sellAmount);
            Assert.Throws<Exception>(() => _marketHandler.ProcessOrder(sellOrder));
        }

        [Fact]
        public void GetBestPrice_Buying_NegativeAmountFalse()
        {
            decimal buyAmount = -.74M;
            Order buyOrder = new BuyingOrder(buyAmount);
            Assert.Throws<Exception>(() => _marketHandler.ProcessOrder(buyOrder));
        }

        [Fact]
        public void GetBestPrice_Buying_UnsuitableBalanceException()
        {
            decimal buyAmount = 4M;
            Order buyOrder = new BuyingOrder(buyAmount);
            Assert.Throws<Exception>(() => _marketHandler.ProcessOrder(buyOrder));
        }

        [Fact]
        public void GetBestPrice_Selling_UnsuitableBalanceException()
        {
            decimal sellAmount = 4M;
            Order sellOrder = new SellingOrder(sellAmount);
            Assert.Throws<Exception>(() => _marketHandler.ProcessOrder(sellOrder));
        }

    }
}