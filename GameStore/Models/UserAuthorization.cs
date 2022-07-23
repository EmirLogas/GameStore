using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class UserAuthorization
    {
        public int UserAuthorizationId { get; set; }
        public int UserId { get; set; }
        public int UserTypeId { get; set; }

        public virtual User User { get; set; } = null!;
        public virtual UserType UserType { get; set; } = null!;
    }
}
