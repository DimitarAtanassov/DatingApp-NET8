using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using Microsoft.IdentityModel.Tokens;
using API.Interfaces;
using Microsoft.AspNetCore.Identity;
namespace API.Services;

// We need to user UserManager in order to add roles to our JWT token
public class TokenService(IConfiguration config, UserManager<AppUser> userManager) : ITokenService
{
    // Logic for creating a JWT for our user when they login or register
    public async Task<string> CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot Access tokenKey from appsetings");
        // securityAlgorithms.HmacSha512Signature this signature requires a length greater than 64
        if (tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer");
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));

        if (user.UserName == null) throw new Exception("No username of user");

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.UserName)
        };

        // Checking what roles our user is in
        var roles = await userManager.GetRolesAsync(user);

        // Adding user's roles to jwt token (All roles will be populated inside of Role Claim (If theres more than one role this claim will contain an array of the roles, if there is only 1 role it will be a string))
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(7),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        // Writing the token to the response and returning it
        return tokenHandler.WriteToken(token);
    }
}
