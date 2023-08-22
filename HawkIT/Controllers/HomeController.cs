using HawkIT.Models;
using HawkIT.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace HawkIT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HawkitDbContext db;

        public HomeController(ILogger<HomeController> logger, HawkitDbContext context)
        {
            _logger = logger;
            db = context;
        }

        public IActionResult Index(int? id)
        {
            var projects = db.Projects.Include(p => p.Tags).ToList();
            var tags = db.Tags.Include(t => t.Projects).Where(t => t.Projects.Count != 0).ToList();
            var workers = db.Workers.ToList();
            var articles = db.Articles.OrderBy(a => a.CreatedDate).Take(3).ToList();

            var tag = db.Tags.Find(id);
            if (tag != null)
            {
                projects = projects.Where(p => p.Tags.Contains(tag)).ToList();
            }

            var mainViewModel = new MainViewModel { Articles = articles, Projects = projects, Tags = tags, Workers = workers, ActiveTagId = id };
            return View(mainViewModel);
        }

        public IActionResult ProjectDetails(int? id)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}