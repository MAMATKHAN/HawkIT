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
                    ModelState.AddModelError("Incorrect loging or password", "Логин или пароль введен неправильно");
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
            return RedirectToAction("Index", "Home");
        }

        // PROJECT CRUD
        // READ PROJECTS
        [Authorize]
        public IActionResult ListProjects(string? name ,int? tagId, int? workerId)
        {
            var projects = db.Projects
                .Include(p => p.Tags)
                .Include(p => p.Workers)
                .ToList();

            var tags = db.Tags
                .Where(t => t.Projects.Count != 0)
                .ToList();

            var workers = db.Workers
                .Where(w => w.Projects.Count != 0)
                .ToList();

            if (name != null) projects = projects
                    .Where(w => w.Name.ToLower().Contains(name.ToLower()))
                    .ToList();
            if (tagId != -1 && tagId != null)
            {
                var tag = db.Tags.Find(tagId);
                if(tag != null) projects = projects
                        .Where(p => (p.Tags != null)? p.Tags.Contains(tag) : false)
                        .ToList();
            }
            if (workerId != -1 && workerId != null)
            {
                var worker = db.Workers.Find(workerId);
                if(worker != null) projects = projects
                        .Where(p => p.Workers.Contains(worker))
                        .ToList();
            }

            var adminProjectViewModel = new AdminProjectsViewModel { Projects = projects, Workers = workers, Tags = tags };
            return View(adminProjectViewModel);
        }

        // CREATE PROJECT
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
            if (Request.Form["workers"].Count == 0) ModelState.AddModelError("Workers", "Это поле обязательно должно быть заполнено");

            if (!(ModelState.IsValid))
            {
                ViewData["Workers"] = db.Workers.ToList();
                ViewData["Tags"] = db.Tags.ToList();
                return View();
            }

            foreach (var workerId in Request.Form["workers"])
            {
                var worker = (workerId != null) ? db.Workers.Find(int.Parse(workerId)) : null;
                if (worker != null) project.Workers.Add(worker);
            }
            foreach (var tagId in Request.Form["tags"])
            {
                var tag = (tagId != null) ? db.Tags.Find(int.Parse(tagId)) : null;
                if (tag != null) project.Tags?.Add(tag);
            }

            project.CreatedDate = DateTime.Now;
            project.ProjectImage = UploadFile(project.ImageFile, "projects");
            project.Documents = UploadFile(project.DocumentFiles, "docs");
            project.Banners = UploadFile(project.BannerImages, "banners");

            db.Projects.Add(project);
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");
        }

        // UPDATE PROJECT
        [Authorize, HttpGet]
        public IActionResult EditProject(int id)
        {
            var project = db.Projects.Include(p => p.Tags).Include(p => p.Workers).ToList().Find(p => p.Id == id);
            ViewData["Tags"] = db.Tags.ToList();
            ViewData["Workers"] = db.Workers.ToList();

            return View(project);

        }

        [Authorize, HttpPost]
        public IActionResult EditProject(Project projectFormModel)
        {
            ModelState.Remove("ImageFile");
            var project = db.Projects.Include(p => p.Tags).Include(p => p.Workers).First(p => p.Id == projectFormModel.Id);
  
            if (!(ModelState.IsValid))
            {
                ViewData["Tags"] = db.Tags.ToList();
                ViewData["Workers"] = db.Workers.ToList();

                return View(project);
            }

            if (Request.Form["tags"].Count == 0) project.Tags?.Clear();
            foreach (var tagId in Request.Form["tags"])
            {
                var tag = (tagId != null)? db.Tags.Find(int.Parse(tagId)) : null;
                if(tag != null) project.Tags?.Add(tag);
            }

            if (Request.Form["tags"].Count == 0) project.Workers.Clear();
            foreach (var workerId in Request.Form["workers"])
            {
                var worker = (workerId != null)? db.Workers.Find(int.Parse(workerId)) : null;
                if(worker != null) project.Workers.Add(worker);
            }

            project.Banners = UploadFile(projectFormModel.BannerImages, "banners", project.Banners);
            project.Documents = UploadFile(projectFormModel.DocumentFiles, "docs", project.Documents);
            project.ProjectImage = UploadFile(projectFormModel.ImageFile, "projects", project.ProjectImage);
            project.Name = projectFormModel.Name;
            project.Text = projectFormModel.Text;
            db.Projects.Update(project);
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");
        }

        // DELETE PROJECT
        [Authorize]
        public IActionResult DeleteProject(int id)
        {
            var project = db.Projects.Find(id);

            if (project != null)
            {
                DeleteFile(project.ProjectImage);
                DeleteFile(project.Documents);
                DeleteFile(project.Banners);
                db.Projects.Remove(project);
            }
            db.SaveChanges();
            return RedirectToAction("ListProjects", "Admin");

        }

        // WORKER CRUD
        // READ WORKERS
        [Authorize, HttpGet]
        public IActionResult ListWorkers(string? name,int projectId, string? specialization)
        {
            var workers = db.Workers.Include(w => w.Projects).ToList();
            var projects = db.Projects
                .Where(p => p.Workers.Count != 0)
                .ToList();

            if(name != null) workers = workers
                    .Where(w => w.Name.ToLower().Contains(name.ToLower()))
                    .ToList();
            if(specialization != null) workers = workers
                    .Where(w => w.Specialization.ToLower().Contains(specialization.ToLower()))
                    .ToList();
            if(projectId != -1)
            {
                var project = db.Projects.Find(projectId);
                if(project != null) workers = workers
                    .Where(w => (w.Projects != null)? w.Projects.Contains(project) : false)
                    .ToList();
            }

            var adminWorkerViewModel = new AdminWorkerViewModel { Workers = workers, Projects = projects };
            return View(adminWorkerViewModel);
        }

        // CREATE WORKER
        [Authorize, HttpGet]
        public IActionResult AddWorker()
        {
            var project = db.Projects.ToList();
            ViewData["Projects"] = project;
            return View();
        }

        [Authorize, HttpPost]
        public IActionResult AddWorker(Worker worker)
        {
            if (!(ModelState.IsValid))
            {
                var projects = db.Projects.ToList();
                ViewData["Projects"] = projects;
                return View();
            }

            foreach (var projectId in Request.Form["projects"])
            {
                var project = (projectId != null) ? db.Projects.Find(int.Parse(projectId)) : null;
                if (project != null) worker.Projects?.Add(project);
            }

			worker.WorkerImage = UploadFile(worker.ImageFile, "workers");
			worker.SpecializationIcon = UploadFile(worker.IconFile, "workers");

			db.Workers.Add(worker);
            db.SaveChanges();
            return RedirectToAction("ListWorkers", "Admin");
        }

        // UPDATE WORKER
        [Authorize, HttpGet]
        public IActionResult EditWorker(int id)
        {
            var worker = db.Workers.Include(a => a.Projects).ToList().Find(w => w.Id == id);

            var projects = db.Projects.ToList();
            ViewData["Projects"] = projects;

            return View(worker);
        }



        [Authorize, HttpPost]
        public IActionResult EditWorker(Worker workerFormModel)
        {
            ModelState.Remove("IconFile");
            ModelState.Remove("ImageFile");
            var worker = db.Workers.Include(w => w.Projects).First(w => w.Id == workerFormModel.Id);
           
            if (!ModelState.IsValid)
            {
                var projects = db.Projects.ToList();
                ViewData["Projects"] = projects;

                return View(worker);
            }

            if (Request.Form["projects"].Count == 0) worker.Projects?.Clear();
            foreach (var projectId in Request.Form["projects"])
            {
                var project = (projectId != null)? db.Projects.Find(int.Parse(projectId)) : null;
                if (project != null) worker.Projects?.Add(project);
            }

            worker.WorkerImage = UploadFile(workerFormModel.ImageFile, "workers", worker.WorkerImage);
            worker.SpecializationIcon = UploadFile(workerFormModel.IconFile, "workers", worker.SpecializationIcon);
            worker.Name = workerFormModel.Name;
            worker.Specialization = workerFormModel.Specialization;
            db.Workers.Update(worker);
            db.SaveChanges();

            return RedirectToAction("ListWorkers", "Admin");
        }

        // DELETE WORKER
        [Authorize, HttpGet]
        public IActionResult DeleteWorker(int id)
        {
            var worker = db.Workers.Find(id);

            if (worker != null)
            {
				DeleteFile(worker.WorkerImage);
                DeleteFile(worker.SpecializationIcon);
                db.Workers.Remove(worker);
            }
            db.SaveChanges();
            return RedirectToAction("ListWorkers", "Admin");
        }

        // Tag CRUD
        // READ TAGS
        [Authorize, HttpGet]
        public IActionResult ListTags(string? searchString)
        {
            List<Tag> tags;
            if (string.IsNullOrEmpty(searchString)) tags = db.Tags.ToList();
            else tags = db.Tags.Where(t => t.Name.ToLower().Contains(searchString.ToLower())).ToList();

            return View(tags);
        }

        // CREATE TAG
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

        // UPDATE TAG
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

        // DELETE TAG
        [Authorize]
        public IActionResult DeleteTag(int id)
        {
            var tag = db.Tags.Find(id);
            if (tag != null)
            {
                db.Tags.Remove(tag);
                db.SaveChanges();
            }
            return RedirectToAction("ListTags", "Admin");
        }

        // ARTICLE CRUD
        // READ ARTICLES
        [Authorize]
        public IActionResult ListArticles(string? articleName, int? tagId)
        {
            var articles = db.Articles.Include(p => p.Tags).ToList();
            var tags = db.Tags
                .Where(t => t.Articles.Count != 0)
                .ToList();
            
            if (articleName != null) articles = articles.Where(a => a.Name.ToLower().Contains(articleName.ToLower())).ToList();
            if (tagId != null && tagId != -1)
            {
                var tag = db.Tags.Find(tagId);
                if (tag != null) articles = articles
                        .Where(a => (a.Tags != null)? a.Tags.Contains(tag) : false)
                        .ToList();
            }


            var adminArticleViewModel = new AdminArticleViewModel { Articles = articles, Tags = tags };
            return View(adminArticleViewModel);
        }

        // CREATE ARTICLE
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
            if (!ModelState.IsValid)
            {
                var tags = db.Tags.ToList();
                ViewData["Tags"] = tags;
                return View();
            }

            foreach (var tagId in Request.Form["tags"])
            {
                var tag = (tagId != null) ? db.Tags.Find(int.Parse(tagId)) : null;
                if (tag != null) article.Tags?.Add(tag);
            }

            article.CreatedDate = DateTime.Now;
			article.ArticleImage = UploadFile(article.ImageFile, "articles");


			db.Articles.Add(article);
            db.SaveChanges();
            return RedirectToAction("ListArticles", "Admin");
        }
        
        // UPDATE ARTICLE
        [Authorize, HttpGet]
        public IActionResult EditArticle(int id)
        {
            var article = db.Articles.Include(a => a.Tags).ToList().Find(a => a.Id == id);

            var tags = db.Tags.ToList();
            ViewData["Tags"] = tags;

            return View(article);
        }

        [Authorize, HttpPost]
        public IActionResult EditArticle(Article articleFormModel)
        {
            ModelState.Remove("ImageFile");
            var article = db.Articles.Include(a => a.Tags).First(a => a.Id == articleFormModel.Id);            
            if (!ModelState.IsValid)
            {
                var tags = db.Tags.ToList();
                ViewData["Tags"] = tags;

                return View(article);
            }

            if (Request.Form["tags"].Count == 0) article.Tags?.Clear();
            foreach (var tagId in Request.Form["tags"])
            {
                var tag = (tagId != null)? db.Tags.Find(int.Parse(tagId)) : null;
                if (tag != null) article.Tags?.Add(tag);
            }

            article.ArticleImage = UploadFile(articleFormModel.ImageFile, "articles", article.ArticleImage);
            article.Name = articleFormModel.Name;
            article.Text = articleFormModel.Text;
            db.Articles.Update(article);
            db.SaveChanges();

            return RedirectToAction("ListArticles", "Admin");
        }

        // DELETE ARTICLE
        [Authorize]
        public IActionResult DeleteArticle(int id)
        {
            var article = db.Articles.Find(id);

            if (article != null && article.ArticleImage != null)
            {
                DeleteFile(article.ArticleImage);
                db.Articles.Remove(article);
            }
            db.SaveChanges();
            return RedirectToAction("ListArticles", "Admin");
        }

        

        private string UploadFile(IFormFile? file, string? folderName = "", string? oldFileName = null)
        {
            if (file == null) return oldFileName ?? "";
            if(!String.IsNullOrEmpty(oldFileName)) DeleteFile(oldFileName);

			var uniqueFileName = GetUniqueFileName(file.FileName);
			var filePath = GetFullPathUploadFile(uniqueFileName, folderName);
			using (var stream = new FileStream(filePath, FileMode.Create))
			{
				file?.CopyTo(stream);
			}
			return $"/uploads/{folderName + "/"}" + uniqueFileName;
		}

        private string UploadFile(IEnumerable<IFormFile>? files, string folderName, string? oldFiles = null)
        {
            if (files == null || files.Count() == 0) 
                return oldFiles ?? "";
            if(oldFiles != null) DeleteFile(oldFiles.Split("||"));

            var fileNames = new List<string>();
            foreach(var file in files)
            {
                var uniqueFileName = GetUniqueFileName(file.FileName);
                var filePath = GetFullPathUploadFile(uniqueFileName, folderName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    file?.CopyTo(stream);
                }
                fileNames.Add($"/uploads/{folderName + "/"}" + uniqueFileName);
            }
            return string.Join("||", fileNames);
        }

        private string GetFullPathUploadFile(string fileName, string? folderName)
        {
            var uploads = Path.Combine(_env.WebRootPath, $"uploads{"/" + folderName}");
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

        private void DeleteFile(string? fileName)
        {
            if (fileName == null) return;

            fileName = fileName.TrimStart('/').Replace("\\", "/");
            var filePath = Path.Combine(_env.WebRootPath, fileName);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
        }

        private void DeleteFile(IEnumerable<string>? filesName)
        {
            if (filesName == null) return;

            foreach (var file in filesName)
            {
                string fileName = file.Replace("\\", "/");
                string filePath = Path.Combine(_env.WebRootPath + fileName);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
            }
        }
    }
}
