using Newtonsoft.Json;

namespace aksjeapp_backend.Models
{

    public class StockPrices
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("Last")]
        public double Last { get; set; }
        
        [JsonProperty("change")]
        public double Change { get; set; }
        
        [JsonProperty("todayDifference")]
        public double TodayDifference { get; set; }
        
        [JsonProperty("buy")]
        public double Buy { get; set; }
        
        [JsonProperty("sell")]
        public double Sell { get; set; }
        
        [JsonProperty("high")]
        public double High { get; set; }
        
        [JsonProperty("low")]
        public double Low { get; set; }
        
        [JsonProperty("volume")]
        public double Turnover { get; set; }
        
        [JsonProperty("ticker")]
        public string Symbol { get; set; }

        [JsonProperty("results")]
        public List<Results>? results { get; set; }
    }
}
