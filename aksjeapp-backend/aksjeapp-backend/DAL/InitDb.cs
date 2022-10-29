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

            using (StreamReader r = new StreamReader("stockChangeValuesSheet.json"))
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
                    Transactions = new List<Transaction>(),
                    PostalArea = postalArea1
                };
                var transaction1 = new Transaction()
                {
                    SocialSecurityNumber = "12345678910",
                    Date = "2022-09-25",
                    Symbol = "AAPL",
                    Amount = 230,
                    TotalPrice = 27624,
                    IsActive = true,
                    Awaiting = false
                };
                var transaction2 = new Transaction()
                {
                    SocialSecurityNumber = "12345678910",
                    Date = "2022-09-18",
                    Symbol = "AAPL",
                    Amount = 100,
                    TotalPrice = 10231,
                    IsActive = true,
                    Awaiting = false
                };
                var transaction3 = new Transaction()
                {
                    SocialSecurityNumber = "12345678910",
                    Date = "2022-10-05",
                    Symbol = "GOOG",
                    Amount = 20,
                    TotalPrice = 2801,
                    IsActive = true,
                    Awaiting = false
                };
                var transaction4 = new Transaction()
                {
                    SocialSecurityNumber = "12345678910",
                    Date = "2022-09-18",
                    Symbol = "ZS",
                    Amount = 25,
                    TotalPrice = 9335,
                    IsActive = true,
                    Awaiting = false
                };
                var transaction5 = new Transaction()
                {
                    SocialSecurityNumber = "12345678910",
                    Date = "2022-09-18",
                    Symbol = "ENPH",
                    Amount = 700,
                    TotalPrice = 14812,
                    IsActive = true,
                    Awaiting = false
                };

                customer1.Transactions.Add(transaction1);
                customer1.Transactions.Add(transaction2);
                customer1.Transactions.Add(transaction3);
                customer1.Transactions.Add(transaction4);
                customer1.Transactions.Add(transaction5);
                context.Transactions.Add(transaction1);
                context.Transactions.Add(transaction2);
                context.Transactions.Add(transaction3);
                context.Transactions.Add(transaction4);
                context.Transactions.Add(transaction5);

                context.Customers.Add(customer1);
                context.SaveChanges();
            }
        }
    }
}