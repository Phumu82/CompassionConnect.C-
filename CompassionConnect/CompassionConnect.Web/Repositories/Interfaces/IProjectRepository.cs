using CompassionConnect.Web.Models;

namespace CompassionConnect.Web.Repositories.Interfaces;

public interface IProjectRepository : IRepository<ReliefProject>
{
    Task<ReliefProject?> GetBySlugAsync(string slug);
    Task<IReadOnlyList<ReliefProject>> GetActiveAsync();
    Task<IReadOnlyList<ReliefProject>> GetEmergencyAsync();
    Task<bool> SlugExistsAsync(string slug);
}
