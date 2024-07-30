namespace API.Entities;
using API.Extensions;
/*
    With Entity Framework, the class define the table and the Properties are the column of the table
    The table name is defined in DataContext file (public DbSet<AppUser> Users { get; set; }) So our table will be name Users and each entity in this table will be of type AppUser
*/
public class AppUser
{
    public int Id { get; set; }
    public required string UserName { get; set; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt {get; set;} = [];
    public DateOnly DateOfBirth { get; set; }
    public required string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;    // Postgres and other DB technologies require utc dates, and the client browser always converts utc date to local time. 
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public required string Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public required string City { get; set; }
    public required string Country { get; set; }
    public List<Photo> Photos { get; set; } = []; // This is refered to as Navigation Property
    public List<UserLike> LikedByUsers { get; set; } = []; //List of users the current user is liked by
    public List<UserLike> LikedUsers { get; set; } = [];    //List of users the current user has liked
    
    /*

    We have a list of Photos in our AppUser Entity, A Photo is another Entity
    Entity Framework by conventions is automatically going to recognize this as a realtionship between our AppUser and Our Photos
    (in db terminology) One User can have many photos , this is called a one to many realtionship
    Entity Framework will recognize this and by convention it will create a database table for our Photos as well.

    We can say Photos is a navigation property inside of our AppUser class
    Navigation Property: A property in an entity class that allows navigation from one entity to another.

    */


}
