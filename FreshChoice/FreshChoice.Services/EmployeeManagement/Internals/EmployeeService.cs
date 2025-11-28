using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.EmployeeManagement.Extensions;
using FreshChoice.Services.EmployeeManagement.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshChoice.Services.EmployeeManagement.Internals;

internal class EmployeeService : IEmployeeService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<EmployeeService> logger;

    public EmployeeService(ApplicationDbContext context, ILogger<EmployeeService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<EmployeeModel>> GetAllEmployeesAsync() =>
        await this.context
            .Employees
            .Include(x => x.EmployeeShifts)
            .AsNoTracking()
            .Select(x => x.ToModel())
            .ToListAsync();


    public async Task<EmployeeModel?> GetEmployeeByIdAsync(Guid employeeId) =>
        await this.context
            .Employees
            .Include(x => x.EmployeeShifts)
            .Where(x => x.Id == employeeId)
            .Select(x => x.ToModel())
            .FirstOrDefaultAsync();

    public async Task<MutationResult> UpdateEmployeeAsync(EmployeeModel employee)
    {
        try
        {
            var employeeEntity = await this.context.Employees.FindAsync(employee.Id);

            if (employeeEntity == null)
            {
                return MutationResult.ResultFrom(null, "EmployeeNotFound");
            }
            
            employeeEntity.FirstName = employee.FirstName;
            employeeEntity.LastName = employee.LastName;
            employeeEntity.Email = employee.Email;
            
            await this.context.SaveChangesAsync();
            return MutationResult.ResultFrom(employeeEntity, "EmployeeUpdated");
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error updating employee");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<MutationResult> CreateEmployeeAsync(EmployeeModel employee)
    {
        try
        {
            var employeeEntity = new Employee()
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Email = employee.Email,
            };
            
            this.context.Employees.Add(employeeEntity);
            await this.context.SaveChangesAsync();
            
            return MutationResult.ResultFrom(employeeEntity, "EmployeeCreated");
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error creating employee");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<StandardResult> DeleteEmployeeAsync(Guid employeeId)
    {
        try
        {
            var employeeEntity = await this.context.Employees.FindAsync(employeeId);
            if (employeeEntity == null)
            {
                return StandardResult.UnsuccessfulResult("EmployeeNotFound");
            }
            
            this.context.Employees.Remove(employeeEntity);
            await this.context.SaveChangesAsync();
            
            return StandardResult.SuccessfulResult();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error deleting employee");
            return StandardResult.UnsuccessfulResult("EmployeeNotFound");
        }
    }
}