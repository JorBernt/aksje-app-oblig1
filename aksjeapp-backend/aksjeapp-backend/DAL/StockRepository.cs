﻿using aksjeapp_backend.Models;
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
            var stock = await PolygonAPI.GetStockPrices(symbol, fromDate, toDate,1);

            return stock;
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            // Gets todays date
            string date = GetTodaysDate();

            try
            {
                //Get todays price and and set the todays date
                var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, date);

                double totalPrice = stockPrice.OpenPrice * number;
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

                stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, GetTodaysDate());

                for (int i = 0; i < transactions.Count; i++)
                {

                    if (transactions[i].Amount > number)
                    {


                        // Need to split transaction since we are not selling the same amount we bought.
                        var transaction = new Transaction()
                        {
                            SocialSecurityNumber = transactions[i].SocialSecurityNumber,
                            Date = GetTodaysDate(),
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
                    customer.Balance -= stockPrice.ClosePrice * (double)number1;
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
            if (keyPhrase != "")
            {
                var stocks = await _db.Stocks.Where(k => k.Symbol.Contains(keyPhrase) || k.Name.ToUpper().Contains(keyPhrase) || k.Country.ToUpper().Contains(keyPhrase) || k.Sector.ToUpper().Contains(keyPhrase)).OrderBy(k => k.Name).ToListAsync();
                return stocks;
            }
            else
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
                    if (GetTodaysDate() == transaction.Date && transaction.IsActive == true)
                    {
                        //Removes transaction price from customers balance
                        customer.Balance += transaction.TotalPrice;

                        // Gets todays price for the new stock
                        var stockPrice = await PolygonAPI.GetOpenClosePrice(changeTransaction.Symbol, GetTodaysDate());

                        transaction.Symbol = changeTransaction.Symbol;
                        transaction.Amount = changeTransaction.Amount;
                        transaction.TotalPrice = stockPrice.ClosePrice * changeTransaction.Amount;

                        customer.Balance -= transaction.TotalPrice;
                        await _db.SaveChangesAsync();
                        return true;
                    }
                    Console.WriteLine("Transaction is too old to change, you could sell it and buy it again");
                }
                return false;
            }
            catch
            {
                Console.Write("Could not update stock");
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
                    if (GetTodaysDate() == transaction.Date && transaction.IsActive == true)
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
        public async Task<StockChangeValue> StockChange(string symbol, string fromDate)
        {
            try
            {
                var stockPrice1 = await PolygonAPI.GetStockPrices(symbol, fromDate, GetTodaysDate(), 1);
                
                if (stockPrice1.results != null)
                {
                    List<Models.Results> results = stockPrice1.results;
                
                    Console.WriteLine(stockPrice1.results);
                double change = ((results.Last().ClosePrice - results.First().ClosePrice) / results.Last().ClosePrice ) * 100;

                var stockChange = new StockChangeValue()
                {
                    Symbol = symbol,
                    Change = change,
                    Value = results.Last().ClosePrice
                };
                Console.WriteLine(change);

                return stockChange;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }


        public static string GetTodaysDate()
        {
            DateTime date1 = DateTime.Now;
            int month = date1.Month - 1; //Uses one month old data since polygon cant get todays prices
            string date = date1.Year + "-" + month.ToString("D2") + "-" + date1.Day.ToString("D2");
            return date;
        }


    }

}
