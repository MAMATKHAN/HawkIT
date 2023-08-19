using HawkIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace HawkIT.Controllers
{
    public class BlogController : Controller
    {
        private readonly HawkitDbContext db;
        public BlogController(HawkitDbContext context)
        {
            db = context;
        }

        public IActionResult ListItems()
        {
            var articles = db.Articles.ToList();
            return View();
        }
    }
}
