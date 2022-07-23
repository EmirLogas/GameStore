using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class ContentImage
    {
        public ContentImage()
        {
            Games = new HashSet<Game>();
        }

        public int ContentImageId { get; set; }
        public string ContentImagePath { get; set; } = null!;

        public virtual ICollection<Game> Games { get; set; }
    }
}
