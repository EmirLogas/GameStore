using Microsoft.AspNetCore.Mvc;
using GameStore.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

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
        public IActionResult Register(User u)
        {
            u.UserName = u.UserEmail.Split('@')[0];
            u.UserTypeId = 2;
            db.Users.Add(u);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(User u)
        {
            User user = db.Users.First(x => x.UserEmail == u.UserEmail && x.UserPassword == u.UserPassword);
            if (user == null)
            {
                return View();
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.UserEmail),
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim("Role", user.UserTypeId.ToString())
                };

                var claimsIdentity = new ClaimsIdentity(
                    claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var authProperties = new AuthenticationProperties
                {
                    //AllowRefresh = <bool>,
                    // Refreshing the authentication session should be allowed.

                    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                    // The time at which the authentication ticket expires. A 
                    // value set here overrides the ExpireTimeSpan option of 
                    // CookieAuthenticationOptions set with AddCookie.

                    //IsPersistent = true,
                    // Whether the authentication session is persisted across 
                    // multiple requests. When used with cookies, controls
                    // whether the cookie's lifetime is absolute (matching the
                    // lifetime of the authentication ticket) or session-based.

                    //IssuedUtc = <DateTimeOffset>,
                    // The time at which the authentication ticket was issued.

                    //RedirectUri = <string>
                    // The full path or absolute URI to be used as an http 
                    // redirect response value.
                };

                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIdentity),
                    authProperties);

                if (user.UserTypeId == 1)
                {
                    return RedirectToAction("AdminIndex");
                }
                else
                {
                    return RedirectToAction("Index");
                }

            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index");
        }

        [Authorize]
        public IActionResult EditAccountPage(int id)
        {
            User u = db.Users.Where(x => x.UserId == id).First();
            return View(u);
        }

        [Authorize]
        public IActionResult EditAccount(User u)
        {
            User entity = db.Users.Where(x => x.UserId == u.UserId).First();
            entity.UserName = u.UserName;
            entity.UserEmail = u.UserEmail;
            entity.UserPassword = u.UserPassword;
            db.SaveChanges();
            _ = Login(u);
            return RedirectToAction("Index");
        }
    }
}
