using System.Diagnostics;
using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Services.Interfaces;
using GiftOfTheGivers.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GiftOfTheGivers.Web.Controllers;

public class HomeController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IDonationService _donationService;
    private readonly IVolunteerService _volunteerService;
    private readonly INewsService _newsService;

    public HomeController(
        IProjectService projectService,
        IDonationService donationService,
        IVolunteerService volunteerService,
        INewsService newsService)
    {
        _projectService = projectService;
        _donationService = donationService;
        _volunteerService = volunteerService;
        _newsService = newsService;
    }

    public async Task<IActionResult> Index()
    {
        var emergency = await _projectService.GetEmergencyAsync();
        var allProjects = await _projectService.GetActiveAsync();
        var volunteers = await _volunteerService.GetAllAsync();
        var news = await _newsService.GetLatestAsync(3);

        var model = new HomeViewModel
        {
            EmergencyCampaigns = emergency,
            LatestNews = news,
            TotalRaised = await _donationService.GetTotalRaisedAsync(),
            PeopleAssisted = allProjects.Sum(p => p.PeopleAssisted),
            ActiveVolunteers = volunteers.Count,
            ActiveProjects = allProjects.Count
        };

        return View(model);
    }

    public IActionResult About() => View();

    public IActionResult Contact() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}


