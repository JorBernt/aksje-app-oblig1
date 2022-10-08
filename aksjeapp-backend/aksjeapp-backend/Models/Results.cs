using Newtonsoft.Json;

namespace aksjeapp_backend.Models
{
    public class Results
    {
        [JsonProperty("c")]
        public double ClosePrice { get; set; }

        [JsonProperty("o")]
        public double OpenPrice { get; set; }

        [JsonProperty("h")]
        public double HighestPrice { get; set; }

        [JsonProperty("l")]
        public double LowestPrice { get; set; }


    }
}
