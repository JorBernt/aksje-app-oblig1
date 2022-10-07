using Newtonsoft.Json;

namespace aksjeapp_backend.Models
{
    public class Results
    {
        /*
                [JsonProperty("t")]
                public long Index
                {
                    get { return Index; }
                    set { Index = (value / 1000 / 3600 / 24) - 1; } // From Millis to day
                }*/

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
