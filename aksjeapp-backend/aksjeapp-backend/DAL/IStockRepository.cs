using aksjeapp_backend.Models;

namespace aksjeapp_backend.DAL
{
    public interface IStockRepository
    {
        Task<List<Stock>> GetAllStocks();
        Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate);

        Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number);
        Task<bool> SellStock(string socialSecurityNumber, string symbol, int number);
        Task<List<Stock>> ReturnSearchResults(string keyPhrase);
        Task<List<Transaction>> GetAllTransactions(string socialSecurityNumber);
        Task<Transaction> GetTransaction(string socialSecurityNumber, int id);
        Task<bool> UpdateTransaction(Transaction transaction);
        Task<bool> DeleteTransaction(string socialSecurityNumber, int id);
        Task<StockChangeValue> StockChange(string symbol);


    }
}
