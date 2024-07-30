namespace API.Helpers;

public class PaginationParams
{
    private const int MaxPageSize = 50; // Max number of items per page

    public int PageNumber { get; set; } = 1; //Default page number

    private int _pageSize = 10;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value;
    }// value refers to the value that is being passed in by the user when the user calls .set on this property.
}
