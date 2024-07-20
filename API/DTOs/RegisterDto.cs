using System.ComponentModel.DataAnnotations;

namespace API.DTOs;
/*
    Inside this class all we need to do is specify the parameters we are expecting (From the body of our HTTP request)
*/
public class RegisterDto
{
    [Required]
    public string Username { get; set; } = string.Empty;
    
    [Required]
    [StringLength(8, MinimumLength = 4)]    // [StringLength(8, MinimumLength = 4)]  Sets maxlength for a password to 8, min length for a password to 4
    public string Password { get; set; } = string.Empty;
}

/* 
We moved  public required string Password { get; set; } the 'required' and kept it an an attirbute of the property ([Required])
This is because the message we saw on the client side with a bad request, was not a really helpful message
From Postman:
 {
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "$": [
            "JSON deserialization for type 'API.DTOs.RegisterDto' was missing required properties, including the following: username, password"
        ],
        "registerDto": [
            "The registerDto field is required."
        ]
    },
    "traceId": "00-e888d88607cdf1fafe72359b000b51b4-d5c8806c2dbf16c8-00"
}

Now when we got rid of the required but kept it as an attr, this is what we get back on the client side
From Postman:
{
    "type": "https://tools.ietf.org/html/rfc9110#section-15.5.1",
    "title": "One or more validation errors occurred.",
    "status": 400,
    "errors": {
        "Password": [
            "The Password field is required.",
            "The field Password must be a string with a minimum length of 4 and a maximum length of 8."
        ],
        "Username": [
            "The Username field is required."
        ]
    },
    "traceId": "00-9d6868e5f4976967b1ee5581dbf67ad6-e84cf47403da5011-00"
}

Much more informative
*/

