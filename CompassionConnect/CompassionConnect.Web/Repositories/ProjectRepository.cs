using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class ProjectRepository : Repository<ReliefProject>, IProjectRepository
{
    public ProjectRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<ReliefProject?> GetBySlugAsync(string slug) =>
        await DbSet.Include(p => p.ProjectLead)
            .FirstOrDefaultAsync(p => p.Slug == slug);

    public async Task<IReadOnlyList<ReliefProject>> GetActiveAsync() =>
        await DbSet.Where(p => p.Status == ProjectStatus.Active)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();

    public async Task<IReadOnlyList<ReliefProject>> GetEmergencyAsync() =>
        await DbSet.Where(p => p.IsEmergency && p.Status == ProjectStatus.Active)
            .OrderByDescending(p => p.StartDate)
            .ToListAsync();

    public async Task<bool> SlugExistsAsync(string slug) => await DbSet.AnyAsync(p => p.Slug == slug);
}
