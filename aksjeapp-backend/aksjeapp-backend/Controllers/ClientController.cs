using aksjeapp_backend.DAL;
using Microsoft.AspNetCore.Mvc;
namespace aksjeapp_backend.Controller
{
    [Route("[controller]/[action]")]
    public class ClientController : ControllerBase {
    
        private readonly IClientRepository _db;
        private readonly ILogger<ClientController> _logger;
        
        public ClientController(IClientRepository db, ILogger<ClientController> logger)
        {
            _db = db;
            _logger = logger;
        }
    }
}