using API.Extensions;
var builder = WebApplication.CreateBuilder(args);

// Add services to container
//We are extending the builder.Services in ApplicationServiceExtentions so we do not need to pass it in
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// MIDDLEWARE START (!Ordering of middleware is important)
// Cross Origin Resource Sharing middleware  so we can run our app locally on HTTPS
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().
WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// MIDDLEWARE END

app.Run();
