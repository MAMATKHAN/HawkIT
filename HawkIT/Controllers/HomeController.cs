using HawkIT.Models;
using HawkIT.Services;
using HawkIT.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Xml.Linq;

namespace HawkIT.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HawkitDbContext db;
        private readonly SmtpHandling _smtp;

        public HomeController(ILogger<HomeController> logger, HawkitDbContext context)
        {
            _logger = logger;
            db = context;
            _smtp = new SmtpHandling();
        }

        public IActionResult Index(int? id)
        {
            var projects = db.Projects.Include(p => p.Tags).OrderByDescending(p => p.CreatedDate).ToList();
            var tags = db.Tags.Include(t => t.Projects)
                .Where(t => t.Projects.Count != 0)
                .ToList();
            var workers = db.Workers.ToList();
            var articles = db.Articles.OrderBy(a => a.CreatedDate).Take(3).ToList();

            var tag = db.Tags.Find(id);
            if (tag != null)
            {
                projects = projects
                    .Where(p => (p.Tags != null)? p.Tags.Contains(tag) : false)
                    .ToList();
            }

            var mainViewModel = new MainViewModel { Articles = articles, Projects = projects, Tags = tags, Workers = workers, ActiveTagId = id };
            return View(mainViewModel);
        }


        public IActionResult ProjectDetails(int id)
        {
            var project = db.Projects.Include(p => p.Workers).ToList().Find(p => p.Id == id);
            var lastThreeAddedProjects = db.Projects
                .Where(p => p.Id !=  id)
                .Include(p => p.Tags)
                .OrderByDescending(p => p.CreatedDate)
                .Take(3)
                .ToList();

            if(project == null) return View("~/Views/Shared/NotFound.cshtml");
            var model = new ProjectDetailsViewModel { Project = project, MoreProjects = lastThreeAddedProjects };
            return View(model);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult PartialProjectsList(int id)
        {
            var projects = db.Projects.Include(p => p.Tags).OrderByDescending(p => p.CreatedDate).ToList();

            var tag = db.Tags.Find(id);
            if (tag != null) projects = projects
                                        .Where(p => (p.Tags != null)? p.Tags.Contains(tag) : false)
                                        .ToList();


            return PartialView("_PartialProjectsList", projects);
        }

        public string TrySendBid(string name, string email, string phone, string telegram, string message)
        {
            string status = "ok";
            var sender = new SenderInfo();
            sender.Name = name;
            sender.Email = email;
            sender.Phone = phone;
            sender.Telegram = telegram;
            sender.Message = message;

            try
            {
                _smtp.SendMessage(sender);
            }
            catch
            {
                status = "error";
            }

            return status;
        }
    }
}