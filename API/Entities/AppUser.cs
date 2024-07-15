namespace API.Entities;

/*
    With Entity Framework, the class define the table and the Properties are the column of the table
    The table name is defined in DataContext file (public DbSet<AppUser> Users { get; set; }) So our AppUser table will be name Users
*/
public class AppUser
{

    public int Id { get; set; }
    public required string UserName { get; set; }
}
