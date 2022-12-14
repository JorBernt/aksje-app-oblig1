using aksjeapp_backend.Controller;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using aksjeapp_backend.Models.News;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Net;
using Results = aksjeapp_backend.Models.Results;

namespace UnitTesting_aksjeapp

{
    public class UnitTestStockController
    {
        private const string _loggedIn = "SocialSecurityNumber";
        private const string _notLoggedIn = "";

        private static readonly Mock<IStockRepository> MockRep = new Mock<IStockRepository>();
        private static readonly Mock<ILogger<StockController>> MockLog = new Mock<ILogger<StockController>>();
        private readonly StockController _stockController = new StockController(MockRep.Object, MockLog.Object);

        private readonly Mock<HttpContext> mockHttpContext = new Mock<HttpContext>();
        private readonly MockHttpSession mockSession = new MockHttpSession();


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
            var stockList = new List<Stock>
            {
                stock1,
                stock2,
                stock3
            };

            MockRep.Setup(k => k.GetAllStocks()).ReturnsAsync(stockList);
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

            MockRep.Setup(k => k.GetAllStocks()).ReturnsAsync(stockList);
            //Act
            var result = await _stockController.GetAllStocks() as BadRequestObjectResult;
            //Assert
            Assert.Equal("Not found", result.Value);
        }

        [Fact]
        public async Task GetStockPrices_Ok()
        {
            //Arrange
            var results = new List<Results>
            {
                new Results
                {
                    Date = "2022-05-11", ClosePrice = 10.00, OpenPrice = 9.00, HighestPrice = 12.00, LowestPrice = 9.00
                },
                new Results
                {
                    Date = "2022-05-12", ClosePrice = 11.00, OpenPrice = 9.00, HighestPrice = 13.50, LowestPrice = 8.00
                }
            };

            var stockPrices = new StockPrices
            {
                Symbol = "AAPL",
                Name = "Apple Inc Common Stock",
                Last = 12.00,
                Change = 1.4,
                TodayDifference = 0.5,
                Buy = 100,
                Sell = 40,
                High = 13.00,
                Low = 9.00,
                Turnover = 10000000,
                results = results
            };

            MockRep.Setup(k => k.GetStockPrices("AAPL", "2022-05-11")).ReturnsAsync(stockPrices);

            //Act
            var result = await _stockController.GetStockPrices("AAPL", "2022-05-11") as OkObjectResult;

            //Assert
            Assert.Equal(stockPrices, result.Value);
        }

        [Fact]
        public async Task GetStockPrices_Empty()
        {

            MockRep.Setup(k => k.GetStockPrices(It.IsAny<string>(),It.IsAny<string>())).ReturnsAsync( () => null );

            //Act
            var result = await _stockController.GetStockPrices("AAPL", "2022-05-11") as BadRequestObjectResult;

            //Assert
            Assert.Equal("Not found", result.Value);
        }

        [Fact]
        public async Task GetStockPrices_EmptySymbol()
        {
            //Act
            var result = await _stockController.GetStockPrices("", "2022-10-24") as BadRequestObjectResult;

            //Assert 
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task GetStockPrices_EmptyDate()
        {

            //Act
            var result = await _stockController.GetStockPrices("AAPL", "") as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task BuyStockLoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = 10;


            MockRep.Setup(k => k.BuyStock(socialSecurityNumber, symbol.ToUpper(), amount)).ReturnsAsync(true);
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;


            //Act
            var result = await _stockController.BuyStock(symbol, amount) as OkObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Stock bought", result.Value);
        }

        [Fact]
        public async Task BuyStockLoggedInNegativeNumber()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = -10;
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.BuyStock(symbol, amount) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Cannot buy negative amount of stock", result.Value);
        }

        [Fact]
        public async Task BuyStockLoggedInWrongInput()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "A";
            var amount = 10;


            //MockRep.Setup(k => k.BuyStock(socialSecurityNumber, symbol.ToUpper(), amount)).ReturnsAsync(true);
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;


            //Act
            var result = await _stockController.BuyStock(symbol, amount) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }


        [Fact]
        public async Task BuyStockNotLoggedIn()
        {
            //Arrange

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            //Act
            var result = await _stockController.BuyStock(It.IsAny<string>(), It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task BuyStock_Empty()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            MockRep.Setup(k => k.BuyStock(socialSecurityNumber, symbol.ToUpper(), amount)).ReturnsAsync(false);
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(s => s.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.BuyStock(symbol, amount) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in buyStock", result.Value);
        }

        [Fact]
        public async Task SellStockLoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = 10;

            MockRep.Setup(k => k.SellStock(socialSecurityNumber, symbol.ToUpper(), amount)).ReturnsAsync(true);
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.SellStock(symbol, amount) as OkObjectResult;
            
            //Assert
            Assert.Equal("Stock sold", result.Value);
        }
        [Fact]
        public async Task SellStockLoggedInWrongInput()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "A";
            var amount = 10;

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.SellStock(symbol, amount) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task SellStock_LoggedInFault()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = 10;
            
            MockRep.Setup(k => k.SellStock(socialSecurityNumber, symbol.ToUpper(), amount)).ReturnsAsync(false);
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _stockController.SellStock(symbol, amount) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in sellStock", result.Value);
        }
        
        [Fact]
        public async Task SellStockLoggedInNegativeNumber()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = -10;
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.SellStock(symbol, amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Cannot sell negative stock", result.Value);
        }
        
        [Fact]
        public async Task SellStockNotLoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var symbol = "AAPL";
            var amount = 10;
            
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _stockController.SellStock(symbol, amount) as UnauthorizedResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, result.StatusCode);
        }
        
        [Fact]
        public async Task SearchResult_Ok()
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


            MockRep.Setup(k => k.ReturnSearchResults("A")).ReturnsAsync(stockList);

            //Act
            var result = await _stockController.SearchResults("A") as OkObjectResult;

            //Assert
            Assert.Equal(stockList, result.Value);
        }

        [Fact]
        public async Task SearchResult_EmptyKeyPhrase()
        {
            var stockList = new List<Stock>();
            //Arrange
            //MockRep.Setup(k => k.ReturnSearchResults("")).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.SearchResults("") as OkObjectResult;

            //Assert
            Assert.Equal(stockList, result.Value);
        }

        [Fact]
        public async Task SearchResult_Empty()
        {
            //Arrange
            var stockList = new List<Stock>();

            MockRep.Setup(k => k.ReturnSearchResults("ABCDEFGH")).ReturnsAsync(stockList);

            //Act
            var result = await _stockController.SearchResults("ABCDEFGH") as OkObjectResult;

            //Assert
            Assert.Equal(stockList, result.Value);
        }

        [Fact]
        public async Task GetAllTransactionsLoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var transactions = new List<Transaction>();
            transactions.Add(new Transaction
            {
                Id = 1,
                Amount = 3,
                Awaiting = false,
                Date = "2022-05-20",
                Symbol = "APLL",
                TotalPrice = 456,
                SocialSecurityNumber = socialSecurityNumber
            });
            transactions.Add(new Transaction
            {
                Id = 2,
                Amount = 6,
                Awaiting = false,
                Date = "2022-10-01",
                Symbol = "TESL",
                TotalPrice = 154,
                SocialSecurityNumber = socialSecurityNumber
            });
            transactions.Add(new Transaction
            {
                Id = 3,
                Amount = 5,
                Awaiting = false,
                Date = "2022-09-14",
                Symbol = "ALD",
                TotalPrice = 785,
                SocialSecurityNumber = socialSecurityNumber
            });
            MockRep.Setup(k => k.GetAllTransactions(socialSecurityNumber)).ReturnsAsync(transactions);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
                
            //Act
            var results = await _stockController.GetAllTransactions() as OkObjectResult;

            //Assert
            Assert.Equal<List<Transaction>>(transactions, (List<Transaction>)results.Value);
        }
        
        [Fact]
        public async Task GetAllTransactionsNotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var results = await _stockController.GetAllTransactions() as UnauthorizedResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.Unauthorized, results.StatusCode);
        }

        [Fact]
        public async Task GetAllTransactions_Empty()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            MockRep.Setup(k => k.GetAllTransactions(socialSecurityNumber)).ReturnsAsync(new List<Transaction>());

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var results = await _stockController.GetAllTransactions() as OkObjectResult;

            //Assert
            Assert.Equal(new List<Transaction>(),results.Value);
        }
        
        [Fact]
        public async Task GetAllTransactions_CustomerNotFound()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            MockRep.Setup(k => k.GetAllTransactions(socialSecurityNumber)).ReturnsAsync((() => null));

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var results = await _stockController.GetAllTransactions() as BadRequestObjectResult;

            //Assert
            Assert.Equal("Customer not found",results.Value);
        }
        
        [Fact]
        public async Task GetSpecificTransactions_Ok()
        {
            //Arrange
            var symbol = "AAPL";
            var socialSecurityNumber = "12345678910";

            var myList = new List<Transaction>
            {
                new()
                {
                    Id = 1,
                    Amount = 3,
                    Awaiting = false,
                    Date = "2022-05-20",
                    Symbol = "AAPL",
                    TotalPrice = 456,
                    SocialSecurityNumber = socialSecurityNumber
                },
                new()
                {
                    Id = 2,
                    Amount = 6,
                    Awaiting = false,
                    Date = "2022-10-01",
                    Symbol = "AAPL",
                    TotalPrice = 154,
                    SocialSecurityNumber = socialSecurityNumber
                },
                new()
                {
                    Id = 3,
                    Amount = 5,
                    Awaiting = false,
                    Date = "2022-09-14",
                    Symbol = "AAPl",
                    TotalPrice = 785,
                    SocialSecurityNumber = socialSecurityNumber
                }
            };

            MockRep.Setup(k => k.GetSpecificTransactions(symbol)).ReturnsAsync(myList);

            //Act
            var result = await _stockController.GetSpecificTransactions(symbol) as OkObjectResult;

            //Assert
            Assert.Equal(myList, result.Value);
        }

        [Fact]
        public async Task GetSpecificTransactions_WrongInput()
        {
            //Arrange
            var symbol = "A";

            //Act
            var result = await _stockController.GetSpecificTransactions(symbol) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task GetSpecificTransactions_Empty()
        {
            //Arrange
            var symbol = "AAPL";

            MockRep.Setup(k => k.GetSpecificTransactions(symbol)).ReturnsAsync(new List<Transaction>());

            //Act
            var result = await _stockController.GetSpecificTransactions(symbol) as OkObjectResult;

            //Assert
            Assert.Null(result.Value);
        }


        [Fact]
        public async Task GetTransactionLoggedIn_Ok()
        {
            //Arrange
            var transaction = new Transaction
            {
                Id = 1,
                SocialSecurityNumber = "12345678910",
                Date = "2022/09/10",
                Symbol = "AAPl",
                Amount = 200,
                TotalPrice = 3500
            };

            MockRep.Setup(k => k.GetTransaction(transaction.SocialSecurityNumber, transaction.Id)).ReturnsAsync(transaction);
            
            mockSession[_loggedIn] = transaction.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.GetTransaction(transaction.Id) as OkObjectResult;

            //Assert
            Assert.Equal(transaction, result.Value);
        }
        
        [Fact]
        public async Task GetTransactionNotLoggedIn()
        {
            //Arrange

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.GetTransaction(It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }


        [Fact]
        public async Task GetTransactionLoggedIn_Empty()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var id = 10;

            MockRep.Setup(k => k.GetTransaction(socialSecurityNumber, id)).ReturnsAsync(() => null);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await _stockController.GetTransaction(id) as BadRequestObjectResult;

            // Assert
            Assert.Equal("Transaction does not exist", result.Value);
        }

        [Fact]
        public async Task StockChange_Ok()
        {
            //Arrange
            var symbol = "AAPL";

            var myStockChangeValue = new StockChangeValue()
            {
                StockId = 1,
                Date = "2022-02-02",
                Symbol = "AAPL",
                Change = 1.01,
                Value = 200
            };

            MockRep.Setup(k => k.StockChange(symbol)).ReturnsAsync(myStockChangeValue);

            //Act
            var result = await _stockController.StockChange(symbol) as OkObjectResult;

            //Assert
            Assert.Equal(myStockChangeValue, result.Value);
        }

        [Fact]
        public async Task StockChange_WrongInput()
        {
            //Arrange
            var symbol = "A";

            //Act
            var result = await _stockController.StockChange(symbol) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task StockChange_Empty()
        {
            //Arrange
            var symbol = "AAPL";

            MockRep.Setup(k => k.StockChange(symbol)).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.StockChange(symbol) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Stockchange not found", result.Value);
        }


        [Fact]
        public async Task GetStockOverview_Ok()
        {
            //Arrange
            var myStockOverview = new StockOverview
            {
                Symbol = "AAPL",
                Name = "Apple INC.",
                Change = 1.01,
                Value = 1
            };
            var myStockOverview2 = new StockOverview
            {
                Symbol = "GOOGL",
                Name = "ALPHABET",
                Change = 3.2,
                Value = 90
            };
            var myStockOverview3 = new StockOverview
            {
                Symbol = "A",
                Name = "TSLA MOTOR COMPANY",
                Change = -2,
                Value = 100
            };
            var myList = new List<StockOverview>
            {
                myStockOverview,
                myStockOverview2,
                myStockOverview3
            };

            MockRep.Setup(k => k.GetStockOverview()).ReturnsAsync(myList);

            //Act
            var result = await _stockController.GetStockOverview() as OkObjectResult;

            //Assert
            Assert.Equal(myList, result.Value);
        }

        [Fact]
        public async Task GetStockOverview_Empty()
        {
            //Arrange
            MockRep.Setup(k => k.GetStockOverview()).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.GetStockOverview() as BadRequestObjectResult;

            //Assert
            Assert.Equal("Stock overview not found", result.Value);
        }

        [Fact]
        public async Task GetWinners_Ok()
        {
            //Arrange
            List<StockChangeValue> myList = new List<StockChangeValue>();

            var myStockChangeValue = new StockChangeValue()
            {
                StockId = 1,
                Date = "2022-02-02",
                Symbol = "AAPL",
                Change = 1.01,
                Value = 200
            };
            var myStockChangeValue2 = new StockChangeValue()
            {
                StockId = 2,
                Date = "2022-05-02",
                Symbol = "TSLA",
                Change = 200.9,
                Value = 1.2
            };
            var myStockChangeValue3 = new StockChangeValue()
            {
                StockId = 3,
                Date = "2022-02-09",
                Symbol = "GOOGL",
                Change = 2.2,
                Value = 1000.1
            };
            myList.Add(myStockChangeValue);
            myList.Add(myStockChangeValue2);
            myList.Add(myStockChangeValue3);

            MockRep.Setup(k => k.GetWinners()).ReturnsAsync(myList);

            //Act
            var result = await _stockController.GetWinners() as OkObjectResult;

            //Assert
            Assert.Equal(myList, result.Value);
        }

        [Fact]
        public async Task GetWinners_Empty()
        {
            //Arrange
            MockRep.Setup(k => k.GetWinners()).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.GetWinners() as BadRequestObjectResult;

            //Assert
            Assert.Equal("Winners not found", result.Value);
        }

        [Fact]
        public async Task GetLosers_Ok()
        {
            //Arrange
            var myStockChangeValue = new StockChangeValue()
            {
                StockId = 1,
                Date = "2022-02-02",
                Symbol = "AAPL",
                Change = -1.01,
                Value = 200
            };
            var myStockChangeValue2 = new StockChangeValue()
            {
                StockId = 2,
                Date = "2022-05-02",
                Symbol = "TSLA",
                Change = -200.9,
                Value = 1.2
            };
            var myStockChangeValue3 = new StockChangeValue()
            {
                StockId = 3,
                Date = "2022-02-09",
                Symbol = "GOOGL",
                Change = -2.2,
                Value = 1000.1
            };
            List<StockChangeValue> myList = new List<StockChangeValue>
            {
                myStockChangeValue,
                myStockChangeValue2,
                myStockChangeValue3
            };

            MockRep.Setup(k => k.GetLosers()).ReturnsAsync(myList);

            //Act
            var result = await _stockController.GetLosers() as OkObjectResult;

            //Assert
            Assert.Equal(myList, result.Value);
        }

        [Fact]
        public async Task GetLosers_Empty()
        {
            //Arrange
            MockRep.Setup(k => k.GetLosers()).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.GetLosers() as BadRequestObjectResult;

            //Assert
            Assert.Equal("Losers not found", result.Value);
        }

        [Fact]
        public async Task GetCustomerPortfolioLoggedIn_Ok()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";

            List<Transaction> myTransactions = new List<Transaction>();

            myTransactions.Add(new Transaction
            {
                Id = 1,
                Amount = 3,
                Awaiting = false,
                Date = "2022-05-20",
                Symbol = "APLL",
                TotalPrice = 456,
                SocialSecurityNumber = socialSecurityNumber
            });
            myTransactions.Add(new Transaction
            {
                Id = 2,
                Amount = 6,
                Awaiting = false,
                Date = "2022-10-01",
                Symbol = "TESL",
                TotalPrice = 154,
                SocialSecurityNumber = socialSecurityNumber
            });
            myTransactions.Add(new Transaction
            {
                Id = 3,
                Amount = 5,
                Awaiting = false,
                Date = "2022-09-14",
                Symbol = "ALD",
                TotalPrice = 785,
                SocialSecurityNumber = socialSecurityNumber
            });

            Portfolio myPortfolio = new Portfolio();

            myPortfolio.PortfolioId = 1;
            myPortfolio.SocialSecurityNumber = "12345678910";
            myPortfolio.Value = 1000;

            List<PortfolioList> myPortfolioList = new List<PortfolioList>
            {
                new PortfolioList
                {
                    PortfolioListId = 1,
                    Symbol = "AAPL",
                    Name = "APPLE NAME",
                    Amount = 1000,
                    Change = 100.1,
                    Value = 20000.1,
                    PortfolioId = 1
                }
            };

            var myCustomer = new Customer()
            {
                SocialSecurityNumber = socialSecurityNumber,
                FirstName = "Stevie",
                LastName = "Wonder",
                Address = "Bygaten 1",
                Balance = 100000.20,
                Transactions = myTransactions,
                PostalCode = "2008",
                PostCity = "Fjerdingby",
                Portfolio = myPortfolio
            };

            MockRep.Setup(k => k.GetCustomerPortfolio(socialSecurityNumber)).ReturnsAsync(myCustomer);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _stockController.GetCustomerPortfolio() as OkObjectResult;

            //Assert
            Assert.Equal(myCustomer, result.Value);
        }
        [Fact]
        public async Task GetCustomerPortfolioNotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            //Act
            var result = await _stockController.GetCustomerPortfolio() as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task GetCustomerPortfolioLoggedIn_Empty()
        {
            var socialSecurityNumber = "12345678910";

            MockRep.Setup(k => k.GetCustomerPortfolio(socialSecurityNumber)).ReturnsAsync(() => null);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            //Act
            var result = await _stockController.GetCustomerPortfolio() as BadRequestObjectResult;

            //Assert
            Assert.Equal("Customer not found", result.Value);
        }

        [Fact]
        public async Task GetNews_Ok()
        {
            //Arrange
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

            var myNewsList = new News();

            var myResultsList = new List<NewsResults>();
            myNewsList.Results = myResultsList;

            myResultsList.Add(myNews);

            MockRep.Setup(k => k.GetNews("AAPL")).ReturnsAsync(myNewsList);
            var stockController = new StockController(MockRep.Object, MockLog.Object);

            //Act
            var result = await stockController.GetNews("AAPL") as OkObjectResult;

            //Assert
            Assert.Equal(myNewsList, result.Value);
        }

        [Fact]
        public async Task GetNews_WrongInput()
        {
            //Arrange
            var stockController = new StockController(MockRep.Object, MockLog.Object);

            //Act
            var result = await stockController.GetNews("A") as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in input", result.Value);
        }

        [Fact]
        public async Task GetNews_empty()
        {
            //Arrange
            var symbol = "AAPL";

            MockRep.Setup(k => k.GetNews(symbol)).ReturnsAsync(new News());

            //Act
            var res = await _stockController.GetNews(symbol) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Could not find any news", res.Value);
        }

        [Fact]
        public async Task GetStockName_Ok()
        {
            // Arrange
            var symbol = "AAPL";

            var name = "Apple inc. Common stock";

            MockRep.Setup(k => k.GetStockName(symbol)).ReturnsAsync(name);
            
            //Act
            var result = await _stockController.GetStockName(symbol) as OkObjectResult;

            //Arrange
            Assert.Equal(name, result.Value);
        }

        [Fact]
        public async Task GetStockName_Empty()
        {
            //Arrange
            var symbol = "AAPL";

            var name = "";

            MockRep.Setup(k => k.GetStockName(symbol)).ReturnsAsync(name);
            
            //Act
            var res = await _stockController.GetStockName(symbol) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Could not find a stock for the symbol", res.Value);
        }
        
        [Fact]
        public async Task GetStockNameWrongInput()
        {
            //Arrange
            var symbol = "A";
            
            //Act            
            var res = await _stockController.GetStockName(symbol) as BadRequestObjectResult;

            Assert.Equal("Fault in input", res.Value);
        }

        [Fact]
        public async Task UpdateTransactionLoggedIn_OK()
        {
            //Arrange
            var transaction = new Transaction
            {
                SocialSecurityNumber = "12345678910",
                Date = "2022-10-01",
                Symbol = "AAPL",
                Amount = 350,
                TotalPrice = 1800,
                Awaiting = true
            };

            MockRep.Setup(k => k.UpdateTransaction(transaction)).ReturnsAsync(true);

            mockSession[_loggedIn] = transaction;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act            
            var result = await _stockController.UpdateTransaction(transaction) as OkObjectResult;

            //Assert
            Assert.Equal("Transaction updated", result.Value);
        }
        
        [Fact]
        public async Task UpdateTransactionNotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            
            // Act
            var result = await _stockController.UpdateTransaction(It.IsAny<Transaction>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task UpdateTransaction_Empty()
        {
            // Arrange
            var transaction = new Transaction
            {
                SocialSecurityNumber = "12345678910",
                Date = "2022-10-01",
                Symbol = "AAPL",
                Amount = 350,
                TotalPrice = 1800,
                Awaiting = true
            };

            MockRep.Setup(k => k.UpdateTransaction(transaction)).ReturnsAsync(false);

            mockSession[_loggedIn] = transaction;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            // Act
            var result = await _stockController.UpdateTransaction(transaction) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Transaction not updated", result.Value);
        }


        [Fact]
        public async Task DeleteTransactionLoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var id = 10;

            MockRep.Setup(k => k.DeleteTransaction(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.DeleteTransaction(id) as OkObjectResult;

            //Assert
            Assert.Equal("Transaction deleted", result.Value);
        }
        [Fact]
        public async Task DeleteTransactionNotLoggedIn()
        {
            //Arrange

            MockRep.Setup(k => k.DeleteTransaction(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(true);

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.DeleteTransaction(It.IsAny<int>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task DeleteTransaction_Empty()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";
            var id = 10;

            MockRep.Setup(k => k.DeleteTransaction(socialSecurityNumber, id)).ReturnsAsync(false);

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.DeleteTransaction(id) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Transaction not deleted", result.Value);
        }

        [Fact]
        public async Task RegisterCustomer_Ok()
        {
            var user = new User
            {
                Password = "hemmelig"
            };
            
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Jorgen",
                LastName = "Berntsen",
                Address = "Osloveien 47a",
                Balance = 0,
                PostalCode = "1234",
                PostCity = "Oslo",
                User = user
            };

            MockRep.Setup(k => k.RegisterCustomer(customer)).ReturnsAsync(true);
            var results = await _stockController.RegisterCustomer(customer) as OkObjectResult;
            Assert.Equal("Customer registered", results.Value);
        }

        [Fact]
        public async Task RegisterCustomer_WrongInput()
        {
            //Arrange
            _stockController.ModelState.AddModelError("SocialSecurityNumber","Fault in input");

            //Act
            var results = await _stockController.RegisterCustomer(It.IsAny<Customer>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in input", results.Value);
        }

        [Fact]
        public async Task RegisterCustomer_Empty()
        {
            //Arrange
            var user = new User
            {
                Password = "hemmelig"
            };
            
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Jorgen",
                LastName = "Berntsen",
                Address = "Osloveien 47a",
                Balance = 0,
                PostalCode = "1234",
                PostCity = "Oslo",
                User = user
            };

            MockRep.Setup(k => k.RegisterCustomer(customer)).ReturnsAsync(false);
            
            //Act
            var results = await _stockController.RegisterCustomer(customer) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Fault in registerCustomer", results.Value);
        }

        [Fact]
        public async Task UpdateCustomer_LoggedIn()
        {
            //Arrange
            var user = new User
            {
                Password = "hemmelig"
            };
            
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Parkveien 3",
                PostalCode = "0245",
                PostCity = "Oslo",
                User = user
            };
            
            mockSession[_loggedIn] = customer.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.UpdateCustomer(customer)).ReturnsAsync(true);
            //Act
            var result = await _stockController.UpdateCustomer(customer) as OkObjectResult;

            //Assert
            Assert.Equal("Customer updated", result.Value);
        }
        
        [Fact]
        public async Task UpdateCustomer_LoggedInWrongInput()
        {
            //Arrange
            var customer = new Customer
            {
                SocialSecurityNumber = "1234567891",
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Parkveien 3",
                PostalCode = "0245",
                PostCity = "Oslo"
            };
            
            mockSession[_loggedIn] = customer.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            
            _stockController.ModelState.AddModelError("SocialSecurityNumber", "Fault in input");
            
            
            //Act
            var result = await _stockController.UpdateCustomer(customer) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Fault in input", result.Value);
        }
        
        [Fact]
        public async Task UpdateCustomer_LoggedInFault()
        {
            //Arrange
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Ola",
                LastName = "Nordmann",
                Address = "Parkveien 3",
                PostalCode = "0245",
                PostCity = "Oslo"
            };
            
            mockSession[_loggedIn] = customer.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.UpdateCustomer(It.IsAny<Customer>())).ReturnsAsync(false);
            
            //Act
            var result = await _stockController.UpdateCustomer(customer) as BadRequestObjectResult;

            //Assert
            Assert.Equal("Customer not updated", result.Value);
        }
        
        [Fact]
        public async Task UpdateCustomer_NotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = "";
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.UpdateCustomer(It.IsAny<Customer>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);

        }


        [Fact]
        public async Task logIn_Ok()
        {
            //Arrange
            var line = new User
            {
                Username = "12345678910",
                Password = "Password1"
            };

            MockRep.Setup(k => k.LogIn(It.IsAny<User>())).ReturnsAsync(true);

            mockSession[_loggedIn] = line.Username;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var res = await _stockController
                .LogIn(line) as OkObjectResult; // We are not using It.Any<User> since we use the username as session key.

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, res.StatusCode);
            Assert.Equal("Ok", res.Value);
        }

        [Fact]
        public async Task logIn_WrongUsernamePassword()
        {
            //Arrange
            var line = new User
            {
                Username = "12345678910",
                Password = "Password2"
            };

            MockRep.Setup(k => k.LogIn(It.IsAny<User>())).ReturnsAsync(false);

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.LogIn(line) as BadRequestObjectResult; // We are not using It.Any<User> since we use the username as session key.

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Failed", result.Value);
        }

        [Fact]
        public async Task logIn_WrongInput()
        {
            //Arrange

            MockRep.Setup(k => k.LogIn(It.IsAny<User>())).ReturnsAsync(true);

            _stockController.ModelState.AddModelError("Username", "Fault in input");
            
            //Act
            var result = await _stockController.LogIn(It.IsAny<User>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int)HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }
        
        [Fact]
        public void LogOut()
        {
            //Arrange
            var username = "12345678910";
            
            // Sets a session
            mockSession[_loggedIn] = username;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = _stockController.LogOut() as OkObjectResult;
            
            //Assert
            Assert.Equal((int)HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("", mockSession[_loggedIn]);
        }

        [Fact]
        public void IsLoggedIn_LoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = "12345678910";
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = _stockController.IsLoggedIn() as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.True((bool)result.Value);
        }

        [Fact]
        public void IsLoggedIn_NotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = _stockController.IsLoggedIn() as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.False((bool) result.Value);
        }

        [Fact]
        public async Task Deposit_LoggedIn()
        {
            //Arrange
            double amount = 10000;
            string socialSecurityNumber = "12345678910";
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.Deposit(socialSecurityNumber, amount)).ReturnsAsync(true);
            
            //Act
            var result = await _stockController.Deposit(amount) as OkObjectResult;
            
            //Assert
            Assert.Equal("Amount deposited", result.Value);

        }
        
        [Fact]
        public async Task Deposit_Fault_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = 10000;
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.Deposit(It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(false);
            
            //Act
            var result = await _stockController.Deposit(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Amount not deposited", result.Value);

        }
        
        [Fact]
        public async Task Deposit_NegativeAmount_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = -10000;
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Deposit(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Cannot deposit negative or 0", result.Value);

        }
        
        [Fact]
        public async Task Deposit_Zero_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = 0;
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Deposit(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Cannot deposit negative or 0", result.Value);

        }
        
        [Fact]
        public async Task Deposit_NotLoggedIn()
        {
            //Arrange


            mockSession[_loggedIn] = "";
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Deposit(It.IsAny<double>()) as UnauthorizedResult;
            
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);

        }
        
        [Fact]
        public async Task Withdraw_LoggedIn()
        {
            //Arrange
            double amount = 10000;
            string socialSecurityNumber = "12345678910";
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.Withdraw(socialSecurityNumber, amount)).ReturnsAsync(true);
            
            //Act
            var result = await _stockController.Withdraw(amount) as OkObjectResult;
            
            //Assert
            Assert.Equal("Amount withdrawn", result.Value);

        }
        
        [Fact]
        public async Task Withdraw_Fault_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = 10000;
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.Withdraw(It.IsAny<string>(), It.IsAny<double>())).ReturnsAsync(false);
            
            //Act
            var result = await _stockController.Withdraw(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Amount not withdrawn", result.Value);

        }
        
        [Fact]
        public async Task Withdraw_NegativeAmount_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = -10000;
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Withdraw(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Cannot withdraw negative or 0", result.Value);

        }
        
        [Fact]
        public async Task Withdraw_Zero_LoggedIn()
        {
            //Arrange
            string socialSecurityNumber = "12345678910";
            double amount = 0;
            
            
            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Withdraw(amount) as BadRequestObjectResult;
            
            //Assert
            Assert.Equal("Cannot withdraw negative or 0", result.Value);

        }
        
        [Fact]
        public async Task Withdraw_NotLoggedIn()
        {
            //Arrange


            mockSession[_loggedIn] = "";
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.Withdraw(It.IsAny<double>()) as UnauthorizedResult;
            
            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);

        }

        [Fact]
        public async Task GetCustomerData_LoggedIn()
        {
            //Arrange
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Line",
                LastName = "Jensen",
                Address = "Karl Johansgate 3",
                Balance = 1000,
                PostalCode = "0245",
                PostCity = "Oslo"
            };

            mockSession[_loggedIn] = customer.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.GetCustomerData(It.IsAny<string>())).ReturnsAsync(customer);

            //Act
            var result = await _stockController.GetCustomerData() as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal(customer,result.Value);
        }
        
        [Fact]
        public async Task GetCustomerData_LoggedInNotFound()
        {
            //Arrange
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Line",
                LastName = "Jensen",
                Address = "Karl Johansgate 3",
                Balance = 1000,
                PostalCode = "0245",
                PostCity = "Oslo"
            };

            mockSession[_loggedIn] = customer.SocialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.GetCustomerData(It.IsAny<string>())).ReturnsAsync(() => null);

            //Act
            var result = await _stockController.GetCustomerData() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Failed",result.Value);
        }
        
        [Fact]
        public async Task GetCustomerData_NotLoggedIn()
        {
            //Arrange
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Line",
                LastName = "Jensen",
                Address = "Karl Johansgate 3",
                Balance = 1000,
                PostalCode = "0245",
                PostCity = "Oslo"
            };

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.GetCustomerData() as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task DeleteCustomer_LoggedIn()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.DeleteCustomer(socialSecurityNumber)).ReturnsAsync(true);

            //Act
            var result = await _stockController.DeleteCustomer() as OkObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
            Assert.Equal("Customer deleted", result.Value);
        }
        
        [Fact]
        public async Task DeleteCustomer_LoggedInFault()
        {
            //Arrange
            var socialSecurityNumber = "12345678910";

            mockSession[_loggedIn] = socialSecurityNumber;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.DeleteCustomer(socialSecurityNumber)).ReturnsAsync(false);

            //Act
            var result = await _stockController.DeleteCustomer() as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Balance and portfolio must be empty", result.Value);
        }
        [Fact]
        public async Task DeleteCustomer_NotLoggedIn()
        {
            //Arrange
            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;
            

            //Act
            var result = await _stockController.DeleteCustomer() as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

        [Fact]
        public async Task ChangePassword_LoggedIn()
        {
            //Arrange
            var user = new User
                {
                    Username = "12345678910",
                    Password = "Password2"
                };
                
                
                mockSession[_loggedIn] = user.Username;
                mockHttpContext.Setup(k => k.Session).Returns(mockSession);
                _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

                MockRep.Setup(k => k.ChangePassword(user)).ReturnsAsync(true);
                
                //Act
                var result = await _stockController.ChangePassword(user) as OkObjectResult;

                //Assert
                Assert.Equal((int) HttpStatusCode.OK, result.StatusCode);
                Assert.Equal("Password updated", result.Value);
        }
        [Fact]
        public async Task ChangePassword_LoggedInFault()
        {
            //Arrange
            var user = new User
            {
                Username = "12345678910",
                Password = "Password2"
            };
                
                
            mockSession[_loggedIn] = user.Username;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.ChangePassword(user)).ReturnsAsync(false);
                
            //Act
            var result = await _stockController.ChangePassword(user) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Password not updated", result.Value);
        }
        [Fact]
        public async Task ChangePassword_LoggedWrongInput()
        {
            //Arrange
            var user = new User
            {
                Username = "12345678910",
                Password = "Password2"
            };
                
                
            mockSession[_loggedIn] = user.Username;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            MockRep.Setup(k => k.ChangePassword(user)).ReturnsAsync(false);
            _stockController.ModelState.AddModelError("Username", "Fault in input");
            //Act
            var result = await _stockController.ChangePassword(It.IsAny<User>()) as BadRequestObjectResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.BadRequest, result.StatusCode);
            Assert.Equal("Fault in input", result.Value);
        }
        
        [Fact]
        public async Task ChangePassword_NotLoggedIn()
        {
            //Arrange

            mockSession[_loggedIn] = _notLoggedIn;
            mockHttpContext.Setup(k => k.Session).Returns(mockSession);
            _stockController.ControllerContext.HttpContext = mockHttpContext.Object;

            //Act
            var result = await _stockController.ChangePassword(It.IsAny<User>()) as UnauthorizedResult;

            //Assert
            Assert.Equal((int) HttpStatusCode.Unauthorized, result.StatusCode);
        }

    }
}