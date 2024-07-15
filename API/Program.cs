using API.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<DataContext>(opt => 
{
    
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

builder.Services.AddCors();

// MIDDLEWARE START
app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().
WithOrigins("http://localhost:4200","https://localhost:4200"));

app.MapControllers();
// MIDDLEWARE END

app.Run();
