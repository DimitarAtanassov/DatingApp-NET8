using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
/*
    Inside this class all we need to do is specify the parameters we are expecting (from our body in the http requst?)
*/
public class RegisterDto
{
    [Required]
    public required string Username { get; set; }
    [Required]
    public required string Password { get; set; }
}
