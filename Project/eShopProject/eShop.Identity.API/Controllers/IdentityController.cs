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

        [HttpGet("login")]
        public IActionResult GetLogin()
        {
            return Redirect("https://localhost:7224/Identity/Account/Login");
        }

        [HttpGet("register")]
        public IActionResult GetRegister()
        {
            return Redirect("https://localhost:7224/Identity/Account/Register");
        }

        [HttpGet("logout")]
        public IActionResult GetLogout()
        {
            return Redirect("https://localhost:7224/Identity/Account/Logout");
        }

        [HttpGet("manage")]
        public IActionResult GetManage()
        {
            return Redirect("https://localhost:7224/Identity/Account/Manage");
        }
    }
}
