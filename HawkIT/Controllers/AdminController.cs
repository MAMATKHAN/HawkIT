using HawkIT.Models;
using HawkIT.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting.Internal;
using System.Linq;
using System.Security.Claims;



namespace HawkIT.Controllers
{
    public class AdminController : Controller
    {
        private readonly HawkitDbContext db;
        private readonly IWebHostEnvironment _env;

        public AdminController(HawkitDbContext context, IWebHostEnvironment env)
        {
            db = context;
            _env = env;
        }

        // AUTHENTICATION
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {

            if (ModelState.IsValid)
            {
                User? user = db.Users.FirstOrDefault(u => u.Login == model.Login && u.Password == model.Password);

                if (user == null)
                {
                    ModelState.AddModelError("Name", "Логин или пароль введен неправильно");
                    return View();
                };

                var claims = new List<Claim> { new Claim(ClaimTypes.Name, user.Login) };
                ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, "Cookie");
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("ListProjects", "Admin");
            }
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Admin");
        }

        // Project CRUD
        [Authorize]
        public IActionResult ListProjects(string? name ,int? tagId, int? workerId)
        {
            var projects = db.Projects.Include(p => p.Tags).Include(p => p.Workers).ToList();
            var tags = db.Tags.ToList();
            var workers = db.Workers.ToList();

            if (name != null) projects = projects.Where(w => w.Name.ToLower().Contains(name.ToLower())).ToList();
            if (tagId != -1 && tagId != null)
            {
                var tag = db.Tags.Find(tagId);
                projects = projects.Where(p => p.Tags.Contains(tag)).ToList();
            }
            if (workerId != -1 && workerId != null)
            {
                var worker = db.Workers.Find(workerId);
                projects = projects.Where(p => p.Workers.Contains(worker)).ToList();
            }

            var adminProjectViewModel = new AdminProjectsViewModel { Projects = projects, Workers = workers, Tags = tags };
            return View(adminProjectViewModel);
        }

        [Authorize, HttpGet]
        public IActionResult AddProject()
        {
            ViewData["Workers"] = db.Workers.ToList();
            ViewData["Tags"] = db.Tags.ToList();
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult AddProject(Project project)
        {
            if (Request.Form["workers"].Count == 0)
            {
                ModelState.AddModelError("Workers", "Это поле обязательно должно быть заполнено");
            }
            if (!(ModelState.IsValid))
            {
                ViewData["Workers"] = db.Workers.ToList();
                ViewData["Tags"] = db.Tags.ToList();
                return View();
            }

            if (project.ImageFile != null)
            {
                var uniqueFileName = GetUniqueFileName(project.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "projects");
                using(var stream  = new FileStream(filePath, FileMode.Create))
                {
                    project.ImageFile.CopyTo(stream);
                }
                project.ProjectImage = "/uploads/projects/" + uniqueFileName;
            }

          

            foreach (var workerId in Request.Form["workers"])
            {
                project.Workers.Add(db.Workers.Find(int.Parse(workerId)));
            }
            foreach (var tagId in Request.Form["tags"])
            {
                project.Tags.Add(db.Tags.Find(int.Parse(tagId)));
            }
            project.CreatedDate = DateTime.Now;


            db.Projects.Add(project);
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");
        }

        [Authorize, HttpGet]
        public IActionResult EditProject(int id)
        {
            var project = db.Projects.Include(p => p.Tags).Include(p => p.Workers).ToList().Find(p => p.Id == id);
            ViewData["Tags"] = db.Tags.ToList();
            ViewData["Workers"] = db.Workers.ToList();

            return View(project);

        }

        [Authorize, HttpPost]
        public IActionResult EditProject(Project p)
        {
            var project = db.Projects.Include(proj => proj.Tags).Include(proj => proj.Workers).First(proj => proj.Id == p.Id);
            
            if (p.ImageFile != null)
            {
                var imageName = project.ProjectImage.Split("/").Last();
                DeleteUploadFile(imageName, "projects");
                var uniqueFileName = GetUniqueFileName(p.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "projects");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    p.ImageFile.CopyTo(stream);
                }
                project.ProjectImage = "/uploads/projects/" + uniqueFileName;
            }
            else ModelState.Remove("ImageFile");
            if (!(ModelState.IsValid))
            {
                ViewData["Tags"] = db.Tags.ToList();
                ViewData["Workers"] = db.Workers.ToList();

                return View(project);
            }

            foreach (var tagId in Request.Form["tags"])
            {
                project.Tags.Clear();
                project.Tags.Add(db.Tags.Find(int.Parse(tagId)));
            }
            foreach (var workerId in Request.Form["workers"])
            {
                project.Workers.Clear();
                project.Workers.Add(db.Workers.Find(int.Parse(workerId)));
            }

            project.Name = p.Name;
            project.Text = p.Text;
            db.Projects.Update(project);
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");
        }

        [Authorize]
        public IActionResult DeleteProject(int id)
        {
            var project = db.Projects.Find(id);

            if (project != null)
            {
                var imageName = project.ProjectImage.Split("/").Last();
                DeleteUploadFile(imageName, "projects");
                db.Projects.Remove(project);
            }
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");

        }

        // Worker CRUD
        [Authorize, HttpGet]
        public IActionResult ListWorkers(string? name,int? projectId, string? specialization)
        {
            var workers = db.Workers.Include(w => w.Projects).ToList();
            var projects = db.Projects.ToList();

            if(name != null) workers = workers.Where(w => w.Name.ToLower().Contains(name.ToLower())).ToList();
            if(specialization != null) workers = workers.Where(w => w.Specialization.ToLower().Contains(specialization.ToLower())).ToList();
            if(projectId != -1 && projectId != null)
            {
                var project = db.Projects.Find(projectId);
                workers = workers.Where(w => w.Projects.Contains(project)).ToList();
            }

            var adminWorkerViewModel = new AdminWorkerViewModel { Workers = workers, Projects = projects };
            return View(adminWorkerViewModel);
        }

        [Authorize, HttpGet]
        public IActionResult AddWorker()
        {
            var p = db.Projects.ToList();
            ViewData["Projects"] = p;
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult AddWorker(Worker worker)
        {
            if (!(ModelState.IsValid))
            {
                var p = db.Projects.ToList();
                ViewData["Projects"] = p;
                return View();
            }

            if (worker.ImageFile != null)
            {
                var uniqueFileName = GetUniqueFileName(worker.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "workers");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    worker.ImageFile.CopyTo(stream);
                }
                worker.WorkerImage = "/uploads/workers/" + uniqueFileName;
            }

            if (worker.IconFile != null)
            {
                var uniqueFileName = GetUniqueFileName(worker.IconFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "workers");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    worker.IconFile.CopyTo(stream);
                }
                worker.SpecializationIcon = "/uploads/workers/" + uniqueFileName;
            }

            foreach (var projectId in Request.Form["projects"])
            {
                worker.Projects.Add(db.Projects.Find(int.Parse(projectId)));
            }

            db.Workers.Add(worker);
            db.SaveChanges();
            return RedirectToAction("ListWorkers", "Admin");
        }

        [Authorize, HttpGet]
        public IActionResult EditWorker(int id)
        {
            var worker = db.Workers.Include(a => a.Projects).ToList().Find(w => w.Id == id);

            var projects = db.Projects.ToList();
            ViewData["Projects"] = projects;

            return View(worker);
        }



        [Authorize, HttpPost]
        public IActionResult EditWorker(Worker w)
        {
            var worker = db.Workers.Include(work => work.Projects).First(work => work.Id == w.Id);
            
            if (w.ImageFile != null)
            {
                var imageName = worker.WorkerImage.Split("/").Last();
                DeleteUploadFile(imageName, "workers");
                var uniqueFileName = GetUniqueFileName(w.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "workers");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    w.ImageFile.CopyTo(stream);
                }
                worker.WorkerImage = "/uploads/workers/" + uniqueFileName;
            }
            else ModelState.Remove("ImageFile");
            if (w.IconFile != null)
            {
                var imageName = worker.SpecializationIcon.Split("/").Last();
                DeleteUploadFile(imageName, "workers");
                var uniqueFileName = GetUniqueFileName(w.IconFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "workers");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    w.IconFile.CopyTo(stream);
                }
                worker.SpecializationIcon = "/uploads/workers/" + uniqueFileName;
            }
            else ModelState.Remove("IconFile");
            if (!(ModelState.IsValid))
            {
                var projects = db.Projects.ToList();
                ViewData["Projects"] = projects;

                return View(worker);
            }

            foreach (var project in Request.Form["projects"])
            {
                worker.Projects.Clear();
                worker.Projects.Add(db.Projects.Find(int.Parse(project)));
            }

            worker.Name = w.Name;
            worker.Specialization = w.Specialization;
            db.Workers.Update(worker);
            db.SaveChanges();

            return RedirectToAction("ListWorkers", "Admin");
        }

        [Authorize, HttpGet]
        public IActionResult DeleteWorker(int id)
        {
            var worker = db.Workers.Find(id);

            if (worker != null)
            {
                var imageName = worker.WorkerImage.Split("/").Last();
                var iconName = worker.SpecializationIcon.Split("/").Last();
                DeleteUploadFile(imageName, "workers");
                DeleteUploadFile(iconName, "workers");
                db.Workers.Remove(worker);
            }
            db.SaveChanges();
            return RedirectToAction("ListWorkers", "Admin");
        }

        // Tag CRUD
        [Authorize, HttpGet]
        public IActionResult ListTags(string? searchString)
        {
            List<Tag> tags;
            if (string.IsNullOrEmpty(searchString)) tags = db.Tags.ToList();
            else tags = db.Tags.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).ToList();

            return View(tags);
        }


        [Authorize, HttpGet]
        public IActionResult AddTag()
        {
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult AddTag(Tag tag)
        {
            if (String.IsNullOrEmpty(tag.Name)) return View();

            db.Tags.Add(tag);
            db.SaveChanges();
            return RedirectToAction("ListTags", "Admin");
        }

        [Authorize]
        public IActionResult DeleteTag(int id)
        {
            var tag = db.Tags.Find(id);
            if(tag != null)
            {
                db.Tags.Remove(tag);
                db.SaveChanges();
            }
            return RedirectToAction("ListTags", "Admin");
        }

        [Authorize, HttpGet]
        public IActionResult EditTag(int id)
        {

            var tag = db.Tags.Find(id);
            return View(tag);
        }

        [Authorize, HttpPost]
        public IActionResult EditTag(Tag tag)
        {
            if (!(ModelState.IsValid))
            {
                var t = db.Tags.Find(tag.Id);
                return View(t);
            }

            db.Tags.Update(tag);
            db.SaveChanges();
            return RedirectToAction("ListTags", "Admin");
        }

        // Article CRUD
        [Authorize]
        public IActionResult ListArticles(string? articleName, int? tagId)
        {
            var articles = db.Articles.Include(p => p.Tags).ToList();
            var tags = db.Tags.ToList();
            
            if (articleName != null) articles = articles.Where(a => a.Name.ToLower().Contains(articleName.ToLower())).ToList();
            if (tagId != null && tagId != -1)
            {
                var tag = db.Tags.Find(tagId);
                articles = articles.Where(a => a.Tags.Contains(tag)).ToList();
            }


            var adminArticleViewModel = new AdminArticleViewModel { Articles = articles, Tags = tags };
            return View(adminArticleViewModel);
        }

        [Authorize, HttpGet]
        public IActionResult AddArticle()
        {
            var tags = db.Tags.ToList();
            ViewData["Tags"] = tags;
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult AddArticle(Article article)
        {
            if (!(ModelState.IsValid))
            {
                var tags = db.Tags.ToList();
                ViewData["Tags"] = tags;
                return View();
            }

            if(article.ImageFile != null)
            {
                var uniqueFileName = GetUniqueFileName(article.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "articles");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    article.ImageFile.CopyTo(stream);
                }
                article.ArticleImage = "/uploads/articles/" + uniqueFileName;
            }

            foreach (var tag in Request.Form["tags"])
            {
                article.Tags.Add(db.Tags.Find(int.Parse(tag)));
            }

            if (Request.Form["tags"].Count == 0) article.Tags = null;

            article.CreatedDate = DateTime.Now;

            db.Articles.Add(article);
            db.SaveChanges();
            return RedirectToAction("ListArticles", "Admin");
        }

        [Authorize, HttpGet]
        public IActionResult EditArticle(int id)
        {
            var article = db.Articles.Include(a => a.Tags).ToList().Find(a => a.Id == id);

            var tags = db.Tags.ToList();
            ViewData["Tags"] = tags;

            return View(article);
        }

        [Authorize, HttpPost]
        public IActionResult EditArticle(Article a)
        {
            var article = db.Articles.Include(art => art.Tags).First(art => art.Id == a.Id);
            if (a.ImageFile != null)
            {
                var imageName = article.ArticleImage.Split("/").Last();
                DeleteUploadFile(imageName, "articles");
                var uniqueFileName = GetUniqueFileName(a.ImageFile.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, "articles");
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    a.ImageFile.CopyTo(stream);
                }
                article.ArticleImage = "/uploads/articles/" + uniqueFileName;
            }
            else
            {
                ModelState.Remove("ImageFile");
            }
            if (!(ModelState.IsValid))
            {
                var tags = db.Tags.ToList();
                ViewData["Tags"] = tags;

                return View(article);
            }

            foreach (var tag in Request.Form["tags"])
            {
                article.Tags.Clear();
                article.Tags.Add(db.Tags.Find(int.Parse(tag)));
            }

            article.Name = a.Name;
            article.Text = a.Text;
            db.Articles.Update(article);
            db.SaveChanges();
            return RedirectToAction("ListArticles", "Admin");
        }

        [Authorize]
        public IActionResult DeleteArticle(int id)
        {
            var article = db.Articles.Find(id);

            if (article != null)
            {
                var imageName = article.ArticleImage.Split("/").Last();
                DeleteUploadFile(imageName, "articles");
                db.Articles.Remove(article);
            }
            db.SaveChanges();
            return RedirectToAction("ListArticles", "Admin");
        }

        
        private string GetFullPathUploadFile(string fileName, string folderName = "")
        {
            var uploads = Path.Combine(_env.WebRootPath, $"uploads{"\\" + folderName}");
            var filePath = Path.Combine(uploads, fileName);
            return filePath;
        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                      + "_"
                      + Guid.NewGuid().ToString().Substring(0, 4)
                      + Path.GetExtension(fileName);
        }

        private void DeleteUploadFile(string fileName, string folderName = "")
        {
            var uploads = Path.Combine(_env.WebRootPath, $"uploads{"\\" + folderName}");
            var filePath = Path.Combine(uploads, fileName);
            if(System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }
    }
}
