using FreshChoice.Services.Shifts.Models;

namespace FreshChoice.Services.EmployeeManagement.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public List<ShiftModel> Shifts { get; set; }
    
}