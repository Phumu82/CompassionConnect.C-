using CompassionConnect.Web.Models;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

/// <summary>Handles the public donation flow. Anonymous visitors can donate
/// without registering an account, per project requirements.</summary>
public class DonationController : Controller
{
    private readonly IDonationService _donationService;
    private readonly IProjectService _projectService;
    private readonly UserManager<ApplicationUser> _userManager;

    public DonationController(
        IDonationService donationService,
        IProjectService projectService,
        UserManager<ApplicationUser> userManager)
    {
        _donationService = donationService;
        _projectService = projectService;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int? projectId)
    {
        var model = new DonateViewModel
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
                model.PhoneNumber = user.PhoneNumber;
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(DonateViewModel model)
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

        var donation = await _donationService.ProcessDonationAsync(model, applicationUserId);
        return RedirectToAction(nameof(Success), new { reference = donation.ReferenceNumber });
    }

    [HttpGet]
    public async Task<IActionResult> Success(string reference)
    {
        var donation = await _donationService.GetByReferenceAsync(reference);
        if (donation is null) return NotFound();
        return View(donation);
    }
}
