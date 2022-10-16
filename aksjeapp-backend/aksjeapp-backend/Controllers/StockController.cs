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
            return await _db.GetStockPrices(symbol.ToUpper(), fromDate, toDate);
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            return await _db.BuyStock(socialSecurityNumber, symbol.ToUpper(), number);
        }

        public async Task<bool> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            return await _db.SellStock(socialSecurityNumber, symbol.ToUpper(), number);
        }

        public async Task<List<Stock>> SearchResults(string keyPhrase)
        {
            return await _db.ReturnSearchResults(keyPhrase.ToUpper());
        }

        public async Task<List<Transaction>> GetAllTransactions(string socialSecurityNumber)
        {
            return await _db.GetAllTransactions(socialSecurityNumber);
        }

        public async Task<Transaction> GetTransaction(string socialSecurityNumber, int id)
        {
            return await _db.GetTransaction(socialSecurityNumber, id);
        }
        public async Task<bool> UpdateTransaction(Transaction transaction)
        {
            return await _db.UpdateTransaction(transaction);
        }
        public async Task<bool> DeleteTransaction(string socialSecurityNumber, int id)
        {
            return await _db.DeleteTransaction(socialSecurityNumber, id);
        }
        public async Task<StockChangeValue> StockChange(string symbol)
        {
            return await _db.StockChange(symbol);
        }
        public async Task<List<StockOverview>> GetStockOverview()
        {
            return await _db.GetStockOverview();
        }
        public async Task<Customer> GetCustomerPortofolio(string socialSecurityNumber)
        {
            return await _db.GetCustomerPortofolio(socialSecurityNumber);
        }
    }
}
