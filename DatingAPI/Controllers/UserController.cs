using AutoMapper;
using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers;


[Authorize]
public class UserController(IUserRepository userRepository) : BaseApiController
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


    }

