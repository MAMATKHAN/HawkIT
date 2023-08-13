using Microsoft.AspNetCore.Mvc;

namespace HawkIT.Controllers
{
    public class AuthenticationController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }
    }
}
