using aksjeapp_backend.Controller;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTesting_aksjeapp
{
    public class UnitTest_ClientController
    {
        private static readonly Mock<IClientRepository> mockRep = new();
        private static readonly Mock<ILogger<ClientController>> mockLog = new();
        private readonly ClientController _clientController = new(mockRep.Object, mockLog.Object);

        [Fact]
        public async Task RegisterCustomer_Ok()
        {
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Jorgen",
                LastName = "Berntsen",
                Address = "Osloveien 47a",
                Balance = 0,
                Transactions = new List<Transaction>(),
                PostalCode = "1234",
                PostCity = "Oslo",
                Portfolio = new Portfolio
                {
                    PortfolioId = 0,
                    SocialSecurityNumber = "12345678910",
                    StockPortfolio = new List<PortfolioList>(),
                    Value = 0
                }
            };

            mockRep.Setup(k => k.RegisterCustomer(customer)).ReturnsAsync(true);
            var results =  await _clientController.RegisterCustomer(customer) as OkObjectResult;
            Assert.Equal("Customer registered", results.Value);
        }
        [Fact]
        public async Task RegisterCustomer_Empty()
        {
            var customer = new Customer
            {
                SocialSecurityNumber = "12345678910",
                FirstName = "Jorgen",
                LastName = "Berntsen",
                Address = "Osloveien 47a",
                Balance = 0,
                Transactions = new List<Transaction>(),
                PostalCode = "1234",
                PostCity = "Oslo",
                Portfolio = new Portfolio
                {
                    PortfolioId = 0,
                    SocialSecurityNumber = "12345678910",
                    StockPortfolio = new List<PortfolioList>(),
                    Value = 0
                }
            };

            mockRep.Setup(k => k.RegisterCustomer(customer)).ReturnsAsync(false);
            var results =  await _clientController.RegisterCustomer(customer) as BadRequestObjectResult;
            Assert.Equal("Fault in registerCustomer", results.Value);
        }
    }
}