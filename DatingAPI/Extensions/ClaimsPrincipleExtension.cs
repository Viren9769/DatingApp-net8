using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace DatingAPI.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("cannot get username from token");
            return username;
        }
    }
}
