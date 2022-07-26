using Microsoft.AspNetCore.Mvc;
using GameStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GameStore.Controllers
{
    public class GameController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();
        [Authorize]
        public IActionResult ListGame()
        {
            List<Game> games = db.Games.ToList();
            ViewBag.Categories = db.Categories.ToList();
            return View(games);
        }

        [Authorize]
        public IActionResult AddGame()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult AddGame(Game game, IFormFile formFile, List<IFormFile> formFiles)
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            game.User = user;
            game.GameCoverImagePath = UploadFile(formFile);
            //game.GameContentImage = UploadFiles(formFiles);
            db.Games.Add(game);
            db.SaveChanges();

            return RedirectToAction("ListGame");
        }
        public IActionResult Game(int id)
        {
            Game game = db.Games.First(x => x.GameId == id);
            return View(game);
        }

        [Authorize]
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

        private string UploadFile(IFormFile formFile)
        {
            string fileName = formFile.FileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            return filePath;
        }
        private string UploadFiles(List<IFormFile> formFiles)
        {
            string filePath = "";
            foreach (var formFile in formFiles)
            {
                filePath = UploadFile(formFile);
            }
            return filePath;
        }
    }
}
