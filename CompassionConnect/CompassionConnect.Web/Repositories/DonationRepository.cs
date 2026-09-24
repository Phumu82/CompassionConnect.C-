using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class DonationRepository : Repository<Donation>, IDonationRepository
{
    public DonationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<Donation?> GetByReferenceAsync(string reference) =>
        await DbSet.Include(d => d.Donor)
            .Include(d => d.Project)
            .Include(d => d.TaxCertificate)
            .FirstOrDefaultAsync(d => d.ReferenceNumber == reference);

    public async Task<IReadOnlyList<Donation>> GetByDonorIdAsync(int donorId) =>
        await DbSet.Include(d => d.Project)
            .Where(d => d.DonorId == donorId)
            .OrderByDescending(d => d.DonationDate)
            .ToListAsync();

    public async Task<IReadOnlyList<Donation>> GetRecentAsync(int count) =>
        await DbSet.Include(d => d.Donor)
            .Include(d => d.Project)
            .OrderByDescending(d => d.DonationDate)
            .Take(count)
            .ToListAsync();

    public async Task<bool> ReferenceExistsAsync(string reference) =>
        await DbSet.AnyAsync(d => d.ReferenceNumber == reference);

    public async Task<int> CountForYearAsync(int year) =>
        await DbSet.CountAsync(d => d.DonationDate.Year == year);

    public async Task<decimal> GetTotalRaisedAsync() =>
        await DbSet.Where(d => d.Status == DonationStatus.Completed).SumAsync(d => (decimal?)d.Amount) ?? 0m;
}
