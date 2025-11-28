using Essentials.Results;
using FreshChoice.Services.Shifts.Models;

namespace FreshChoice.Services.Shift.Contracts;

public interface IShiftService
{
    Task<IEnumerable<ShiftModel>> GetAllShiftsAsync();
    Task<ShiftModel?> GetShiftByIdAsync(long id);
    Task<MutationResult> CreateShiftAsync(ShiftModel shift);
    Task<MutationResult> UpdateShiftAsync(ShiftModel shift);
    Task<StandardResult> DeleteShiftAsync(long id);
}