using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class ContentImage
    {
        public int ContentImageId { get; set; }
        public int GameId { get; set; }
        public string ContentImagePath { get; set; } = null!;

        public virtual Game Game { get; set; } = null!;
    }
}
