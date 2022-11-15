using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using Results = aksjeapp_backend.Models.Results;

namespace aksjeapp_backend.Controller
{
    [Route("[controller]/[action]")]
    public class StockController : ControllerBase
    {
        private readonly IStockRepository _db;
        private readonly ILogger<StockController> _logger;
        public const string _loggedIn = "12345678910";


        public StockController(IStockRepository db, ILogger<StockController> logger)
        {
            _db = db;
            _logger = logger;
        }


        public async Task<ActionResult> GetAllStocks()
        {
            var allStocks = await _db.GetAllStocks();
            if (allStocks.Count == 0 || allStocks == null)
            {
                _logger.LogInformation("Not found");
                return BadRequest("Not found");
            }

            return Ok(allStocks);
        }

        public async Task<ActionResult> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            if (symbol == "")
            {
                _logger.LogInformation("Empty stock parameter");
                return BadRequest("Symbol is empty");
            }
            var stockPrices = await _db.GetStockPrices(symbol.ToUpper(), fromDate, toDate);

            if (stockPrices == null)
            {
                _logger.LogInformation("GetStockPrices not found");
                return BadRequest("Not found");
            }

            return Ok(stockPrices);
        }

        public async Task<ActionResult> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            if (number < 0)
            {
                _logger.LogInformation("Inserted negative number in amount");
                return BadRequest("Cannot buy negative amount of stock");
            }
            bool returnOK = await _db.BuyStock(_loggedIn, symbol.ToUpper(), number);
            if (!returnOK)
            {
                _logger.LogInformation("Fault in buyStock");
                return BadRequest("Fault in buyStock");
            }

            return Ok("Stock bought");
        }

        public async Task<ActionResult> SellStock(string symbol, int number)
        {
            if(number < 0)
            {
                _logger.LogInformation("Inserted negative number in amount");
                return BadRequest("Cannot sell negative stock");
            }
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            bool returnOK = await _db.SellStock(_loggedIn, symbol.ToUpper(), number);
            if (!returnOK)
            {
                _logger.LogInformation("Fault in sellStock");
                return BadRequest("Fault in sellStock");
            }

            return Ok("Stock sold");
        }

        public async Task<ActionResult> SearchResults(string keyPhrase)
        {
            if (keyPhrase == "")
            {
                return BadRequest("KeyPhrase is empty");
            }

            var searchReults = await _db.ReturnSearchResults(keyPhrase.ToUpper());
            return Ok(searchReults);
        }

        public async Task<ActionResult> GetAllTransactions()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }

            var transactions = await _db.GetAllTransactions(_loggedIn);
            if (transactions.Count <= 0)
            {
                _logger.LogInformation("No transactions");
                return BadRequest("No transactions");
            }

            return Ok(transactions);
        }

        public async Task<ActionResult> GetSpecificTransactions(string symbol)
        {
            var transactions = await _db.GetSpecificTransactions(symbol);
            if (transactions.Count <= 0)
            {
                _logger.LogInformation("No transactions");
                return BadRequest("No transactions");
            }

            return Ok(transactions);
        }

        public async Task<ActionResult> GetTransaction(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            var transaction = await _db.GetTransaction(_loggedIn, id);
            if (transaction == null)
            {
                _logger.LogInformation("Not found transaction belonging to " + _loggedIn + " with id " + id);
                return BadRequest("Transaction does not exist");
            }

            return Ok(transaction);
        }
        [HttpPost]
        public async Task<ActionResult> UpdateTransaction([FromBody] Transaction transaction)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            bool returnOK = await _db.DeleteTransaction(_loggedIn, id);
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
            if (stockChange == null)
            {
                _logger.LogInformation("StockChange not found");
                return BadRequest("Stockchange not found");
            }

            return Ok(stockChange);
        }

        public async Task<ActionResult> GetStockOverview()
        {
            var stockOverview = await _db.GetStockOverview();
            if (stockOverview == null)
            {
                _logger.LogInformation("Stock overview not found");
                return BadRequest("Stock overview not found");
            }

            return Ok(stockOverview);
        }

        public async Task<ActionResult> GetWinners()
        {
            var winners = await _db.GetWinners();
            if (winners == null)
            {
                _logger.LogInformation("Winners not found");
                return BadRequest("Winners not found");
            }

            return Ok(winners);
        }

        public async Task<ActionResult> GetLosers()
        {
            var Losers = await _db.GetLosers();
            if (Losers == null)
            {
                _logger.LogInformation("Losers not found");
                return BadRequest("Losers not found");
            }

            return Ok(Losers);
        }

        public async Task<ActionResult> GetCustomerPortfolio()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
            {
                return Unauthorized();
            }
            var customer = await _db.GetCustomerPortfolio(_loggedIn);
            if (customer == null)
            {
                _logger.LogInformation("Customer not found");
                return BadRequest("Customer not found");
            }

            return Ok(customer);
        }

        public async Task<ActionResult> GetNews(string symbol)
        {
            var news = await _db.GetNews(symbol.ToUpper());

            if (news.Results == null)
            {
                _logger.LogInformation("Fault");
                return BadRequest("Could not find any news");
            }

            return Ok(news);
        }

        public async Task<ActionResult> GetStockName(string symbol)
        {
            symbol = symbol.ToUpper();
            Regex reg = new Regex(@"^[A-Z]{2,4");
            if (reg.IsMatch(symbol)) {
            var name = await _db.GetStockName(symbol);
            if (name == "")
            {
                _logger.LogInformation("No stocks found in database");
                return BadRequest("Could not find a stock");
            }

            return Ok(name);
            }
            return BadRequest("Fault in input get stock name");
        }

        public async Task<ActionResult> LogIn(User user)
        {
            if (ModelState.IsValid)
            {
                bool returnOK = await _db.LogIn(user);
                //_loggedIn = user.Username;
                if (!returnOK)
                {
                    _logger.LogInformation("Error in StockController/LogIn (Login failed)");
                    HttpContext.Session.SetString(_loggedIn, "");
                    return Ok("Failed");

                }

                HttpContext.Session.SetString(_loggedIn, "LoggedIn");
                Console.WriteLine(_loggedIn);
                return Ok("Ok");
            }
            _logger.LogInformation("Fault in regular expression in logIn");
            return BadRequest("Fault in input");
        }

        public async Task<ActionResult> LogOut()
        {
            HttpContext.Session.SetString(_loggedIn, "");
            return Ok("Ok");
        }
    }
}