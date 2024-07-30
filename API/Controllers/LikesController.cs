using API.Controllers;
using API.DTOs;
using API.Extensions;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace API;

// Inject Repository
public class LikesController(ILikesRespository likesRepository) : BaseApiController
{
    /*
        This method will be a toggle, and it will either:
        Create a resource in our database OR
        Delete a resource in our database
        So even tho we are using an HttpPost request its not necesserialy 100% accurate
    */
    [HttpPost("{targetUserId:int}")]
    public async Task<ActionResult> ToggleLike(int targetUserId)
    {
        // First Get Source User Id: Id of the User who is liking the other user (Which is going to be our current user)
        var sourceUserId = User.GetUserId();

        if (sourceUserId == targetUserId) return BadRequest("You cannot like yourself");
        
        var existingLike = await likesRepository.GetUserLike(sourceUserId, targetUserId);

        if (existingLike == null)
        {
            var like = new UserLike
            {
                SourceUserId = sourceUserId,
                TargetUserId = targetUserId
            };

            likesRepository.AddLike(like);
        }
        else
        {
            // Deleting like
            likesRepository.DeleteLike(existingLike);
        }

        if (await likesRepository.SaveChanges()) return Ok();

        return BadRequest("Failed to update like");
    }

    [HttpGet("list")]
    public async Task<ActionResult<IEnumerable<int>>> GetCurrentUserLikeIds()
    {
        return Ok(await likesRepository.GetCurrentUserLikeIds(User.GetUserId()));
    }


    /*
        This HttpGet endpoint will check for the string predicate in the query string.
        Bc when simple types like int string or bool are used as params in an HTTP GET request
        ASP.NET Core by default looks for these valeus in the query string or route data

        By Default with HTTP POST, when a complex type paramter is passed to an action method ASP.NET core by convention binds this paramter from the body of the request.
        If we pass a simple type paramter like string int bool by default ASP.NET core will look for these parameters in the route data or query string
    */
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MemberDto>>> GetUserLikes([FromQuery]LikesParams likesParams)  
    {
        likesParams.UserId = User.GetUserId();
        var users = await likesRepository.GetUserLikes(likesParams);

        Response.AddPaginationHeader(users);

        return Ok(users);
    }
}
