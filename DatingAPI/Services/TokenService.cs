using DatingAPI.Entities;
using DatingAPI.Interface;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DatingAPI.Services
{
    public class TokenService(IConfiguration config): ITokenService 
    {
        public string CreateToken(appUser user)
        {
            var tokenKey = config["TokenKey"] ?? throw new Exception( "Cannot access tokenKey from appsetting");
            if (tokenKey.Length < 64) throw new Exception("Youe tokenkey need to be longer");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.UserName)
            };
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);
            var tokenDescripter = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            var tokenaHandler = new JwtSecurityTokenHandler();
            var token = tokenaHandler.CreateToken(tokenDescripter);

            return tokenaHandler.WriteToken(token);
        }
    }
}
