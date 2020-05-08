using System;
using ZanP.OrderBooks.Enums;

namespace ZanP.OrderBooks.Models.Data
{
    public class OrderBook
    {
        public int? Id { get; set; }
        public DateTime Time { get; set; }
        public string Type { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
    }
}