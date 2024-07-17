namespace API;

// The properties of this classes are what are returned by our request
public class UserDto
{
    public required string Username { get; set; }

    public required string Token { get; set; }
}
