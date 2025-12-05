using Essentials.Results;
using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace FreshChoice.Services.EmployeeManagement.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();
    Task<EmployeeModel?> GetEmployeeByIdAsync(Guid employeeId);
    Task<IdentityResult> UpdateEmployeeAsync(EmployeeModel employee);
    Task<IdentityResult> CreateEmployeeAsync(EmployeeModel employee);
    Task<StandardResult> DeleteEmployeeAsync(Guid employeeId);
}