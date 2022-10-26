﻿using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Newtonsoft.Json;

namespace aksjeapp_backend.DAL
{
    public class PolygonAPI
    {

        public static async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate, int interval)
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var url = new Uri($"https://api.polygon.io/v2/aggs/ticker/{symbol}/range/{interval}/day/{fromDate}/{toDate}?adjusted=true&sort=asc&limit=120&apiKey={PolygonKeys()}");

                    var responce = await client.GetAsync(url);
                    string json;
                    using (var content = responce.Content)
                    {
                        json = await content.ReadAsStringAsync();


                    }

                    if (json.Contains("error"))
                    {
                        Console.WriteLine("API cooldown");
                        Thread.Sleep(10000);

                        return await GetStockPrices(symbol, fromDate, toDate, interval); // Starts over
                    }

                    return JsonConvert.DeserializeObject<StockPrices>(json);

                }

            }
            catch
            {
                Console.WriteLine("Cannot getStockPrices");
                return null;
            }
        }
        //Gets a stocks price for one day
        public static async Task<OpenCloseStockPrice> GetOpenClosePrice(string symbol, string date)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = new Uri($"https://api.polygon.io/v1/open-close/{symbol}/{date}?adjusted=true&apiKey={PolygonKeys()}");

                    var responce = await client.GetAsync(url);
                    string json;
                    using (var content = responce.Content)
                    {
                        json = await content.ReadAsStringAsync();


                    }

                    Console.WriteLine(json);

                    // Checks if the API returns a bad response
                    if (json.Contains("error"))
                    {
                        Console.WriteLine("API cooldown");
                        Thread.Sleep(5000);
                        return await GetOpenClosePrice(symbol, date); // Starts over
                    }


                    return JsonConvert.DeserializeObject<OpenCloseStockPrice>(json);

                }

            }
            catch
            {
                Console.WriteLine("Cannot getStockPrice");
                return null;
            }
        }

        public static async Task<News> GetNews(string symbol)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var url = new Uri($"https://api.polygon.io/v2/reference/news?ticker={symbol}&limit=7&apiKey={PolygonKeys()}");

                    var responce = await client.GetAsync(url);
                    string json;
                    using (var content = responce.Content)
                    {
                        json = await content.ReadAsStringAsync();


                    }

                    Console.WriteLine(json);

                    // Checks if the API returns a bad response
                    if (json.Contains("error"))
                    {
                        Console.WriteLine("API cooldown");
                        Thread.Sleep(5000);
                        return await GetNews(symbol); // Starts over
                    }

                    Console.WriteLine(json);
                    return JsonConvert.DeserializeObject<News>(json);

                }

            }
            catch
            {
                Console.WriteLine("Cannot get news");
                return null;
            }
        }

        // List with tokens for the Poltgon API
        public static List<String> polygonKeys = new List<string>() { "C1cckwJuZuvEgVJbCmv42HuUZnJSgjeJ", "uWXhChA2H2mRpH7fCrGH5NebvagOZEBT", "udqqYjjU5_yjzUpSGVpLBaqGv54SWsIY", "rt8cZtaDtDUSPGsC0nPpHKnl9tpGPPld", "ku6Bcu6IthG2r5m4pBV8bozJqhOobJWq", "vmCz3EMpwNkSZH_1ekwUvCpJ_dPR2Zhy", "bUPoDNh4OFrbTkQ6DLpsSweKMZKaCqcG", "hK9nttsunnnZ4WqCz1cWJC6yZ7l4LM2U", "eBe50HwzILuzw_bAEwqCHOnMOHbkVjdu", "G_pBRj9ts_Bcbwzl2vKEbdW_i9_XROND" };

        public static string PolygonKeys()
        {
            // Shuffles keys every time we use it
            var polygonKey = polygonKeys[0];
            polygonKeys.RemoveAt(0);
            polygonKeys.Add(polygonKey);
            Console.WriteLine(polygonKey);
            return polygonKey;
        }
    }
}
