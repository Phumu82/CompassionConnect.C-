using CompassionConnect.Web.Repositories.Interfaces;
using CompassionConnect.Web.Services.Interfaces;
using CompassionConnect.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompassionConnect.Web.Controllers;

public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IUnitOfWork _unitOfWork;

    public ProjectController(IProjectService projectService, IUnitOfWork unitOfWork)
    {
        _projectService = projectService;
        _unitOfWork = unitOfWork;
    }

    [HttpGet]
    public async Task<IActionResult> Index(string? category, string? province)
    {
        var projects = await _projectService.GetActiveAsync();

        if (!string.IsNullOrWhiteSpace(category))
            projects = projects.Where(p => p.Category.ToString() == category).ToList();

        if (!string.IsNullOrWhiteSpace(province))
            projects = projects.Where(p => p.Province == province).ToList();

        return View(projects);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string slug)
    {
        var project = await _projectService.GetBySlugAsync(slug);
        if (project is null) return NotFound();
        return View(project);
    }

    // ------------------------------------------------------------ Employee CRUD
    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var model = new ProjectEditViewModel
        {
            StartDate = DateTime.UtcNow,
            AvailableEmployees = await _unitOfWork.Employees.GetAllAsync()
        };
        return View(model);
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProjectEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableEmployees = await _unitOfWork.Employees.GetAllAsync();
            return View(model);
        }

        var project = await _projectService.CreateAsync(model);
        TempData["Success"] = $"'{project.Title}' has been created.";
        return RedirectToAction("Projects", "Employee");
    }

    [Authorize(Roles = "Employee")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var project = await _projectService.GetByIdAsync(id);
        if (project is null) return NotFound();

        var model = new ProjectEditViewModel
        {
            ProjectId = project.ProjectId,
            Title = project.Title,
            Province = project.Province,
            Category = project.Category,
            Status = project.Status,
            Summary = project.Summary,
            Description = project.Description,
            GoalAmount = project.GoalAmount,
            RaisedAmount = project.RaisedAmount,
            PeopleAssisted = project.PeopleAssisted,
            ImageUrl = project.ImageUrl,
            IsEmergency = project.IsEmergency,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            EmployeeId = project.EmployeeId,
            AvailableEmployees = await _unitOfWork.Employees.GetAllAsync()
        };

        return View(model);
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProjectEditViewModel model)
    {
        if (!ModelState.IsValid)
        {
            model.AvailableEmployees = await _unitOfWork.Employees.GetAllAsync();
            return View(model);
        }

        await _projectService.UpdateAsync(id, model);
        TempData["Success"] = $"'{model.Title}' has been updated.";
        return RedirectToAction("Projects", "Employee");
    }

    [Authorize(Roles = "Employee")]
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        await _projectService.DeleteAsync(id);
        TempData["Success"] = "The project has been removed.";
        return RedirectToAction("Projects", "Employee");
    }
}
