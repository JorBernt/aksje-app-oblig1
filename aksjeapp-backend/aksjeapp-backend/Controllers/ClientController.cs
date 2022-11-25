using aksjeapp_backend.DAL;
using Microsoft.AspNetCore.Mvc;

namespace aksjeapp_backend.Controller
{
    [Route("[controller]/[action]")]
    public class ClientController : ControllerBase
    {

        private readonly IClientRepository _db;
        private readonly ILogger<ClientController> _logger;

        public ClientController(IClientRepository db, ILogger<ClientController> logger)
        {
            _db = db;
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> RegisterCustomer([FromBody] Customer customer)
        {
            Console.WriteLine(HttpContext.Session.GetString("SocialSecurityNumber"));
            var returnOk = await _db.RegisterCustomer(customer);
            if (!returnOk)
            {
                _logger.LogInformation("Fault in registerCustomer");
                return BadRequest("Fault in registerCustomer");
            }
            return Ok("Customer registered");
        }

        
    }
}