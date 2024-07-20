/*
    We will use this as our Error Handling Middleware, so that any exceptions generated anywhere in our application can be caught by our middleware
    This way we will also have a standaredized error response for both Production and Development Enviornments  
*/

using System.Net;
using System.Text.Json;
using API.Errors;

namespace API;
// IHostEnvironment is used to see if we are running in production or development mode
public class ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment env)
{
    // When we are using middleware we have to have a method named InvokeAsync b/c that is what is being expected when we call the next() method in the Middleware pipeline wirh WebApplications (As per microsft.com)
    // This function is middleware and we are working with the HTTP request so we have access to the HttpContext (which is the request object)
    public async Task InvokeAsync(HttpContext context)  
    {
        try
        {
            // Simply moving on to the next request delegate, we are not going to touch the HTTP request (conext) unless there is an exception 
            await next(context);
        }
        catch (Exception ex)
        {
            
            logger.LogError(ex, ex.Message);    // This will log out to our terminal
            
            // context.Response is our http response
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // If enviornment is in Development Mode we create a new ApiException with the stack trace.
            // If enviornment is in Production Mode we create a new ApiException without the stack trace.
            var response = env.IsDevelopment() ? new ApiException(context.Response.StatusCode, ex.Message, ex.StackTrace)
            : new ApiException(context.Response.StatusCode, ex.Message, "Internal Server Error");  

            // Now we need to format the response to be in JSON so we can work with it on our Client - Side
            // Configure the JSON serializer options 
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            // Convert the response to json using our options
            var json = JsonSerializer.Serialize(response,options);

            // Write the json to our body of the request response 
            await context.Response.WriteAsync(json);

            // Add this middleware to your application middleware in Program.cs

        }
    }
}
