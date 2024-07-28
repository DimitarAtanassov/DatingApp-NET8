using API.DTOs;
using API.Entities;
using API.Helpers;

namespace API;

public interface IUserRepository
{
    void Update(AppUser user); 
    Task<bool> SaveAllAsync(); // Returns a task that returns a bool, so when we save our changes in our database we will return a boolean saying wether or not something has changed in our database 
    Task<IEnumerable<AppUser>> GetUsersAsync(); // It is common to add Async to the end of any method names that return a task, so that people using this method know they need to await this resposne.
    Task<AppUser?> GetUserByIdAsync(int id);    // optional return type so it can be null
    Task<AppUser?> GetUserByUsernameAsync(string username); 
    Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams);
    Task<MemberDto?> GetMemberAsync(string username);
}
