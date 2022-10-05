using test_backend.DAL;
using test_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace test_backend.Controller
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
            return await _db.GetStockPrices();
        }
        public async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            return await _db.GetStockPrices(symbol, fromDate, toDate);
        }
        
        public async Task<bool> BuyStock(string symbol, int antall, string dato)
        {
            return await _db.BuyStock(symbol, antall, dato);
        }
    }
}
