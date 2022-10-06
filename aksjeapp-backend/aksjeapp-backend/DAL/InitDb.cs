using aksjeapp_backend.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace aksjeapp_backend.DAL
{
    public class InitDb
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<StockContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Oversetter Json fila til en liste og lagrer verdiene i databasen
            using (StreamReader r = new StreamReader("nasdaqSheet.json"))
            {
                ;
                string json = r.ReadToEnd();
                List<Stock> stockList = JsonConvert.DeserializeObject<List<Stock>>(json);

                foreach (var stock in stockList) {

                    context.Stocks.Add(stock);
                    context.SaveChanges();
                         }

            }
            context.SaveChanges();
           
        }
    }
}
