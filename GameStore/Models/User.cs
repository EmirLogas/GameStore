using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            FriendUserUserId1Navigations = new HashSet<FriendUser>();
            FriendUserUserId2Navigations = new HashSet<FriendUser>();
            Games = new HashSet<Game>();
            UserGames = new HashSet<UserGame>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string UserEmail { get; set; } = null!;
        public string UserPassword { get; set; } = null!;
        public int UserTypeId { get; set; }
        public DateTime? UserRegisterDate { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<FriendUser> FriendUserUserId1Navigations { get; set; }
        public virtual ICollection<FriendUser> FriendUserUserId2Navigations { get; set; }
        public virtual ICollection<Game> Games { get; set; }
        public virtual ICollection<UserGame> UserGames { get; set; }
    }
}
