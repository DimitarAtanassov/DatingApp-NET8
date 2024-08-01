using API.Controllers;
using API.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API;

public class AdminController(UserManager<AppUser> userManager) : BaseApiController
{
    // Endpoints to test our newly created policies
    
    [Authorize(Policy = "RequireAdminRole")] //RequireAdminRole is the name of a policy we set in IdentityServiceExtension
    [HttpGet("users-with-roles")] //Route with route parm so the route is https://localhost:5001/api/admin/users-with-roles (/api comes from BaseApiController, /admin comes from this controller)
    public async Task<ActionResult> GetUsersWithRoles()
    {
        var users = await userManager.Users
            .OrderBy(x => x.UserName)
            .Select(x => new 
            {
                x.Id,
                Username = x.UserName,
                Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
            }).ToListAsync();
        
        return Ok(users);
    }

    [Authorize(Policy = "RequireAdminRole")]
    [HttpPost("edit-roles/{username}")]
    public async Task<ActionResult> EditRoles(string username, string roles)    // Since roles is a string it will look for the value in the query params of the request.
    {
        if (string.IsNullOrEmpty(roles)) return BadRequest("You must select at least one role");

        var selectedRoles = roles.Split(',').ToArray();

        var user = await userManager.FindByNameAsync(username);

        if (user == null) return BadRequest("User not found");

        // Getting the roles of the user before adding more roles
        var userRoles = await userManager.GetRolesAsync(user);

        // We are updating the roles with the roles we are passing in and the roles the user is already in
        var result = await userManager.AddToRolesAsync(user, selectedRoles.Except(userRoles));

        if (!result.Succeeded) return BadRequest("Failed to add to roles");

        // Removing the old roles of the user so now the user only has the selectedRoles
        result = await userManager.RemoveFromRolesAsync(user, userRoles.Except(selectedRoles));

        if (!result.Succeeded) return BadRequest("Failed to remove from roles");

        // Returning the updated list of roles to the client
        return Ok(await userManager.GetRolesAsync(user));
    }

    [Authorize(Policy = "ModeratePhotoRole")] 
    [HttpGet("photos-to-moderate")]
    public ActionResult GetPhotosForModeration()
    {
        return Ok("Admins or Moderators can see this");
    }
}
