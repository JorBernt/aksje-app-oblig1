
using Newtonsoft.Json;

namespace aksjeapp_backend.Models.News
{
    public class News
    {

        [JsonProperty("results")]
        public List<NewsResults> Results { get; set; }
    }
}
