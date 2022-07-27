using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            Games = new HashSet<Game>();
            UserAuthorizations = new HashSet<UserAuthorization>();
            UserGames = new HashSet<UserGame>();
            UserId1s = new HashSet<User>();
            UserId2s = new HashSet<User>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public DateTime? UserRegisterDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<UserAuthorization> UserAuthorizations { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }

        public virtual ICollection<User> UserId1s { get; set; }
        public virtual ICollection<User> UserId2s { get; set; }
    }
}
