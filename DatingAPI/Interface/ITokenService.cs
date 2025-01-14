using DatingAPI.Entities;

namespace DatingAPI.Interface
{
    public interface ITokenService
    {
        string CreateToken(appUser user); 
    }
}
