using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

[ApiController]
[Route("api/[controller]")] //api/users ([controller] is replaced with the first part of our Controller class name so Users)

//Primary constructor syntax
public class UsersController(DataContext context) : ControllerBase
{
    

    // This Creats an HttpGet EndPoint, then we create a method that we will use to return http response to client (We are returning an ActionResult from our Api Endpoint that contains a collection)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
    {
        // Gets List of users from Database
        var users = await context.Users.ToListAsync();
        
        return users;   // Because we use ActionResult this will return an HTTP ok response (200) for return type of IEnumerable<AppUser>
    }

    [HttpGet("{id:int}")]   // /api/users/3  Adding a route parameter id of type int
    public async Task<ActionResult<AppUser>> GetUser(int id)
    {
        
        var user = await context.Users.FindAsync(id);
        
        if (user == null) return NotFound(); // 404 Not Found
        return user; 
    }

}
