using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
//using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{

    public class AccountController( UserManager<appUser> userManager, ITokenService tokenService,  IMapper mapper) : BaseApiController
    {
        [HttpPost("register")] //acount register

        public async Task<ActionResult<UserDTO>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("UserName is Taken");

            
            var user = mapper.Map<appUser>(registerDto);
            user.UserName = registerDto.Username.ToLower();
            /*user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
            user.PasswordSalt = hmac.Key;*/
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded) return BadRequest(result.Errors);
      
            return new UserDTO
            {
                Username = user.UserName,
                token = await tokenService.CreateToken(user),
                KnownAs = user.KnownAs,
                Gender = user.Gender
            };

        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDTO>> Login(LoginDTO loginDTO)
        {
            var user = await userManager.Users.
                Include(p => p.Photos)
                .FirstOrDefaultAsync(x => x.NormalizedUserName == loginDTO.Username.ToUpper());
            if (user == null || user.UserName == null) return Unauthorized("Invalid username");

            var result = await userManager.CheckPasswordAsync(user, loginDTO.Password);
            if (!result) return Unauthorized();
            return new UserDTO
            {
                Username = user.UserName,
                KnownAs = user.KnownAs,
                token = await tokenService.CreateToken(user),
                Gender = user.Gender,
                PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };
    }

        

        private async Task<bool> UserExists(string username)
        {
            return await userManager.Users.AnyAsync(
                x => x.NormalizedUserName == username.ToUpper());
        }
    }
}