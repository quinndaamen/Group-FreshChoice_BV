using FreshChoice.Data;
using FreshChoice.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Web.Controllers
{
    public class EmployeeScheduleController : Controller
    {
        private readonly ApplicationDbContext _db;

        public EmployeeScheduleController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: Index
        public async Task<IActionResult> Index()
        {
            var schedule = await _db.EmployeeShifts
                .Include(e => e.Employee)
                .Include(s => s.Shift)
                .ToListAsync();

            return View(schedule);
        }

        // GET: Create
        public async Task<IActionResult> Create()
        {
            ViewBag.Employees = await _db.Employees.ToListAsync();
            ViewBag.Shifts = await _db.Shifts.ToListAsync();
            ViewBag.Departments = Enum.GetValues(typeof(Department));
            return View();
        }

        // POST: Create
        [HttpPost]
        public async Task<IActionResult> Create(EmployeeShift shift)
        {
            if (ModelState.IsValid)
            {
                _db.EmployeeShifts.Add(shift);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = await _db.Employees.ToListAsync();
            ViewBag.Shifts = await _db.Shifts.ToListAsync();
            ViewBag.Departments = Enum.GetValues(typeof(Department));
            return View(shift);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var shift = await _db.EmployeeShifts.FindAsync(id);
            if (shift == null) return NotFound();

            ViewBag.Employees = await _db.Employees.ToListAsync();
            ViewBag.Shifts = await _db.Shifts.ToListAsync();
            ViewBag.Departments = Enum.GetValues(typeof(Department));

            return View(shift);
        }

        // POST: Edit
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeShift shift)
        {
            if (ModelState.IsValid)
            {
                _db.EmployeeShifts.Update(shift);
                await _db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = await _db.Employees.ToListAsync();
            ViewBag.Shifts = await _db.Shifts.ToListAsync();
            ViewBag.Departments = Enum.GetValues(typeof(Department));

            return View(shift);
        }
    }
}
