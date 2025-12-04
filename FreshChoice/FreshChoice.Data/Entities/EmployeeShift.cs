using FreshChoice.Data;
using FreshChoice.Data.Entities;

public class EmployeeShift : Entity
{
    public Guid EmployeeId { get; set; }
    public long ShiftId { get; set; }

    public Department Department { get; set; } // enum storage, not FK
    public Department DepartmentId { get; set; }  // use the enum directly


    public Employee Employee { get; set; }
    public Shift Shift { get; set; }
}
