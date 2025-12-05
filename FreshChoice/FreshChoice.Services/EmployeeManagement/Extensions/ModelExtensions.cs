using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Models;

namespace FreshChoice.Services.EmployeeManagement.Extensions;

public static class ModelExtensions
{
    public static EmployeeModel ToModel(this Employee employee) =>
        new()
        {
            Id = employee.Id,
            FirstName = employee.FirstName,
            LastName = employee.LastName,
            Email = employee.Email,
            WagePerHour = employee.WagePerHour,
            PhoneNumber = employee.PhoneNumber,
            HireDate = DateTime.SpecifyKind(employee.HireDate, DateTimeKind.Utc),
        };
}