using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;


public class DataContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
    public DbSet<UserLike> Likes { get; set; }  // This is a relationship table we are configuring for EF, rather than EF configuring for us, we need to give it some config

    // Overriding Entity Framework conventions:
    protected override void OnModelCreating(ModelBuilder builder)  // When we create a migration, it takes a look at this configuration
    {
        base.OnModelCreating(builder);

        // Configure Below
        builder.Entity<UserLike>()
        .HasKey(k => new {k.SourceUserId, k.TargetUserId}); //Primary Key.

        // Configuring the two sides of our relationship:
        builder.Entity<UserLike>()
        .HasOne(s => s.SourceUser)
        .WithMany(l => l.LikedUsers)
        .HasForeignKey(s => s.SourceUserId)
        .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<UserLike>()
        .HasOne(s => s.TargetUser)
        .WithMany(l => l.LikedByUsers)
        .HasForeignKey(s => s.TargetUserId)
        .OnDelete(DeleteBehavior.Cascade);
    }

    /*
        We know that we will store photo in its own table
        Does this mean we need to add a DbSet for the Photos table?
        - We can but it is not required
        - We add a DbSet when we want to query that entity
        - We can add our Photos as a DbSet inside of here but it depends on how we want to make the queries:
            - Do we need to query the Photo Entity directly?
            - Is there a point in where we will need to get a specific photo based on its Id?
            - And for this app the answer is no
            - So we do not need to create a DbSet and can leave it to entity framework conventions to create the appropriate Table 
        Inside our data context, we know we have a Navigation Property from AppUsers to our Photos, therefore we can access our photos by querying Users.photos

        When we use DbSet<AppUser> Users entity framework uses the name of the DbSet as the Table Name
        But if we do not specify DbSets for our Entities, Entity Framework will use the Entity Name as the table name, so if we didn't have a DbSet for AppUser called Users the table name would be AppUser
        For Photos since we do not have a DbSet it will use the class name for our photo entity which is Photo, but we want the names of our tables to be plural.
        When conventions don't work for us we can use attributes or annotations to tell Entity Framework what we want

    */

}
