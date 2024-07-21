namespace API.Extensions;
// Rather than requiring the DateOfBirth on the client side we want to display the age of the user in the client, so we need to Calculate age from DOB
// This file will contain methods like the one above, and we call these types of methods extension methods
// Extension methods need to be inside of a static class
public static class DateTimeExtensions
{
    /* 
    'this' keyword in the parameter is used to define an extension method in C#
    Extension Methods allow you to "add" methods to existing types without modifying the original type or creating a new derived type.
    By using 'this' we are specifying that the method is an extension method for the type specified(in this case, 'DateOnly')
    Ex:    
        DateOnly birthDate = new DateOnly(1990, 7, 21);
        int age = birthDate.CalculateAge();
    */ 
    
    public static int CalculateAge(this DateOnly dob)
    {
        // We are not accounting for leap years 
        // Get todays date
        var today = DateOnly.FromDateTime(DateTime.Now);

        var age = today.Year - dob.Year;    // Doesn't acount for the fact that a user has had their brithday this year or is yet to have their birthday this year

        // .AddYears(-age) Computes the date on which the person had their birthday this year. Subtracts age from the current year to get the birthday of the current year
        // If DOB is greater than the above computation than they have no had their birthday yet this year. 
        if (dob > today.AddYears(-age)) age--;  // Have not had their birthdays this year

        return age;

    }

}
