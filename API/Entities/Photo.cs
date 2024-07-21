using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities;

[Table("Photos")]   // We are not creating a DbSet for Photos in our DataContext.cs, so we use this DataAnnotation Attribute to tell entity framework that we want this table to be called Photos.
public class Photo
{
    public int Id { get; set; }
    public required string Url { get; set; }
    public bool IsMain { get; set; }
    public string? PublicId { get; set; }

    //Navigation properties for a required One to Many Relationship
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; } = null!;

    /*
        null! This is the null forgiving operator
        In Entity Framework documentation:
        public int AppUserId { get; set; }  : This is the 'Required' foreign key property, by convention we need to use the name of the class (AppUser) followed by Id.
        
        public AppUser AppUser { get; set; } = null!; //We need to add the 'Required' reference navigation principal, in this case AppUserId is a foreign key that references the AppUser Id in our AppUser table
        - We cannot make this required by using the keyword because entity framework will throw an exception, and we can't initializes it with anything either but it does need to be null and required

        What this is saying overall,
        In our Photos table we will have a foreign key called AppUserId, that references an Id found in the AppUser table.
    */
}