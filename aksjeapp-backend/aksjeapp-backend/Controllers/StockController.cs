using System.Text.RegularExpressions;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Castle.Core.Internal;
using Microsoft.AspNetCore.Mvc;

namespace aksjeapp_backend.Controller;

[Route("[controller]/[action]")]
public class StockController : ControllerBase
{
    private const string _loggedIn = "SocialSecurityNumber";
    private readonly IStockRepository _db;
    private readonly ILogger<StockController> _logger;


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

    public async Task<ActionResult> GetStockPrices(string symbol, string fromDate) // Date should be written as "YYYY-MM-DD"     TODO: Mulighet til å velge hvor mange dager som skal synes på grafen
    {
        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        var regDate = new Regex(@"^[0-9]{4}-[0-9]{2}-[0-9]{2}");

        if (!reg.IsMatch(symbol) || !regDate.IsMatch(fromDate))
        {
            _logger.LogInformation("Fault in input in getstockprices");
            return BadRequest("Fault in input");
        }

        var stockPrices = await _db.GetStockPrices(symbol, fromDate);

        if(stockPrices == null)
        {
            _logger.LogInformation("GetStockPrices not found");
            return BadRequest("Not found");
        }

        return Ok(stockPrices);
    }

    public async Task<ActionResult> BuyStock(string symbol, int number)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
        {
            return Unauthorized();
        }

        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (!reg.IsMatch(symbol))
        {
            _logger.LogInformation("Fault in input in buy stock");
            return BadRequest("Fault in input");
        }

        if (number < 0)
        {
            _logger.LogInformation("Inserted negative number in amount");
            return BadRequest("Cannot buy negative amount of stock");
        }

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var returnOK = await _db.BuyStock(socialSecurityNumber, symbol, number);
        if (!returnOK)
        {
            _logger.LogInformation("Fault in buyStock");
            return BadRequest("Fault in buyStock");
        }

        return Ok("Stock bought");
    }

    public async Task<ActionResult> SellStock(string symbol, int number)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
        {
            return Unauthorized();
        }

        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (!reg.IsMatch(symbol))
        {
            _logger.LogInformation("Fault in input in sell stock");
            return BadRequest("Fault in input");
        }

        if (number < 0)
        {
            _logger.LogInformation("Inserted negative number in amount");
            return BadRequest("Cannot sell negative stock");
        }

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var returnOK = await _db.SellStock(socialSecurityNumber, symbol, number);
        if (!returnOK)
        {
            _logger.LogInformation("Fault in sellStock");
            return BadRequest("Fault in sellStock");
        }

        return Ok("Stock sold");
    }

    public async Task<ActionResult> SearchResults(string keyPhrase)
    {
        if (keyPhrase.IsNullOrEmpty())
        {
            return Ok(new List<Stock>());
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

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var transactions = await _db.GetAllTransactions(socialSecurityNumber);
        if (transactions == null)
        {
            _logger.LogInformation("Customer not found in getAlltransactions");
            return BadRequest("Customer not found");
        }

        return Ok(transactions);
    }

    public async Task<ActionResult> GetSpecificTransactions(string symbol)
    {
        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (reg.IsMatch(symbol))
        {
            var transactions = await _db.GetSpecificTransactions(symbol);
            if (transactions.Count <= 0)
            {
                _logger.LogInformation("No transactions");
                return Ok(null);
            }

            return Ok(transactions);
        }

        _logger.LogInformation("Fault in input in GetSpecificTransaction");
        return BadRequest("Fault in input");
    }

    public async Task<ActionResult> GetTransaction(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
        {
            return Unauthorized();
        }

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn)!;
        var transaction = await _db.GetTransaction(socialSecurityNumber, id);
        if (transaction == null)
        {
            _logger.LogInformation("Not found transaction belonging to " + _loggedIn + " with id " + id);
            return BadRequest("Transaction does not exist");
        }

        return Ok(transaction);
    }

    [HttpPost]
    public async Task<ActionResult> UpdateTransaction([FromBody] Transaction transaction) //TODO: Regex on transactions
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

    public async Task<ActionResult> DeleteTransaction(int id)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
        {
            return Unauthorized();
        }

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var returnOK = await _db.DeleteTransaction(socialSecurityNumber, id);
        if (!returnOK)
        {
            _logger.LogInformation("Transaction not deleted");
            return BadRequest("Transaction not deleted");
        }

        return Ok("Transaction deleted");
    }

    public async Task<ActionResult> StockChange(string symbol)
    {
        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (reg.IsMatch(symbol))
        {
            var stockChange = await _db.StockChange(symbol);
            if (stockChange == null)
            {
                _logger.LogInformation("StockChange not found");
                return BadRequest("Stockchange not found");
            }

            return Ok(stockChange);
        }

        _logger.LogInformation("Fault in input in spesific transaction method");
        return BadRequest("Fault in input");
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
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn))) return Unauthorized();

        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var customer = await _db.GetCustomerPortfolio(socialSecurityNumber);
        if (customer == null)
        {
            _logger.LogInformation("Customer not found");
            return BadRequest("Customer not found");
        }

        return Ok(customer);
    }

    public async Task<ActionResult> GetNews(string symbol)
    {
        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (!reg.IsMatch(symbol))
        {
            _logger.LogInformation("Fault in input in sell stock");
            return BadRequest("Fault in input");
        }
        
        var news = await _db.GetNews(symbol.ToUpper());

        if (news.Results == null)
        {
            _logger.LogInformation("Could not find any news about" + symbol);
            return BadRequest("Could not find any news");
        }

        return Ok(news);
    }

    public async Task<ActionResult> GetStockName(string symbol)
    {
        symbol = symbol.ToUpper();
        var reg = new Regex(@"^[A-Z]{2,4}");
        if (!reg.IsMatch(symbol))
        {
            _logger.LogInformation("Fault in input getStockName");
            return BadRequest("Fault in input");
        }
        var name = await _db.GetStockName(symbol);
        if (name == "")
        {
            _logger.LogInformation("No stocks found in database");
            return BadRequest("Could not find a stock for the symbol");
        }

        return Ok(name);

    }

    [HttpPost]
    public async Task<ActionResult> ChangePassword([FromBody] User user)
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn))) return Unauthorized();

        if (ModelState.IsValid)
        {
            bool returOk = await _db.ChangePassword(user);
            if (!returOk)
            {
                _logger.LogInformation($"Password for {user.Username} not updated");
                return BadRequest("Password not updated");
            }

            return Ok("Password updated");
        }

        _logger.LogInformation("Username or password does not correspond with regex");
        return BadRequest("Fault in input");
    }

    [HttpPost]
    public async Task<ActionResult> LogIn([FromBody] User user)
    {
        if (ModelState.IsValid)
        {
            bool returnOk = await _db.LogIn(user);
            if (!returnOk)
            {
                _logger.LogInformation("Error in StockController/LogIn (Login failed)");
                HttpContext.Session.SetString(_loggedIn, "");
                return Ok("Failed");
            }

            HttpContext.Session.SetString(_loggedIn, user.Username);
            return Ok("Ok");
        }

        _logger.LogInformation("Fault in regular expression in logIn");
        return BadRequest("Fault in input");
    }

    public async Task<ActionResult> GetCustomerData()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn))) return Unauthorized();
        
        var socialSecurityNumber = HttpContext.Session.GetString(_loggedIn);
        var myCustomer = await _db.GetCustomerData(socialSecurityNumber);
        if (myCustomer == null)
        {
            _logger.LogInformation("Fault in GetCustomerData");
            return Ok("Failed");
        }

        return Ok(myCustomer);

    }
    
    [HttpPost]
    public async Task<ActionResult> RegisterCustomer([FromBody] Customer customer)
    {
        var returnOk = await _db.RegisterCustomer(customer);
        if (!returnOk)
        {
            _logger.LogInformation("Fault in registerCustomer");
            return BadRequest("Fault in registerCustomer");
        }
        return Ok("Customer registered");
    }

    public ActionResult LogOut()
    {
        HttpContext.Session.SetString(_loggedIn, "");
        return Ok("Ok");
    }

    public ActionResult IsLoggedIn()
    {
        if (string.IsNullOrEmpty(HttpContext.Session.GetString(_loggedIn)))
        {
            return Ok(false);
        }

        return Ok(true);
    }
}