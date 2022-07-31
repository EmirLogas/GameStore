using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class Osystem
    {
        public Osystem()
        {
            GameOsystems = new HashSet<GameOsystem>();
        }

        public int OsystemId { get; set; }
        public string OsystemName { get; set; } = null!;

        public virtual ICollection<GameOsystem> GameOsystems { get; set; }
    }
}
