using GiftOfTheGivers.Web.Models;
using GiftOfTheGivers.Web.Repositories.Interfaces;
using GiftOfTheGivers.Web.Services.Interfaces;
using GiftOfTheGivers.Web.ViewModels;

namespace GiftOfTheGivers.Web.Services;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IReadOnlyList<ReliefProject>> GetAllAsync() => await _unitOfWork.Projects.GetAllAsync();

    public async Task<IReadOnlyList<ReliefProject>> GetActiveAsync() => await _unitOfWork.Projects.GetActiveAsync();

    public async Task<IReadOnlyList<ReliefProject>> GetEmergencyAsync() => await _unitOfWork.Projects.GetEmergencyAsync();

    public async Task<ReliefProject?> GetByIdAsync(int id) => await _unitOfWork.Projects.GetByIdAsync(id);

    public async Task<ReliefProject?> GetBySlugAsync(string slug) => await _unitOfWork.Projects.GetBySlugAsync(slug);

    public async Task<ReliefProject> CreateAsync(ProjectEditViewModel model)
    {
        var slug = await BuildUniqueSlugAsync(model.Title);

        var project = new ReliefProject
        {
            Title = model.Title,
            Slug = slug,
            Province = model.Province,
            Category = model.Category,
            Status = model.Status,
            Summary = model.Summary,
            Description = model.Description,
            GoalAmount = model.GoalAmount,
            RaisedAmount = model.RaisedAmount,
            PeopleAssisted = model.PeopleAssisted,
            ImageUrl = string.IsNullOrWhiteSpace(model.ImageUrl) ? "/images/campaigns/default-campaign.jpg" : model.ImageUrl,
            IsEmergency = model.IsEmergency,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            EmployeeId = model.EmployeeId
        };

        await _unitOfWork.Projects.AddAsync(project);
        await _unitOfWork.CompleteAsync();
        return project;
    }

    public async Task UpdateAsync(int id, ProjectEditViewModel model)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Project {id} not found.");

        project.Title = model.Title;
        project.Province = model.Province;
        project.Category = model.Category;
        project.Status = model.Status;
        project.Summary = model.Summary;
        project.Description = model.Description;
        project.GoalAmount = model.GoalAmount;
        project.RaisedAmount = model.RaisedAmount;
        project.PeopleAssisted = model.PeopleAssisted;
        if (!string.IsNullOrWhiteSpace(model.ImageUrl)) project.ImageUrl = model.ImageUrl;
        project.IsEmergency = model.IsEmergency;
        project.StartDate = model.StartDate;
        project.EndDate = model.EndDate;
        project.EmployeeId = model.EmployeeId;

        _unitOfWork.Projects.Update(project);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var project = await _unitOfWork.Projects.GetByIdAsync(id);
        if (project is null) return;
        _unitOfWork.Projects.Remove(project);
        await _unitOfWork.CompleteAsync();
    }

    private async Task<string> BuildUniqueSlugAsync(string title)
    {
        var baseSlug = title.ToLowerInvariant()
            .Replace(" ", "-")
            .Where(c => char.IsLetterOrDigit(c) || c == '-')
            .Aggregate(string.Empty, (acc, c) => acc + c);

        var slug = baseSlug;
        var suffix = 1;
        while (await _unitOfWork.Projects.SlugExistsAsync(slug))
        {
            slug = $"{baseSlug}-{suffix++}";
        }
        return slug;
    }
}


