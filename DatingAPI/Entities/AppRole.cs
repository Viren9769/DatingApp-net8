﻿using Microsoft.AspNetCore.Identity;

namespace DatingAPI.Entities
{
    public class AppRole: IdentityRole<int>
    {
        public ICollection<AppUserRole> UserRoles { get; set; } = [];
    }
}
