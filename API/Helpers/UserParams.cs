namespace API.Helpers;

/*

We will use this class as a helper class for pagination implementation:
 - We need the client to tell us what page they want and what page size they want. This class will help us with this.

When it comes to pagination you do not want to give the user an unlimited amout that they could return.
 - Instead, we set a default maximum, and then let them choose from a number of page sizes. 

*/
public class UserParams : PaginationParams
{

    
    // Filters
    public string? Gender { get; set; } // User on client side will be able to select what gender they want to filter by
    
    public string? CurrentUser { get; set; } // This filter will not be used by the user directly, it will be used to exclude them from the result that we return back, so the user can't see themselves in the list.
    
    public int MinAge { get; set; } = 18;
    
    public int MaxAge { get; set; } = 100;
    
    // Sorting Filter
    public string OrderBy { get; set; } = "lastActive";
    
}
