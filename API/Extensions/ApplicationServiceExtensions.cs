using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

// Extension methods are static
// Extnesions are used to group services which our builder.config uses outside of program.cs and we can import any services in our method useing builder.Services(builder.Configuration) (This is done for cleaning up code)

public static class ApplicationServiceExtensions
{
    // The this keyword is used to define what this static method is extending "We are extending this IServiceCollection"
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddControllers();
        services.AddDbContext<DataContext>(opt =>
        {

            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        // Setting up ITokenSerivce(A service we created) for dependancy injection
        // Common practice to pass in interface and implementation class for service (provides high level abstraction and decoupling)
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<ILikesRespository,LikesRepository>();
        services.AddScoped<LogUserActivity>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        return services;
    }
}
