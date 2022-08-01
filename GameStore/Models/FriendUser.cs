using System;
using System.Collections.Generic;

namespace GameStore.Models
{
    public partial class FriendUser
    {
        public int FriendUsersId { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }

        public virtual User UserId1Navigation { get; set; } = null!;
        public virtual User UserId2Navigation { get; set; } = null!;
    }
}
