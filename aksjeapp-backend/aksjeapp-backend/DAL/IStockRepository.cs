using test_backend.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace test_backend.DAL
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<StockPrices> GetStockPrices(string symbol, string fraDato, string tilDato);

        Task<bool> BuyStock(string symbol, int antall, string dato);
    }
}
