// This controller would no be built on a regular app, this is just to demonstrate the different types of errors we can encounter, and different kinds of ways we can handle these errors
// This Contorller: Will return errors to the Client
using API.Controllers;
using API.Data;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API;

public class BuggyController(DataContext context) : BaseApiController
{
    [Authorize]     //Going to be testing authentication errors, so we set this as a protected route
    [HttpGet("auth")]
    public ActionResult<string> GetAuth()
    {
        return "secret text";
    }

    [HttpGet("not-found")]
    public ActionResult<AppUser> GetNotFound()
    {
        var thing = context.Users.Find(-1); // Will look for an ID -1 which ofc does not exist

        if (thing == null) return NotFound();   //Returns a 404 Not Found (User Error b/c they have asked to find something that does not exist)

        return thing;
    }



    [HttpGet("server-error")]
    public ActionResult<AppUser> GetServerError()
    {
        // If context.User.Find(-1) returns null our exception will be thrown
        var thing = context.Users.Find(-1) ?? throw new Exception("A bad thing has happened"); //A Bad thing has happened will be shown in the postman platform
        
        return thing; 
    }

    // Bad Request is usually a HTTP 400 Type Response (Any errors in the 400-499 range are considered User Side Errors)
    [HttpGet("bad-request")]
    public ActionResult<string> GetBadRequest()
    {
        return BadRequest("This was not a good request");
    }
}
