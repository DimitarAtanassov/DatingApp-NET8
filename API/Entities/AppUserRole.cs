using API.Entities;
using Microsoft.AspNetCore.Identity;

namespace API;

// Join Table between AppUser and AppRole Entities
public class AppUserRole : IdentityUserRole<int>
{
    public AppUser User { get; set; } = null!;
    public AppRole Role { get; set; } = null!;
    
}
