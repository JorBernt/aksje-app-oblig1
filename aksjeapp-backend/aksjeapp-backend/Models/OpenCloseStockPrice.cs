using Newtonsoft.Json;

namespace aksjeapp_backend.Models
{
    public class OpenCloseStockPrice
    {
        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("open")]
        public double OpenPrice { get; set; } // Bruker kun åpnings pris

        [JsonProperty("close")]
        public double ClosePrice { get; set; }   

        
    }
}
