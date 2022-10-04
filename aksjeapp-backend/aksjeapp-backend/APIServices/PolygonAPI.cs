using AksjeAPI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AksjeAPI.DAL
{
    public class PolygonAPI
    {

        public static async Task<AksjePriser> HentAksjePriser(string symbol, string fraDato, string tilDato)
        {
            const string polygonKey = "C1cckwJuZuvEgVJbCmv42HuUZnJSgjeJ";

            try
            {
                using (var client = new HttpClient())
                {
                    var url = new Uri($"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/1/day/2021-07-22/2022-07-22?adjusted=true&sort=asc&limit=120&apiKey={polygonKey}");

                    var responce = await client.GetAsync(url);
                    string json = " ";
                    using (var content = responce.Content)
                    {
                        json = await content.ReadAsStringAsync();


                    }
                    Console.WriteLine(json);

                    return JsonConvert.DeserializeObject<AksjePriser>(json);

                }

            }
            catch
            {
                Console.WriteLine("Feil i hentAksjePriser metoden");
                return null;
            }
        }
    }
}
