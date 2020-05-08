using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace ZanP.OrderBooks.Models
{
    public class Exchange
    {
        [JsonProperty("AcqTime")]
        public DateTime Acquired { get; set; }
        public List<Bid> Bids { get; set; }
    }
}