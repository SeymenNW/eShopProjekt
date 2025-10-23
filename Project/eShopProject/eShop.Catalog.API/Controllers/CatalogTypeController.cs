using eShop.Catalog.API.Data;
using eShop.Catalog.API.Entitites;
using Microsoft.AspNetCore.Mvc;

namespace eShop.Catalog.API.Controllers
{

    [ApiController]
    [Route("api/catalog-types")]
    public class CatalogTypeController : Controller
    {

        private readonly CatalogDbContext _db;

        public CatalogTypeController(CatalogDbContext db)
        {
            _db = db;
        }

        [HttpGet()]
        public async Task<IActionResult> GetTypes()
        {
            var types = _db.Types.ToList();
            return Ok(types);
        }

        [HttpGet("add")]
        public async Task<IActionResult> AddTypes(string typeName)
        {
            try
            {
                _db.Types.Add(new Entitites.Type { TypeName = typeName });
                _db.SaveChanges();
                return Ok($"Type with name '{typeName}' has been added");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

