using API.Entities;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AutoMapper;
using API.DTOs;

namespace API.Controllers;
[Authorize]
public class UsersController(IUserRepository userRepository) : BaseApiController
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
}