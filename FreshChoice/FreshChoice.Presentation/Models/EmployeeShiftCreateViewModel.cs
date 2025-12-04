using FreshChoice.Data;
using FreshChoice.Data.Entities;

namespace FreshChoice.Presentation.Models;

public class EmployeeShiftCreateViewModel
{
    public Guid EmployeeId { get; set; }

    // Shift inputs
    public DateTime Date { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }

    public Department DepartmentId { get; set; }

    // Dropdown lists
    public List<Employee> Employees { get; set; } = new();
    public IEnumerable<Department> Departments { get; set; } 
        = Enum.GetValues(typeof(Department)).Cast<Department>();
}
