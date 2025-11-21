namespace FreshChoice.Data.Entities;

public class Department : Entity
{
    public string Name { get; set; }
    public ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();
}