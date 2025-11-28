using FreshChoice.Data;

namespace FreshChoice.Services.Shift.Models;

public class ShiftModel
{
    public long Id { get; set; }
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Department? Department { get; set; }
    public TimeSpan TotalTime { get; set; }
}