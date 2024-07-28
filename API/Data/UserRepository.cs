using API.Data;
using API.DTOs;
using API.Entities;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

public class UserRepository(DataContext context, IMapper mapper) : IUserRepository
{
    public async Task<MemberDto?> GetMemberAsync(string username)
    {
        // From Users table find the AppUser that has the same username as our method parameter, map it to a memberDto using our configuration which is found in AutoMapperProfiles.cs
        return await context.Users
            .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(mapper.ConfigurationProvider) //ProjectTo comes from automapper and it is a queryable extension
            .SingleOrDefaultAsync();    //Returns the mapped to memberDto if a matching username is found else will return null
    }

    public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
    {
        // Deferred Execution: Query is not executed immeditaly but is constructed as an expression tree, query only gets executed when the results are enumerated 
        // Start with all users as an IQueryable
        var query = context.Users.AsQueryable();   // query needs to be of type Queryable if we want to use a where clause

        // Filter out the current user
        query = query.Where(x => x.UserName != userParams.CurrentUser);

        // Add gender filtering to query if it exists
        if (userParams.Gender != null)
        {
            query = query.Where(x => x.Gender == userParams.Gender); 
        }

        // Age Filtering:
        // If someone was 100, the minDob of someone who is 100 years old in 2024 would be, 1924 
        var minDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MaxAge - 1));    // Adding a neg number = subtraction, -1 if the user hasn't had their bday yet this year
        var maxDob = DateOnly.FromDateTime(DateTime.Today.AddYears(-userParams.MinAge));

        // Add age filter to query
        query = query.Where(x=> x.DateOfBirth >= minDob && x.DateOfBirth <= maxDob);

        // Add sorting to query
        query = userParams.OrderBy switch
        {
            // if userParams.OrderBy == "created" our query will sort by the created property, else it will sort by the LastActive Property
            "created" => query.OrderByDescending(x => x.Created),
             _ => query.OrderByDescending(x => x.LastActive)
        };


        
        // Project the filtered query (.CreateAsync is what actually executes our query) to MemberDto and created a paginated list
        return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(mapper.ConfigurationProvider), userParams.PageNumber, userParams.PageSize);
    }

    public async Task<AppUser?> GetUserByIdAsync(int id)
    {
        return await context.Users.FindAsync(id);
    }

    public async Task<AppUser?> GetUserByUsernameAsync(string username)
    {
        return await context.Users
        .Include(x => x.Photos)
        .SingleOrDefaultAsync(x => x.UserName == username);
    }

    public async Task<IEnumerable<AppUser>> GetUsersAsync()
    {
        // Including the Photos from the photos table in our response
        return await context.Users
            .Include(x => x.Photos)
            .ToListAsync();
    }

    public async Task<bool> SaveAllAsync()
    {
        // .SaveChangesAsync() returns a int for the number of changes saved, so if greater than 0 there have been changes in db so it will return true
        return await context.SaveChangesAsync() > 0;
    }

    public void Update(AppUser user)
    {
        context.Entry(user).State = EntityState.Modified; // Explicitly telling Entity Framework that the row associated with our AppUser entity has been modified.
    }
}
