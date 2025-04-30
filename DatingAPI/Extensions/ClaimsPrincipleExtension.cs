/*using System.CodeDom;
using System.Runtime.CompilerServices;
using System.Security.Claims;

namespace DatingAPI.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.Name)
                ?? throw new Exception("cannot get username from token");
            return username;
        }
        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("cannot get username from token"));
            return userId;
        }
    }
}
*/
using System.Security.Claims;

namespace DatingAPI.Extensions
{
    public static class ClaimsPrincipleExtension
    {
        public static string GetUsername(this ClaimsPrincipal user)
        {
            var username = user.FindFirstValue(ClaimTypes.Name)
                ?? throw new Exception("cannot get username from token");
            return username;
        }

        public static int GetUserId(this ClaimsPrincipal user)
        {
            var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier)
                ?? throw new Exception("cannot get userId from token"));
            return userId;
        }
    }
}
