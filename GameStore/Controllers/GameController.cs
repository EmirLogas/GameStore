using Microsoft.AspNetCore.Mvc;
using GameStore.Models;

namespace GameStore.Controllers
{
    public class GameController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();
        public IActionResult ListGame()
        {
            List<Game> games = db.Games.ToList();
            ViewBag.Categories = db.Categories.ToList();
            return View(games);
        }
        public IActionResult AddGame()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        [HttpPost]
        public IActionResult AddGame(Game game)
        {
            User user = db.Users.First(x => x.UserId == 1);
            game.User = user;
            db.Games.Add(game);
            db.SaveChanges();

            return RedirectToAction("ListGame");
        }
        public IActionResult Game(int id)
        {
            Game game = db.Games.First(x => x.GameId == id);
            return View(game);
        }
        
        [HttpPost]
        public string DeleteGame(int id)
        {
            try
            {
                Game game = db.Games.First(x => x.GameId == id);
                db.Games.Remove(game);
                db.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return "fail";
            }
        }
    }
}
