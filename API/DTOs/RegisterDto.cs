using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
/*
    Inside this class all we need to do is specify the parameters we are expecting (From the body of our HTTP request)
*/
public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}
