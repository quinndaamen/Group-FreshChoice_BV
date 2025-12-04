using FreshChoice.Data.Entities;
using FreshChoice.Services.Shift.Contracts;
using FreshChoice.Services.Shift.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using FreshChoice.Data;
using FreshChoice.Services.EmployeeManagement.Contracts;

namespace FreshChoice.Presentation.Controllers
{
    [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    public class EmployeeScheduleController : Controller
    {
        private readonly IShiftService _shiftService;
        private readonly IEmployeeService _employeeService;

        public EmployeeScheduleController(IShiftService shiftService,  IEmployeeService employeeService)
        {
            _shiftService = shiftService;
            _employeeService = employeeService;
        }

        // GET: Index - list all shifts with employees
        public async Task<IActionResult> Index()
        {
            var shifts = await _shiftService.GetAllShiftsAsync();
            return View(shifts);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Departments = Enum.GetValues(typeof(Department));
            ViewBag.Employees = await _employeeService.GetAllEmployeesAsync();
            return View(new ShiftModel());
        }


        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(Guid employeeId, ShiftModel model)
        {
            if (model.EndTime <= model.StartTime)
                ModelState.AddModelError("", "End time must be after start time.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = Enum.GetValues(typeof(Department));
                return View(model);
            }

            var result = await _shiftService.CreateShiftWithEmployeeAsync(employeeId, model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Message);
                ViewBag.Departments = Enum.GetValues(typeof(Department));
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Edit shift
        public async Task<IActionResult> Edit(long shiftId)
        {
            var shift = await _shiftService.GetShiftByIdAsync(shiftId);
            if (shift == null) return NotFound();

            ViewBag.Departments = Enum.GetValues(typeof(Department));
            return View(shift);
        }

        // POST: Edit shift
        [HttpPost]
        public async Task<IActionResult> Edit(ShiftModel model)
        {
            if (model.EndTime <= model.StartTime)
                ModelState.AddModelError("", "End time must be after start time.");

            if (!ModelState.IsValid)
            {
                ViewBag.Departments = Enum.GetValues(typeof(Department));
                return View(model);
            }

            var result = await _shiftService.UpdateShiftAsync(model);

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", result.Message);
                ViewBag.Departments = Enum.GetValues(typeof(Department));
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Delete shift
        [HttpPost]
        public async Task<IActionResult> Delete(long shiftId)
        {
            var result = await _shiftService.DeleteShiftAsync(shiftId);

            if (!result.Succeeded)
                TempData["Error"] = result.Message;

            return RedirectToAction(nameof(Index));
        }
    }
}
