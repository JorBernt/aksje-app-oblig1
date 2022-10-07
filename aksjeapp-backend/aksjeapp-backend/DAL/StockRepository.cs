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
            var stock = await PolygonAPI.GetStockPrices(symbol, fromDate, toDate);

            return stock;
        }

        public async Task<bool> BuyStock(string socialSecurityNumber, string symbol, int number)
        {
            // Gets todays date
            string date = GetTodaysDate();
            Console.WriteLine(date);
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
                    Number = number,
                    TotalPrice = totalPrice
                };
                Console.WriteLine("Buying stock");
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);

                if (customer == null)
                {
                    return false;
                }
                customer.Transactions.Add(stockTransaction);

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
            try
            {
                var customer = await _db.Customers.FindAsync(socialSecurityNumber);
                if (customer == null)
                {
                    return false;
                }
                // Finds transactions
                List<Transaction> transactions = customer.Transactions.Where(k => k.Symbol == symbol && k.IsActive == true).ToList();

                for (int i = 0; i < transactions.Count; i++)
                {
                    if (transactions[i].Number > number)
                    {
                        var stockPrice = await PolygonAPI.GetOpenClosePrice(symbol, GetTodaysDate());
                        // Need to split transaction since we are not selling the same amount we bought.
                        var transaction = new Transaction()
                        {
                            SocialSecurityNumber = transactions[i].SocialSecurityNumber,
                            Date = GetTodaysDate(),
                            Symbol = transactions[i].Symbol,
                            Number = transactions[i].Number - number
                        };
                        transaction.TotalPrice = stockPrice.OpenPrice * transaction.Number; // Updates after since we need stock number to be updated

                        transactions[i].IsActive = false;
                        
                        customer.Transactions.Add(transaction);
                        number = 0;
                        break;
                    }
                    else if (transactions[i].Number < number)
                    {
                        var transaction = transactions[i];
                        number -= transaction.Number;
                        transactions[i].IsActive = false;
                        //customer.Transactions.Add(transaction);

                    }
                    else if (transactions[i].Number == number)
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
        public static string GetTodaysDate()
        {
            DateTime date1 = DateTime.Now;
            int month = date1.Month - 1; //Uses one month old data since polygon cant get todays prices
            string date = date1.Year + "-" + month.ToString("D2") + "-" + date1.Day.ToString("D2");
            return date;
        }
    }


}