using aksjeapp_backend.Controller;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTesting_aksjeapp
{
    public class UnitTest_StockController
    {
        
        private static readonly Mock<IStockRepository> mockRep = new Mock<IStockRepository>();
        private static readonly Mock<ILogger<StockController>> mockLog = new Mock<ILogger<StockController>>();
        private readonly StockController _stockController = new StockController(mockRep.Object, mockLog.Object);

        [Fact]
        public async Task GetAllStocks_Ok()
        {

            //Arrange
            var stock1 = new Stock
            {
                Symbol = "AAPL",
                Name = "Apple Inc. Common Stock",
                Country = "United States",
                Sector = "Technology"
            };
            var stock2 = new Stock
            {
                Symbol = "ABMD",
                Name = "ABIOMED Inc. Common Stock",
                Country = "United States",
                Sector = "Health Care"
            };
            var stock3 = new Stock
            {
                Symbol = "ABNB",
                Name = "Airbnb Inc. Class A Common Stock",
                Country = "United States",
                Sector = "Consumer Discretionary"
            };
            var stockList = new List<Stock>();
            stockList.Add(stock1);
            stockList.Add(stock2);
            stockList.Add(stock3);

            mockRep.Setup(k => k.GetAllStocks()).ReturnsAsync(stockList);
            //Act
            var result = await _stockController.GetAllStocks() as OkObjectResult;
            //Assert
            Assert.Equal(stockList, result.Value);
        }
        [Fact]
        public async Task GetAllStocks_NotOk()
        {
            //Arrange
            var stockList = new List<Stock>();

            mockRep.Setup(k => k.GetAllStocks()).ReturnsAsync(stockList);
            //Act
            var result = await _stockController.GetAllStocks() as BadRequestObjectResult;
            //Assert
            Assert.Equal("Not found", result.Value);
        }


        public async Task GetAllTransactions_Ok()
        {
            //Arrange
            var SSN = "12345678910";
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction
            {
                Id = 1,
                Amount = 3,
                Awaiting = false,
                Date = "2022-05-20",
                Symbol = "APLL",
                TotalPrice = 456,
                SocialSecurityNumber = SSN
            });
            transactions.Add(new Transaction
            {
                Id = 2,
                Amount = 6,
                Awaiting = false,
                Date = "2022-10-01",
                Symbol = "TESL",
                TotalPrice = 154,
                SocialSecurityNumber = SSN
            });
            transactions.Add(new Transaction
            {
                Id = 3,
                Amount = 5,
                Awaiting = false,
                Date = "2022-09-14",
                Symbol = "ALD",
                TotalPrice = 785,
                SocialSecurityNumber = SSN
            });
            mockRep.Setup(k => k.GetAllTransactions(SSN)).ReturnsAsync(transactions);

            //Act
            var results = await _stockController.GetAllTransactions(SSN) as OkObjectResult;

            //Assert
            Assert.Equal<List<Transaction>>(transactions, (List<Transaction>)results.Value);
        }

        [Fact]
        public async Task GetAllTransactions_Empty()
        {
            //Arrange
            var SSN = "12345678910";
            mockRep.Setup(k => k.GetAllTransactions(SSN)).ReturnsAsync(new List<Transaction>());

            //Act
            var results = await _stockController.GetAllTransactions(SSN) as OkObjectResult;

            //Assert
            Assert.Null(results);
        }

         [Fact]
        public async Task GetTransaction()
        {

            var transaction = new Transaction
            {
                Id = 1,
                SocialSecurityNumber = "12345678910",
                Date = "2022/09/10",
                Symbol = "AAPl",
                Amount = 200,
                TotalPrice = 3500
            };

            mockRep.Setup(k => k.GetTransaction(transaction.SocialSecurityNumber, transaction.Id)).ReturnsAsync(transaction);
            var stockController = new StockController(mockRep.Object, mockLog.Object);
            var res = await stockController.GetTransaction(transaction.SocialSecurityNumber, transaction.Id) as OkObjectResult;
            
            Assert.Equal(transaction, transaction);

        }

        [Fact]
        public async Task BuyStock()
        {
            //Arrange
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            mockRep.Setup(k => k.BuyStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(true);

            //Act
            var res = await _stockController.BuyStock(pers, symbol, amount) as OkObjectResult;
            //Assert
            Assert.Equal("Stock bought", res.Value);

        }


        [Fact]
        public async Task SellStock()
        {
            //Arrange
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            mockRep.Setup(k => k.SellStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(true);

            //Act
            var res = await _stockController.SellStock(pers, symbol, amount) as OkObjectResult;
            // Assert
            Assert.Equal("Stock sold", res.Value);

        }

        [Fact]
        public async Task GetNews_Ok()
        {
            var myPublisher = new Publisher();
            myPublisher.Name = "TestPublisher";

            List<String> myStocks = new List<string>();
            myStocks.Add("AAPL");
            var myNews = new NewsResults
            {
                Publisher = myPublisher,
                Title = "Prices are skyrocketing",
                Author = "Per Hansen",
                Date = "2022-05-20",
                Stocks = myStocks,
                url = "localhost:3000"
            };

            News myNewsList = new News();

            List<NewsResults> myResultsList = new List<NewsResults>();
            myNewsList.Results = myResultsList;

            myResultsList.Add(myNews);

            mockRep.Setup(k => k.GetNews("AAPL")).ReturnsAsync(myNewsList);
            var stockController = new StockController(mockRep.Object, mockLog.Object);

            var res = await stockController.GetNews("AAPL") as OkObjectResult;

            Assert.Equal(myNewsList, res.Value);
        }
        [Fact]
        public async Task GetNews_empty()
        {
            var myPublisher = new Publisher();
            myPublisher.Name = "TestPublisher";

            List<String> myStocks = new List<string>();
            myStocks.Add("AAPL");
            var myNews = new NewsResults
            {
                Publisher = myPublisher,
                Title = "Prices are skyrocketing",
                Author = "Per Hansen",
                Date = "2022-05-20",
                Stocks = myStocks,
                url = "localhost:3000"
            };

            News myNewsList = new News();

            List<NewsResults> myResultsList = new List<NewsResults>();
            myNewsList.Results = myResultsList;

            myResultsList.Add(myNews);
            var symbol = "AAPL";

            mockRep.Setup(k => k.GetNews(symbol)).ReturnsAsync(new News());
            var stockController = new StockController(mockRep.Object, mockLog.Object);

            var res = await stockController.GetNews(symbol) as BadRequestObjectResult;

            //Assert.Equal(myNewsList, res.Value);
            Assert.Equal("Could not find any news", res.Value);
        }
    }
    
}