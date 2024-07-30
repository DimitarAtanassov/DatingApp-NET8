using API.DTOs;
using API.Helpers;

namespace API;

public interface ILikesRespository
{
    Task<UserLike?> GetUserLike(int sourceUserId, int targetUserId);

    // Users will have the option to select: the users that liked them, the users they liked, and mutual likes.
    Task<PagedList<MemberDto>> GetUserLikes(LikesParams likesParams);
    
    // Will use this so on the client side we can show which users the current user has liked.
    Task<IEnumerable<int>> GetCurrentUserLikeIds(int currentUserId);

    void DeleteLike(UserLike like);
    void AddLike(UserLike like);
    Task<bool> SaveChanges();
}
