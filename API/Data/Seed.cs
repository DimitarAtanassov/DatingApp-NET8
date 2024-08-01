using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class Seed
{
    // B/c this is static we do not need to create an Seed class object in order to use this method

    // Does not use constructor dependency injection so we need to add these services (UserManager and Role Manager services) to program.cs inorder for them to work here.
    public static async Task SeedUsers(UserManager<AppUser> userManager, RoleManager<AppRole> roleManager)    // UserManager gives us access to our db like dbcontext but with additional features.
    {
        // If there are any Users this will be true, we do this to check if we have any users in our database before seeding additional data in our database, this will avoid duplicating users
        if (await userManager.Users.AnyAsync()) return;

        // Get data from json file (In order for this to work make sure that we have all the required properties of AppUser listed inside of our json objects)
        var userData = await File.ReadAllTextAsync("Data/UserSeedData.json");

        // Deseralize the JSON data so we can use the data as C# objects
        var options = new JsonSerializerOptions{PropertyNameCaseInsensitive=true};

        // Stores AppUsers, by deserializing the data into a List of AppUsers
        var users = JsonSerializer.Deserialize<List<AppUser>>(userData, options);

        if (users == null) return;


        // Populating example roles for our seeded users (we are going to use rolemanager to loop over these roles and actually create them)
        var roles = new List<AppRole>
        {
            new() {Name = "Member"},
            new() {Name = "Admin"},
            new() {Name = "Moderator"}
        };

        // RoleManager and UserManager are like DbContext and let us access our tables in the db, here we use our roleManager while looping over role and create them in our table.
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }
        
        // Loop through the users and create the Password hash and password salt of each AppUser. 
        foreach (var user in users)
        {
            // using var hmac = new HMACSHA512();  // using keyword makes sure each HMAC instance is properly disposed of after it is used within each iteration of the loop, so it also creates a new instance for each user that will have a different salt :D
            

            // user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Pa$$w0rd"));   //Pa$$w0rd is acting like the string to user entered for their password when they registered.
            // user.PasswordSalt = hmac.Key;   // Get the salt (key) which was used during hashing in the line above 

            // context.Users.Add(user);    // At this point: Entityframe work is tracking this user entity but it is still not saved into our Database.
            user.UserName = user.UserName!.ToLower();
            await userManager.CreateAsync(user, "Pa$$w0rd");       //Create Async will create the specified user in the backing store, overloading with a default password for our seeded users (these are users to test our app so its ok)
            
            // Giving the newly created user a role
            await userManager.AddToRoleAsync(user, "Member");
        }

        // Creating an Admin User. 
        var admin = new AppUser
        {
            UserName = "admin",
            KnownAs = "Admin",
            Gender = "",        // By setting the gender to nothing, the Admin User will not show up in results that are returned to client, for list of members
            City = "",
            Country = ""
        };
        await userManager.CreateAsync(admin, "Pa$$w0rd");    
        
        // Adding our newly created Admin User to roles
        await userManager.AddToRolesAsync(admin, ["Admin", "Moderator"]);
        
        // No longer need to save data because .CreateAsync (above) will create user and save changes for us.
        // await context.SaveChangesAsync(); // Now we save the data

    }
}
