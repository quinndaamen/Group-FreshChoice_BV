using Essentials.Results;
using FreshChoice.Data;
using FreshChoice.Services.Shift.Models;

namespace FreshChoice.Services.Shift.Contracts;

public interface IShiftService
{
    Task<IEnumerable<ShiftModel>> GetAllShiftsAsync();
    Task<ShiftModel?> GetShiftByIdAsync(long id);
    Task<MutationResult> CreateShiftAsync(ShiftModel shift);
    Task<MutationResult> UpdateShiftAsync(ShiftModel shift);
    Task<StandardResult> DeleteShiftAsync(long id);
    Task<StandardResult> RemoveEmployeeFromShiftAsync(Guid employeeId, long shiftId);
    Task<MutationResult> CreateShiftWithEmployeeAsync(Guid employeeId, ShiftModel shift);
}