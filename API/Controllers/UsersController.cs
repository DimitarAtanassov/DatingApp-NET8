using System.Security.Claims;
using API.Data;
using API.DTOs;
using API.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers;

[Authorize] // All endpoints in this controller are now authenticated / protected endpoints
//Primary constructor syntax (Constructor Dependancy injection) Route : api/users ([controller] is replaced with the first part of our Controller class name so Users), we inherit from BaseApi controller that wehre api/ comes from
public class UsersController(IUserRepository userRepository, IMapper mapper) : BaseApiController 
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()    // This Creats an HttpGet EndPoint, then we create a method that we will use to return http response to client (We are returning an ActionResult from our Api Endpoint that contains a collection)
    {
        // Gets List of users from Database
        var users = await userRepository.GetMembersAsync();

        return Ok(users);  
    }

    [HttpGet("{username}")]   // api/users/username  Adding a route parameter username of type string
    public async Task<ActionResult<MemberDto>> GetUser(string username)
    {
        
        var user = await userRepository.GetMemberAsync(username);
        
        if (user == null) return NotFound(); // 404 Not Found
        
        return user;
    }

    // Update End Point
    [HttpPut]
    public async Task<ActionResult> UpdateUser(MemberUpdateDto MemberUpdateDto)
    {
        // We can get a hold of our user here, because all of these routes require authorization, so the user sends up their bearer token, which contains the claims about who the user is 
        var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;   // We specified the NameIdentifier of the Claim to store our user's username in the TokenService

        if (username == null) return BadRequest("No username found in token");

        // Get user with Entity Framework (EF is tracking this user now)
        var user = await userRepository.GetUserByUsernameAsync(username);

        if (user == null) return BadRequest("Could not find user");

        // Here we update the user object with mapper.Map, we map the properties from MemberUpdateDto into the user properties
        mapper.Map(MemberUpdateDto, user);

        // Save Changes
        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

}
