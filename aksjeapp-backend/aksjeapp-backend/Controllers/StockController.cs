using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Microsoft.AspNetCore.Mvc;

namespace aksjeapp_backend.Controller
{
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _db;
        private readonly ILogger<StockController> _logger;

        public StockController(IStockRepository db, ILogger<StockController> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<ActionResult> GetAllStocks()
        {
           var allStocks =  await _db.GetAllStocks();
            _logger.LogInformation("Getting all stocks");
           return Ok(allStocks);
        }
        public async Task<ActionResult> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            var stockPrices =  await _db.GetStockPrices(symbol.ToUpper(), fromDate, toDate);
            return Ok(stockPrices);
        }

        public async Task<ActionResult> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            bool returnOK = await _db.BuyStock(socialSecurityNumber, symbol.ToUpper(), number);
            if (!returnOK)
            {
                _logger.LogInformation("Fault in buyStock");
                return BadRequest("Fault in buyStock");
            }
            return Ok("Stock bought");
        }

        public async Task<ActionResult> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            bool returnOK = await _db.SellStock(socialSecurityNumber, symbol.ToUpper(), number);
            if (!returnOK)
            {
                _logger.LogInformation("Fault in sellStock");
                return BadRequest("Fault in sellStock");
            }
            return Ok("Stock sold");
        }

        public async Task<ActionResult> SearchResults(string keyPhrase)
        {
            var searchReults = await _db.ReturnSearchResults(keyPhrase.ToUpper());
            if(searchReults.Count <= 0)
            {
                _logger.LogInformation("Returned 0 results");
                return BadRequest("0 stocks found");
            }
            return Ok(searchReults);

        }

        public async Task<ActionResult> GetAllTransactions(string socialSecurityNumber)
        {
            var transactions = await _db.GetAllTransactions(socialSecurityNumber);
            if(transactions.Count <= 0)
            {
                _logger.LogInformation("No transactions");
                return BadRequest("No transactions");
            }
            return Ok(transactions);
        }

        public async Task<ActionResult> GetTransaction(string socialSecurityNumber, int id)
        {
            var transaction = await _db.GetTransaction(socialSecurityNumber, id);
            if (transaction == null)
            {
                _logger.LogInformation("Not found transaction belonging to " + socialSecurityNumber + " with id " + id);
                return BadRequest("Transaction does not exist");
            }
            return Ok(transaction);

        }
        public async Task<ActionResult> UpdateTransaction(Transaction transaction)
        {
            var returnOK = await _db.UpdateTransaction(transaction);
            if (!returnOK)
            {
                _logger.LogInformation("Transaction not updated");
                return BadRequest("Transaction not updated");
            }
            return Ok("Transaction updated");
        }
        public async Task<ActionResult> DeleteTransaction(string socialSecurityNumber, int id)
        {
            bool returnOK = await _db.DeleteTransaction(socialSecurityNumber, id);
            if (!returnOK)
            {
                _logger.LogInformation("Transaction not deleted");
                return BadRequest("Transaction not deleted");
            }
            return Ok("Transaction deleted");

        }
        public async Task<ActionResult> StockChange(string symbol)
        {
            var stockChange = await _db.StockChange(symbol);

        }
        public async Task<List<StockOverview>> GetStockOverview()
        {
            return await _db.GetStockOverview();
        }

    }
}
