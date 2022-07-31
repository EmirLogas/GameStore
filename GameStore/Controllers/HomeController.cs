using GameStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;

namespace GameStore.Controllers
{
    public class HomeController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        
        public IActionResult Index()
        {
            List<Game> games = db.Games.ToList();

            ViewBag.UserName = this.User.FindFirstValue(ClaimTypes.Name);
            ViewBag.Email = this.User.FindFirstValue(ClaimTypes.Email);

            return View(games);
        }

        [Authorize(Policy = "AdminOnly")]
        public IActionResult AdminIndex()
        {
            // Total games sold count
            int soldGameCount = db.UserGames.Count();
            ViewBag.SoldGameCount = soldGameCount;

            // Total revenue from games sold
            decimal soldGameRevenue = db.UserGames.Sum(x => x.Game.GamePrice);
            ViewBag.SoldGameRevenue = soldGameRevenue;

            int GameCount = db.Games.Count();
            ViewBag.GameCount = GameCount;

            int User = db.Users.Count();
            ViewBag.UserCount = User;

            // Total comments count
            int commentCount = db.Comments.Count();
            ViewBag.CommentCount = commentCount;

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}