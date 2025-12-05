using FreshChoice.Services.Announcement.Contracts;
using FreshChoice.Services.Announcement.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FreshChoice.Presentation.Controllers;

[Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
public class AnnouncementController : Controller
{
    private readonly IAnnouncementService _announcementService;

    public AnnouncementController(IAnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View(new AnnouncementModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(AnnouncementModel model)
    {
        model.Date = model.Date.Date;
        
        if (!ModelState.IsValid)
            return View(model);

        var createResult = await _announcementService.CreateAnnouncementAsync(model);
        var updateResult = await _announcementService.UpdateAnnouncementAsync(model);

        if (updateResult.MutatedEntityId == null)
        {
            // failure
            Console.WriteLine("Update failed: " + updateResult.Message);
        }
        else
        {
            // success
            Console.WriteLine("Update succeeded with ID: " + updateResult.MutatedEntityId);
        }
        
        return RedirectToAction("Index", "Home");
    }
}