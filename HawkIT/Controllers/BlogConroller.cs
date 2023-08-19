using Microsoft.AspNetCore.Mvc;

namespace HawkIT.Controllers
{
    public class BlogConroller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
