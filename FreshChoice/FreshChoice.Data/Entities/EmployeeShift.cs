using FreshChoice.Data;
using FreshChoice.Data.Entities;

public class EmployeeShift : Entity
{
    public Guid EmployeeId { get; set; }
    public long ShiftId { get; set; }
    public long DepartmentId { get; set; }   // ← You MUST keep this FK

    public Employee Employee { get; set; }
    public Department Department { get; set; }
    public Shift Shift { get; set; }
}