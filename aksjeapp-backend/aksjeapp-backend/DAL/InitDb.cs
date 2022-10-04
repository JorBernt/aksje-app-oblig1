using AksjeAPI.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AksjeAPI.DAL
{
    public class InitDb
    {
        public static void Initialize(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var context = serviceScope.ServiceProvider.GetService<AksjeContext>();

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Oversetter Json fila til en liste og lagrer verdiene i databasen
            using (StreamReader r = new StreamReader("nasdaqSheet.json"))
            {
                ;
                string json = r.ReadToEnd();
                List<Aksje> aksjeListe = JsonConvert.DeserializeObject<List<Aksje>>(json);

                foreach (var aksje in aksjeListe) {

                    context.Aksjer.Add(aksje);
                    context.SaveChanges();
                         }

            }
            context.SaveChanges();
           
        }
    }
}
