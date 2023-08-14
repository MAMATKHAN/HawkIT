using HawkIT.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HawkIT.Controllers
{
    public class AdminController : Controller
    {
        private readonly HawkitDbContext db;

        public AdminController(HawkitDbContext context)
        {
            db = context;
        }

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

        [Authorize]
        public IActionResult ListProjects()
        {
            return View();
        }

        [Authorize]
        public IActionResult ListWorkers()
        {
            return View();
        }


        // Tag CRUD
        [Authorize]
        public IActionResult ListTags()
        {
            var tags = db.Tags.ToList();
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
            db.Tags.Remove(tag);
            db.SaveChanges();

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
            if (String.IsNullOrEmpty(tag.Name)) return View();

            db.Tags.Update(tag);
            db.SaveChanges();
            return RedirectToAction("ListTags", "Admin");
        }

        [Authorize]
        public IActionResult ListArticles()
        {
            return View();
        }

        [Authorize]
        public IActionResult AddProject()
        {
            return View();
        }

        


        
    }
}
