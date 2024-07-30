using API.Entities;

namespace API;

// Represents our manually created Join Table, to represent our AppUser Many To Many Realtionship
// Many to Many: An AppUser can be liked by many AppUsers ; An AppUser can like many AppUsers.
// Primary Key for this table will be the combination of the two users
public class UserLike
{
    /*
    Source User: The user that is doing the liking
    Target User: The user that is receving the like 
    */
    public AppUser SourceUser { get; set; } = null!;    // Null forgiving operator (for nullable reference types)
    public int SourceUserId { get; set; }
    public AppUser TargetUser { get; set; } = null!;
    public int TargetUserId { get; set; }

}

// In this table we will have a foreign key sourceUserId, that references an app user 
// In this table we will have a foreign key TargetUserId, that reference an app user.
