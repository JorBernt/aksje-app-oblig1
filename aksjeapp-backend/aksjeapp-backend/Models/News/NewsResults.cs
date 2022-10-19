using Newtonsoft.Json;

namespace aksjeapp_backend.Models.News
{
    public class NewsResults
    {
        [JsonProperty("publisher")]
        public Publisher Publisher { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("published_utc")]
        public string Date { get; set; }

        [JsonProperty("tickers")]
        public List<string> Stocks { get; set; }
    }
}
