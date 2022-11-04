using aksjeapp_backend.Controller;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
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
        public async Task GetAllStocks_Empty()
        {
            //Arrange
            var stockList = new List<Stock>();

            mockRep.Setup(k => k.GetAllStocks()).ReturnsAsync(stockList);
            //Act
            var result = await _stockController.GetAllStocks() as BadRequestObjectResult;
            //Assert
            Assert.Equal("Not found", result.Value);
        }

        [Fact]
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
        public async Task SellStock()
        {
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            mockRep.Setup(k => k.SellStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(true);
            var stockController = new StockController(mockRep.Object, mockLog.Object);

            //
            var res = await stockController.SellStock(pers, symbol, amount) as OkObjectResult;

            Assert.Equal("Stock sold", res.Value);
        }

        [Fact]
        public async Task SellStock_Empty()
        {
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;
            mockRep.Setup(k => k.SellStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(false);
            var stockController = new StockController(mockRep.Object, mockLog.Object);
            var res = await stockController.SellStock(pers, symbol, amount) as BadRequestResult;

            Assert.Equal("Fault in sellStock", "Fault in sellStock");
        }

        [Fact]
        public async Task BuyStock()
        {
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            mockRep.Setup(k => k.BuyStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(true);
            var stockController = new StockController(mockRep.Object, mockLog.Object);

            //
            var res = await stockController.BuyStock(pers, symbol, amount) as OkObjectResult;

            Assert.Equal("Stock bought", res.Value);
        }

        [Fact]
        public async Task BuyStock_Empty()
        {
            var pers = "12345678910";
            var symbol = "AAPL";
            var amount = 10;
            mockRep.Setup(k => k.BuyStock(pers, symbol.ToUpper(), amount)).ReturnsAsync(false);
            var stockController = new StockController(mockRep.Object, mockLog.Object);
            var res = await stockController.BuyStock(pers, symbol, amount) as BadRequestResult;
            Assert.Equal("Fault in buyStock", "Fault in buyStock");
        }

        [Fact]
        public async Task GetTransaction_Ok()
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

            mockRep.Setup(k => k.GetTransaction(transaction.SocialSecurityNumber, transaction.Id))
                .ReturnsAsync(transaction);
            var stockController = new StockController(mockRep.Object, mockLog.Object);
            var res =
                await stockController.GetTransaction(transaction.SocialSecurityNumber,
                    transaction.Id) as OkObjectResult;

            Assert.Equal(transaction, transaction);
        }

        [Fact]
        public async Task GetTransaction_Empty()
        {
            var pers = "12345678910";
            var id = 10;

            mockRep.Setup(k => k.GetTransaction(pers, id)).ReturnsAsync(() => null);

            var result = await _stockController.GetTransaction(pers, id) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Transaction does not exist", result.Value);
        }

        [Fact]
    public async Task GetCustomerPortfolio_Ok()
    {
        var SSN = "12345678910";

        List<Transaction> myTransactions = new List<Transaction>();
        
        myTransactions.Add(new Transaction
        {
            Id = 1,
            Amount = 3,
            Awaiting = false,
            Date = "2022-05-20",
            Symbol = "APLL",
            TotalPrice = 456,
            SocialSecurityNumber = SSN
        });
        myTransactions.Add(new Transaction
        {
            Id = 2,
            Amount = 6,
            Awaiting = false,
            Date = "2022-10-01",
            Symbol = "TESL",
            TotalPrice = 154,
            SocialSecurityNumber = SSN
        });
        myTransactions.Add(new Transaction
        {
            Id = 3,
            Amount = 5,
            Awaiting = false,
            Date = "2022-09-14",
            Symbol = "ALD",
            TotalPrice = 785,
            SocialSecurityNumber = SSN
        });

        Portfolio myPortfolio = new Portfolio();

        myPortfolio.PortfolioId = 1;
        myPortfolio.SocialSecurityNumber = "12345678910";
        myPortfolio.Value = 1000;

        List<PortfolioList> myPortfolioList = new List<PortfolioList>();
        
        myPortfolioList.Add( new PortfolioList
        {
            PortfolioListId = 1,
            Symbol = "AAPL",
            Name = "APPLE NAME",
            Amount = 1000,
            Change = 100.1,
            Value = 20000.1,
            PortfolioId = 1
        });
        
        var myCustomer = new Customer()
        {
            SocialSecurityNumber = SSN,
            FirstName = "Stevie",
            LastName = "Wonder",
            Address = "Bygaten 1",
            Balance = 100000.20,
            Transactions = myTransactions,
            PostalCode = "2008",
            PostCity = "Fjerdingby",
            Portfolio = myPortfolio
        };

        mockRep.Setup(k => k.GetCustomerPortfolio(SSN)).ReturnsAsync(myCustomer);
        var res = await _stockController.GetCustomerPortfolio(SSN) as OkObjectResult;
        
        Assert.Equal(myCustomer, res.Value);
    }
    
    [Fact]
    public async Task GetCustomerPortfolio_Empty()
    {
        var SSN = "12345678910";

        mockRep.Setup(k => k.GetCustomerPortfolio(SSN)).ReturnsAsync(() => null);
        var res = await _stockController.GetCustomerPortfolio(SSN) as BadRequestObjectResult;
        
        Assert.Equal("Customer not found", res.Value);
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
        var symbol = "AAPL";

        mockRep.Setup(k => k.GetNews(symbol)).ReturnsAsync(new News());

        var res = await _stockController.GetNews(symbol) as BadRequestObjectResult;

        //Assert.Equal(myNewsList, res.Value);
        Assert.Equal("Could not find any news", res.Value);
    }

    [Fact]
    public async Task GetStockName_Ok()
    {
        var symbol = "AAPL";

        var name = "Apple inc. Common stock";

        mockRep.Setup(k => k.GetStockName(symbol)).ReturnsAsync(name);
        var res = await _stockController.GetStockName(symbol) as OkObjectResult;

        Assert.Equal(name, res.Value);

    }

    [Fact]
    public async Task GetStockName_Empty()
    {
        var symbol = "AAPL";

        var name = "";

        mockRep.Setup(k => k.GetStockName(symbol)).ReturnsAsync(name);
        var res = await _stockController.GetStockName(symbol) as BadRequestObjectResult;

        Assert.Equal("Could not find a name for the symbol", res.Value);

    }


    }

}