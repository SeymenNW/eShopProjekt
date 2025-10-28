using eShop.Identity.API.Areas.Identity.Pages.Account;
using eShop.Identity.API.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;

namespace eShop.Identity.API.Controllers
{

    [ApiController]
    [Route("/api/[controller]")]
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public IdentityController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

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

        //[HttpPost("login")]
        //public async Task<IActionResult> FindUserLogin([FromBody] LoginModel model)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    // Find brugeren
        //    var user = await _userManager.GetUserAsync(User);

        //    // Hent e-mail fra bruger-objektet
        //    var email = user?.Email;
        //    if (user == null)
        //    {
        //        return Unauthorized("Invalid login attempt");
        //    }

        //    var result

        //    if (result.Succeeded)
        //    {
        //        // Login succesfuldt - redirect til localhost:7000
        //        return Redirect("http://localhost:7000");
        //    }

        //    //return Ok(new { Email = email });


        //}

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
