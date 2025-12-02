using FreshChoice.Services.Announcement.Contracts;
using FreshChoice.Services.Announcement.Models;
using Microsoft.AspNetCore.Mvc;
using Essentials.Results;


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