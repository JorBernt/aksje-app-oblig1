using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;

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

        public async Task<StockPrices> GetStockPrices(string symbol, string fromDate, string toDate) // dato skal skrives som "YYYY-MM-DD"
        {
            var stock = await PolygonAPI.GetStockPrices(symbol, fromDate, toDate, 1);

            var results = stock.results;

            if (results == null)
            {
                return null;
            }


            DateTime date;

            if (DateTime.TryParse(fromDate, out date))
            {
                foreach (var stocks in results)
                {
                    if (date.DayOfWeek.Equals(DayOfWeek.Saturday))
                    {
                        date = date.AddDays(2);
                    }

                    if (date.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        date = date.AddDays(1);
                    }

                    stocks.Date = date.ToString("yyyy-MM-dd");
                    date = date.AddDays(1);
                }

                stock.results = results;
            }

            double res1 = results[^1].ClosePrice;
            double res2 = results[^2].ClosePrice;
            double resDiff = res1 - res2;
            double resAvg = (res1 + res2) / 2;
            double resPercent = (resDiff / ((resAvg) / 2) * 100) / 2;

            stock.Name = symbol;
            stock.Last = results[^1].ClosePrice;
            stock.Change = resPercent;
            stock.TodayDifference = results[^1].ClosePrice - results[^2].ClosePrice;
            stock.Buy = 0;
            stock.Sell = 0;
            stock.High = results[^1].HighestPrice;
            stock.Low = results[^1].LowestPrice;
            return stock;
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
                    return false;
                }
                //Get todays price and and set the todays date
                var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                // If we should get an error with the price it will return false and not complete the transaction
                if (stockPrice.ClosePrice == 0)
                {
                    return false;
                }

                double totalPrice = stockPrice.ClosePrice * number;

                //Creates transaction
                var stockTransaction = new TransactionBought
                {
                    Date = date,
                    SocialSecurityNumber = socialSecurityNumber,
                    Symbol = symbol,
                    Amount = number,
                    TotalPrice = totalPrice
                };

                await _db.TransactionsBought.AddAsync(stockTransaction);
                customer.Balance -= stockTransaction.TotalPrice - 5; //5 dollars in brokerage 
                await _db.SaveChangesAsync();

                //await UpdatePortfolio(socialSecurityNumber);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            int number1 = number;
            // Gets todays date
            string date = GetTodaysDate().ToString("yyyy-MM-dd");

            try
            {
                await UpdatePortfolio(socialSecurityNumber);
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);
                //var portfolio = _db.Portfolios

                // Checks if customer exists
                if (customer == null)
                {
                    return false;
                }

                // Finds the stock we want to sell
                var portfolio = await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                if (portfolio == null)
                {
                    return false;
                }

                var portfolioList = await _db.PortfolioList.FirstOrDefaultAsync(k => k.PortfolioId == portfolio.PortfolioId && k.Symbol == symbol);
                if (portfolioList == null)
                {
                    return false;
                }
                // If we does not have enough of that stock
                if (portfolioList.Amount < number)
                {
                    return false;
                }

                //Get todays price and and set the todays date
                var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                // If we should get an error with the price it will return false and not complete the transaction
                if (stockPrice.ClosePrice == 0)
                {
                    return false;
                }

                double totalPrice = stockPrice.ClosePrice * number;

                //Creates transaction
                var stockTransaction = new TransactionSold
                {
                    Date = date,
                    SocialSecurityNumber = socialSecurityNumber,
                    Symbol = symbol,
                    Amount = number,
                    TotalPrice = totalPrice
                };

                //customer.TransactionsSold.Add(stockTransaction);
                await _db.TransactionsSold.AddAsync(stockTransaction);
                customer.Balance += stockTransaction.TotalPrice;

                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<List<Stock>> ReturnSearchResults(string keyPhrase)
        {
            try
            {
                if (keyPhrase.Equals(""))
                {
                    return null;
                }

                var stocks = await _db.Stocks
                    .Where(k => k.Symbol.Contains(keyPhrase) || k.Name.ToUpper().Contains(keyPhrase) ||
                                k.Country.ToUpper().Contains(keyPhrase) || k.Sector.ToUpper().Contains(keyPhrase))
                    .OrderBy(k => k.Name).ToListAsync();
                return stocks;
            }
            catch
            {
                return new List<Stock>();
            }
        }

        public async Task<List<Transaction>> GetAllTransactions(string socialSecurityNumber)
        {
            // Checks if socialSecuritynumber is correct
            var customer = await _db.Customers.FindAsync(socialSecurityNumber);

            if (customer == null)
            {
                return null;
            }
            // Lists all transactions that belongs to the owner
            var transactionsfromDb = await _db.TransactionsBought.Where(k => k.SocialSecurityNumber == socialSecurityNumber).ToListAsync();

            var transactions = new List<Transaction>();


            foreach (var transactionDB in transactionsfromDb)
            {
                var transaction1 = new Transaction
                {
                    Id = transactionDB.BoughtId,
                    SocialSecurityNumber = transactionDB.SocialSecurityNumber,
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
            var transactionsfromDbSold = await _db.TransactionsSold.Where(k => k.SocialSecurityNumber == socialSecurityNumber).ToListAsync();

            foreach (var transactionDBSold in transactionsfromDbSold)
            {
                var transaction1 = new Transaction
                {
                    Id = -1,
                    SocialSecurityNumber = transactionDBSold.SocialSecurityNumber,
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
            if (symbol != null)
            {
                var transactionsfromDb = await _db.TransactionsBought.Where(k => k.Symbol == symbol).ToListAsync();
                var transactions = new List<Transaction>();


                foreach (var transactionDB in transactionsfromDb)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDB.BoughtId,
                        SocialSecurityNumber = transactionDB.SocialSecurityNumber,
                        Date = transactionDB.Date,
                        Symbol = transactionDB.Symbol,
                        Amount = transactionDB.Amount,
                        TotalPrice = transactionDB.TotalPrice

                    };
                    transactions.Add(transaction1);

                }

                var transactionsfromDbSold = await _db.TransactionsSold.Where(k => k.Symbol == symbol).ToListAsync();
                foreach (var transactionDB in transactionsfromDbSold)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDB.SoldId,
                        SocialSecurityNumber = transactionDB.SocialSecurityNumber,
                        Date = transactionDB.Date,
                        Symbol = transactionDB.Symbol,
                        Amount = -transactionDB.Amount,
                        TotalPrice = transactionDB.TotalPrice
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
            var transactionfromDB = await _db.TransactionsBought.FindAsync(id);
            var transaction = new Transaction
            {
                Id = transactionfromDB.BoughtId,
                SocialSecurityNumber = transactionfromDB.SocialSecurityNumber,
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
                if (transaction.Date.Equals(GetTodaysDate().ToString("yyyy-MM-dd")))
                {
                    //Removes transaction price from customers balance
                    customer.Balance += transaction.TotalPrice;

                    // Gets todays price for the new stock
                    var stockPrice = await PolygonAPI.GetOpenClosePrice(changeTransaction.Symbol, GetTodaysDate().ToString("yyyy-MM-dd"));

                    transaction.Symbol = changeTransaction.Symbol;
                    transaction.Amount = changeTransaction.Amount;
                    transaction.TotalPrice = stockPrice.ClosePrice * changeTransaction.Amount;

                    customer.Balance -= transaction.TotalPrice;
                    await _db.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteTransaction(string socialSecurityNumber, int id)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);
                if (customer != null)
                {
                    var transaction = _db.TransactionsBought.Find(id);

                    //Returns if the transaction does not exist
                    if (transaction == null)
                    {
                        return false;
                    }

                    // Deletes transaction that is still active
                    if (transaction.Date.Equals(GetTodaysDate().ToString("yyyy-MM-dd")))
                    {
                        _db.TransactionsBought.Remove(transaction);
                        customer.Balance += transaction.TotalPrice + 5; //Updates balance for customer and refunds brokerage fee
                        await _db.SaveChangesAsync();
                        return true;
                    }
                    else
                    {
                        _db.TransactionsBought.Remove(transaction);
                        await _db.SaveChangesAsync();
                        return true;
                    }
                }

                // If the customer is not in the database
                return false;
            }
            catch
            {
                return false;
            }
        }

        // Might save to DB to save API calls to polygon
        // StockChange for a single stock the last week
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

                var stockChange2 = await _db.StockChangeValues.FirstOrDefaultAsync(k => k.Symbol == symbol && k.Date == GetTodaysDate().ToString("yyyy-MM-dd"));

                // Returns stockChange if its already in the database. If not it will access the API
                if (stockChange2 != null)
                {
                    return stockChange2;
                }

                //Adds to stock change table
                _db.StockChangeValues.Add(stockChange);
                await _db.SaveChangesAsync();

                return stockChange;

            }
            catch
            {
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
                        continue;
                    var stockObject = new StockOverview()
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
            catch
            {
                return null;
            }
        }
        private async Task<bool> UpdatePortfolio(string socialSecurityNumber)
        {
            try
            {
                // Gets customer object from DB
                var customerFromDB = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customerFromDB == null)
                {
                    return false;
                }

                // Gets transaction objects from DB
                var transactionsBought = await _db.TransactionsBought.Where(k => k.SocialSecurityNumber == socialSecurityNumber).ToListAsync();
                var transactionsSold = await _db.TransactionsSold.Where(k => k.SocialSecurityNumber == socialSecurityNumber).ToListAsync();

                StockChangeValue stockChange;

                var portfolio = await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                // Initilizes a portfolio object if it does not exist
                if (portfolio == null)
                {
                    portfolio = new Portfolio();
                    portfolio.SocialSecurityNumber = socialSecurityNumber;
                    await _db.Portfolios.AddAsync(portfolio);
                }

                // Resets portfolio value
                portfolio.Value = 0;

                var portfolioList = await _db.PortfolioList.Where(k => k.PortfolioId == portfolio.PortfolioId).ToListAsync();

                // Initilizes a portfoliolist object if it does not exist
                if (portfolioList == null)
                {
                    portfolioList = new List<PortfolioList>();
                }

                // Sets amount and value to 0
                foreach (var portfolioListItem in portfolioList)
                {
                    portfolioListItem.Value = 0;
                    portfolioListItem.Amount = 0;
                }

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
                        portfolioList.Add(portfolioItem); // Adds the new portfolio item to the portfolio list. It will be added to the DB when we save later

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

                customerFromDB.Portfolio = portfolio;

                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<Customer> GetCustomerPortfolio(string socialSecurityNumber)
        {
            try
            {
                // Return name, cumtomer info, combined list of transactions, total value of portfolio

                var customerFromDB = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customerFromDB == null)
                {
                    return null;
                }

                var transactionsfromDB = await _db.TransactionsBought.Where(k => k.SocialSecurityNumber == socialSecurityNumber).ToListAsync();
                List<Transaction> transactions = new List<Transaction>();

                // Builds transaction list
                foreach (var transactionDB in transactionsfromDB)
                {
                    var transaction1 = new Transaction
                    {
                        Id = transactionDB.BoughtId,
                        SocialSecurityNumber = transactionDB.SocialSecurityNumber,
                        Date = transactionDB.Date,
                        Symbol = transactionDB.Symbol,
                        Amount = transactionDB.Amount,
                        TotalPrice = transactionDB.TotalPrice
                    };
                    transactions.Add(transaction1);
                }

                // Runs UpdatePortfolio to update it
                await UpdatePortfolio(socialSecurityNumber);

                var portfolio = await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber);
                var portfolioList = await _db.PortfolioList.ToListAsync();

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
                    SocialSecurityNumber = customerFromDB.SocialSecurityNumber,
                    FirstName = customerFromDB.FirstName,
                    LastName = customerFromDB.LastName,
                    Address = customerFromDB.Address,
                    Balance = customerFromDB.Balance,
                    Transactions = transactions,
                    PostalCode = customerFromDB.PostalArea.PostalCode,
                    PostCity = customerFromDB.PostalArea.PostCity,
                    Portfolio = portfolio

                };


                return customer;
            }
            catch
            {
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
            return News;
        }
        public async Task<String> GetStockName(string symbol)
        {
            var stock = await _db.Stocks.FindAsync(symbol);
            return stock == null ? "" : stock.Name;
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
                return false;
            }
        }

        public static DateTime GetTodaysDate()
        {
            //DateTime date1 = DateTime.Now;
            //date1 = date.AddDays(-1)


            var date = new DateTime(2022, 09, 20);  // Using fixed date since it takes a couple of minutes to get the stock change

            // If day of week is a weekend then the last price if from friday
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

    }
}