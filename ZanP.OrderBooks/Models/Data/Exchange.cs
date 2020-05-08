using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZanP.OrderBooks.Models.Data
{
    public class Exchange
    {
        [JsonProperty("AcqTime")]
        public DateTime Acquired { get; set; }
        public List<OrderItem> Bids { get; set; }
        public List<OrderItem> Asks { get; set; }
        public Balance Balance { get; set; }
    }
}