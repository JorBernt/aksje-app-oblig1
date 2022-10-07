using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace aksjeapp_backend.Controller
{
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _db;

        public StockController(IStockRepository db)
        {
            _db = db;
        }


        public async Task<List<Stock>> GetAllStocks()
        {
            return await _db.GetAllStocks();
        }
        public async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            return await _db.GetStockPrices(symbol, fromDate, toDate);
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            return await _db.BuyStock(socialSecurityNumber, symbol, number);
        }

        public async Task<bool> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            return await _db.SellStock(socialSecurityNumber, symbol, number);
        }
    }
}
