using Microsoft.AspNetCore.Mvc;

namespace eShop.Identity.API.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class IdentityController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Identity API is running... Adam");
        }
    }
}
