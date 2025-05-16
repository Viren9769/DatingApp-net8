using DatingAPI.Entities;

namespace DatingAPI.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(appUser user); 
    }
}
