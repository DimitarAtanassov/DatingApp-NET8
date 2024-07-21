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
public class UsersController(IUserRepository userRepository) : BaseApiController 
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

}
