using Essentials.Results;
using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Models;

namespace FreshChoice.Services.EmployeeManagement.Contracts;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync();
    Task<EmployeeModel?> GetEmployeeByIdAsync(Guid employeeId);
    Task<MutationResult> UpdateEmployeeAsync(EmployeeModel employee);
    Task<MutationResult> CreateEmployeeAsync(EmployeeModel employee);
    Task<StandardResult> DeleteEmployeeAsync(Guid employeeId);
}