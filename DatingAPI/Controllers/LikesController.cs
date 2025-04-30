using DatingAPI.Data;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Mvc;

namespace DatingAPI.Controllers
{
    public class LikesController(ILikesRepository likesRepository): BaseApiController
    {
        [HttpPost("{targetUserId:int}")]
        public async Task<ActionResult> ToggleLike(int targetUserId)
        {
            var sourceUserId = User.GetUserId();
            if(sourceUserId == targetUserId) return BadRequest("You cannot like yourself");
            var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);
            if(existingLike == null)
            {
                var like = new UserLike
                { 
                    SourceuserId = sourceUserId,
                    TargetuserId = targetUserId,
                };
                likesRepository.AddLike(like);
            }
            else
            {
                likesRepository.DeleteLike(existingLike);

            }
            if (await likesRepository.SaveChanges()) return Ok();
            return BadRequest("Failed to Update like");

        }

        [HttpGet("list")]
        public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikesIds()
        {
            return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MembersDTO>>> GetUserLikes([FromQuery] LikesParams likesParams)
        {
            likesParams.UserId = User.GetUserId();
            var users = await likesRepository.GetUserLikes(likesParams);
            Response.AddPaginationHeader(users);
            return Ok(users);
        }
    }
}
