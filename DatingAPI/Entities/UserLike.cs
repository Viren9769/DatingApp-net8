using Microsoft.Identity.Client;

namespace DatingAPI.Entities
{
    public class UserLike
    {
        public appUser SourceUser { get; set; } = null!;
        public int SourceuserId { get; set; }
        public appUser TargetUser { get; set; } = null!;
        public int TargetuserId { get; set; }

    }
}
