using API.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;

namespace API;

/* 
In our usercontroller we have ActionResults an each action result is a type of action
An Action filter, will let us carry out an action before the action is executed at the endpoint
Or an action filter will let us carry out an action after the action is executed at the endpoint (the return)
So when a user does an action that interacts with any of our api endpoints we will update their last active property
*/

// Using this a service that is scoped to the HTTP Request
public class LogUserActivity : IAsyncActionFilter
{
    // ActionExecutionDelegate next is a function that we will execute, anything we do before this function happens before our action (endpoint) is executed
    // anything after the next function exectution, will happen after the action(endpoint) is executed
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var resultContext = await next();
        
        // Check if user is authenticated
        if (context.HttpContext.User.Identity?.IsAuthenticated != true) return;

        var userId = resultContext.HttpContext.User.GetUserId();

        var repo = resultContext.HttpContext.RequestServices.GetRequiredService<IUserRepository>(); //Getting a hold of repository so we can update an entity in our database

        var user = await repo.GetUserByIdAsync(userId);
        if (user == null) return;
        user.LastActive = DateTime.UtcNow;
        await repo.SaveAllAsync();
    }
}
