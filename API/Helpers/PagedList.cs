using Microsoft.EntityFrameworkCore;

namespace API.Helpers;
// Pagination Helper Class
public class PagedList<T> : List<T>
{
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; } // Total number of pages required to display all items
    public int PageSize { get; set; }
    public int TotalCount { get; set; } // Total number of items in our database

    public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalPages = (int) Math.Ceiling(count / (double) pageSize); // Count is number of results
        PageSize = pageSize;
        TotalCount = count;
        AddRange(items);

    }

    /*
        pageSize: Total number of items to be displayed per page
        pageNumber: The page the user wants to fetch
        We are implementing pagination using zero based indexing
        

    */
    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        
        /* .Skip is used to calculate how many data items we want to skip 
            - We do pageNumber - 1 for zero based indexing
            - And then we multiply by pageSize 
            - offset = (pageNumber - 1) * pageSize
           .Take is used to get the elements and we use .ToListAsync to execute our query which is the source
        */
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PagedList<T>(items,count, pageNumber, pageSize);
    }
}
