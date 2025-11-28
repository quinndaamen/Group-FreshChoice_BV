using FreshChoice.Data.Entities;
using FreshChoice.Services.EmployeeManagement.Models;
using FreshChoice.Services.Shifts.Models;

namespace FreshChoice.Services.Shift.Extensions;

public static class ModelExtensions
{
    public static ShiftModel ToModel(this Data.Entities.Shift shift) =>
        new()
        {
            Id = shift.Id,
            Date = shift.Date,
            Department = shift.Department,
            StartTime = shift.StartTime,
            EndTime = shift.EndTime,
            TotalTime = shift.TotalTime,
        };
}