using CompassionConnect.Web.Data;
using CompassionConnect.Web.Models;
using CompassionConnect.Web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CompassionConnect.Web.Repositories;

public class TaxCertificateRepository : Repository<TaxCertificate>, ITaxCertificateRepository
{
    public TaxCertificateRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<TaxCertificate?> GetByCertificateNumberAsync(string certificateNumber) =>
        await DbSet.Include(t => t.Donation).ThenInclude(d => d!.Project)
            .Include(t => t.Donor)
            .FirstOrDefaultAsync(t => t.CertificateNumber == certificateNumber);

    public async Task<TaxCertificate?> GetByDonationIdAsync(int donationId) =>
        await DbSet.FirstOrDefaultAsync(t => t.DonationId == donationId);

    public async Task<IReadOnlyList<TaxCertificate>> GetByDonorIdAsync(int donorId) =>
        await DbSet.Include(t => t.Donation).ThenInclude(d => d!.Project)
            .Where(t => t.DonorId == donorId)
            .OrderByDescending(t => t.DateIssued)
            .ToListAsync();

    public async Task<bool> CertificateNumberExistsAsync(string certificateNumber) =>
        await DbSet.AnyAsync(t => t.CertificateNumber == certificateNumber);

    public async Task<int> CountForYearAsync(int year) =>
        await DbSet.CountAsync(t => t.DateIssued.Year == year);
}
