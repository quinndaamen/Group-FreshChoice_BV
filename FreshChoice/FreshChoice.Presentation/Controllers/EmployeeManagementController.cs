using FreshChoice.Services.EmployeeManagement.Contracts;
using FreshChoice.Services.EmployeeManagement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshChoice.Presentation.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class EmployeeManagementController : Controller
{
    private readonly IEmployeeService _employeeService;

    public EmployeeManagementController(IEmployeeService employeeService)
    {
        _employeeService = employeeService;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await _employeeService.GetAllEmployeesAsync();
        return View(employees);
    }


    public IActionResult Create() => View();

    [HttpPost]
    public async Task<IActionResult> Create(EmployeeModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _employeeService.CreateEmployeeAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    public async Task<IActionResult> Edit(Guid id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EmployeeModel model)
    {
        if (!ModelState.IsValid) return View(model);

        await _employeeService.UpdateEmployeeAsync(model);
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(Guid id)
    {
        var employee = await _employeeService.GetEmployeeByIdAsync(id);
        if (employee == null) return NotFound();
        return View(employee);
    }

    [HttpPost, ActionName("Delete")]
    public async Task<IActionResult> DeleteConfirmed(Guid id)
    {
        await _employeeService.DeleteEmployeeAsync(id);
        return RedirectToAction(nameof(Index));
    }
}