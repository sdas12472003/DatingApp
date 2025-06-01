using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace API.Controllers;
public class AccountController(UserManager<AppUser> userManager,ITokenService tokenService,IMapper mapper) :BaseApiController
{
    [HttpPost("register")]//account/register
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if(await UserExists(registerDto.Username))
        {
            return BadRequest("Username is taken");
        }
        
        // using var hmac=new HMACSHA512();
        var user=mapper.Map<AppUser>(registerDto);
        user.UserName=registerDto.Username.ToLower();
        // user.PasswordHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        // user.PasswordSalt=hmac.Key;

        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);
        
        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        return new UserDto
        {
            Username = user.UserName,
            Token =await tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender

        };
    }
    [HttpPost("login")]
    public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
    {
        var user= await userManager.Users
            .Include(p=>p.Photos)
                .FirstOrDefaultAsync(x=>x.NormalizedUserName==loginDto.Username.ToUpper());
        if(user==null || user.UserName==null)
            return Unauthorized("Invalid username");

        var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!result) return Unauthorized();
        // using var hmac=new HMACSHA512(user.PasswordSalt);
        // var computeHash=hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
        // for(int i=0; i<computeHash.Length; i++)
        // {
        //     if(computeHash[i]!=user.PasswordHash[i])
        //         return Unauthorized("Invalid password");
        // }
        return new UserDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token =await tokenService.CreateToken(user),
            Gender = user.Gender,
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain)?.Url
        };

    }
    private async Task<bool> UserExists(string username)
    {
        return await userManager.Users.AnyAsync(x=>x.NormalizedUserName==username.ToUpper());
    }
}