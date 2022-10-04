using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace AksjeAPI.Models
{
    
    public class AksjePriser
    {
        
        [JsonProperty("ticker")]
        public string Ticker { get; set; }

        [JsonProperty("results")]
        public List<Results> resultater { get; set; }
    }
}
