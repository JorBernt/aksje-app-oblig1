using Newtonsoft.Json;

namespace aksjeapp_backend.Models
{

    public class StockPrices
    {

        [JsonProperty("ticker")]
        public string Symbol { get; set; }

        //[JsonProperty("results")]
        //public List<Results> results { get; set; }
    }
}
