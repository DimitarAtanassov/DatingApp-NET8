﻿using System.Security.Cryptography;
using System.Text;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API.DTOs;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Identity;

namespace API.Controllers;

// Primary constructor dependancy injectiion + inheritance
public class AccountController(UserManager<AppUser> userManager, ITokenService tokenService, IMapper mapper) : BaseApiController
{
    [HttpPost("register")] // api/account/register: api/ comes from BaseApiController; /account/ comes from this controller, register is the final portion which we set in HttpPost attribute
    // The Properties of what ever Our Task<ActionResult<>> wraps is what is returned in our http request because of our return statement
    // Because we are passing an object as a parameter, this endpoint will default to looking at the body of the request to find information it needs, by convention.
    public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
    {
        if (await UserExists(registerDto.Username)) return BadRequest("Username is taken");
        

        // Using automapper to create new user with registerDto info
        var user = mapper.Map<AppUser>(registerDto);    // Map into AppUser from registerDto

        // Setting the registerDto making username lower case in user object and registerDto does not have PasswordHash and PasswordSalt so we are setting them 
        user.UserName = registerDto.Username.ToLower();
        // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password));
        // user.PasswordSalt = hmac.Key;

        // context.Users.Add(user);
        // await context.SaveChangesAsync();
        var result = await userManager.CreateAsync(user, registerDto.Password);
        if (!result.Succeeded) return BadRequest(result.Errors);

        return new UserDto
        {
            Username = user.UserName,
            Token = await tokenService.CreateToken(user),
            KnownAs = user.KnownAs,
            Gender = user.Gender
        };
        
    }

    [HttpPost("login")] // account/login
    public async Task<ActionResult<UserDto>> Login(LoginDto  loginDto)
    {
        var user = await userManager.Users
        .Include(p => p.Photos)
        .FirstOrDefaultAsync(x => 
        x.NormalizedUserName == loginDto.Username.ToUpper());

        if (user == null || user.UserName == null) return Unauthorized("Invalid Username");

        // using var hmac = new HMACSHA512(user.PasswordSalt); // Using the same key as when the user registered so when we compute hash we can validate 

        // var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

        // for (int i = 0; i < computedHash.Length; i++)
        // {
        //     if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
        // }

        return new UserDto
        {
            Username = user.UserName,
            KnownAs = user.KnownAs,
            Token = await tokenService.CreateToken(user),
            PhotoUrl = user.Photos.FirstOrDefault(x => x.IsMain = true)?.Url,
            Gender = user.Gender
        };
    }

    private async Task<bool> UserExists(string username)
    {
        // context is a DataContext type which lets our program now we are working with the Users Table 
        // .NET Identity provides a Normalized Username column in our users table and the normalized version is all uppercase
        return await userManager.Users.AnyAsync(x => x.NormalizedUserName == username.ToUpper());
    }

}
