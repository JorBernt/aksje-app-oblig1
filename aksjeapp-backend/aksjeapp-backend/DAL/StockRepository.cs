using aksjeapp_backend.Models;
using Microsoft.EntityFrameworkCore;

namespace aksjeapp_backend.DAL
{
    public class StockRepository : IStockRepository
    {
        private readonly StockContext _db;

        public StockRepository(StockContext db)
        {
            _db = db;
        }

        public async Task<List<Stock>> GetAllStocks()
        {

            List<Stock> aksjeListe = await _db.Stocks.ToListAsync();
            return aksjeListe;

        }
        public async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            var stock = await PolygonAPI.GetStockPrices(symbol, fromDate, toDate);

            return stock;
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            // Gets todays date
            DateTime date1 = DateTime.Now;
            string date = date1.Year + "-" + date1.Month + "-" + date1.Day;
            Console.WriteLine(date);

            //Get todays price and and set the todays date
            var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

            double totalPrice = stockPrice.OpenPrice * number;
            //Creates transaction
            var stockTransaction = new Transaction
            {
                Date = date,
                SocialSecurityNumber = socialSecurityNumber,
                Symbol = symbol,
                Number = number,
                TotalPrice = totalPrice
            };
            Console.WriteLine("Buying stock");
            try
            {

                var customer = await _db.Customers.FindAsync(socialSecurityNumber);
                Console.WriteLine(customer);

                if (customer == null)
                {
                    return false;
                }
                customer.Transactions.Add(stockTransaction);
                Console.WriteLine("Buying stock");
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {

                return false;
            }

        }

    }
}
