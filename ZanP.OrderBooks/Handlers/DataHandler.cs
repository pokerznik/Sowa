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
        private string m_filePath;
        private List<Exchange> m_exchanges;

        public DataHandler(string p_filePath)
        {
            m_filePath = p_filePath;
            m_exchanges = new List<Exchange>();
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
                m_exchanges.Add(exchange);
            }
        }

        private void SetBalance(Exchange p_exchange)
        {
            int minBalance = 0;
            int maxBalance = 2000;
            Random rnd = new Random();
            decimal balance = new decimal(rnd.NextDouble() * (maxBalance - minBalance) + minBalance);
            p_exchange.Balance = new Balance(balance);
        }

        private void PrepareData()
        {
            string[] fileContent = ReadFile();
            Deserialize(fileContent);
        }

        public List<Exchange> GetExchanges()
        {
            PrepareData();
            return m_exchanges;
        }
    }
}