using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZanP.OrderBooks.Models.Data
{
    public class Exchange
    {
        [JsonProperty("AcqTime")]
        public DateTime acquired { get; set; }
        public List<OrderItem> bids { get; set; }
        public List<OrderItem> asks { get; set; }
        public Balance balance { get; set; }
    }
}