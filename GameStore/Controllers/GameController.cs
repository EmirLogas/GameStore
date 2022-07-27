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
        public IActionResult ListReleased()
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Game> games = db.Games.Where(x => x.UserId == user.UserId).ToList();
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
            game.User = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            game.GameCoverImagePath = UploadFile(formFile);

            db.Games.Add(game);
            db.SaveChanges();
            UploadFiles(formFiles, game);

            return RedirectToAction("ListReleased");
        }
        public IActionResult Game(int id)
        {
            Game game = db.Games.First(x => x.GameId == id);
            List<ContentImage> contentImages = db.ContentImages.Where(x => x.GameId == id).ToList();
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) != null)
            {
                User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
                List<UserGame> userGames = db.UserGames.Where(x => x.UserId == user.UserId && x.GameId == id).ToList();
                ViewBag.userGames = userGames;
            }

            ViewBag.ContentImages = contentImages;
            return View(game);
        }

        [Authorize]
        [HttpPost]
        public string DeleteGame(int id)
        {
            try
            {
                Game game = db.Games.First(x => x.GameId == id);
                FileInfo GameCoverImageFile = new FileInfo(@"wwwroot\" + game.GameCoverImagePath);
                if (GameCoverImageFile.Exists)//check file exsit or not  
                {
                    GameCoverImageFile.Delete();
                }

                IQueryable<ContentImage> contentImages = db.ContentImages.Where(x => x.GameId == id);

                foreach (ContentImage contentImage in contentImages)
                {
                    string path = @"wwwroot\" + contentImage.ContentImagePath;
                    FileInfo ContentImageFile = new FileInfo(path);
                    if (ContentImageFile.Exists)//check file exsit or not  
                    {
                        ContentImageFile.Delete();
                    }
                    db.ContentImages.Remove(contentImage);
                }
                db.Games.Remove(game);
                db.SaveChanges();
                return "success";
            }
            catch (Exception)
            {
                return "fail";
            }
        }
        [Authorize]
        private string UploadFile(IFormFile formFile)
        {
            string fileName = formFile.FileName;
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", fileName);
            string filePathForDB = Path.Combine("images", fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            return filePathForDB;
        }
        [Authorize]
        private void UploadFiles(List<IFormFile> formFiles, Game game)
        {
            string filePath = "";
            foreach (var formFile in formFiles)
            {
                filePath = UploadFile(formFile);
                ContentImage contentImage = new ContentImage();
                contentImage.GameId = game.GameId;
                contentImage.ContentImagePath = filePath;
                db.ContentImages.Add(contentImage);
                db.SaveChanges();
            }
        }

        [Authorize]
        public IActionResult BuyGame(int id)
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            Game game = db.Games.First(x => x.GameId == id);
            db.UserGames.Add(new UserGame() { UserId = user.UserId, GameId = game.GameId });
            db.SaveChanges();
            return RedirectToAction("ListLibrary");
        }

        public IActionResult ListLibrary()
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Game> games = db.UserGames.Where(x => x.UserId == user.UserId).Select(x => x.Game).ToList();
            ViewBag.Categories = db.Categories.ToList();
            return View(games);
        }
    }
}
