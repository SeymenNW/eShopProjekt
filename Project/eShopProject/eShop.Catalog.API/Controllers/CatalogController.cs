using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{
    [ApiController]
    [Route("/api/[controller]")]
    public class CatalogController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Catalog API is running... Adam");
        }
    }
}
