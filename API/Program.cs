using API;
using API.Data;
using API.Extensions;
using Microsoft.EntityFrameworkCore;
var builder = WebApplication.CreateBuilder(args);

// Add services to container
//We are extending the builder.Services in ApplicationServiceExtentions so we do not need to pass it in
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// MIDDLEWARE START (!!!!!!Ordering of middleware is important)
// Error/Exception Handling Middleware needs to go at the very top
app.UseMiddleware<ExceptionMiddleware>();
// Cross Origin Resource Sharing middleware  so we can run our app locally on HTTPS
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().
WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// MIDDLEWARE END

// Logic for creating and applying migrations in our code and seed the users in our database

// We need to access a service and we are not injecting anything into this file/class. 
// So we need to use a pattern called Service Locator, to get a hold of a service we want to use outside of dependacy injection 
using var scope = app.Services.CreateScope(); // We create a scope (for dependancy injection inside of program class) and we want this scope to be disposed after we are finished with it so we add the using keyword
var services = scope.ServiceProvider; // Used to reslove the required services for the operations within the scope above
// Adding error handling here because we are inside of our program class which means we will not get the benfit of our Exception Middleware we created earlier.
try
{
    var context = services.GetRequiredService<DataContext>();  //Getting the DataContexet service 
    await context.Database.MigrateAsync();  // Applies migrations and creates database if it hasn't been created already
    await Seed.SeedUsers(context);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}


app.Run();
