using Microsoft.AspNetCore.Mvc;

namespace test_backend.Controllers
{

    [Route("[controller]/[action]")]
    public class ExampleController : Controller
    {
        [HttpGet]
        public JsonResult Example()
        {
            return Json("Default action...");
        }
    }
}