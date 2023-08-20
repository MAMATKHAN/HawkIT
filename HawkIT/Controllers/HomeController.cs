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
            var tags = db.Tags.ToList();
            var workers = db.Workers.ToList();
            var articles = db.Articles.ToList();

            var t = db.Tags.Find(id);
            if (t != null)
            {
                projects = projects.Where(p => p.Tags.Contains(t)).ToList();
            }

            var mainViewModel = new MainViewModel { Articles = articles, Projects = projects, Tags = tags, Workers = workers, ActiveTagId = id };
            return View(mainViewModel);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}