
namespace API.Errors;

// Primary Consturctor
// We make details and optional string (string? lets it be null), and this is because exceptions usually return a stack trace but not always 
public class ApiException(int statusCode, string message, string? details  )
{
    public int StatusCode { get; set; } = statusCode;

    public string Message { get; set; } = message;

    public string? Details { get; set; } = details;
}
