﻿using aksjeapp_backend.Models;
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

            return stock;
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
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
                customer.Balance -= stockTransaction.TotalPrice;
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {

                return false;
            }

        }

        public async Task<bool> SellStock(string socialSecurityNumber, string symbol, int number)
        {
            OpenCloseStockPrice stockPrice;
            int number1 = number;
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                //Breaks if we cant find customer
                if (customer == null)
                {
                    return false;
                }

                // Finds transactions
                List<Transaction> transactions = customer.Transactions.Where(k => k.Symbol == symbol && k.IsActive == true).ToList();

                stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, GetTodaysDate().ToString("yyyy-MM-dd"));

                for (int i = 0; i < transactions.Count; i++)
                {

                    if (transactions[i].Amount > number)
                    {


                        // Need to split transaction since we are not selling the same amount we bought.
                        var transaction = new Transaction()
                        {
                            SocialSecurityNumber = transactions[i].SocialSecurityNumber,
                            Date = GetTodaysDate().ToString("yyyy-MM-dd"),
                            Symbol = transactions[i].Symbol,
                            Amount = transactions[i].Amount - number
                        };
                        transaction.TotalPrice = stockPrice.ClosePrice * transaction.Amount; // Updates after since we need stock number to be updated

                        transactions[i].IsActive = false;

                        customer.Transactions.Add(transaction);
                        number = 0;
                        break;
                    }
                    else if (transactions[i].Amount < number)
                    {
                        var transaction = transactions[i];
                        number -= transaction.Amount;
                        transactions[i].IsActive = false;

                    }
                    else if (transactions[i].Amount == number)
                    {
                        transactions[i].IsActive = false;
                        number = 0;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Fault in sell stock method");
                        return false;
                    }

                }

                if (number == 0)
                {
                    customer.Balance += stockPrice.ClosePrice * (double)number1;
                    await _db.SaveChangesAsync();

                    return true;
                }
                else { return false; }
            }
            catch
            {
                return false;
            }
        }

        public async Task<List<Stock>> ReturnSearchResults(string keyPhrase)
        {
            try {
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
                var transactions = customer.Transactions.Where(k => k.IsActive == true).ToList();
                return transactions;
            }
            return null;
        }

        public async Task<Transaction> GetTransaction(string socialSecurityNumber, int id)
        {
            // Checks if socialSecuritynumber is correct
            var customer = await _db.Customers.FindAsync(socialSecurityNumber);
            if (customer != null)
            {
                Transaction transaction = await _db.Transactions.FindAsync(id);
                return transaction;
            }
            return null;
        }

        // It is only possible to change transaction after you have bought it, you cannot change it if you sold
        public async Task<bool> UpdateTransaction(Transaction changeTransaction)
        {
            try
            {
                var customer = await _db.Customers.FindAsync(changeTransaction.SocialSecurityNumber);
                if (customer != null)
                {
                    Transaction transaction = await _db.Transactions.FindAsync(changeTransaction.Id);
                    if (GetTodaysDate().ToString("yyyy-MM-dd") == transaction.Date && transaction.IsActive == true)
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
                    var transaction = _db.Transactions.Find(id);

                    //Returns if the transaction does not exist
                    if (transaction == null)
                    {
                        return false;
                    }

                    // Deletes transaction that is still active
                    if (GetTodaysDate().ToString("yyyy-MM-dd") == transaction.Date && transaction.IsActive == true)
                    {
                        _db.Transactions.Remove(transaction);
                        customer.Balance += transaction.TotalPrice; //Updates balance for customer
                        await _db.SaveChangesAsync();
                        return true;

                    }
                    else
                    {
                        _db.Transactions.Remove(transaction);
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

                var stockChange = await _db.StockChangeValues.FirstOrDefaultAsync(k => k.Symbol == symbol && k.Date == "2022-09-18");

                // Returns stockChange if its already in the database. If not it will access the API
                if (stockChange != null)
                {
                    return stockChange;
                }


                var date = GetTodaysDate().AddDays(-7); ;

                var fromDate = date.ToString("yyyy-MM-dd");

                var stockPrice1 = await PolygonAPI.GetStockPrices(symbol, fromDate, "2022-09-18", 1);

                if (stockPrice1.results != null)
                {
                    List<Models.Results> results = stockPrice1.results;

                    Console.WriteLine(stockPrice1.results);
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
                return null;
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
                    if(myStock == null)
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

        public async Task<Customer?> GetCustomerPortofolio(string socialSecurityNumber)
        {
            // Return name, cumtomer info, combined list of transactions, total value of portofolio

            var customerFromDB = await _db.Customers.FindAsync(socialSecurityNumber);
            var transactions = await _db.Transactions.Where(k => k.SocialSecurityNumber == socialSecurityNumber && k.IsActive == true).ToListAsync();
            if (customerFromDB == null)
            {
                return null;
            }
            var customer = new Customer()
            {
                SocialSecurityNumber = customerFromDB.SocialSecurityNumber,
                FirstName = customerFromDB.FirstName,
                LastName = customerFromDB.LastName,
                Address = customerFromDB.Address,
                Balance = customerFromDB.Balance,
                Transactions = customerFromDB.Transactions,
                PostalCode = customerFromDB.PostalArea.PostalCode,
                PostCity = customerFromDB.PostalArea.PostCity,
            };
            var portofolio = new Portofolio();
            var portofolioList = new List<PortofoilioList>();

            // Find the amount of each stock the customer has
            foreach (var transaction in transactions)
            {
                int index = portofolioList.FindIndex(k => k.Symbol.Equals(transaction.Symbol));
                // Sums the amount of stocks if found
                if (index >= 0)
                {
                    portofolioList[index].Amount += transaction.Amount;
                }
                // Adds the first of this symbol to portofolio list
                else
                {
                    var portofolioItem = new PortofoilioList()
                    {
                        Symbol = transaction.Symbol,
                        Amount = transaction.Amount,
                    };
                    portofolioList.Add(portofolioItem);
                }
            }
            portofolio.StockPortofolio = portofolioList;
            foreach (var stock in portofolio.StockPortofolio)
            {
                var stockChange = await StockChange(stock.Symbol);
                portofolio.Value += stockChange.Value * stock.Amount;
            }
            customer.Portofolio = portofolio;
            return customer;
        }

        public async Task<List<StockChangeValue>> GetWinners()
        {
            var Winners = await _db.StockChangeValues.OrderByDescending(k => k.Change).Take(7).ToListAsync();
            if (Winners.Count < 7)
            {
                await GetStockOverview();
                return await GetWinners();
            }
            return Winners;
        }
        public async Task<List<StockChangeValue>> GetLosers()
        {
            var Losers = await _db.StockChangeValues.OrderBy(k => k.Change).Take(7).ToListAsync();
            if (Losers.Count < 7)
            {
                await GetStockOverview();
                return await GetLosers();
            }
            return Losers;
        }

        public async Task<News> GetNews(string symbol)
        {
            var News = await PolygonAPI.GetNews(symbol);
            return News;
        }

        public static DateTime GetTodaysDate()
        {
            DateTime date1 = DateTime.Now;
            date1 = date1.AddMonths(-1);//Uses one month old data since polygon cant get todays date
            //var date1 = new DateTime(25/09/2022);

            // If day of week is a weekend then the last price if from friday
            if (date1.DayOfWeek.Equals("Saturday"))
            {
                date1 = date1.AddDays(-1);
            }
            if (date1.DayOfWeek.Equals("Sunday"))
            {
                date1 = date1.AddDays(-2);
            }
            return date1;

        }


    }

}
