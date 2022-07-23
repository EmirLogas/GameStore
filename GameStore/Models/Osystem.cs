using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class Osystem
    {
        public Osystem()
        {
            Games = new HashSet<Game>();
        }

        public int OsystemId { get; set; }
        public string OsystemName { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}
