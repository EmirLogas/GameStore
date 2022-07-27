using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class UserGame
    {
        public int UserId { get; set; }
        public int GameId { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
