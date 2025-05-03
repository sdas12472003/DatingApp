using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using API.DTOs;
using System.Security.Claims;

namespace API.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository,IMapper mapper) : BaseApiController
{


    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
    {
        var users= await userRepository.GetMembersAsync();
        
        return Ok(users);
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<MemberDto>> GetUsers(string username)
    {
        var user=await userRepository.GetMemberAsync(username);
        if(user==null)
        {
            return NotFound();
        }
        return user;
    }
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
    {
        var username=User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if(username==null) return BadRequest("No username found in token");

        var user=await userRepository.GetUserByUsernameAsync(username);

        if(user==null) return BadRequest("Could not find user");

        mapper.Map(memberUpdateDto,user);

        userRepository.Update(user);
        
        if(await userRepository.SaveAllAsync()) return NoContent();
        return BadRequest("Failed to updated the user");
    }
}