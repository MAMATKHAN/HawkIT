using HawkIT.Models;
using HawkIT.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var articles = db.Articles.Include(a => a.Tags).ToList();
            return View(articles);
        }

        public IActionResult Details(int? id)
        {
            var article = db.Articles.Find(id);
            var lastThreeArticle = db.Articles.Where(a => a.Id != id).OrderByDescending(a => a.CreatedDate).Take(3).ToList();

            if(article == null) { return View("~/Views/Shared/NotFound.cshtml"); }

            var articleDetailsViewModel = new ArticleDetailsViewModel() { Aritcle = article, MoreArticles = lastThreeArticle };
            return View(articleDetailsViewModel);
        }
    }
}
