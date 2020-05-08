using System;
using ZanP.OrderBooks.Enums;

namespace ZanP.OrderBooks.Models
{
    public class OrderBook
    {
        private string m_type;

        public int? Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { 
            get 
            {
                return m_type;
            }

            set
            {
                m_type = value;
                if(value == "buy")
                {
                    BookType = OrderBookType.Buy;
                }

                if(value == "sell")
                {
                    BookType = OrderBookType.Sell;
                }
            }
        }
        public double Amount { get; set; }
        public double Price { get; set; }
        public OrderBookType BookType { get; private set; }
    }
}