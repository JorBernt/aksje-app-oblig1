using aksjeapp_backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<StockPrices> GetStockPrices(string symbol, string fraDato, string tilDato) // dato skal skrives som "YYYY-MM-DD"
        {
            var aksje = await PolygonAPI.HentAksjePriser(symbol, fraDato, tilDato);

            return aksje;
        }

        public async Task<bool> BuyStock(string symbol, int antall, string dato)
        {
           //Get todays price and and set the todays date 
            return true;
        }
    }
}
