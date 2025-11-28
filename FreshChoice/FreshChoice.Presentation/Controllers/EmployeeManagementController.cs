using FreshChoice.Services.EmployeeManagement.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace FreshChoice.Presentation.Controllers;

public class EmployeeManagementController : Controller
{
    private readonly IEmployeeService _employeeService;
    
    // GET
    public IActionResult Index()
    {
        return View();
    }
}