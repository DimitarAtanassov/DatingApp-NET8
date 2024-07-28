namespace API.Helpers;

/*
    We want to be able to return the pagination results to our client, to do this we will use our header, so the pagination details will be in the response header
*/

// We will use this in a extension, so we can easily add a pagination header to our response
public class PaginationHeader(int currentPage, int itemsPerPage, int totalItems, int totalPages)
{
    public int CurrentPage { get; set; } = currentPage;
    
    public int ItemsPerPage { get; set; } = itemsPerPage;

    public int TotalItems { get; set; } = totalItems;

    public int TotalPages { get; set; } = totalPages;
}
