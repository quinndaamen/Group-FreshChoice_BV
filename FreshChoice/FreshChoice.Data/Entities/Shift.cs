using System.Runtime.CompilerServices;

namespace FreshChoice.Data.Entities;

public class Shift : Entity
{
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public Department? Department { get; set; }
    public TimeSpan TotalTime { get; set; }

    public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
}
