using aksjeapp_backend.Controller;
using aksjeapp_backend.DAL;
using aksjeapp_backend.Models;
using Microsoft.Extensions.Logging;
using Moq;

namespace UnitTesting_aksjeapp
{
    public class UnitTest_ClientController
    {

        private static readonly Mock<IClientRepository> mockRep = new();
        private static readonly Mock<ILogger<ClientController>> mockLog = new();
        private readonly ClientController _clientController = new(mockRep.Object, mockLog.Object);
    }
}