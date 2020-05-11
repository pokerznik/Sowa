using System;
using ZanP.OrderBooks.Enums;

namespace ZanP.OrderBooks.Models.Data
{
    public class OrderBook
    {
        public int? id { get; set; }
        public DateTime time { get; set; }
        public string type { get; set; }
        public decimal amount { get; set; }
        public decimal price { get; set; }
    }
}