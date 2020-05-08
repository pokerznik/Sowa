using System.IO;
using ZanP.OrderBooks.Models;
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
        private string m_filePath;
        public List<Exchange> Exchanges { get; private set; }

        public DataHandler(string p_filePath)
        {
            m_filePath = p_filePath;
            Exchanges = new List<Exchange>();
            PrepareData();
        }

        private string[] ReadFile()
        {
            return File.ReadAllLines(m_filePath);
        }

        private void Deserialize(string[] p_lines)
        {
            foreach(var line in p_lines)
            {
                string[] separated = line.Split('\t'); // separated[0] ... timestamp; separated[1] ... json data we're interested in
                Exchange exchange = JsonConvert.DeserializeObject<Exchange>(separated[1]);
                SetBalance(exchange); // Random balance is set for every Exchange
                Exchanges.Add(exchange);
            }
        }

        private void SetBalance(Exchange p_exchange)
        {
            int minBalance = 0;
            int maxBalance = 1400;
            Random rnd = new Random();
            double balance = rnd.NextDouble() * (maxBalance - minBalance) + minBalance;
            p_exchange.Balance = new Balance(balance);
        }

        private void PrepareData()
        {
            string[] fileContent = ReadFile();
            Deserialize(fileContent);
        }

        public List<Exchange> GetExchanges()
        {
            return Exchanges;
        }
    }
}