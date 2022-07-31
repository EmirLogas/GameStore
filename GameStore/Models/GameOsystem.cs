using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class GameOsystem
    {
        public int GameOsystemsId { get; set; }
        public int GameId { get; set; }
        public int OsystemId { get; set; }

        public virtual Game Game { get; set; } = null!;
        public virtual Osystem Osystem { get; set; } = null!;
    }
}
