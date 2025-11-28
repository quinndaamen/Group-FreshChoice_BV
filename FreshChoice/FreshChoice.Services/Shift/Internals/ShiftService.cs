using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.Shift.Contracts;
using FreshChoice.Services.Shift.Extensions;
using FreshChoice.Services.Shift.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FreshChoice.Services.Shift.Internals;

internal class ShiftService : IShiftService
{
    private readonly ApplicationDbContext context;
    private readonly Logger<ShiftService> logger;

    public ShiftService(ApplicationDbContext context, Logger<ShiftService> logger)
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
            .Where(x => x.Id == id)
            .Select(x => x.ToModel())
            .FirstOrDefaultAsync();

    public async Task<MutationResult> CreateShiftAsync(ShiftModel shift)
    {
        try
        {
            var shiftEntity = new Data.Entities.Shift()
            {
                Id = shift.Id,
                StartTime = shift.StartTime,
                EndTime = shift.EndTime,
                TotalTime = shift.TotalTime,
                Date = shift.Date,
                Department = shift.Department,
            };

            this.context.Shifts.Add(shiftEntity);
            await this.context.SaveChangesAsync();

            return MutationResult.ResultFrom(shiftEntity, "Shift created");
        }
        catch (Exception e)
        {
            this.logger.LogError("Error creating a shift");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<MutationResult> UpdateShiftAsync(ShiftModel shift)
    {
        try
        {
            var shiftEntity = await this.context.Shifts.FindAsync(shift.Id);

            if (shiftEntity == null)
            {
                return MutationResult.ResultFrom("Shift not found");
            }
            
            shiftEntity.StartTime = shift.StartTime;
            shiftEntity.EndTime = shift.EndTime;
            shiftEntity.TotalTime = shift.TotalTime;
            shiftEntity.Date = shift.Date;
            shiftEntity.Department = shift.Department;
            
            await this.context.SaveChangesAsync();
            return MutationResult.ResultFrom(shiftEntity, "Shift updated");
        }
        catch (Exception e)
        {
            this.logger.LogError("Error updating a shift");
            return MutationResult.ResultFrom(e);
        }
    }

    public async Task<StandardResult> DeleteShiftAsync(long id)
    {
        try
        {
            var shiftEntity = await this.context.Shifts.FindAsync(id);

            if (shiftEntity == null)
            {
                return StandardResult.UnsuccessfulResult("Shift not found");
            }
            
            this.context.Shifts.Remove(shiftEntity);
            await this.context.SaveChangesAsync();
            
            return StandardResult.SuccessfulResult();
        }
        catch (Exception e)
        {
            this.logger.LogError("Error deleting a shift");
            return StandardResult.UnsuccessfulResult(e.Message);
        }
    }
}