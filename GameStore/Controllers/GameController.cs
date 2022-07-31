using Microsoft.AspNetCore.Mvc;
using GameStore.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace GameStore.Controllers
{
    public class GameController : Controller
    {
        GameStoreDBContext db = new GameStoreDBContext();

        [Authorize(Policy = "UserOnly")]
        public IActionResult ListReleased()
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Game> games = db.Games.Where(x => x.UserId == user.UserId).ToList();
            ViewBag.Categories = db.Categories.ToList();
            List<int> gameSaleCount = new List<int>();
            // Game sale count
            for (int i = 0; i < games.Count; i++)
            {
                gameSaleCount.Add(db.UserGames.Where(x => x.GameId == games[i].GameId).Count());
            }
            ViewBag.gameSaleCountVB = gameSaleCount;
            return View(games);
        }

        [Authorize(Policy = "UserOnly")]
        public IActionResult AddGame()
        {
            ViewBag.Categories = db.Categories.ToList();
            return View();
        }

        [Authorize(Policy = "UserOnly")]
        [HttpPost]
        public IActionResult AddGame(Game game, IFormFile formFile, List<IFormFile> formFiles, String Windows, String Linux, String MacOS)
        {
            game.User = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            game.GameCoverImagePath = UploadFile(formFile, game);
            db.Games.Add(game);
            db.SaveChanges();

            if (Windows == "Windows")
            {
                GameOsystem gameOsystem = new GameOsystem();
                gameOsystem.GameId = game.GameId;
                gameOsystem.OsystemId = 1;
                db.GameOsystems.Add(gameOsystem);
            }

            if (Linux == "Linux")
            {
                GameOsystem gameOsystem = new GameOsystem();
                gameOsystem.GameId = game.GameId;
                gameOsystem.OsystemId = 2;
                db.GameOsystems.Add(gameOsystem);
            }

            if (MacOS == "Macos")
            {
                GameOsystem gameOsystem = new GameOsystem();
                gameOsystem.GameId = game.GameId;
                gameOsystem.OsystemId = 3;
                db.GameOsystems.Add(gameOsystem);
            }

            db.SaveChanges();

            UploadFiles(formFiles, game);

            return RedirectToAction("ListReleased");
        }

        [Authorize(Policy = "UserOnly")]
        private string UploadFile(IFormFile formFile, Game game)
        {
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(formFile.FileName);
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", game.GameName + fileName);
            string filePathForDB = Path.Combine("images", game.GameName + fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                formFile.CopyTo(stream);
            }
            return filePathForDB;
        }

        [Authorize(Policy = "UserOnly")]
        private void UploadFiles(List<IFormFile> formFiles, Game game)
        {
            string filePath = "";
            foreach (var formFile in formFiles)
            {
                filePath = UploadFile(formFile, game);
                ContentImage contentImage = new ContentImage();
                contentImage.GameId = game.GameId;
                contentImage.ContentImagePath = filePath;
                db.ContentImages.Add(contentImage);
                db.SaveChanges();
            }
        }

        [Authorize(Policy = "UserOnly")]
        public IActionResult EditGame(int id)
        {
            Game game = db.Games.First(x => x.GameId == id);
            ViewBag.Categories = db.Categories.ToList();
            return View(game);
        }

        [Authorize(Policy = "UserOnly")]
        public IActionResult UpdateGame(Game game/*, IFormFile formFile, List<IFormFile> formFiles*/)
        {
            Game entity = db.Games.First(x => x.GameId == game.GameId);
            if (entity != null)
            {

                entity.GameName = game.GameName;
                entity.GamePrice = game.GamePrice;
                entity.GameDescription = game.GameDescription;
                entity.GamePublisher = game.GamePublisher;
                entity.GameDeveloper = game.GameDeveloper;
                entity.GameCategoryId = game.GameCategoryId;
            }
            db.SaveChanges();
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
                int userGamesCount = userGames.Count();
                ViewBag.userGamesCount = userGamesCount;
            }
            List<Comment> comments = db.Comments.Where(x => x.GameId == id).ToList();
            List<User> commentsUsers = new List<User>();
            foreach (var comment in comments)
            {
                commentsUsers.Add(db.Users.First(x => x.UserId == comment.UserId));
            }
            ViewBag.commentsUsers = commentsUsers;
            ViewBag.comments = comments;
            ViewBag.ContentImages = contentImages;
            return View(game);
        }

        [Authorize(Policy = "UserOnly")]
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
                // Delete Game Comments
                IQueryable<Comment> comments = db.Comments.Where(x => x.GameId == id);
                foreach (Comment comment in comments)
                {
                    db.Comments.Remove(comment);
                }

                IQueryable<UserGame> userGames = db.UserGames.Where(x => x.GameId == id);
                foreach (UserGame userGame in userGames)
                {
                    db.UserGames.Remove(userGame);
                }

                IQueryable<GameOsystem> gameOsystems = db.GameOsystems.Where(x => x.GameId == id);
                foreach (GameOsystem gameOsystem in gameOsystems)
                {
                    db.GameOsystems.Remove(gameOsystem);
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

        [Authorize(Policy = "UserOnly")]
        public IActionResult BuyGame(int id)
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            Game game = db.Games.First(x => x.GameId == id);
            db.UserGames.Add(new UserGame() { UserId = user.UserId, GameId = game.GameId });
            db.SaveChanges();
            return RedirectToAction("ListLibrary");
        }

        [Authorize(Policy = "UserOnly")]
        public IActionResult ListLibrary()
        {
            User user = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<Game> games = db.UserGames.Where(x => x.UserId == user.UserId).Select(x => x.Game).ToList();
            ViewBag.Categories = db.Categories.ToList();
            return View(games);
        }

        [Authorize(Policy = "UserOnly")]
        public IActionResult AddComment(int GameId, Comment comment)
        {
            comment.User = db.Users.First(x => x.UserId.ToString() == User.FindFirstValue(ClaimTypes.NameIdentifier));
            comment.GameId = GameId;
            db.Comments.Add(comment);
            db.SaveChanges();
            return RedirectToAction("Game", new { id = GameId });
        }
    }
}
