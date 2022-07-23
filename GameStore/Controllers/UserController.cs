using Microsoft.AspNetCore.Mvc;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class UserController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Register(String UserEmail, String UserPassword)
        {
            User user = new User();
            user.UserEmail = UserEmail;
            user.UserPassword = UserPassword;
            user.UserName = "example";
            db.Users.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(String UserEmail, String UserPassword)
        {
            User user = db.Users.First(x => x.UserEmail == UserEmail && x.UserPassword == UserPassword);
            if (user != null)
            {
                ViewBag.CurrentUser = user;
                return RedirectToAction("Index");
            }
            else
                return View();
        }
    }
}
