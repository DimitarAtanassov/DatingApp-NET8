using API.Data;
using API.DTOs;
using API.Helpers;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API;

// Dependency Injection
public class LikesRepository(DataContext context, IMapper mapper) : ILikesRespository
{
    public void AddLike(UserLike like)
    {
        context.Likes.Add(like);
    }

    public void DeleteLike(UserLike like)
    {
        context.Likes.Remove(like);
    }

    public async Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId)
    {
        // Gives us the Target User Ids that the current user has liked.
        return await context.Likes
            .Where(x => x.SourceUserId == currentUserId)
            .Select(x => x.TargetUserId)
            .ToListAsync();
    }

    public async Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId)
    {
        // FindAsync needs the primary key for the entity to be found
        // Our Primary Key consists of two columns in this table which are SourceUserId and TargetUserId
        return await context.Likes.FindAsync(sourceUserId,targetUserId);
    }

    // userId: The Id of the user for whom we are retrieving likes
    public async Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams)
    {
        var likes = context.Likes.AsQueryable();
        IQueryable<MemberDto> query;

        switch (likesParams.Predicate)
        {
            case "liked":
                query = likes
                    .Where(x => x.SourceUserId == likesParams.UserId)
                    .Select(x => x.TargetUser)  // Only intrested in getting the users the current user has liked.
                    // At this point we are working with a type of AppUser and want to map it to a MemberDto so we use auto mapper.
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
            
            case "likedBy":
               query = likes
                    .Where(x => x.TargetUserId == likesParams.UserId)
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
            
            default:
                // Mutual Likes
                // First get a list of all the users the current user has liked.
                var likeIds = await GetCurrentUserLikeIds(likesParams.UserId);  // GetCurrentUserLikeIds returns a list of TargetUserIds that the current user has liked.

                query = likes
                /*
                    Where caluse filters the likes table to find rows where TargetUserId equals the current user's Id (userId). So this means our current user is liked by the targetId
                    And then checks if the sourceUserId (the user who likes the current user) is in the list of users that the current user has liked 
                */
                    .Where(x => x.TargetUserId == likesParams.UserId && likeIds.Contains(x.SourceUserId))
                    .Select(x => x.SourceUser)
                    .ProjectTo<MemberDto>(mapper.ConfigurationProvider);
                break;
        
        }
        return await PagedList<MemberDto>.CreateAsync(query, likesParams.PageNumber, likesParams.PageSize);      
    }

    public async Task<bool> SaveChanges()
    {
        return await context.SaveChangesAsync() > 0;
    }
}
