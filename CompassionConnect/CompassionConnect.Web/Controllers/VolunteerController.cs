using CompassionConnect.Web.Models;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

public class VolunteerController : Controller
{
    private readonly IVolunteerService _volunteerService;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public VolunteerController(
        IVolunteerService volunteerService,
        IProjectService projectService,
        UserManager<ApplicationUser> userManager)
    {
        _volunteerService = volunteerService;
        _projectService = projectService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? projectId)
    {
        var model = new VolunteerRegisterViewModel
        {
            ProjectId = projectId,
            AvailableProjects = await _projectService.GetActiveAsync()
        };

        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user is not null)
            {
                model.FullName = user.FullName;
                model.Email = user.Email ?? string.Empty;
                model.PhoneNumber = user.PhoneNumber ?? string.Empty;
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(VolunteerRegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableProjects = await _projectService.GetActiveAsync();
            return View(model);
        }

        string? applicationUserId = null;
        if (User.Identity?.IsAuthenticated == true)
        {
            var user = await _userManager.GetUserAsync(User);
            applicationUserId = user?.Id;
        }

        var volunteer = await _volunteerService.RegisterAsync(model, applicationUserId);
        return RedirectToAction(nameof(Success), new { reference = volunteer.ReferenceNumber });
    }

    [HttpGet]
    public async Task<IActionResult> Success(string reference)
    {
        var volunteer = await _volunteerService.GetByReferenceAsync(reference);
        if (volunteer is null) return NotFound();
        return View(volunteer);
    }
}
