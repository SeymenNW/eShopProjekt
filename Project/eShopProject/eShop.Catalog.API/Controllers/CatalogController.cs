using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{
    [ApiController]
    [Route("/api/catalog")]
    public class CatalogController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Adil skal købe mad");
        }
    }
}
