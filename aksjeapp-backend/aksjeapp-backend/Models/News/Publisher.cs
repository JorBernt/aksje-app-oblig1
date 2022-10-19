using Newtonsoft.Json;

namespace aksjeapp_backend.Models.News
{
    public class Publisher
    {
        [JsonProperty("name")]
        public string Name { get; set; }

    }
}
