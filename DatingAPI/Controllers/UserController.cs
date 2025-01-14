using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    
    public class UserController(DataContext context) : BaseApiController
    {
        // private readonly DataContext _context = context;
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<appUser>>> GetUser()
        {
            var user = await context.Users.ToListAsync();

            return Ok(user);

        }
        [Authorize]
        [HttpGet("{id:int}")]    // api/users/id
        public async Task<ActionResult<appUser>> GetUserById(int id)
        {
            var user = await context.Users.FindAsync(id);
            if(user == null) {return NotFound(); }

            return user;
        }


    }
}
