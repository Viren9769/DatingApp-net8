using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DatingAPI.Controllers;


[Authorize]
public class UserController(IUserRepository userRepository, IMapper mapper) : BaseApiController
    {
    // private readonly DataContext _context = context;
    
    [HttpGet]
        public async Task<ActionResult<IEnumerable<MembersDTO>>> GetUser()
        {
            var user = await userRepository.GetAllMembersAsync();
            

            return Ok(user);

        }
   
    [HttpGet("{username}")]    // api/users/id
        public async Task<ActionResult<MembersDTO>> GetUserById(string username)
        {
            var user = await userRepository.GetMemberAsync(username);
            if(user == null) {return NotFound(); }

        return user;
        }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (username == null) return BadRequest("No username found in token");
        var user = await userRepository.GetUserByNameAsync(username);
        if (user == null) return BadRequest("could not find user");
        mapper.Map(memberUpdateDto, user);
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to upddate the user");
    }

    }

