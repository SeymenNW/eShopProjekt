using Microsoft.AspNetCore.Mvc;

namespace eShop.Identity.API.Controllers
{

    [ApiController]
    [Route("/api/identity")]
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Adil skal rejse hjem");
        }
    }
}
