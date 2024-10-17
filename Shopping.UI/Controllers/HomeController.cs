using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shopping.UI.Models;
using System.Diagnostics;

namespace Shopping.UI.Controllers
{
    [Route("ui/[controller]")] // Adding a route prefix for this controller to avoid route conflicts
    [Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Route for Index action: /ui/home/index
        [HttpGet("index")]
        public IActionResult Index()
        {
            return View();
        }

        // Route for Privacy action: /ui/home/privacy
        [HttpGet("privacy")]
        public IActionResult Privacy()
        {
            var token = HttpContext.GetTokenAsync("access_token");
            return View();
        }

        // The [Authorize] attribute ensures that only authenticated users can access this action
        [Authorize]
        [HttpGet("Login")]
        public   IActionResult Login(string returnUrl = "/")
        {
            
            return  Challenge(new AuthenticationProperties { RedirectUri = returnUrl }, "oidc");
        }
        //[Authorize]
        //[HttpGet("logout")]
        //public IActionResult Logout()
        //{
        //    // This will sign out from the cookie authentication and redirect to OpenID Connect sign-out
        //    return SignOut(
        //        new AuthenticationProperties { RedirectUri = "/" }, // Redirect after logout
        //        CookieAuthenticationDefaults.AuthenticationScheme,
        //        OpenIdConnectDefaults.AuthenticationScheme); // Sign out from both schemes
        //}

        [Authorize]
        [HttpGet("logout")]
        public IActionResult Logout()
        {
            // Sign out of both cookies and OpenID Connect
            return SignOut("Cookies", "oidc");
        }
        // This handles any errors and will be routed to /ui/home/error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        [HttpGet("error")]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
