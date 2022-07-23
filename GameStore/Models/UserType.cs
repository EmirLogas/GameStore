using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class UserType
    {
        public UserType()
        {
            UserAuthorizations = new HashSet<UserAuthorization>();
        }

        public int UserTypeId { get; set; }
        public string UserTypeName { get; set; } = null!;

        public virtual ICollection<UserAuthorization> UserAuthorizations { get; set; }
    }
}
