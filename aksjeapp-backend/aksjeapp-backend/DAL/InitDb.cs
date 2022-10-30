using aksjeapp_backend.Models;
using Newtonsoft.Json;

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

            // Translates the JSON file to a list and adds the list to the database

            using (StreamReader r = new StreamReader("StockChangeValuesSheet.json"))
            {
                string json = r.ReadToEnd();
                List<StockChangeValue> stockList = JsonConvert.DeserializeObject<List<StockChangeValue>>(json);

                context.StockChangeValues.AddRange(stockList);
            }

            using (StreamReader r = new StreamReader("nasdaqSheet.json"))
            {
                string json = r.ReadToEnd();
                List<Stock> stockList = JsonConvert.DeserializeObject<List<Stock>>(json);

                context.Stocks.AddRange(stockList);


                // Adding customers
                var postalArea1 = new PostalAreas
                {
                    PostalCode = "0134",
                    PostCity = "Oslo"
                };


                var customer1 = new Customers
                {
                    FirstName = "Line",
                    SocialSecurityNumber = "12345678910",
                    LastName = "Jensen",
                    Address = "Karl Johansgate 3",
                    Balance = 100_000,
                    PostalArea = postalArea1
                };
                
                context.Customers.Add(customer1);
                context.SaveChanges();
            }
        }
    }
}