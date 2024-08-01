using API.Entities;

namespace API.Interfaces;

// Interfaces are a form of abstraction
public interface ITokenService
{
    // Need to make CreateToken an async task because it is accessing our DB so we change the return type to Task so we can use the async keyword
    Task<string> CreateToken(AppUser user);
}
