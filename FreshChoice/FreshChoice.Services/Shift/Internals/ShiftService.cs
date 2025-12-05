using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Services.Shift.Contracts;
using FreshChoice.Services.Shift.Extensions;
using FreshChoice.Services.Shift.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshChoice.Services.Shift.Internals;

internal class ShiftService : IShiftService
{
    private readonly ApplicationDbContext context;
    private readonly ILogger<ShiftService> logger;

    public ShiftService(ApplicationDbContext context, ILogger<ShiftService> logger)
    {
        this.context = context;
        this.logger = logger;
    }

    public async Task<IEnumerable<ShiftModel>> GetAllShiftsAsync() =>
        await this.context
            .Shifts
            .AsNoTracking()
            .Select(x => x.ToModel())
            .ToListAsync();

    public async Task<ShiftModel?> GetShiftByIdAsync(long id) =>
        await this.context
            .Shifts
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(x => x.ToModel())
            .FirstOrDefaultAsync();

    public async Task<MutationResult> CreateShiftAsync(ShiftModel shift)
    {
        try
        {
            // Business rule
            if (shift.EndTime <= shift.StartTime)
                return MutationResult.ResultFrom("End time must be after start time.");

            // Convert model → entity (UTC applied in extension)
            var shiftEntity = new Data.Entities.Shift
            {
                Id = shift.Id,
                StartTime = DateTime.SpecifyKind(shift.StartTime, DateTimeKind.Utc),
                EndTime = DateTime.SpecifyKind(shift.EndTime, DateTimeKind.Utc),
                Date = DateTime.SpecifyKind(shift.Date, DateTimeKind.Utc),
                Department = shift.Department,
            };

            this.context.Shifts.Add(shiftEntity);
            await this.context.SaveChangesAsync();

            return MutationResult.ResultFrom(shiftEntity.ToModel(), "Shift created");
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error creating a shift");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<MutationResult> UpdateShiftAsync(ShiftModel shift)
    {
        try
        {
            var existing = await this.context.Shifts.FindAsync(shift.Id);

            if (existing == null)
                return MutationResult.ResultFrom("Shift not found");

            // Business rule
            if (shift.EndTime <= shift.StartTime)
                return MutationResult.ResultFrom("End time must be after start time.");

            // Apply updated fields (converted to UTC)
            
            existing.StartTime = shift.StartTime.ToUniversalTime();
            existing.EndTime = shift.EndTime.ToUniversalTime();
            existing.Date = DateTime.SpecifyKind(shift.Date.Date, DateTimeKind.Utc);
            existing.TotalTime = shift.TotalTime;
            existing.Department = shift.Department;

            await this.context.SaveChangesAsync();
            return MutationResult.ResultFrom(existing.ToModel(), "Shift updated");
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error updating a shift");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<StandardResult> DeleteShiftAsync(long id)
    {
        try
        {
            var shiftEntity = await this.context.Shifts.FindAsync(id);

            if (shiftEntity == null)
                return StandardResult.UnsuccessfulResult("Shift not found");

            this.context.Shifts.Remove(shiftEntity);
            await this.context.SaveChangesAsync();

            return StandardResult.SuccessfulResult();
        }
        catch (Exception e)
        {
            this.logger.LogError(e, "Error deleting a shift");
            return StandardResult.UnsuccessfulResult(e.Message);
        }
    }
    
    public async Task<MutationResult> CreateShiftWithEmployeeAsync(Guid employeeId, ShiftModel shift)
    {
        try
        {
            if (shift.EndTime <= shift.StartTime)
                return MutationResult.ResultFrom("End time must be after start time.");

            var shiftEntity = new Data.Entities.Shift
            {
                StartTime = shift.StartTime.ToUniversalTime(),
                EndTime = shift.EndTime.ToUniversalTime(),
                Date = DateTime.SpecifyKind(shift.Date.Date, DateTimeKind.Utc),  // ✅ FIXED!
                TotalTime = shift.TotalTime,
                Department = shift.Department
            };

            context.Shifts.Add(shiftEntity);
            await context.SaveChangesAsync();

            var employeeShift = new Data.Entities.EmployeeShift
            {
                EmployeeId = employeeId,
                ShiftId = shiftEntity.Id,
                Department = shift.Department
            };

            context.EmployeeShifts.Add(employeeShift);
            await context.SaveChangesAsync();

            return MutationResult.ResultFrom(shiftEntity.ToModel(), "Shift created and employee assigned");
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error creating shift + employee assignment");
            return MutationResult.ResultFrom(e);
        }
    }


    public async Task<StandardResult> RemoveEmployeeFromShiftAsync(Guid employeeId, long shiftId)
    {
        try
        {
            var rel = await context.EmployeeShifts
                .FirstOrDefaultAsync(x => x.EmployeeId == employeeId && x.ShiftId == shiftId);

            if (rel == null)
                return StandardResult.UnsuccessfulResult("Employee is not assigned to this shift");

            context.EmployeeShifts.Remove(rel);
            await context.SaveChangesAsync();

            return StandardResult.SuccessfulResult();
        }
        catch (Exception e)
        {
            logger.LogError(e, "Error removing employee from shift");
            return StandardResult.UnsuccessfulResult(e.Message);
        }
    }
}
