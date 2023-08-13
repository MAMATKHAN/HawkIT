using HawkIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace HawkIT.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(LoginModel model)
        {

            return RedirectToAction("Index", "Admin");
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
