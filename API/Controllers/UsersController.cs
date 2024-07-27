using System.Security.Claims;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Authorize] // All endpoints in this controller are now authenticated / protected endpoints
//Primary constructor syntax (Constructor Dependancy injection) Route : api/users ([controller] is replaced with the first part of our Controller class name so Users), we inherit from BaseApi controller that wehre api/ comes from
public class UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService) : BaseApiController 
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
       // Get user with Entity Framework (EF is tracking this user now)
       // User.GetUsername(): Gets the username of the user we want to get from our database,
       // We can get the username from the bearer token our user sends up to get authenticated, bc this is a protected route.
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        if (user == null) return BadRequest("Could not find user");

        // Here we update the user object with mapper.Map, we map the properties from MemberUpdateDto into the user properties
        mapper.Map(MemberUpdateDto, user);

        // Save Changes
        if (await userRepository.SaveAllAsync()) return NoContent();

        return BadRequest("Failed to update the user");
    }

    
    /*

        Our photo after we have uploaded it will have a public ID
        So we are going to return this to the client as well

    */
    [HttpPost("add-photo")]
    public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        if (user == null) return BadRequest("Cannot update user");

        var result = await photoService.AddPhotoAsync(file);

        // result has type CloudinaryResponse so we can use .Error to check if there was an error while adding the photo to cloudinary
        if (result.Error != null) return BadRequest(result.Error.Message);

        // Storing the information returned from our Cloudinary photo to a Photo Object we created
        var photo = new Photo
        {
            Url = result.SecureUrl.AbsoluteUri,
            PublicId = result.PublicId
        };

        if (user.Photos.Count == 0) photo.IsMain = true;

        // Adding to the user.Photos Property which is of type List<Photo>; so we are adding the photo to the user's photolist
        user.Photos.Add(photo);

        // Updating Database with the new entity information
        if (await userRepository.SaveAllAsync())
        {
            // Each Method Name in this class is an Action "AddPhoto" "UpdateUser" etc
            /*
            CreatedAtAction: 
            Returns Status Code: 201 Created
            Sets the location header of our response to the location of the photo, in this case the location in is in the users object itself, nested in List<Photo>    
            */
            return CreatedAtAction(nameof(GetUser), 
            new {username = user.UserName}, mapper.Map<PhotoDto>(photo));
        }

        return BadRequest("Problem adding photo");
    }

    [HttpPut("set-main-photo/{photoId:int}")]
    public async Task<ActionResult> SetMainPhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());

        if (user == null) return BadRequest("Could not find user");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain ) return BadRequest("Cannot use this as main photo");

        var currentMain = user.Photos.FirstOrDefault(x => x.IsMain);

        if (currentMain != null) currentMain.IsMain = false;
        photo.IsMain = true;

        if (await userRepository.SaveAllAsync()) return NoContent();    // HTTP PUT returns no content

        return BadRequest("Problem setting main photo");

    }

    [HttpDelete("delete-photo/{photoId:int}")]
    public async Task<ActionResult> DeletePhoto(int photoId)
    {
        var user = await userRepository.GetUserByUsernameAsync(User.GetUsername());
        
        if (user == null) return BadRequest("User not found");

        var photo = user.Photos.FirstOrDefault(x => x.Id == photoId);

        if (photo == null || photo.IsMain) return BadRequest("This photo cannot be deleted");

        if (photo.PublicId != null)
        {
            var result = await photoService.DeletePhotoAsync(photo.PublicId);
            if (result.Error != null) return BadRequest(result.Error.Message);
        }

        user.Photos.Remove(photo);

        if (await userRepository.SaveAllAsync()) return Ok();

        return BadRequest("Problem Deleting photo");
    }



}
