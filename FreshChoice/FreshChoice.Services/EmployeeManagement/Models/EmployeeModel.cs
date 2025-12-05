using FreshChoice.Services.Shift.Models;
using Microsoft.AspNetCore.Identity;

namespace FreshChoice.Services.EmployeeManagement.Models;

public class EmployeeModel
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }
    public List<ShiftModel>? Shifts { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? PhoneNumber { get; set; }
    public string Role { get; set; }
    public float WagePerHour { get; set; }
    public DateTime HireDate { get; set; }
}