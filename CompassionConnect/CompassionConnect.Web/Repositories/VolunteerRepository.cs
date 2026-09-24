using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class VolunteerRepository : Repository<Volunteer>, IVolunteerRepository
{
    public VolunteerRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Volunteer?> GetByReferenceAsync(string reference) =>
        await DbSet.Include(v => v.Project).FirstOrDefaultAsync(v => v.ReferenceNumber == reference);

    public async Task<IReadOnlyList<Volunteer>> GetRecentAsync(int count) =>
        await DbSet.Include(v => v.Project)
            .OrderByDescending(v => v.DateApplied)
            .Take(count)
            .ToListAsync();

    public async Task<bool> ReferenceExistsAsync(string reference) =>
        await DbSet.AnyAsync(v => v.ReferenceNumber == reference);

    public async Task<int> CountForYearAsync(int year) =>
        await DbSet.CountAsync(v => v.DateApplied.Year == year);
}
