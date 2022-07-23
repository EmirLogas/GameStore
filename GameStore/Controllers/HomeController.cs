using GameStore.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using GameStore.Models;

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
            
            return View(games);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}