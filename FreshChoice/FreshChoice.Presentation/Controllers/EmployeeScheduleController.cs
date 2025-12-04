using FreshChoice.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Web.Controllers;

public class EmployeeScheduleController : Controller
{
    private readonly ApplicationDbContext _db;

    public EmployeeScheduleController(ApplicationDbContext db)
    {
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        var schedule = await _db.EmployeeShifts
            .Include(e => e.Employee)
            .Include(s => s.Shift)
            .Include(d => d.Department)
            .ToListAsync();

        return View(schedule);
    }
}