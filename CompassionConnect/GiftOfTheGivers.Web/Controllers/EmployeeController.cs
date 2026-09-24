using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using GiftOfTheGivers.Web.Services.Interfaces;
using GiftOfTheGivers.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers;

[Authorize(Roles = "Employee")]
public class EmployeeController : Controller
{
    private readonly IDonationService _donationService;
    private readonly IVolunteerService _volunteerService;
    private readonly IProjectService _projectService;
    private readonly INewsService _newsService;
    private readonly IUnitOfWork _unitOfWork;

    public EmployeeController(
        IDonationService donationService,
        IVolunteerService volunteerService,
        IProjectService projectService,
        INewsService newsService,
        IUnitOfWork unitOfWork)
    {
        _donationService = donationService;
        _volunteerService = volunteerService;
        _projectService = projectService;
        _newsService = newsService;
        _unitOfWork = unitOfWork;
    }

    public async Task<IActionResult> Dashboard()
    {
        var projects = await _projectService.GetAllAsync();
        var volunteers = await _volunteerService.GetAllAsync();

        var model = new EmployeeDashboardViewModel
        {
            TotalRaised = await _donationService.GetTotalRaisedAsync(),
            TotalDonations = (await _donationService.GetRecentAsync(int.MaxValue)).Count,
            ActiveVolunteers = volunteers.Count(v => v.Status is VolunteerStatus.Approved or VolunteerStatus.Placed),
            ActiveProjects = projects.Count(p => p.Status == ProjectStatus.Active),
            RecentDonations = await _donationService.GetRecentAsync(8),
            RecentVolunteers = volunteers.OrderByDescending(v => v.DateApplied).Take(8).ToList(),
            Projects = projects
        };

        return View(model);
    }

    public async Task<IActionResult> Donations()
    {
        var donations = await _donationService.GetRecentAsync(int.MaxValue);
        return View(donations);
    }

    public async Task<IActionResult> Volunteers()
    {
        var volunteers = await _volunteerService.GetAllAsync();
        return View(volunteers);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateVolunteerStatus(int volunteerId, VolunteerStatus status)
    {
        await _volunteerService.UpdateStatusAsync(volunteerId, status);
        TempData["Success"] = "Volunteer status updated.";
        return RedirectToAction(nameof(Volunteers));
    }

    public async Task<IActionResult> Projects()
    {
        var projects = await _projectService.GetAllAsync();
        return View(projects);
    }

    public async Task<IActionResult> Reports()
    {
        var projects = await _projectService.GetAllAsync();
        var donations = await _donationService.GetRecentAsync(int.MaxValue);

        ViewBag.TotalRaised = donations.Where(d => d.Status == DonationStatus.Completed).Sum(d => d.Amount);
        ViewBag.TotalGoal = projects.Sum(p => p.GoalAmount);
        ViewBag.PeopleAssisted = projects.Sum(p => p.PeopleAssisted);

        return View(projects);
    }

    public async Task<IActionResult> News()
    {
        var articles = await _newsService.GetAllAsync();
        return View(articles);
    }
}


