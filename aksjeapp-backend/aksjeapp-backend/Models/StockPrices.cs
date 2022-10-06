using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace aksjeapp_backend.Models
{
    
    public class StockPrices
    {
        
        [JsonProperty("ticker")]
        public string Symbol { get; set; }

        [JsonProperty("results")]
        public List<Results> results { get; set; }
    }
}
