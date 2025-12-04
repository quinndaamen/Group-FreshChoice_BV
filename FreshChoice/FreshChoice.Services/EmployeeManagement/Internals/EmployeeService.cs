using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.EmployeeManagement.Extensions;
using FreshChoice.Services.EmployeeManagement.Models;
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
    public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync() =>
        await _context.Employees
            .Include(e => e.EmployeeShifts)
            .AsNoTracking()
            .Select(e => e.ToModel())
            .ToListAsync();

    // ---------------- GET BY ID ----------------
    public async Task<EmployeeModel?> GetEmployeeByIdAsync(Guid employeeId) =>
        await _context.Employees
            .Include(e => e.EmployeeShifts)
            .Where(e => e.Id == employeeId)
            .Select(e => e.ToModel())
            .FirstOrDefaultAsync();

    // ---------------- CREATE ----------------
    public async Task<MutationResult> CreateEmployeeAsync(EmployeeModel employee)
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
                NormalizedUserName = employee.Email.ToUpper()
            };
            
            var result = await _userManager.CreateAsync(newEmployee, employee.Password ?? string.Empty);

            if (result.Succeeded)
                return MutationResult.ResultFrom(newEmployee, "EmployeeCreated");

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return MutationResult.ResultFrom(new Exception(errors));
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error creating employee");
            return MutationResult.ResultFrom(e);
        }
    }

    // ---------------- UPDATE ----------------
    public async Task<MutationResult> UpdateEmployeeAsync(EmployeeModel employee)
    {
        try
        {
            var existing = await _context.Employees.FindAsync(employee.Id);
            if (existing == null)
                return MutationResult.ResultFrom(null, "EmployeeNotFound");

            existing.FirstName = employee.FirstName;
            existing.LastName = employee.LastName;
            existing.Email = employee.Email;
            existing.UserName = employee.Email;
            existing.NormalizedEmail = employee.Email.ToUpper();
            existing.NormalizedUserName = employee.Email.ToUpper();

            await _context.SaveChangesAsync();
            return MutationResult.ResultFrom(existing, "EmployeeUpdated");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error updating employee");
            return MutationResult.ResultFrom(e);
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

