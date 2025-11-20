namespace FreshChoice.Services.Identity.Constants;

public class DefaultRoles
{
    public const string Admin = "Admin"; 
    public const string Employee = "Employee";
    public const string Manager = "Manager";
    
    public static readonly string[] AllRoles = { Admin, Employee, Manager };
}