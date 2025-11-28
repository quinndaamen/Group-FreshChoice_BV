namespace FreshChoice.Data.Entities;

public class EmployeeShift : Entity
{
    public Guid EmployeeId { get; set; }
    public long ShiftId { get; set; }

    // Navigation
    public Employee Employee { get; set; }
    public Department Task { get; set; }
    public Shift Shift { get; set; }
}