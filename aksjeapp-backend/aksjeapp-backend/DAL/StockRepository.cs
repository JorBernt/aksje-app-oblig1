using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Microsoft.EntityFrameworkCore;

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
                    stocks.Date = date.ToString("yyyy-MM-dd");
                    date = date.AddDays(1);
                }
                stock.results = results;
            }

            stock.Name = symbol;
            stock.Last = results[0].ClosePrice;
            stock.Change = results[0].ClosePrice / results[^1].ClosePrice;
            stock.TodayDifference = results[0].ClosePrice - results[^1].ClosePrice;
            stock.Buy = 10;
            stock.Sell = 100;
            stock.High = results[0].LowestPrice;
            stock.Low = results[0].HighestPrice;
            stock.Turnover = 10000;
            return stock;
        }

        /* public async Task<bool> BuyStock2(string socialSecurityNumber, string symbol, int number)
         {
             // Gets todays date
             string date = GetTodaysDate().ToString("yyyy-MM-dd");

             try
             {
                 //Get todays price and and set the todays date
                 var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                 double totalPrice = stockPrice.ClosePrice * number;
                 //Creates transaction
                 var stockTransaction = new Transaction
                 {
                     Date = date,
                     SocialSecurityNumber = socialSecurityNumber,
                     Symbol = symbol,
                     Amount = number,
                     TotalPrice = totalPrice
                 };

                 var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                 if (customer == null)
                 {
                     return false;
                 }
                 customer.Transactions.Add(stockTransaction);
                 customer.Balance -= stockTransaction.TotalPrice - 5; //5 dollars in brokerage fee
                 await _db.SaveChangesAsync();

                 return true;
             }
             catch
             {
                 return false;
             }

         }*/
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
                var portfolio = await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber && k.LastUpdated == GetTodaysDate().ToString("yyyy-MM-dd"));
                if (portfolio == null)
                {
                    Console.WriteLine("Finner ikke portfolio");
                    return false;
                }

                var portfolioList = await _db.PortfolioList.FirstOrDefaultAsync(k => k.PortfolioId == portfolio.PortfolioId && k.Symbol == symbol);
                if (portfolioList == null)
                {
                    Console.WriteLine("Finner ikke portfolioListe");
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
                Console.WriteLine("Noe annet som er feil");
                return false;
            }

        }

        public async Task<List<Stock>> ReturnSearchResults(string keyPhrase)
        {
            try
            {
                if (keyPhrase == "")
                {
                    return null;
                }
                var stocks = await _db.Stocks.Where(k => k.Symbol.Contains(keyPhrase) || k.Name.ToUpper().Contains(keyPhrase) || k.Country.ToUpper().Contains(keyPhrase) || k.Sector.ToUpper().Contains(keyPhrase)).OrderBy(k => k.Name).ToListAsync();
                return stocks;

            }
            catch
            {
                return null;
            }
        }

        public async Task<List<Transaction>> GetAllTransactions(string socialSecurityNumber)
        {
            // Checks if socialSecuritynumber is correct
            var customer = await _db.Customers.FindAsync(socialSecurityNumber);

            if (customer != null)
            {
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
                if (GetTodaysDate().ToString("yyyy-MM-dd") == transaction.Date)
                {
                    //Removes transaction price from customers balance
                    customer.Balance += transaction.TotalPrice;

                    // Gets todays price for the new stock
                    var stockPrice = await PolygonAPI.GetOpenClosePrice(changeTransaction.Symbol, GetTodaysDate().ToString("yyyy-mm-dd"));

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
                    if (GetTodaysDate().ToString("yyyy-MM-dd") == transaction.Date)
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

                var stockChange = await _db.StockChangeValues.FirstOrDefaultAsync(k => k.Symbol == symbol && k.Date == GetTodaysDate().ToString("yyyy-MM-dd"));

                // Returns stockChange if its already in the database. If not it will access the API
                if (stockChange != null)
                {
                    return stockChange;
                }


                var date = GetTodaysDate().AddDays(-7); ;

                var fromDate = date.ToString("yyyy-MM-dd");

                var stockPrice1 = await PolygonAPI.GetStockPrices(symbol, fromDate, GetTodaysDate().ToString("yyyy-MM-dd"), 1);

                if (stockPrice1.results == null)
                {
                    return null;
                }
                List<Models.Results> results = stockPrice1.results;

                double change = ((results.Last().ClosePrice - results.First().ClosePrice) / results.Last().ClosePrice) * 100;

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
                Console.WriteLine("Returnerer null");
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
                    portfolio.LastUpdated = GetTodaysDate().ToString("yyyy-MM-dd");
                }

                // Resets portfolio value
                portfolio.Value = 0;
                portfolio.LastUpdated = GetTodaysDate().ToString("yyyy-MM-dd");

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
                        Console.WriteLine(transaction.Amount);
                        portfolioList[index].Amount += transaction.Amount;
                        portfolioList[index].Value += stockChange.Value * transaction.Amount;
                    }
                    // Adds the first of this symbol to portofolio list
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
                await _db.Portfolios.AddAsync(portfolio);
                await _db.PortfolioList.AddRangeAsync(portfolioList);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<Customer> GetCustomerPortofolio(string socialSecurityNumber)
        {
            try
            {
                // Return name, cumtomer info, combined list of transactions, total value of portofolio

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

                var portofolio = await _db.Portfolios.FirstOrDefaultAsync(k => k.SocialSecurityNumber == socialSecurityNumber && k.LastUpdated == GetTodaysDate().ToString("yyyy-MM-dd"));
                var portofolioList = await _db.PortfolioList.ToListAsync();

                // Adds portfoliolist to portfolio
                if (portofolio == null)
                {
                    portofolio = new Portfolio();
                }
                else
                {
                    portofolio.StockPortfolio = portofolioList;
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
                    Portfolio = portofolio

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
            var Winners = await _db.StockChangeValues.OrderByDescending(k => k.Change).Where(k => k.Date == GetTodaysDate().ToString("yyyy-MM-dd") && k.Change > 0).Take(7).ToListAsync();

            return Winners;
        }
        public async Task<List<StockChangeValue>> GetLosers()
        {
            var Losers = await _db.StockChangeValues.OrderBy(k => k.Change).Where(k => k.Date == GetTodaysDate().ToString("yyyy-MM-dd") && k.Change < 0).Take(7).ToListAsync();

            return Losers;
        }

        public async Task<News> GetNews(string symbol)
        {
            var News = await PolygonAPI.GetNews(symbol);
            return News;
        }

        public static DateTime GetTodaysDate()
        {
            //DateTime date1 = DateTime.Now;
            //date1 = date1.AddMonths(-1);//Uses one month old data since polygon cant get todays date
            var date1 = new DateTime(2022, 09, 25);

            // If day of week is a weekend then the last price if from friday
            if (date1.DayOfWeek.Equals(DayOfWeek.Saturday))
            {
                date1 = date1.AddDays(-1);
            }
            if (date1.DayOfWeek.Equals(DayOfWeek.Sunday))
            {
                date1 = date1.AddDays(-2);
            }

            return date1;

        }


    }

}
