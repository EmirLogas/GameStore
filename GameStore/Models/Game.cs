using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class Game
    {
        public Game()
        {
            Comments = new HashSet<Comment>();
            Osystems = new HashSet<Osystem>();
            Tags = new HashSet<Tag>();
            Users = new HashSet<User>();
        }

        public int UserId { get; set; }
        public int GameId { get; set; }
        public string GameName { get; set; } = null!;
        public string GameDescription { get; set; } = null!;
        public decimal GamePrice { get; set; }
        public int GameCategoryId { get; set; }
        public DateTime GameReleaseDate { get; set; }
        public string GameCoverImagePath { get; set; } = null!;
        public int GameContentImageId { get; set; }
        public string GameDeveloper { get; set; } = null!;
        public string GamePublisher { get; set; } = null!;

        public virtual Category GameCategory { get; set; } = null!;
        public virtual ContentImage GameContentImage { get; set; } = null!;
        public virtual User User { get; set; } = null!;
        public virtual ICollection<Comment> Comments { get; set; }

        public virtual ICollection<Osystem> Osystems { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
