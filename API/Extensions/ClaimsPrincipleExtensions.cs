using System.Security.Claims;

namespace API.Extensions;

/*
    Purpose of this class is to streamline the process of getting a username from a jwt claim in order to find and be able to use a user in from database in our code
*/
public static class ClaimPrincipleExtensions
{
    // We are extending the User Object has a type of "ClaimsPrinciple" and this is going to be an "extension method" of this object.
    public static string GetUsername(this ClaimsPrincipal user)
    {
        // We can get a hold of our user here, because all of these routes require authorization, so the user sends up their bearer token, which contains the claims about who the user is 
        //So we use FindFirstValue and this Returns the claim value for the first claim with the specified type which is the username of the user. 
        // // We specified the Name of the Claim to store our user's username in the TokenService
        //  ?? null coalescing operator, so if the return from FindFirstValue call is null, we will throw the exception
        var username = user.FindFirstValue(ClaimTypes.Name) ?? throw new Exception("Cannot get username from token");
        return username;

    }

        public static int GetUserId(this ClaimsPrincipal user)
    {
        var userId = int.Parse(user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("Cannot get username from token")); // string -> int conversion
        return userId;

    }
}