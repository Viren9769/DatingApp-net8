using DatingAPI.Data;
using DatingAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers;

    public class BuggyController(DataContext datacontext): BaseApiController
    {
        [Authorize]
        [HttpGet("auth")]
        public ActionResult<string> GetAuth()
        {
            return "secret text";
        }
    [HttpGet("not-found")]
    public ActionResult<appUser> GetNotFound()
    {
        var thing = datacontext.Users.Find(-1);
        if(thing == null) return NotFound();
        return thing; 
    }

    [HttpGet("server-error")]
    public ActionResult<appUser> GetServerError()
    {
            var thing = datacontext.Users.Find(-1) ?? throw new Exception("a bad thing happened");
        return thing;
    }
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not good request");
    }

}
