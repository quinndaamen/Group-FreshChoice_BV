using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.EmployeeManagement.Extensions;
using FreshChoice.Services.EmployeeManagement.Models;
using FreshChoice.Services.Identity.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshChoice.Services.EmployeeManagement.Internals;

internal class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<EmployeeService> _logger;
    private readonly UserManager<Employee> _userManager;

    public EmployeeService(
        ApplicationDbContext context,
        ILogger<EmployeeService> logger,
        UserManager<Employee> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    // ---------------- GET ALL ----------------
    public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync()
    {
        var employees = await _context.Employees
            .Include(e => e.EmployeeShifts)
            .AsNoTracking()
            .ToListAsync();
        
        var employeeModels = new List<EmployeeModel>();
        foreach (var e in employees)
        {
            var roles = await _userManager.GetRolesAsync(e);
            employeeModels.Add(new EmployeeModel
            {
                Id = e.Id,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                Role = roles.FirstOrDefault() ?? "None",
                WagePerHour = e.WagePerHour,
                PhoneNumber = e.PhoneNumber,
                HireDate = e.HireDate,
            });
        }

        return (employeeModels);
    }
        

    // ---------------- GET BY ID ----------------
    public async Task<EmployeeModel?> GetEmployeeByIdAsync(Guid employeeId) =>
        await _context.Employees
            .Include(e => e.EmployeeShifts)
            .Where(e => e.Id == employeeId)
            .Select(e => e.ToModel())
            .FirstOrDefaultAsync();

    // ---------------- CREATE ----------------
    public async Task<IdentityResult> CreateEmployeeAsync(EmployeeModel employee)
    {
        try
        {
            var newEmployee = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
                UserName = employee.Email,
                NormalizedEmail = employee.Email.ToUpper(),
                NormalizedUserName = employee.Email.ToUpper(),
                HireDate = DateTime.SpecifyKind(employee.HireDate, DateTimeKind.Utc),
                PhoneNumber = employee.PhoneNumber,
                WagePerHour = employee.WagePerHour,
            };
            
            var result = await _userManager.CreateAsync(newEmployee, employee.Password ?? string.Empty);
            if (!result.Succeeded)
            {
                return IdentityResult.Failed(result.Errors.ToArray());
            }

            if (!string.IsNullOrWhiteSpace(employee.Role))
            {
                var roleResult = await _userManager.AddToRoleAsync(newEmployee, employee.Role);

                if (!roleResult.Succeeded)
                {
                    // Delete user to avoid half-registered users
                    await _userManager.DeleteAsync(newEmployee);
                    return IdentityResult.Failed(roleResult.Errors.ToArray());
                }
            }

            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An exception occurred while creating an employee with email: {Email}", employee.Email);
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UserCreationException",
                Description = "An unexpected error occurred while creating the user.",
            });
        }
    }

    // ---------------- UPDATE ----------------
    public async Task<IdentityResult> UpdateEmployeeAsync(EmployeeModel employee)
    {
        try
        {
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null)
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserNotFound",
                    Description = "User not found.",
                });

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.UserName = employee.Email;
            existing.NormalizedEmail = employee.Email.ToUpper();
            existing.NormalizedUserName = employee.Email.ToUpper();
            existing.HireDate = DateTime.SpecifyKind(employee.HireDate, DateTimeKind.Utc);
            existing.PhoneNumber = employee.PhoneNumber;
            
            var updateResult = await _userManager.UpdateAsync(existing);
            if (!updateResult.Succeeded)
            {
                return updateResult;
            }
            
            var currentRoles = await _userManager.GetRolesAsync(existing);
            var currentRole = currentRoles.FirstOrDefault();

            if (!string.Equals(currentRole, employee.Role, StringComparison.OrdinalIgnoreCase))
            {
                if (currentRoles.Any())
                {
                    var removeResult = await _userManager.RemoveFromRolesAsync(existing, currentRoles);
                    if (!removeResult.Succeeded)
                    {
                        return removeResult;
                    }
                }

                var addResult = await _userManager.AddToRoleAsync(existing, employee.Role);
                if (!addResult.Succeeded)
                {
                    return addResult;
                }

                if (!string.IsNullOrWhiteSpace(employee.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(existing);
                    var passwordResult = await _userManager.ResetPasswordAsync(existing, token, employee.Password);

                    if (!passwordResult.Succeeded)
                    {
                        return passwordResult;
                    }
                }
            }
            
            return IdentityResult.Success;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while updating user with ID {UserId}", employee.Id);
            return IdentityResult.Failed(new IdentityError
            {
                Code = "UpdateUserException",
                Description = "An unexpected error occurred while updating the user.",
            });
        }
    }

    // ---------------- DELETE ----------------
    public async Task<StandardResult> DeleteEmployeeAsync(Guid employeeId)
    {
        try
        {
            var employee = await _context.Employees.FindAsync(employeeId);
            if (employee == null)
                return StandardResult.UnsuccessfulResult("EmployeeNotFound");

            // Remove employee using UserManager to handle Identity properly
            var result = await _userManager.DeleteAsync(employee);
            if (result.Succeeded)
                return StandardResult.SuccessfulResult();

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            _logger.LogError("Error deleting employee: {errors}", errors);
            return StandardResult.UnsuccessfulResult(errors);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting employee");
            return StandardResult.UnsuccessfulResult("EmployeeDeleteFailed");
        }
    }
}

