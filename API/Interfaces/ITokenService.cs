using API.Entities;

namespace API.Interfaces;

// Interfaces are a form of abstraction
public interface ITokenService
{
    string CreateToken(AppUser user);
}
