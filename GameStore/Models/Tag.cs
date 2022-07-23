using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class Tag
    {
        public Tag()
        {
            Games = new HashSet<Game>();
        }

        public int TagId { get; set; }
        public string TagName { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}
