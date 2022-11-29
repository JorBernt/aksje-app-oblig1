using System.ComponentModel;
using System.Security.Cryptography;
using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace aksjeapp_backend.DAL
{
    public class StockRepository : IStockRepository
    {
        private readonly StockContext _db;
        private ILogger<StockRepository> _logger;

        public StockRepository(StockContext db, ILogger<StockRepository> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Stock>> GetAllStocks()
        {
            List<Stock> aksjeListe = await _db.Stocks.ToListAsync();
            return aksjeListe;
        }

        public async Task<StockPrices?>
            GetStockPrices(string symbol, string fromDate) // dato skal skrives som "YYYY-MM-DD"
        {
            try
            {
                //DateTime date;
                var date = DateTime.Parse(fromDate);
                //Goes back to friday if it is a saturday or sunday
                date = BackToFriday(date);


                var stock = await PolygonAPI.GetStockPrices(symbol, date.ToString("yyyy-MM-dd"),
                    GetTodaysDate().ToString("yyyy-MM-dd"), 1);

                var results = stock.results;

                if (results == null)
                {
                    _logger.LogInformation("Could not get the results from API");
                    return null;
                }


                //Skips saturdays and sunday since we does not receive any data from those days
                foreach (var stocks in results)
                {
                    date = GoToMonday(date);

                    stocks.Date = date.ToString("yyyy-MM-dd");
                    date = date.AddDays(1);

                    stock.results = results;
                }

                double res1 = results[^1].ClosePrice;
                double res2 = results[^2].ClosePrice;
                double resDiff = res1 - res2;
                double resAvg = (res1 + res2) / 2;
                double resPercent = (resDiff / ((resAvg) / 2) * 100) / 2;

                // Getting buy and sell numbers
                var boughtList = await _db.TransactionsBought.Where(k => k.Symbol == symbol).ToListAsync();
                int bought = 0;
                foreach (var transaction in boughtList)
                {
                    bought += transaction.Amount;
                }

                var soldList = await _db.TransactionsSold.Where(k => k.Symbol == symbol).ToListAsync();
                int sold = 0;
                foreach (var transaction in soldList)
                {
                    bought += transaction.Amount;
                }


                stock.Name = symbol;
                stock.Last = results[^1].ClosePrice;
                stock.Change = resPercent;
                stock.TodayDifference = results[^1].ClosePrice - results[^2].ClosePrice;
                stock.Buy = bought;
                stock.Sell = sold;
                stock.High = results[^1].HighestPrice;
                stock.Low = results[^1].LowestPrice;
                return stock;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            // Gets todays date
            string date = GetTodaysDate().ToString("yyyy-MM-dd");

            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                // Checks if customer exists
                if (customer == null)
                {
                    _logger.LogInformation("Customer does not exist in buy stock");
                    return false;
                }

                //Get todays price and and set the todays date
                var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                // If we should get an error with the price it will return false and not complete the transaction
                if (stockPrice.ClosePrice == 0)
                {
                    _logger.LogInformation("Price is 0 and will not buy stock");
                    return false;
                }

                double totalPrice = stockPrice.ClosePrice * number;


                if (customer.Balance < totalPrice)
                {
                    return false;
                }
                //Creates transaction
                var stockTransaction = new TransactionBought
                {
                    Date = date,
                    Symbol = symbol,
                    Amount = number,
                    TotalPrice = totalPrice
                };
                customer.TransactionsBought.Add(stockTransaction);
                customer.Balance -= stockTransaction.TotalPrice + 5; //5 dollars in brokerage 
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            // Gets todays date
            string date = GetTodaysDate().ToString("yyyy-MM-dd");

            try
            {
                await UpdatePortfolio(socialSecurityNumber);
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                // Checks if customer exists
                if (customer == null)
                {
                    _logger.LogInformation("Could not find customer in sell stock");
                    return false;
                }

                // Finds the stock we want to sell
                var portfolio =
                    await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                if (portfolio == null)
                {
                    _logger.LogInformation("Customer does not hava a portfolio");
                    return false;
                }

                var portfolioList = await _db.PortfolioList.FirstOrDefaultAsync(k =>
                    k.PortfolioId == portfolio.PortfolioId && k.Symbol == symbol);
                // Customer does not have any of this stock
                if (portfolioList == null)
                {
                    _logger.LogInformation("Customer does not hava a stock from this particular stock");
                    return false;
                }

                // If we does not have enough of that stock
                if (portfolioList.Amount < number)
                {
                    _logger.LogInformation("Customer does not have enough of this stock");
                    return false;
                }

                //Get todays price and and set the todays date
                var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                // If we should get an error with the price it will return false and not complete the transaction
                if (stockPrice.ClosePrice == 0)
                {
                    _logger.LogInformation("Stock price is 0 will not complete sell");
                    return false;
                }

                double totalPrice = stockPrice.ClosePrice * number;

                //Creates transaction
                var stockTransaction = new TransactionSold
                {
                    Date = date,
                    Symbol = symbol,
                    Amount = number,
                    TotalPrice = totalPrice
                };

                customer.TransactionsSold.Add(stockTransaction);
                customer.Balance += stockTransaction.TotalPrice;

                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<List<Stock>> ReturnSearchResults(string keyPhrase)
        {
            try
            {
                var stocks = await _db.Stocks.Where(k =>
                        k.Symbol.Contains(keyPhrase) ||
                        k.Name.ToUpper().Contains(keyPhrase) ||
                        k.Country.ToUpper().Contains(keyPhrase) ||
                        k.Sector.ToUpper().Contains(keyPhrase))
                    .OrderBy(k => k.Name).ToListAsync();
                return stocks;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return new List<Stock>();
            }
        }

        public async Task<List<Transaction>> GetAllTransactions(string socialSecurityNumber)
        {
            // Checks if socialSecuritynumber is correct
            var customer = await _db.Customers.FindAsync(socialSecurityNumber);

            if (customer == null)
            {
                _logger.LogInformation("Customer does not exist");
                return null;
            }

            if ((customer.TransactionsBought == null || customer.TransactionsSold == null))
            {
                return null;
            }

            // Lists all transactions that belongs to the owner
            var transactionsfromDb = customer.TransactionsBought.ToList();

            var transactions = new List<Transaction>();


            foreach (var transactionDB in transactionsfromDb)
            {
                var transaction1 = new Transaction
                {
                    Id = transactionDB.BoughtId,
                    SocialSecurityNumber = customer.SocialSecurityNumber,
                    Date = transactionDB.Date,
                    Symbol = transactionDB.Symbol,
                    Amount = transactionDB.Amount,
                    TotalPrice = transactionDB.TotalPrice
                };
                if (transaction1.Date.Equals(GetTodaysDate().ToString("yyyy-MM-dd")))
                {
                    transaction1.Awaiting = true;
                }

                transactions.Add(transaction1);
            }

            // Lists all the transactions that is sold too, just as negative number
            var transactionsfromDbSold = customer.TransactionsSold;

            foreach (var transactionDBSold in transactionsfromDbSold)
            {
                var transaction1 = new Transaction
                {
                    Id = -1,
                    SocialSecurityNumber = customer.SocialSecurityNumber,
                    Date = transactionDBSold.Date,
                    Symbol = transactionDBSold.Symbol,
                    Amount = -transactionDBSold.Amount,
                    TotalPrice = transactionDBSold.TotalPrice
                };

                transactions.Add(transaction1);
            }

            return transactions;
        }

        public async Task<List<Transaction>> GetSpecificTransactions(string symbol)
        {
            if (symbol != "")
            {
                var transactionsfromDb = await _db.TransactionsBought.Where(k => k.Symbol == symbol).ToListAsync();
                var transactions = new List<Transaction>();


                foreach (var transactionDb in transactionsfromDb)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDb.BoughtId,
                        Date = transactionDb.Date,
                        Symbol = transactionDb.Symbol,
                        Amount = transactionDb.Amount,
                        TotalPrice = transactionDb.TotalPrice
                    };
                    transactions.Add(transaction1);
                }

                var transactionsfromDbSold = await _db.TransactionsSold.Where(k => k.Symbol == symbol).ToListAsync();
                foreach (var transactionDb in transactionsfromDbSold)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDb.SoldId,
                        Date = transactionDb.Date,
                        Symbol = transactionDb.Symbol,
                        Amount = -transactionDb.Amount,
                        TotalPrice = transactionDb.TotalPrice
                    };
                    transactions.Add(transaction1);
                }

                return transactions;
            }

            return null;
        }

        public async Task<Transaction> GetTransaction(string socialSecurityNumber, int id)
        {
            // Checks if socialSecuritynumber is correct
            var customer = await _db.Customers.FindAsync(socialSecurityNumber);
            if (customer == null)
            {
                return null;
            }

            var transactionfromDB = customer.TransactionsBought.Find(k => k.BoughtId == id);

            //Builds transaction object
            var transaction = new Transaction
            {
                Id = transactionfromDB.BoughtId,
                SocialSecurityNumber = customer.SocialSecurityNumber,
                Date = transactionfromDB.Date,
                Symbol = transactionfromDB.Symbol,
                Amount = transactionfromDB.Amount,
                TotalPrice = transactionfromDB.TotalPrice
            };
            return transaction;
        }

        // It is only possible to change transaction after you have bought it, you cannot change it if you sold
        public async Task<bool> UpdateTransaction(Transaction changeTransaction)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(changeTransaction.SocialSecurityNumber);
                if (customer == null)
                {
                    return false;
                }

                var transaction = await _db.TransactionsBought.FindAsync(changeTransaction.Id);
                if (!transaction.Date.Equals(GetTodaysDate().ToString("yyyy-MM-dd")))
                {
                    return false;
                }

                if (transaction.Amount <= 0)
                {
                    await DeleteTransaction(customer.SocialSecurityNumber, changeTransaction.Id);
                    return true;
                }

                //Removes transaction price from customers balance
                customer.Balance += transaction.TotalPrice;

                // Gets todays price for the new stock
                var stockPrice = await PolygonAPI.GetOpenClosePrice(changeTransaction.Symbol,
                    GetTodaysDate().ToString("yyyy-MM-dd"));
                
                transaction.Symbol = changeTransaction.Symbol;
                transaction.Amount = changeTransaction.Amount;
                transaction.TotalPrice = stockPrice.ClosePrice * changeTransaction.Amount;

                customer.Balance -= transaction.TotalPrice;
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteTransaction(string socialSecurityNumber, int id)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);
                if (customer == null)
                {
                    _logger.LogInformation("Customer is not in database");
                    return false;
                }

                var transaction = await _db.TransactionsBought.FindAsync(id);

                // Deletes transaction that is still active
                if (transaction.Date.Equals(GetTodaysDate().ToString("yyyy-MM-dd")))
                {
                    _db.TransactionsBought.Remove(transaction);
                    customer.Balance += transaction.TotalPrice + 5; //Updates balance for customer and refunds brokerage fee
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<StockChangeValue> StockChange(string symbol)
        {
            try
            {
                var stockChange = await _db.StockChangeValues.FirstOrDefaultAsync(k =>
                    k.Symbol == symbol && k.Date == GetTodaysDate().ToString("yyyy-MM-dd"));

                // Returns stockChange if its already in the database. If not it will access the API
                if (stockChange != null)
                {
                    return stockChange;
                }


                var date = GetTodaysDate().AddDays(-1);

                if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                {
                    date = date.AddDays(2);
                }

                if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                {
                    date = date.AddDays(1);
                }

                var fromDate = date.ToString("yyyy-MM-dd");

                var stockPrice1 =
                    await PolygonAPI.GetStockPrices(symbol, fromDate, GetTodaysDate().ToString("yyyy-MM-dd"), 1);

                if (stockPrice1.results == null)
                {
                    return null;
                }

                List<Models.Results> results = stockPrice1.results;

                double res1 = results[^1].ClosePrice;
                double res2 = results[^2].ClosePrice;
                double resDiff = res1 - res2;
                double resAvg = (res1 + res2) / 2;
                double change = (resDiff / ((resAvg) / 2) * 100) / 2;

                stockChange = new StockChangeValue()
                {
                    Date = GetTodaysDate().ToString("yyyy-MM-dd"),
                    Symbol = symbol,
                    Change = change,
                    Value = results.Last().ClosePrice
                };

                var stockChange2 = await _db.StockChangeValues.FirstOrDefaultAsync(k =>
                    k.Symbol == symbol && k.Date == GetTodaysDate().ToString("yyyy-MM-dd"));

                // Returns stockChange if its already in the database.
                if (stockChange2 != null)
                {
                    return stockChange2;
                }

                //Adds to stock change table
                _db.StockChangeValues.Add(stockChange);
                await _db.SaveChangesAsync();

                return stockChange;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<StockOverview>> GetStockOverview()
        {
            try
            {

            var list = new List<StockOverview>();

                var stocks = await _db.Stocks.ToListAsync();

                foreach (var stock in stocks)
                {
                    var myStock = await StockChange(stock.Symbol);
                    if (myStock == null)
                    {
                        continue;
                    }
                    var stockObject = new StockOverview
                    {
                        Symbol = stock.Symbol,
                        Name = stock.Name,
                        Value = myStock.Value,
                        Change = myStock.Change
                    };
                    list.Add(stockObject);
                }

                return list;

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        private async Task<bool> UpdatePortfolio(string socialSecurityNumber)
        {
            try
            {
                // Gets customer object from DB
                var customerFromDb = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customerFromDb == null)
                {
                    return false;
                }

                // Gets transaction objects from DB
                var transactionsBought = customerFromDb.TransactionsBought;
                var transactionsSold = customerFromDb.TransactionsSold;

                StockChangeValue stockChange;

                var portfolio =
                    await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                // Initilizes a portfolio object if it does not exist
                if (portfolio == null)
                {
                    portfolio = new Portfolio
                    {
                        SocialSecurityNumber = socialSecurityNumber
                    };
                    customerFromDb.Portfolio = portfolio;
                }

                // Resets portfolio value
                portfolio.Value = 0;
                var portfolioList = await _db.PortfolioList.Where(k => k.PortfolioId == portfolio.PortfolioId)
                    .ToListAsync();

                // Initilizes a portfoliolist object 
                portfolioList = new List<PortfolioList>();

                    // Adds the transactions from TransactionsBought
                foreach (var transaction in transactionsBought)
                {
                    stockChange = await StockChange(transaction.Symbol);
                    int index = portfolioList.FindIndex(k => k.Symbol == transaction.Symbol);
                    // Sums the amount of stocks if found
                    if (index >= 0)
                    {
                        portfolioList[index].Amount += transaction.Amount;
                        portfolioList[index].Value += stockChange.Value * transaction.Amount;
                    }
                    // Adds the first of this symbol to portfolio list
                    else
                    {
                        var portfolioItem = new PortfolioList()
                        {
                            Symbol = transaction.Symbol,
                            Amount = transaction.Amount,
                            Change = stockChange.Change,
                            Value = transaction.Amount * stockChange.Value,
                            PortfolioId = portfolio.PortfolioId,
                        };
                        portfolioList.Add(
                            portfolioItem); // Adds the new portfolio item to the portfolio list. It will be added to the DB when we save later
                    }
                }

                portfolio.StockPortfolio = portfolioList;

                // Subtracts the stocks from TransactionsSold
                foreach (var transaction in transactionsSold)
                {
                    stockChange = await StockChange(transaction.Symbol); // Gets todays stockchange and value
                    int index = portfolioList.FindIndex(k => k.Symbol == transaction.Symbol);
                    // Sums the amount of stocks if found
                    if (index >= 0)
                    {
                        portfolioList[index].Amount -= transaction.Amount;
                        portfolioList[index].Value -= stockChange.Value * transaction.Amount;
                    }

                    // Removes 
                    if (portfolioList[index].Amount == 0)
                    {
                        portfolioList.RemoveAt(index);
                    }
                }

                // Calculates the total value of the portfolio
                foreach (var stock in portfolioList)
                {
                    portfolio.Value += stock.Value;
                }

                customerFromDb.Portfolio = portfolio;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }


        public async Task<Customer> GetCustomerPortfolio(string socialSecurityNumber)
        {
            try
            {
                // Return name, cumtomer info, combined list of transactions, total value of portfolio

                var customerFromDb = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customerFromDb == null)
                {
                    _logger.LogInformation("Customer not found");
                    return null;
                }

                var transactionsfromDb = customerFromDb.TransactionsBought;
                List<Transaction> transactions = new List<Transaction>();

                // Builds transaction list
                foreach (var transactionDB in transactionsfromDb)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDB.BoughtId,
                        SocialSecurityNumber = customerFromDb.SocialSecurityNumber,
                        Date = transactionDB.Date,
                        Symbol = transactionDB.Symbol,
                        Amount = transactionDB.Amount,
                        TotalPrice = transactionDB.TotalPrice
                    };
                    transactions.Add(transaction1);
                }

                // Runs UpdatePortfolio to update it
                bool updated = await UpdatePortfolio(socialSecurityNumber);
                if (!updated)
                {
                    return null;
                }

                var portfolio =
                    await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                var portfolioList = await _db.PortfolioList.Where(k => k.PortfolioId == portfolio.PortfolioId)
                    .ToListAsync();

                // Adds portfoliolist to portfolio
                if (portfolio == null)
                {
                    portfolio = new Portfolio();
                }
                else
                {
                    portfolio.StockPortfolio = portfolioList;
                }

                //Converts Customers to Customer
                var customer = new Customer()
                {
                    SocialSecurityNumber = customerFromDb.SocialSecurityNumber,
                    FirstName = customerFromDb.FirstName,
                    LastName = customerFromDb.LastName,
                    Address = customerFromDb.Address,
                    Balance = customerFromDb.Balance,
                    Transactions = transactions,
                    PostalCode = customerFromDb.PostalArea.PostalCode,
                    PostCity = customerFromDb.PostalArea.PostCity,
                    Portfolio = portfolio
                };


                return customer;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return null;
            }
        }

        public async Task<List<StockChangeValue>> GetWinners()
        {
            await GetStockOverview();
            var Winners = await _db.StockChangeValues.OrderByDescending(k => k.Change)
                .Where(k => k.Date == GetTodaysDate().ToString("yyyy-MM-dd") && k.Change > 0).Take(7).ToListAsync();

            return Winners;
        }

        public async Task<List<StockChangeValue>> GetLosers()
        {
            await GetStockOverview();
            var Losers = await _db.StockChangeValues.OrderBy(k => k.Change)
                .Where(k => k.Date == GetTodaysDate().ToString("yyyy-MM-dd") && k.Change < 0).Take(7).ToListAsync();

            return Losers;
        }

        public async Task<News> GetNews(string symbol)
        {
            var News = await PolygonAPI.GetNews(symbol);
            foreach (var n in News.Results)
            {
                var date = n.Date.Split('T')[0];
                var time = n.Date.Split('T')[1];
                var newTime = time.Remove(time.Length - 4);
                n.Date = date + "  " + newTime;

            }
            return News;
        }

        public async Task<string> GetStockName(string symbol)
        {
            var stock = await _db.Stocks.FindAsync(symbol);
            return stock == null ? "" : stock.Name;
        }

        public async Task<bool> ChangePassword(User user)
        {
            try
            {
                var userFromDb = await _db.Users.FindAsync(user.Username);

                byte[] salt = GenSalt();
                byte[] password = GenHash(user.Password, salt);

                userFromDb.Password = password;
                userFromDb.Salt = salt;

                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }


        public async Task<bool> RegisterCustomer(Customer customer)
        {
            try
            {
                var customerFromDb = await _db.Customers.FindAsync(customer.SocialSecurityNumber);
                if (customerFromDb != null)
                {
                    _logger.LogInformation("Customer already exists in database");
                    return false;
                }

                byte[] salt = GenSalt();
                byte[] hash = GenHash(customer.User.Password, salt);
                var user = new Users
                {
                    Username = customer.SocialSecurityNumber,
                    Salt = salt,
                    Password = hash
                };



                var c = new Customers
                {
                    SocialSecurityNumber = customer.SocialSecurityNumber,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Address = customer.Address,
                    Balance = 0,
                };

                var checkPostalArea = await _db.PostalAreas.FindAsync(customer.PostalCode);

                if (checkPostalArea == null)
                {
                    var postalArea = new PostalAreas
                    {
                        PostalCode = customer.PostalCode,
                        PostCity = customer.PostCity
                    };
                    c.PostalArea = postalArea;
                }
                else
                {
                    c.PostalArea = checkPostalArea;
                }

                await _db.Users.AddAsync(user);
                await _db.Customers.AddAsync(c);
                await _db.SaveChangesAsync();
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }

            return true;
        }

        public async Task<bool> UpdateCustomer(Customer customer)
        {
            try
            {
                var customerFromDb = await _db.Customers.FindAsync(customer.SocialSecurityNumber);
                //Checks if customer exits
                if (customerFromDb == null)
                {
                    return false;
                }

                var checkPostalArea = customerFromDb.PostalArea;

                // If postal code is changed
                if (!checkPostalArea.PostalCode.Equals(customer.PostalCode))
                {
                    var postalArea = new PostalAreas
                    {
                        PostalCode = customer.PostalCode,
                        PostCity = customer.PostCity
                    };
                    customerFromDb.PostalArea = postalArea;
                }

                customerFromDb.FirstName = customer.FirstName;
                customerFromDb.LastName = customer.LastName;
                customerFromDb.Address = customer.Address;
                
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> DeleteCustomer(string socialSecurityNumber)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customer.Balance != 0)
                {
                    _logger.LogInformation("Customer balance is not 0");
                    return false;
                }

                if (customer.Portfolio.StockPortfolio.Count != 0)
                {
                    _logger.LogInformation("Portfolio is not empty");
                    return false;
                }

                // Deletes all transactions and portfolio from db
                customer.TransactionsBought.Clear();
                customer.TransactionsSold.Clear();

                _db.Customers.Remove(customer);

                var user = await _db.Users.FindAsync(socialSecurityNumber);
                _db.Users.Remove(user);

                
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> Deposit(string socialSecurityNumber ,double amount)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customer == null)
                {
                    return false;
                }

                customer.Balance += amount;
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<bool> Withdraw(string socialSecurityNumber ,double amount)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customer == null)
                {
                    return false;
                }

                //Empties account balance if amount is larger than balance
                if (customer.Balance < amount)
                {
                    amount = customer.Balance;
                }

                customer.Balance -= amount;
                await _db.SaveChangesAsync();

                return true;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public static byte[] GenHash(string password, byte[] salt)
        {
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA512,
                iterationCount: 1000,
                numBytesRequested: 32);
        }

        public static byte[] GenSalt()
        {
            var csp = RandomNumberGenerator.Create();
            var salt = new byte[24];
            csp.GetBytes(salt);
            return salt;
        }

        public async Task<bool> LogIn(User user)
        {
            try
            {
                var userFound = await _db.Users.FirstOrDefaultAsync(k => k.Username == user.Username);

                if (userFound == null)
                {
                    return false;
                }

                byte[] hash = GenHash(user.Password, userFound.Salt);
                bool ok = hash.SequenceEqual(userFound.Password);

                if (ok)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.Message);
                return false;
            }
        }

        public async Task<Customer> GetCustomerData(string socialSecurityNumber)
        {
            try
            {
                var customerFromDb = await _db.Customers.FindAsync(socialSecurityNumber);

                var customer = new Customer()
                {
                    SocialSecurityNumber = customerFromDb.SocialSecurityNumber,
                    FirstName = customerFromDb.FirstName,
                    LastName = customerFromDb.LastName,
                    Address = customerFromDb.Address,
                    Balance = customerFromDb.Balance,
                    Transactions = new List<Transaction>(),
                    PostalCode = customerFromDb.PostalArea.PostalCode,
                    PostCity = customerFromDb.PostalArea.PostCity,
                    Portfolio = new Portfolio()
                };
                return customer;
            }
            catch (Exception exception)
            {
                return null;
            }

        }

        public static DateTime GetTodaysDate()
        {
            //Gets todays date if you want it to run with live data
            //DateTime date1 = DateTime.Now;
            //date1 = date.AddDays(-1)
            
            var date = new DateTime(2022, 09,
                20); // Using fixed date since it takes a couple of minutes to get the stock change

            // If day of week is a weekend then the last price if from friday
            date = BackToFriday(date);

            return date;
        }

        private static DateTime BackToFriday(DateTime date)
        {
            if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                date = date.AddDays(-1);
            }

            if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                date = date.AddDays(-2);
            }

            return date;
        }

        private static DateTime GoToMonday(DateTime date)
        {
            if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                date = date.AddDays(2);
            }

            if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                date = date.AddDays(1);
            }

            return date;
        }
    }
}