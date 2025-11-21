namespace FreshChoice.Data.Entities;

public class Shift : Entity
{
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
}
