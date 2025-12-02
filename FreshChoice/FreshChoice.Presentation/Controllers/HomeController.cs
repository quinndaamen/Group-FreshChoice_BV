using System.Diagnostics;
using FreshChoice.Data;
using Microsoft.AspNetCore.Mvc;
using FreshChoice.Presentation.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace FreshChoice.Presentation.Controllers;

[AllowAnonymous]
[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var announcements = await _context.Announcements.ToListAsync();
        return View(announcements);
    }
}
