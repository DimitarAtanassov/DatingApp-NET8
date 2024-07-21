// Helps us shape the data we want to return 
//in this class we define the properties we want to return when a user requests a list of users.
namespace API.DTOs;

public class MemberDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public int Age { get; set; }    // When using auto mapper inside of our AppUser we have a GetAge() function, automapper will use this function to populate the Age property for us, using GetAge name for method lets automapper know Get the age from here.
    public string? PhotoUrl { get; set; } // Return the user's main photo with this
    public string? KnownAs { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string? Gender { get; set; }
    public string? Introduction { get; set; }
    public string? Interests { get; set; }
    public string? LookingFor { get; set; }
    public string? City { get; set; }
    public string? Country { get; set; }
    public List<PhotoDto>? Photos { get; set; }
}
