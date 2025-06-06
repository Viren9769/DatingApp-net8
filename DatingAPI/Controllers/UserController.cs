﻿using AutoMapper;
using DatingAPI.DTOs;
using DatingAPI.Entities;
using DatingAPI.Extensions;
using DatingAPI.Helpers;
using DatingAPI.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace DatingAPI.Controllers;


[Authorize]
public class UserController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController
    {
    // private readonly DataContext _context = context;
    
    [HttpGet]
        public async Task<ActionResult<IEnumerable<MembersDTO>>> GetUser([FromQuery]UserParams userParams)
        {
        userParams.CurrentUsername = User.GetUsername();
            var user = await userRepository.GetAllMembersAsync(userParams);

        Response.AddPaginationHeader(user);

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
       
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("could not find user");
        mapper.Map(memberUpdateDto, user);
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to upddate the user");
    }

    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("Cannot update user");
        var result = await photoService.AddPhotoAsync(file);
        if(result.Error != null) return BadRequest(result.Error.Message);
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };
        if(user.Photos.Count == 0) { photo.IsMain = true; }
        user.Photos.Add(photo);
        if (await userRepository.SaveAllAsync())
            return CreatedAtAction(nameof(GetUser),
                new {username = user.UserName},
                mapper.Map<PhotoDto>(photo));
        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("User cannot find");
        var photo = user.Photos.FirstOrDefault(p => p.Id == photoId);
        if (photo == null) return BadRequest("cannot use this as main photo");
        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);
        if(currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Problem setting main photo");

    }

    [HttpDelete("delete-photo/{photoId}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByNameAsync(User.GetUsername());
        if (user == null) return BadRequest("user not found");
        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);
        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");
        if(photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if(result.Error != null) { return BadRequest(result.Error.Message); } 
        }
        user.Photos.Remove(photo);
        if (await userRepository.SaveAllAsync()) return Ok();
        return BadRequest("Problem deleting photos");
    }

    }

