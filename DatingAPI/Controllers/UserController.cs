using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(DataContext context) : ControllerBase
    {
        // private readonly DataContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<appUser>>> GetUser()
        {
            var user = await context.Users.ToListAsync();

            return Ok(user);

        }

        [HttpGet("{id:int}")]    // api/users/id
        public async Task<ActionResult<appUser>> GetUserById(int id)
        {
            var user = await context.Users.FindAsync(id);
            if(user == null) {return NotFound(); }

            return user;
        }


    }
}
