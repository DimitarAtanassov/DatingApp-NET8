using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    // B/c this is static we do not need to create an Seed class object in order to use this method
    public static async Task SeedUsers(DataContext context)
    {
        // If there are any Users this will be true, we do this to check if we have any users in our database before seeding additional data in our database, this will avoid duplicating users
        if (await context.Users.AnyAsync()) return;

        // Get data from json file (In order for this to work make sure that we have all the required properties of AppUser listed inside of our json objects)
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        // Deseralize the JSON data so we can use the data as C# objects
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};

        // Stores AppUsers, by deserializing the data into a List of AppUsers
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;
        
        // Loop through the users and create the Password hash and password salt of each AppUser. 
        foreach (var user in users)
        {
            using var hmac = new HMACSHA512();  // using keyword makes sure each HMAC instance is properly disposed of after it is used within each iteration of the loop, so it also creates a new instance for each user that will have a different salt :D
            user.UserName = user.UserName.ToLower();

            user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));   //Pa$$w0rd is acting like the string to user entered for their password when they registered.
            user.PasswordSalt = hmac.Key;   // Get the salt (key) which was used during hashing in the line above 

            context.Users.Add(user);    // At this point: Entityframe work is tracking this user entity but it is still not saved into our Database.

        }
        await context.SaveChangesAsync(); // Now we save the data

    }
}
