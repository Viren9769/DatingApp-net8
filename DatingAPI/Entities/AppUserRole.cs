using Microsoft.AspNetCore.Identity;

namespace DatingAPI.Entities
{
    public class AppUserRole:IdentityUserRole<int>
    {
        public appUser User { get; set; } = null!;
        public AppRole Role { get; set; } = null!;
    }
}
