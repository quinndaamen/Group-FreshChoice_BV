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
        };
}