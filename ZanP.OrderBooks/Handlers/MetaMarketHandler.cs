using ZanP.OrderBooks.Enums;
using ZanP.OrderBooks.Models.Data;
using System.Collections.Generic;
using ZanP.OrderBooks.Models;
using ZanP.OrderBooks.Models.Orders;
using System;
using System.Linq;

namespace ZanP.OrderBooks.Handlers
{
    /// <summary>
    /// Responsible for making orders and setting best order price
    /// </summary>
    public class MetaMarketHandler 
    {
        private DataHandler _dataHandler;
        private List<Exchange> _exchanges;
        private string _path;

        public MetaMarketHandler(decimal p_customBalance=0.0M, string p_path = "data")
        {
            _path = p_path;
            _dataHandler = new DataHandler(_path);
            
            if(p_customBalance != decimal.Zero)
                _dataHandler.SetCustomBalance(p_customBalance);

            GetExchanges();
        }

        private void GetExchanges()
        {
            _exchanges = _dataHandler.LoadExchanges();
        }

        // refresh all existing balances
        public void ResetData()
        {
            GetExchanges();
        }

        public BestPrice ProcessOrder(Order p_order)
        {
            if(p_order == null)
                throw new Exception("Order cannot be processed, because is not valid.");

            if(p_order.amount <= 0)
                throw new Exception("Order amount should be greater than zero.");

            WriteToConsole(p_order);

            p_order.Handle(_exchanges);
            return p_order.GetBestPrice();
        }

        private void WriteToConsole(Order p_order)
        {
            string actionToString = "Buying";

            if(p_order.type == OrderType.Sell)
                actionToString = "Selling";

            Console.WriteLine($"Processing order of {actionToString} {p_order.amount} BTC ...");
        }
    }
}