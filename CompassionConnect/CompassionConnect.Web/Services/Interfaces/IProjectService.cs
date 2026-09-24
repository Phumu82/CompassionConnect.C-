using CompassionConnect.Web.Models;
using CompassionConnect.Web.ViewModels;

namespace CompassionConnect.Web.Services.Interfaces;

public interface IProjectService
{
    Task<IReadOnlyList<ReliefProject>> GetAllAsync();
    Task<IReadOnlyList<ReliefProject>> GetActiveAsync();
    Task<IReadOnlyList<ReliefProject>> GetEmergencyAsync();
    Task<ReliefProject?> GetByIdAsync(int id);
    Task<ReliefProject?> GetBySlugAsync(string slug);
    Task<ReliefProject> CreateAsync(ProjectEditViewModel model);
    Task UpdateAsync(int id, ProjectEditViewModel model);
    Task DeleteAsync(int id);
}
