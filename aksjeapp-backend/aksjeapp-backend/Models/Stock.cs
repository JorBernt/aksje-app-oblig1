using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace aksjeapp_backend.Models
{
    
    public class Stock
    {
        [Key]
        [JsonProperty("Symbol")]
        public string Symbol { get; set; } = "";

        [JsonProperty("Name")]
        public string Name { get; set; } = "";

        [JsonProperty("Country")]
        public string Country { get; set; } = "";

        [JsonProperty("Sector")]
        public string Sector { get; set; } = "";



    }
}
