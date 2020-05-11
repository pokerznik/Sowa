using System.IO;
using ZanP.OrderBooks.Models.Data;
using System.Collections.Generic;
using Newtonsoft.Json;
using System;

namespace ZanP.OrderBooks.Handlers
{
    /// <summary>
    /// Responsible for reading from file deserialization and exchange limits
    /// </summary>
    public class DataHandler
    {
        private string _filePath;
        private List<Exchange> _exchanges;
        private bool _customBalance = false;
        private decimal _customBalanceValue = .0M;

        public DataHandler(string p_filePath)
        {
            _filePath = p_filePath;
            _exchanges = new List<Exchange>();
        }

        private string[] ReadFile()
        {
            return File.ReadAllLines(_filePath);
        }

        public void SetCustomBalance(decimal p_balance)
        {
            _customBalance = true;
            _customBalanceValue = p_balance;
        }

        private void SetBalance(Exchange p_exchange)
        {
            int minBalance = 0;
            int maxBalance = 2000;
            Random rnd = new Random();
            decimal balance = new decimal(rnd.NextDouble() * (maxBalance - minBalance) + minBalance);
            decimal toSet = balance;
            
            if(_customBalance)
                toSet = _customBalanceValue;

            p_exchange.balance = new Balance(toSet);
        }

        private void PrepareData()
        {
            string[] fileContent = ReadFile();
            foreach(var line in fileContent)
            {
                string[] separated = line.Split('\t'); // separated[0] ... timestamp; separated[1] ... json data we're interested in
                Exchange exchange = ExchangeHandler.LoadFromJson(separated[1]);
                SetBalance(exchange); // Random balance is set for every Exchange
                _exchanges.Add(exchange);
            }
        }

        public List<Exchange> LoadExchanges()
        {
            PrepareData();
            return _exchanges;
        }
    }
}