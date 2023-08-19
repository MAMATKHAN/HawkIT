using HawkIT.Models;
using Microsoft.AspNetCore.Mvc;

namespace HawkIT.Controllers
{
    public class BlogConroller : Controller
    {
        private readonly HawkitDbContext db;
        public BlogConroller(HawkitDbContext context)
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
