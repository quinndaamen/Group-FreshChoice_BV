using Microsoft.AspNetCore.Identity;

namespace FreshChoice.Data.Entities;

public class Employee : IdentityUser<Guid>
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    
    public ICollection<EmployeeShift> EmployeeShifts { get; set; }
}