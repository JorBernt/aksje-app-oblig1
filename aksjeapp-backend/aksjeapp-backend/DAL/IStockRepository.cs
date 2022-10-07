using aksjeapp_backend.Models;

namespace aksjeapp_backend.DAL
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate);

        Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number);
    }
}
