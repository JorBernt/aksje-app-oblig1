using test_backend.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace test_backend.DAL
{
    public class PolygonAPI
    {

        public static async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate)
        {
            const string polygonKey = "C1cckwJuZuvEgVJbCmv42HuUZnJSgjeJ";

            try
            {
                using (var client = new HttpClient())
                {
                    var url = new Uri($"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/1/day/{fromDate}/{toDate}?adjusted=true&sort=asc&limit=120&apiKey={polygonKey}");

                    var responce = await client.GetAsync(url);
                    string json = " ";
                    using (var content = responce.Content)
                    {
                        json = await content.ReadAsStringAsync();


                    }
                    Console.WriteLine(json);

                    return JsonConvert.DeserializeObject<StockPrices>(json);

                }

            }
            catch
            {
                Console.WriteLine("Cannot getStockPrices");
                return null;
            }
        }
    }
}
