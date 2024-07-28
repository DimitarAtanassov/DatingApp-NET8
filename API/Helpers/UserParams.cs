namespace API;

/*

We will use this class as a helper class for pagination implementation:
 - We need the client to tell us what page they want and what page size they want. This class will help us with this.

When it comes to pagination you do not want to give the user an unlimited amout that they could return.
 - Instead, we set a default maximum, and then let them choose from a number of page sizes. 

*/
public class UserParams
{
    private const int MaxPageSize = 50; // Max number of items per page

    public int PageNumber { get; set; } = 1; //Default page number

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }// value refers to the value that is being passed in by the user when the user calls .set on this property.
    
    // Filters
    public string? Gender { get; set; } // User on client side will be able to select what gender they want to filter by
    
    public string? CurrentUser { get; set; } // This filter will not be used by the user directly, it will be used to exclude them from the result that we return back, so the user can't see themselves in the list.
    
    public int MinAge { get; set; } = 18;
    
    public int MaxAge { get; set; } = 100;
    
    // Sorting Filter
    public string OrderBy { get; set; } = "lastActive";
    
}
