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
        public async Task<IActionResult> Create(EmployeeShift model)
        {
            if (!ModelState.IsValid)
            {
                // repopulate ViewBag data if needed
                ViewBag.Employees = _db.Employees.ToList();
                ViewBag.Departments = Enum.GetValues(typeof(Department));
                return View(model);
            }

            // Create a new Shift with the date/time entered in the form
            var newShift = new Shift
            {
                Date = model.Shift.Date,
                StartTime = model.Shift.StartTime,
                EndTime = model.Shift.EndTime,
                TotalTime = model.Shift.EndTime - model.Shift.StartTime
            };

            _db.Shifts.Add(newShift);

            var employeeShift = new EmployeeShift
            {
                EmployeeId = model.EmployeeId,
                Shift = newShift,
                DepartmentId = model.DepartmentId
            };

            _db.EmployeeShifts.Add(employeeShift);

            await _db.SaveChangesAsync();

            return RedirectToAction("Index");
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
